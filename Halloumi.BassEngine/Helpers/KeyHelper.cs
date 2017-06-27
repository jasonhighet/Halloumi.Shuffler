using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using Halloumi.Common.Helpers;

namespace Halloumi.Shuffler.AudioEngine.Helpers
{
    public static class KeyHelper
    {
        private static readonly Dictionary<string, string> CamelotCodes;
        private static readonly Dictionary<string, string> DisplayKeys;
        private static string _applicationFolder;

        static KeyHelper()
        {
            CamelotCodes = new Dictionary<string, string>
            {
                {"C", "8B"},
                {"Db", "3B"},
                {"D", "10B"},
                {"Eb", "5B"},
                {"E", "12B"},
                {"F", "7B"},
                {"Gb", "2B"},
                {"G", "9B"},
                {"Ab", "4B"},
                {"A", "11B"},
                {"Bb", "6B"},
                {"B", "1B"},
                {"Cm", "5A"},
                {"Dbm", "12A"},
                {"Dm", "7A"},
                {"Ebm", "2A"},
                {"Em", "9A"},
                {"Fm", "4A"},
                {"Gbm", "11A"},
                {"Gm", "6A"},
                {"Abm", "1A"},
                {"Am", "8A"},
                {"Bm", "10A"},
                {"Bbm", "3A"}
            };

            DisplayKeys = new Dictionary<string, string>
            {
                {"C", "C Major"},
                {"Db", "D-Flat Major"},
                {"D", "D Major"},
                {"Eb", "E-Flat Major"},
                {"E", "E Major"},
                {"F", "F Major"},
                {"Gb", "F-Sharp Major"},
                {"G", "G Major"},
                {"Ab", "A-Flat Major"},
                {"A", "A Major"},
                {"Bb", "B-Flat Major"},
                {"B", "B Major"},
                {"Cm", "C Minor"},
                {"Dbm", "D-Flat Minor"},
                {"Dm", "D Minor"},
                {"Ebm", "E-Flat Minor"},
                {"Em", "E Minor"},
                {"Fm", "F Minor"},
                {"Gbm", "F-Sharp Minor"},
                {"Gm", "G Minor"},
                {"Abm", "A-Flat Minor"},
                {"Am", "A Minor"},
                {"Bm", "B Minor"},
                {"Bbm", "B-Flat Minor"}
            };

            _applicationFolder = @"C:\Program Files\KeyFinder-WIN";
        }

        public static List<string> GetDisplayKeys()
        {
            return DisplayKeys.Select(x => x.Value).ToList();
        }

        public static string ParseKey(string key)
        {
            key = key.Replace(" ", "");
            key = key.Replace("#", "b");
            key = key.Replace("maj", "");
            key = key.Replace("min", "m");
            key = key.Replace("M", "m");

            return key.Replace("#", "b").Replace(" ", "").Replace("M", "m");
        }

        /// <summary>
        ///     Sets the application folder.
        /// </summary>
        /// <param name="applicationFolder">The application folder.</param>
        public static void SetApplicationFolder(string applicationFolder)
        {
            _applicationFolder = applicationFolder;
        }

        public static string GetCamelotCode(string key)
        {
            if (string.IsNullOrEmpty(key)) return "";
            return !CamelotCodes.ContainsKey(key) ? "" : CamelotCodes[key];
        }

        public static string GetDisplayKey(string key)
        {
            if (string.IsNullOrEmpty(key)) return "";
            return !DisplayKeys.ContainsKey(key) ? "" : DisplayKeys[key];
        }

        public static void CalculateKey(string trackFileName)
        {
            if (!File.Exists(trackFileName)) return;

            var keyFinderExe = "keyfinder.exe";
            keyFinderExe = Path.Combine(_applicationFolder, keyFinderExe);
            if (!File.Exists(keyFinderExe)) return;

            var arguments = $"-f \"{trackFileName}\" -w";

            var process = Process.Start(keyFinderExe, arguments);

            if (process != null)
                process.WaitForExit();
            else
                Thread.Sleep(2000);

            File.SetLastWriteTime(trackFileName, DateTime.Now);
        }

        public static int GetKeyDifference(string key1, string key2)
        {
            if (key1 == "" || key2 == "") return 4;

            var code1 = GetCamelotCode(key1);
            var code2 = GetCamelotCode(key2);

            var number1 = GetCamelotNumber(code1);
            var number2 = GetCamelotNumber(code2);

            var difference = Math.Abs(number1 - number2);
            if (difference > 6) difference = 12 - difference;

            if (GetCamelotCharacter(code1) != GetCamelotCharacter(code2))
                difference++;

            return difference;
        }

        public static int GetKeyMixRank(string key1, string key2)
        {
            var rank = GetKeyDifference(key1, key2);
            rank = 5 - rank;
            if (rank < 0) rank = 0;
            return rank;
        }

        public static string GetKeyMixRankDescription(string key1, string key2)
        {
            if (key1 == "" || key2 == "") return "";
            var rank = GetKeyMixRank(key1, key2);
            if (rank == 5) return "Excellent";
            if (rank == 4) return "Very Good";
            if (rank == 3) return "Good";
            if (rank == 2) return "Bearable";
            return "Not Good";
        }

        private static int GetCamelotNumber(string camelotCode)
        {
            return ConversionHelper.ToInt(StringHelper.GetNumericCharactersOnly(camelotCode));
        }

        private static string GetCamelotCharacter(string camelotCode)
        {
            return StringHelper.GetAlphabeticCharactersOnly(camelotCode);
        }

        public static string GetKeyFromDisplayKey(string displayKey)
        {
            return !DisplayKeys.ContainsValue(displayKey)
                ? ""
                : DisplayKeys.FirstOrDefault(x => x.Value == displayKey).Key;
        }
    }
}