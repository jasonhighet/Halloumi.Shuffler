<#
.SYNOPSIS
    Step 2 of the fade suggestion harness: scores the suggested fade positions against the hand-set ones.

.DESCRIPTION
    For every track cached by Build-AnalysisCache.ps1, asks BeatAnalysis for its fade-in and fade-out suggestions
    (the same calls the Shuffler details screen makes) and measures how close they come to the saved, hand-set
    sections. Prints one row per genre:

      In top1 / In top5      the best guess / any of the suggestions starts within -Tight seconds of the hand-set fade-in
      In near                any of the suggestions starts within -Near seconds
      In close               any of the suggestions starts within -Close seconds
      Out ...                the same for the fade-out, given the hand-set fade-in (as it is on the screen)

    Each genre's FadeProfile is worked out from the hand-set sections of that genre, as the application does.

    Run it in 32-bit Windows PowerShell after building the solution (Release, x86):
      C:\Windows\SysWOW64\WindowsPowerShell\v1.0\powershell.exe -ExecutionPolicy Bypass -File Test-FadeSuggestions.ps1

    Use -Every 5 for a quick run on a fifth of the tracks, -Genre to look at one genre, -Detail to save every row.
#>
param(
    [string]$CacheFolder = (Join-Path $env:LOCALAPPDATA 'Halloumi\FadeEval'),
    [int]$Count = 5,
    [int]$Every = 1,
    [string]$Genre,
    [double]$Tight = 0.15,
    [double]$Near = 1.0,
    [double]$Close = 2.5,
    [string]$Detail
)

$ErrorActionPreference = 'Stop'
if ([IntPtr]::Size -ne 4) { throw 'Run this in 32-bit PowerShell (C:\Windows\SysWOW64\WindowsPowerShell\v1.0\powershell.exe).' }

$repo = Split-Path (Split-Path $PSScriptRoot)
$bin = Join-Path $CacheFolder ("bin-test-" + $PID)
New-Item -ItemType Directory -Force $bin | Out-Null
Copy-Item (Join-Path $repo 'Halloumi.BassEngine\bin\x86\Release\net48\*.dll') $bin -Force
[Reflection.Assembly]::UnsafeLoadFrom("$bin\Bass.Net.dll") | Out-Null
[Reflection.Assembly]::UnsafeLoadFrom("$bin\Halloumi.Shuffler.AudioEngine.dll") | Out-Null
$analysisType = [Halloumi.Shuffler.AudioEngine.Helpers.BeatAnalysis]
$profileType = [Halloumi.Shuffler.AudioEngine.Helpers.FadeProfile]

function Number($text) {
    $value = 0.0
    [void][double]::TryParse($text, [Globalization.NumberStyles]::Float, [Globalization.CultureInfo]::InvariantCulture, [ref]$value)
    return $value
}

$tracks = @(Import-Csv (Join-Path $CacheFolder 'tracks.csv') | ForEach-Object {
    $_ | Add-Member -PassThru -NotePropertyMembers @{
        In = Number $_.FadeIn; InLength = Number $_.FadeInLength
        Out = Number $_.FadeOut; OutLength = Number $_.FadeOutLength; Skip = Number $_.SkipLength
    }
} | Where-Object { $_.InLength -gt 0 -and $_.OutLength -gt 0 })

# a profile for each genre, from its hand-set sections (all tracks if the genre has fewer than 15)
function New-Profile($rows) {
    $starts = New-Object 'Collections.Generic.List[double]'
    $lengths = New-Object 'Collections.Generic.List[double]'
    foreach ($r in $rows) {
        $played = $r.Out + $r.OutLength - $r.In - $r.Skip
        if ($played -le 0) { continue }
        $starts.Add($r.In); $lengths.Add($played)
    }
    return $profileType::Create($starts, $lengths)
}
$allProfile = New-Profile $tracks
$profiles = @{}
foreach ($group in ($tracks | Group-Object Genre)) {
    $profiles[$group.Name] = if ($group.Count -ge 15) { New-Profile $group.Group } else { $allProfile }
}

function Rank($candidates, [double]$truth, [double]$tolerance) {
    for ($i = 0; $i -lt $candidates.Count; $i++) {
        if ([Math]::Abs($candidates[$i].Start - $truth) -le $tolerance) { return $i }
    }
    return 99
}

$results = New-Object Collections.Generic.List[object]
$n = 0
foreach ($t in $tracks) {
    $n++
    if ($n % $Every -ne 0) { continue }
    if ($Genre -and $t.Genre -ne $Genre) { continue }

    $analysis = $analysisType::Load((Join-Path $CacheFolder "$($t.Id).bat"))
    if (-not $analysis) { continue }

    $fadeIns = $analysis.GetFadeInCandidates($profiles[$t.Genre], $Count)
    $fadeOuts = $analysis.GetFadeOutCandidates($t.In, $t.InLength, $profiles[$t.Genre], $Count)

    $nearestIn = if ($fadeIns.Count) { ($fadeIns | ForEach-Object { $_.Start - $t.In } | Sort-Object { [Math]::Abs($_) })[0] } else { [double]::NaN }
    $nearestOut = if ($fadeOuts.Count) { ($fadeOuts | ForEach-Object { $_.Start - $t.Out } | Sort-Object { [Math]::Abs($_) })[0] } else { [double]::NaN }

    $results.Add([pscustomobject]@{
        Track = $t.Track; Genre = $t.Genre
        InTight = Rank $fadeIns $t.In $Tight; InNear = Rank $fadeIns $t.In $Near; InClose = Rank $fadeIns $t.In $Close
        OutTight = Rank $fadeOuts $t.Out $Tight; OutNear = Rank $fadeOuts $t.Out $Near; OutClose = Rank $fadeOuts $t.Out $Close
        NearestIn = [Math]::Round($nearestIn, 3); NearestOut = [Math]::Round($nearestOut, 3)
        HandSetIn = $t.In; HandSetOut = $t.Out
    })
    if ($results.Count % 100 -eq 0) { Write-Host "$($results.Count) tracks..." }
}

function Share($rows, $property, $below) {
    if (-not $rows.Count) { return 0 }
    return [int][Math]::Round(100.0 * @($rows | Where-Object { $_.$property -lt $below }).Count / $rows.Count)
}

function Summary($name, $rows) {
    [pscustomobject]@{
        Genre = $name; Tracks = $rows.Count
        'In top1' = "$(Share $rows InTight 1)%"; 'In top5' = "$(Share $rows InTight $Count)%"
        'In near' = "$(Share $rows InNear $Count)%"; 'In close' = "$(Share $rows InClose $Count)%"
        'Out top1' = "$(Share $rows OutTight 1)%"; 'Out top5' = "$(Share $rows OutTight $Count)%"
        'Out near' = "$(Share $rows OutNear $Count)%"; 'Out close' = "$(Share $rows OutClose $Count)%"
    }
}

$table = @(Summary 'ALL' $results)
foreach ($group in ($results | Group-Object Genre | Sort-Object Count -Descending)) {
    if ($group.Count -ge 10) { $table += Summary $group.Name $group.Group }
}

Write-Host ""
Write-Host "top1/top5 = within ${Tight}s; near = one of $Count within ${Near}s; close = one of $Count within ${Close}s"
$table | Format-Table -AutoSize | Out-String -Width 200 | Write-Host

if ($Detail) {
    $results | Export-Csv $Detail -NoTypeInformation -Encoding UTF8
    Write-Host "Every track: $Detail"
}

Remove-Item $bin -Recurse -Force -ErrorAction SilentlyContinue
