using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using Halloumi.Common.Helpers;

namespace Halloumi.BassEngine.Helpers
{
    public static class KeyHelper
    {
        private static Dictionary<string, string> _camelotCodes;
        private static Dictionary<string, string> _displayKeys;
        private static string _applicationFolder;

        static KeyHelper()
        {
            _camelotCodes = new Dictionary<string, string>();
            _camelotCodes.Add("C", "8B");
            _camelotCodes.Add("Db", "3B");
            _camelotCodes.Add("D", "10B");
            _camelotCodes.Add("Eb", "5B");
            _camelotCodes.Add("E", "12B");
            _camelotCodes.Add("F", "7B");
            _camelotCodes.Add("Gb", "2B");
            _camelotCodes.Add("G", "9B");
            _camelotCodes.Add("Ab", "4B");
            _camelotCodes.Add("A", "11B");
            _camelotCodes.Add("Bb", "6B");
            _camelotCodes.Add("B", "1B");
            _camelotCodes.Add("Cm", "5A");
            _camelotCodes.Add("Dbm", "12A");
            _camelotCodes.Add("Dm", "7A");
            _camelotCodes.Add("Ebm", "2A");
            _camelotCodes.Add("Em", "9A");
            _camelotCodes.Add("Fm", "4A");
            _camelotCodes.Add("Gbm", "11A");
            _camelotCodes.Add("Gm", "6A");
            _camelotCodes.Add("Abm", "1A");
            _camelotCodes.Add("Am", "8A");
            _camelotCodes.Add("Bm", "10A");
            _camelotCodes.Add("Bbm", "3A");

            _displayKeys = new Dictionary<string, string>();
            _displayKeys.Add("C", "C Major");
            _displayKeys.Add("Db", "D-Flat Major");
            _displayKeys.Add("D", "D Major");
            _displayKeys.Add("Eb", "E-Flat Major");
            _displayKeys.Add("E", "E Major");
            _displayKeys.Add("F", "F Major");
            _displayKeys.Add("Gb", "F-Sharp Major");
            _displayKeys.Add("G", "G Major");
            _displayKeys.Add("Ab", "A-Flat Major");
            _displayKeys.Add("A", "A Major");
            _displayKeys.Add("Bb", "B-Flat Major");
            _displayKeys.Add("B", "B Major");
            _displayKeys.Add("Cm", "C Minor");
            _displayKeys.Add("Dbm", "D-Flat Minor");
            _displayKeys.Add("Dm", "D Minor");
            _displayKeys.Add("Ebm", "E-Flat Minor");
            _displayKeys.Add("Em", "E Minor");
            _displayKeys.Add("Fm", "F Minor");
            _displayKeys.Add("Gbm", "F-Sharp Minor");
            _displayKeys.Add("Gm", "G Minor");
            _displayKeys.Add("Abm", "A-Flat Minor");
            _displayKeys.Add("Am", "A Minor");
            _displayKeys.Add("Bm", "B Minor");
            _displayKeys.Add("Bbm", "B-Flat Minor");

            _applicationFolder = @"C:\Program Files\KeyFinder-WIN";
        }

        public static List<string> GetDisplayKeys()
        {
            return _displayKeys.Select(x => x.Value).ToList();
        }

        /// <summary>
        /// Sets the application folder.
        /// </summary>
        /// <param name="applicationFolder">The application folder.</param>
        public static void SetApplicationFolder(string applicationFolder)
        {
            _applicationFolder = applicationFolder;
        }

        public static string GetCamelotCode(string key)
        {
            if (string.IsNullOrEmpty(key)) return "";
            if (!_camelotCodes.ContainsKey(key)) return "";

            return _camelotCodes[key];
        }

        public static string GetDisplayKey(string key)
        {
            if (string.IsNullOrEmpty(key)) return "";
            if (!_displayKeys.ContainsKey(key)) return "";

            return _displayKeys[key];
        }

        public static void CalculateKey(string trackFileName)
        {
            if (!File.Exists(trackFileName)) return;

            var keyFinderExe = "keyfinder.exe";
            keyFinderExe = Path.Combine(_applicationFolder, keyFinderExe);
            if (!File.Exists(keyFinderExe)) return;

            var arguments = string.Format("-f \"{0}\" -w", trackFileName);

            Process.Start(keyFinderExe, arguments);
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
            if (!_displayKeys.ContainsValue(displayKey)) return "";
            return _displayKeys.Where(x => x.Value == displayKey).FirstOrDefault().Key;
        }
    }
}