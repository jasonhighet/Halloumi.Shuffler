using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Halloumi.BassEngine;
using Halloumi.Common.Helpers;
using BE = Halloumi.BassEngine.Models;

namespace Halloumi.Shuffler.Engine
{
    public class LinkedSampleLibrary
    {
        private Dictionary<string, List<LinkedSample>> _cachedLinkedSamples = new Dictionary<string, List<LinkedSample>>();

        /// <summary>
        ///     Initializes a new instance of the LinkedSampleLibrary class.
        /// </summary>
        /// <param name="library">The library.</param>
        public LinkedSampleLibrary(Library library)
        {
            Library = library;
        }

        private Library Library { get; }

        /// <summary>
        ///     Loads the linked samples for a track.
        /// </summary>
        /// <param name="bassPlayer">The bass player.</param>
        /// <param name="track">The track.</param>
        /// <returns>
        ///     A list of samples linked to the track
        /// </returns>
        public List<BE.Sample> LoadLinkedSamples(BassPlayer bassPlayer, BE.Track track)
        {
            var samples = new List<BE.Sample>();
            if (track == null) return samples;

            var linkedSamples = LoadLinkedSamples(track.Description);

            foreach (var linkedSample in linkedSamples)
            {
                var libraryTrack = Library
                    .GetTracksByDescription(linkedSample.TrackDescription)
                    .FirstOrDefault(t => t.IsShufflerTrack);

                if (libraryTrack == null || !File.Exists(libraryTrack.Filename)) continue;

                var sampleTrack = bassPlayer.LoadTrack(libraryTrack.Filename);
                bassPlayer.LoadTrackAudioData(sampleTrack);

                var sample = bassPlayer.LoadSample(sampleTrack, linkedSample.SampleKey);
                if (sample != null) samples.Add(sample);
            }

            return samples;
        }

        /// <summary>
        ///     Links a sample to a track.
        /// </summary>
        /// <param name="track">The track.</param>
        /// <param name="sample">The sample.</param>
        public void LinkSampleToTrack(BE.Track track, BE.Sample sample)
        {
            var linkedSamples = LoadLinkedSamples(track.Description);
            linkedSamples.Add(new LinkedSample
            {
                SampleKey = sample.SampleKey,
                TrackDescription = sample.LinkedTrackDescription
            });
            SaveLinkedSamples(track.Description, linkedSamples);
        }

        /// <summary>
        ///     Unlinks a sample to from a track.
        /// </summary>
        /// <param name="track">The track.</param>
        /// <param name="sample">The sample.</param>
        public void UnlinkSampleFromTrack(BE.Track track, BE.Sample sample)
        {
            var linkedSamples = LoadLinkedSamples(track.Description);

            var linkedSample = linkedSamples
                .FirstOrDefault(ls => ls.SampleKey == sample.SampleKey
                             && ls.TrackDescription == sample.LinkedTrackDescription);

            if (linkedSample == null) return;

            linkedSamples.Remove(linkedSample);
            SaveLinkedSamples(track.Description, linkedSamples);
        }

        /// <summary>
        ///     Determines whether a sample is a linked to a track
        /// </summary>
        /// <param name="track">The track.</param>
        /// <param name="sample">The sample.</param>
        /// <returns>
        ///     True if the sample is linked to the track; otherwise, false.
        /// </returns>
        public bool IsSampleLinkedToTrack(BE.Track track, BE.Sample sample)
        {
            var linkedSamples = LoadLinkedSamples(track.Description);
            if (linkedSamples == null || linkedSamples.Count == 0) return false;

            return linkedSamples
                .Exists(ls => ls.TrackDescription == sample.LinkedTrackDescription
                              && ls.SampleKey == sample.SampleKey);
        }

        /// <summary>
        /// Imports the shuffler details.
        /// </summary>
        /// <param name="importFolder">The import folder.</param>
        /// <param name="deleteAfterImport">If set to true, delete after import.</param>
        public void ImportDetails(string importFolder, bool deleteAfterImport)
        {
            if (!Directory.Exists(importFolder)) return;

            _cachedLinkedSamples = new Dictionary<string, List<LinkedSample>>();

            var importFiles = FileSystemHelper.SearchFiles(importFolder, "*.LinkedSamples.txt", false);
            foreach (var importFile in importFiles)
            {
                var fileName = Path.GetFileName(importFile);
                if (fileName == null) continue;

                var existingFile = Path.Combine(Library.ShufflerFolder, fileName);
                if (!File.Exists(existingFile))
                {
                    FileSystemHelper.Copy(importFile, existingFile);
                }
                else
                {
                    var existingFileDate = File.GetLastWriteTime(existingFile);
                    var importFileDate = File.GetLastWriteTime(importFile);

                    if (existingFileDate < importFileDate)
                    {
                        FileSystemHelper.Copy(importFile, existingFile);
                        File.SetLastWriteTime(existingFile, importFileDate);
                    }
                    else if (!deleteAfterImport && existingFileDate != importFileDate)
                    {
                        FileSystemHelper.Copy(existingFile, importFile);
                        File.SetLastWriteTime(importFile, existingFileDate);
                    }
                }
                if (deleteAfterImport) File.Delete(importFile);
            }

            if (deleteAfterImport) return;

            var existingFiles = FileSystemHelper.SearchFiles(Library.ShufflerFolder, "*.LinkedSamples.txt", false);
            foreach (var existingFile in existingFiles)
            {
                var fileName = Path.GetFileName(existingFile);
                if (fileName == null) continue;

                var importFile = Path.Combine(importFolder, fileName);
                if (File.Exists(importFile)) continue;

                var existingFileDate = File.GetLastWriteTime(existingFile);
                FileSystemHelper.Copy(existingFile, importFile);
                File.SetLastWriteTime(importFile, existingFileDate);
            }
        }

        /// <summary>
        ///     Loads the linked samples for the supplier track.
        /// </summary>
        /// <param name="trackDescription">The track title.</param>
        /// <returns>A list of linked samples</returns>
        private List<LinkedSample> LoadLinkedSamples(string trackDescription)
        {
            if (_cachedLinkedSamples.ContainsKey(trackDescription)) return _cachedLinkedSamples[trackDescription];

            var attributeFile = GetAttributeFile(trackDescription);

            List<LinkedSample> linkedSamples;
            if (File.Exists(attributeFile))
            {
                linkedSamples = File.ReadAllLines(attributeFile)
                    .Distinct()
                    .Select(LinkedSample.FromString)
                    .OrderBy(s => s.TrackDescription)
                    .ThenBy(s => s.SampleKey)
                    .ToList();
            }
            else
            {
                linkedSamples = new List<LinkedSample>();
            }

            _cachedLinkedSamples.Add(trackDescription, linkedSamples);

            return linkedSamples;
        }

        /// <summary>
        ///     Saves the linked samples.
        /// </summary>
        /// <param name="trackDescription">The track description.</param>
        /// <param name="linkedSamples">The linked samples.</param>
        private void SaveLinkedSamples(string trackDescription, List<LinkedSample> linkedSamples)
        {
            var content = new StringBuilder();
            foreach (var linkedSample in linkedSamples)
            {
                content.AppendLine(linkedSample.ToString());
            }

            var filename = GetAttributeFile(trackDescription);
            var blank = (content.Length == 0 || content.ToString().Trim().Replace(Environment.NewLine, "") == "");

            if (blank && File.Exists(filename))
            {
                File.Delete(filename);
            }
            else if (!blank)
            {
                File.WriteAllText(filename, content.ToString(), Encoding.Unicode);
            }
        }

        /// <summary>
        ///     Gets the linked samples attribute file for a track
        /// </summary>
        /// <param name="trackDescription">The track description.</param>
        /// <returns>The linked samples attribute file</returns>
        private string GetAttributeFile(string trackDescription)
        {
            var filename = $"{trackDescription}.LinkedSamples.txt";
            filename = FileSystemHelper.StripInvalidFileNameChars(filename);
            filename = Path.Combine(Library.ShufflerFolder, filename);
            return filename;
        }
    }

    public class LinkedSample
    {
        public string TrackDescription { get; set; }

        public string SampleKey { get; set; }

        public override string ToString()
        {
            return TrackDescription + ", " + SampleKey;
        }

        public static LinkedSample FromString(string value)
        {
            if (value == "") return null;
            var linkedSample = new LinkedSample {SampleKey = "Sample1"};

            if (value.Contains(","))
            {
                var commaIndex = value.LastIndexOf(",", StringComparison.Ordinal);
                linkedSample.TrackDescription = value.Substring(0, commaIndex).Trim();

                var sampleKey = value.Substring(commaIndex + 1).Trim();
                try
                {
                    linkedSample.SampleKey = sampleKey;
                }
                catch
                {
                    // ignored
                }
            }
            else
            {
                linkedSample.TrackDescription = value;
            }

            return linkedSample;
        }
    }
}