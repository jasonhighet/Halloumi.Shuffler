using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Halloumi.Shuffler.TestHarness.AudioSplitter
{
    public static class CSVReader
    {
        public static List<SongInfo> ReadCSV(string filePath)
        {
            var songs = new List<SongInfo>();

            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,   // There is a header row in the CSV
                Delimiter = ",",           // Comma as the delimiter
            };

            using (var reader = new StreamReader(filePath))
            using (var csv = new CsvReader(reader, config))
            {
                csv.Context.RegisterClassMap<SongInfoMap>();
                songs = csv.GetRecords<SongInfo>().ToList();
            }

            // Filter out any entries with missing or invalid values or where Exported has a value
            return songs.Where(s => !string.IsNullOrEmpty(s.Title) &&
                                    !string.IsNullOrEmpty(s.Artist) &&
                                    s.Length != TimeSpan.Zero &&
                                    string.IsNullOrEmpty(s.Exported)).ToList();
        }

        // SongInfo class with properties for each column
        public class SongInfo
        {
            public string Title { get; set; }
            public string Artist { get; set; }
            public string ID { get; set; }    // Added for the ID field
            public TimeSpan Length { get; set; }
            public string Exported { get; set; } // Added for the Exported field
        }

        // Class map to map CSV columns to the SongInfo properties
        private class SongInfoMap : ClassMap<SongInfo>
        {
            public SongInfoMap()
            {
                Map(m => m.Title).Name("Title").Convert(row =>
                {
                    var title = row.Row.GetField("Title");
                    if (string.IsNullOrEmpty(title)) return string.Empty;

                    var parts = title.Split("-".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    var formattedTitle = parts[0].Trim();

                    // Append other parts with parentheses
                    for (int i = 1; i < parts.Length; i++)
                    {
                        formattedTitle += $" ({parts[i].Trim()})";
                    }

                    return formattedTitle;
                });

                Map(m => m.Artist).Name("Artist").Convert(row =>
                {
                    var artist = row.Row.GetField("Artist");
                    if (string.IsNullOrEmpty(artist)) return string.Empty;

                    // Split by comma and return the first element (Artist name)
                    return artist.Split(',').FirstOrDefault();
                });

                Map(m => m.ID).Name("ID");  // Mapping the ID column
                Map(m => m.Length).Name("Length").Convert(row =>
                {
                    var length = row.Row.GetField("Length");
                    if (string.IsNullOrEmpty(length))
                    {
                        return TimeSpan.Zero;
                    }

                    return TimeSpan.ParseExact(length, "m\\:ss", CultureInfo.InvariantCulture);
                });

                Map(m => m.Exported).Name("Exported"); // Mapping the Exported column
            }
        }
    }
}
