using System;
using System.Collections.Generic;
using Halloumi.Common.Helpers;
using Halloumi.Shuffler.AudioEngine.Helpers;
using Halloumi.Shuffler.AudioLibrary.Models;

namespace Halloumi.Shuffler.AudioLibrary.Helpers
{
    public static class ShufflerHelper
    {
        public static string ShufflerFolder => ExtenedAttributesHelper.ShufflerFolder;

        /// <summary>
        ///     Updates the Shuffler files after a track has been changed.
        ///     Assumes the OriginalDescription is the old description,
        ///     and that the ShufflerAttribuesFile/ShufflerMixesFile properties
        ///     point to the old files
        /// </summary>
        /// <param name="track">The track.</param>
        public static void RenameShufferFiles(Track track)
        {
            try
            {
                //var newAttributesFile = GetShufflerAttributeFile(track.Description);
                //var newMixesFile = GetShufflerMixesFile(track);

                //File.Move(track.ShufflerAttribuesFile, newAttributesFile);
                //File.Move(track.ShufflerMixesFile, newMixesFile);

                //track.ShufflerAttribuesFile = newAttributesFile;
                //track.ShufflerMixesFile = newMixesFile;

                //var replacer = new TextReplacer(track.OriginalDescription + ",", track.Description + ",", false, false,
                //    false, false);

                //replacer.Replace(ShufflerFolder, "*.Mixes.txt", false);
            }
            catch
            {
                // ignored
            }
        }

        /// <summary>
        ///     Loads the shuffler details for a track
        /// </summary>
        /// <param name="track">The track.</param>
        public static Dictionary<string, string> LoadShufflerDetails(Track track)
        {
            //if(track.Title.Contains("Escobar"))
            //    Console.WriteLine("Stop");

            track.Key = KeyHelper.ParseKey(track.Key);

            track.IsShufflerTrack = ExtenedAttributesHelper.HasExtendedAttributes(track.Description);

            if (!track.IsShufflerTrack) return null;

            var attributes = ExtenedAttributesHelper.GetExtendedAttributes(track.Description);


            if (attributes.ContainsKey("Rank")) track.Rank = ConversionHelper.ToInt(attributes["Rank"], 1);

            decimal start = 0;
            if (attributes.ContainsKey("FadeIn")) start = ConversionHelper.ToDecimal(attributes["FadeIn"], start);
            var end = track.Length;
            if (attributes.ContainsKey("FadeOut")) end = ConversionHelper.ToDecimal(attributes["FadeOut"], end);
            var length = end - start;

            var inLoopCount = 0;
            if (attributes.ContainsKey("StartLoopCount"))
                inLoopCount = ConversionHelper.ToInt(attributes["StartLoopCount"], inLoopCount);

            decimal inLoopLength = 0;
            if (attributes.ContainsKey("FadeInLengthInSeconds"))
                inLoopLength = ConversionHelper.ToDecimal(attributes["FadeInLengthInSeconds"]);
            if (inLoopLength > 0) track.StartBpm = BpmHelper.GetBpmFromLoopLength(Convert.ToDouble(inLoopLength));

            inLoopCount = inLoopCount - 1;
            if (inLoopCount > 0) length = length + inLoopCount*inLoopLength;

            decimal skipLength = 0;
            if (attributes.ContainsKey("SkipLengthInSeconds"))
                skipLength = ConversionHelper.ToDecimal(attributes["SkipLengthInSeconds"]);
            if (skipLength > 0) length = length - skipLength;

            track.PowerDown = false;
            if (attributes.ContainsKey("PowerDown"))
                track.PowerDown = ConversionHelper.ToBoolean(attributes["PowerDown"]);

            if (attributes.ContainsKey("Key")) track.Key = KeyHelper.ParseKey(attributes["Key"]);

            decimal outLoopLength = 0;
            if (attributes.ContainsKey("FadeOutLengthInSeconds"))
                outLoopLength = ConversionHelper.ToDecimal(attributes["FadeOutLengthInSeconds"], 0);
            if (outLoopLength > 0) track.EndBpm = BpmHelper.GetBpmFromLoopLength(Convert.ToDouble(outLoopLength));

            track.Length = length;

            if (attributes.ContainsKey("StartBPM"))
                track.StartBpm = BpmHelper.NormaliseBpm(ConversionHelper.ToDecimal(attributes["StartBPM"], track.Bpm));
            if (attributes.ContainsKey("EndBPM"))
                track.EndBpm = BpmHelper.NormaliseBpm(ConversionHelper.ToDecimal(attributes["EndBPM"], track.Bpm));

            track.Bpm = BpmHelper.GetAdjustedBpmAverage(track.StartBpm, track.EndBpm);

            return attributes;
        }

        //    if (attributes == null) return;
        //{
        //public static void SetAttribute(string key, string value, IDictionary<string, string> attributes)
        //    if (!attributes.ContainsKey(key))
        //        attributes.Add(key, value);
        //    else
        //        attributes[key] = value;
        //}

        //public static void SaveShufflerAttributes(Track track, Dictionary<string, string> attributes)
        //{
        //    ExtenedAttributesHelper.SaveExtendedAttributes(track.Description, attributes);
        //}

        ///// <summary>
        /////     Gets the shuffler attribute file for a track
        ///// </summary>
        ///// <param name="trackDescription">The track description.</param>
        ///// <returns>
        /////     The shuffler attribute file
        ///// </returns>
        //public static string GetShufflerAttributeFile(string trackDescription)
        //{
        //    var filename = $"{trackDescription}.ExtendedAttributes.txt";
        //    filename = FileSystemHelper.StripInvalidFileNameChars(filename);
        //    filename = Path.Combine(ShufflerFolder, filename);
        //    return filename;
        //}

        ///// <summary>
        /////     Gets the shuffler mixes file for a track
        ///// </summary>
        ///// <param name="track">The track.</param>
        ///// <returns>
        /////     The shuffler mixes file
        ///// </returns>
        //public static string GetShufflerMixesFile(Track track)
        //{
        //    var filename = $"{track.Description}.Mixes.txt";
        //    filename = FileSystemHelper.StripInvalidFileNameChars(filename);
        //    filename = Path.Combine(ShufflerFolder, filename);
        //    return filename;
        //}

        ///// <summary>
        /////     Imports the shuffler details.
        ///// </summary>
        ///// <param name="importFolder">The import folder.</param>
        ///// <param name="deleteAfterImport">If set to true, will delete Shuffler files after importing them</param>
        //public static void ImportShufflerDetails(string importFolder, bool deleteAfterImport)
        //{
        //    if (!Directory.Exists(importFolder)) return;
        //    var importFiles = FileSystemHelper.SearchFiles(importFolder,
        //        "*.ExtendedAttributes.txt;*.AutomationAttributes.xml;", false);
        //    foreach (var importFile in importFiles)
        //    {
        //        var fileName = Path.GetFileName(importFile);
        //        if (fileName == null) continue;

        //        var existingFile = Path.Combine(ShufflerFolder, fileName);
        //        if (!File.Exists(existingFile))
        //        {
        //            FileSystemHelper.Copy(importFile, existingFile);
        //        }
        //        else
        //        {
        //            var existingFileDate = File.GetLastWriteTime(existingFile);
        //            var importFileDate = File.GetLastWriteTime(importFile);

        //            if (existingFileDate < importFileDate)
        //            {
        //                FileSystemHelper.Copy(importFile, existingFile);
        //                File.SetLastWriteTime(existingFile, importFileDate);
        //            }
        //            else if (!deleteAfterImport && existingFileDate != importFileDate)
        //            {
        //                FileSystemHelper.Copy(existingFile, importFile);
        //                File.SetLastWriteTime(importFile, existingFileDate);
        //            }
        //        }
        //        if (deleteAfterImport) File.Delete(importFile);
        //    }

        //    if (!deleteAfterImport)
        //    {
        //        var existingFiles = FileSystemHelper.SearchFiles(ShufflerFolder,
        //            "*.ExtendedAttributes.txt;*.AutomationAttributes.xml;", false);
        //        foreach (var existingFile in existingFiles)
        //        {
        //            var fileName = Path.GetFileName(existingFile);
        //            if (fileName == null) continue;

        //            var importFile = Path.Combine(importFolder, fileName);

        //            if (File.Exists(importFile)) continue;

        //            var existingFileDate = File.GetLastWriteTime(existingFile);
        //            FileSystemHelper.Copy(existingFile, importFile);
        //            File.SetLastWriteTime(importFile, existingFileDate);
        //        }
        //    }


        //}
    }
}