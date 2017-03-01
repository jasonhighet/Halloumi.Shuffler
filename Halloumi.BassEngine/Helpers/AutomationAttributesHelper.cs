using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Halloumi.Common.Helpers;
using Halloumi.Shuffler.AudioEngine.Models;

namespace Halloumi.Shuffler.AudioEngine.Helpers
{
    public class AutomationAttributesHelper
    {
        private static readonly Dictionary<string, AutomationAttributes> AllAttributes =
            new Dictionary<string, AutomationAttributes>();

        //public static void LoadAllAutomationAttributes(List<string> trackDescriptions, string folder)
        //{
        //    foreach (var trackDescription in trackDescriptions)
        //    {
        //        var filepath = GetAttributesFilePath(trackDescription);
        //        if (!File.Exists(filepath)) continue;

        //        var attributes = SerializationHelper<AutomationAttributes>.FromXmlFile(filepath);
        //        attributes.Track = trackDescription;
        //        AllAttributes.Add(trackDescription, attributes);
        //    }
        //}

        public static string ShufflerFolder { get; set; }

        public static void LoadFromDatabase()
        {
            AllAttributes.Clear();
            var filepath = AutomationAttributesFile;
            if(!File.Exists(filepath))
                return;

            var allAttributes = SerializationHelper<List<AutomationAttributes>>.FromXmlFile(filepath);
            foreach (var attributes in allAttributes)
            {
                AllAttributes.Add(attributes.Track, attributes);
            }
        }

        private static string AutomationAttributesFile => Path.Combine(ShufflerFolder, "Haloumi.Shuffler.AutomationAttributes.xml");

        public static void SaveToDatabase()
        {
            var attributes = AllAttributes.Select(x => x.Value).ToList().OrderBy(x => x.Track).ToList();
            var filepath = AutomationAttributesFile;
            SerializationHelper<List<AutomationAttributes>>.ToXmlFile(attributes, filepath);
        }

        public static AutomationAttributes GetAutomationAttributes(string trackDescription)
        {
            if (trackDescription == "") return null;
            return AllAttributes.ContainsKey(trackDescription)
                ? AllAttributes[trackDescription].Clone()
                : new AutomationAttributes { Track = trackDescription} ;
        }

        public static void SaveAutomationAttributes(string trackDescription, AutomationAttributes attributes)
        {
            if (!AllAttributes.ContainsKey(trackDescription))
                AllAttributes.Add(trackDescription, attributes);
            else
                AllAttributes[trackDescription] = attributes;

            Task.Run(() => SaveToDatabase());
        }
    }
}
