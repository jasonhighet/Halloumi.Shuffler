"""
One-off backfill of the "Shuffled" extended attribute from the ShufflerAudioDatabase git history.

For every track in Halloumi.Shuffler.ExtendedAttributes.xml, finds the first commit its shuffler details
appear in, and writes the feasible date range into the track's attributes:

    Shuffled=2018-05-02                 first seen in a commit made within a week of the one before it
    Shuffled=2024-03-01..2024-09-27     first seen in a commit made more than a week after the one before it
    Shuffled=..2016-04-10               already shuffled when the history starts (no earliest date)
    Shuffled=<today>                    not committed yet

Renamed tracks (a name disappears and a new one appears in the same commit with the same fade points)
keep the date of the original name.

Tracks that already have a Shuffled attribute are left alone.

Usage (dry run by default; writes a CSV preview next to --out):
    python -I backfill_shuffled.py <shuffler-folder> --out <preview.csv> [--apply]

Close Halloumi Shuffler before running with --apply: it holds the attributes in memory and writes them
back on save, which would discard the backfill.
"""
import argparse
import csv
import datetime
import os
import re
import subprocess
import sys

ATTRIBUTES_FILE = "Halloumi.Shuffler.ExtendedAttributes.xml"
COMBINED_NAMES = {"Haloumi.Shuffler.ExtendedAtrributes.xml", "Halloumi.Shuffler.ExtendedAtrributes.xml",
                  "Halloumi.Shuffler.ExtendedAttributes.xml"}
PER_TRACK_SUFFIX = ".ExtendedAttributes.txt"
ENTRY_RE = re.compile(r"<Track>(.*?)</Track>\s*<Attributes>(.*?)</Attributes>", re.S)
EXACT_WITHIN_DAYS = 7
KEY = "Shuffled"
# Fade points and duration identify a track's shuffler details well enough to spot a rename.
FINGERPRINT_KEYS = ("Duration", "FadeIn", "FadeOut")


def git(repo, *args):
    return subprocess.run(["git", "-C", repo, "-c", "core.quotepath=off", *args],
                          capture_output=True, check=True).stdout


def unescape(s):
    return (s.replace("&lt;", "<").replace("&gt;", ">").replace("&quot;", '"')
             .replace("&apos;", "'").replace("&amp;", "&"))


def parse_attributes(text):
    result = {}
    for part in text.split(";"):
        if "=" in part:
            k, v = part.split("=", 1)
            result.setdefault(k.strip(), v.strip())
    return result


def fingerprint(attributes):
    values = []
    for k in FINGERPRINT_KEYS:
        try:
            values.append(round(float(attributes[k]), 2))
        except (KeyError, ValueError):
            return None
    return tuple(values)


def decode_per_track(raw):
    for encoding in ("utf-16", "utf-8-sig", "latin-1"):
        try:
            return raw.decode(encoding)
        except UnicodeDecodeError:
            continue
    return ""


def snapshot(repo, sha):
    """Returns {track: attribute fingerprint or None} for one commit."""
    tracks = {}
    for f in git(repo, "ls-tree", "-r", "--name-only", sha).decode("utf-8", "replace").splitlines():
        if f.endswith(PER_TRACK_SUFFIX):
            tracks.setdefault(f[:-len(PER_TRACK_SUFFIX)], ("per-track", f))
        elif f in COMBINED_NAMES:
            text = git(repo, "show", f"{sha}:{f}").decode("utf-8", "replace")
            for name, attrs in ENTRY_RE.findall(text):
                tracks[unescape(name)] = fingerprint(parse_attributes(attrs))
    return tracks


def resolve(repo, sha, tracks, names):
    """Fills in fingerprints for per-track-era entries, reading only the files asked for."""
    for name in names:
        value = tracks.get(name)
        if isinstance(value, tuple) and len(value) == 2 and value[0] == "per-track":
            raw = git(repo, "show", f"{sha}:{value[1]}")
            tracks[name] = fingerprint(parse_attributes(decode_per_track(raw)))
    return tracks


def first_seen(repo):
    """Returns {track: (earliest date or None, latest date, commit, note)} from the git history."""
    commits = [line.split(" ", 1) for line in
               git(repo, "log", "--reverse", "--first-parent", "--format=%H %ad", "--date=short")
               .decode().splitlines()]
    result = {}
    previous, previous_sha, previous_date = {}, None, None
    for sha, date in commits:
        current = snapshot(repo, sha)
        added = [n for n in current if n not in previous]
        removed = [n for n in previous if n not in current]

        renamed_from = {}
        if added and removed:
            resolve(repo, sha, current, added)
            resolve(repo, previous_sha, previous, removed)
            by_print = {}
            for name in removed:
                fp = previous[name]
                if fp is not None:
                    by_print.setdefault(fp, []).append(name)
            for name in added:
                candidates = by_print.get(current[name])
                if candidates and len(candidates) == 1 and candidates[0] in result:
                    renamed_from[name] = candidates[0]

        for name in added:
            if name in result:
                continue  # came back after being removed: keep the first date
            if name in renamed_from:
                old = result[renamed_from[name]]
                result[name] = (old[0], old[1], old[2], f"renamed from '{renamed_from[name]}'")
            elif not previous:
                # the bulk import of everything shuffled before the history starts
                result[name] = (None, date, sha[:7], "in first commit")
            else:
                result[name] = (previous_date, date, sha[:7], "")

        previous, previous_sha, previous_date = current, sha, date
    return result


def shuffled_value(earliest, latest):
    if earliest is None:
        return f"..{latest}"
    gap = (datetime.date.fromisoformat(latest) - datetime.date.fromisoformat(earliest)).days
    return latest if gap <= EXACT_WITHIN_DAYS else f"{earliest}..{latest}"


def insert_attribute(attributes_text, value):
    """Adds Shuffled=value, keeping keys in roughly the order the app writes them (sorted by key)."""
    parts = [p for p in attributes_text.split(";") if p.strip()]
    parts.append(f"{KEY}={value}")
    parts.sort(key=lambda p: p.split("=", 1)[0].lower())
    return "".join(p + ";" for p in parts)


def main():
    parser = argparse.ArgumentParser()
    parser.add_argument("shuffler_folder")
    parser.add_argument("--out", required=True, help="CSV preview of every track's backfilled value")
    parser.add_argument("--apply", action="store_true", help="write the values into the attributes file")
    args = parser.parse_args()

    attributes_path = os.path.join(args.shuffler_folder, ATTRIBUTES_FILE)
    with open(attributes_path, encoding="utf-8", newline="") as fh:
        xml = fh.read()

    history = first_seen(args.shuffler_folder)
    today = datetime.date.today().isoformat()

    rows, counts = [], {}
    updated_entries = {}
    for name, attrs in ENTRY_RE.findall(xml):
        track = unescape(name)
        existing = parse_attributes(attrs)
        if KEY in existing:
            kind, value, commit, note = "already set", existing[KEY], "", ""
        elif track in history:
            earliest, latest, commit, note = history[track]
            value = shuffled_value(earliest, latest)
            kind = "before history" if earliest is None else ("range" if ".." in value else "exact")
        else:
            kind, value, commit, note = "uncommitted", today, "", "not in any commit yet"
        counts[kind] = counts.get(kind, 0) + 1
        rows.append((track, value, kind, commit, note))
        if kind != "already set":
            updated_entries[name] = insert_attribute(attrs, value)

    with open(args.out, "w", newline="", encoding="utf-8") as fh:
        writer = csv.writer(fh)
        writer.writerow(["Track", KEY, "Kind", "Commit", "Note"])
        writer.writerows(rows)

    print(f"{len(rows)} tracks: " + ", ".join(f"{k} {v}" for k, v in sorted(counts.items())))
    print(f"renames carried over: {sum(1 for r in rows if r[4].startswith('renamed'))}")
    print(f"preview written to {args.out}")

    if not args.apply:
        print("dry run: attributes file not changed (pass --apply to write)")
        return

    def replace(match):
        name, attrs = match.group(1), match.group(2)
        if name not in updated_entries:
            return match.group(0)
        return match.group(0).replace(f"<Attributes>{attrs}</Attributes>",
                                      f"<Attributes>{updated_entries[name]}</Attributes>")

    new_xml = ENTRY_RE.sub(replace, xml)
    with open(attributes_path, "w", encoding="utf-8", newline="") as fh:
        fh.write(new_xml)
    print(f"wrote {len(updated_entries)} Shuffled values to {attributes_path}")


if __name__ == "__main__":
    sys.exit(main())
