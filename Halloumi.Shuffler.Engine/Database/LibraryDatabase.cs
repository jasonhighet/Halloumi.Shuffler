using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using Halloumi.Common.Helpers;
using Halloumi.Shuffler.AudioLibrary.Models;

namespace Halloumi.Shuffler.AudioLibrary.Database
{
    internal static class LibraryDatabase
    {
        private static string DatabasePath =>
            Path.Combine(ApplicationHelper.GetUserDataPath(), "Halloumi.Shuffler.Library.db");

        private static string ConnectionString =>
            $"Data Source={DatabasePath};Version=3;";

        private const string CreateTableSql = @"
            CREATE TABLE IF NOT EXISTS Tracks (
                Filename            TEXT PRIMARY KEY,
                Artist              TEXT NOT NULL DEFAULT '',
                AlbumArtist         TEXT NOT NULL DEFAULT '',
                Title               TEXT NOT NULL DEFAULT '',
                Album               TEXT NOT NULL DEFAULT '',
                Genre               TEXT NOT NULL DEFAULT '',
                Key                 TEXT NOT NULL DEFAULT '',
                TrackNumber         INTEGER NOT NULL DEFAULT 0,
                Length              REAL NOT NULL DEFAULT 0,
                FullLength          REAL NOT NULL DEFAULT 0,
                Bitrate             REAL NOT NULL DEFAULT 0,
                LastModified        TEXT NOT NULL DEFAULT '',
                Bpm                 REAL NOT NULL DEFAULT 0,
                StartBpm            REAL NOT NULL DEFAULT 0,
                EndBpm              REAL NOT NULL DEFAULT 0,
                Rank                INTEGER NOT NULL DEFAULT 1,
                IsShufflerTrack     INTEGER NOT NULL DEFAULT 0,
                CannotCalculateBpm  INTEGER NOT NULL DEFAULT 0,
                PowerDown           INTEGER NOT NULL DEFAULT 0,
                OriginalDescription TEXT NOT NULL DEFAULT ''
            );
            CREATE INDEX IF NOT EXISTS idx_tracks_genre          ON Tracks (Genre);
            CREATE INDEX IF NOT EXISTS idx_tracks_artist         ON Tracks (Artist);
            CREATE INDEX IF NOT EXISTS idx_tracks_albumartist    ON Tracks (AlbumArtist);
            CREATE INDEX IF NOT EXISTS idx_tracks_album          ON Tracks (Album);
            CREATE INDEX IF NOT EXISTS idx_tracks_isshuffler     ON Tracks (IsShufflerTrack);
            CREATE INDEX IF NOT EXISTS idx_tracks_rank           ON Tracks (Rank);
            CREATE INDEX IF NOT EXISTS idx_tracks_startbpm       ON Tracks (StartBpm);
            CREATE INDEX IF NOT EXISTS idx_tracks_endbpm         ON Tracks (EndBpm);";

        private const string InsertSql = @"
            INSERT INTO Tracks (
                Filename, Artist, AlbumArtist, Title, Album, Genre, Key,
                TrackNumber, Length, FullLength, Bitrate, LastModified,
                Bpm, StartBpm, EndBpm, Rank, IsShufflerTrack,
                CannotCalculateBpm, PowerDown, OriginalDescription
            ) VALUES (
                @Filename, @Artist, @AlbumArtist, @Title, @Album, @Genre, @Key,
                @TrackNumber, @Length, @FullLength, @Bitrate, @LastModified,
                @Bpm, @StartBpm, @EndBpm, @Rank, @IsShufflerTrack,
                @CannotCalculateBpm, @PowerDown, @OriginalDescription
            )";

        internal static bool Exists()
        {
            return File.Exists(DatabasePath);
        }

        internal static List<Track> LoadTracks()
        {
            EnsureSchema();
            var tracks = new List<Track>();

            using (var conn = new SQLiteConnection(ConnectionString))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT * FROM Tracks";
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                            tracks.Add(ReadTrack(reader));
                    }
                }
            }

            return tracks;
        }

        internal static void SaveTracks(IList<Track> tracks)
        {
            EnsureSchema();

            using (var conn = new SQLiteConnection(ConnectionString))
            {
                conn.Open();
                using (var transaction = conn.BeginTransaction())
                {
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = "DELETE FROM Tracks";
                        cmd.ExecuteNonQuery();
                    }

                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = InsertSql;
                        cmd.Parameters.Add(new SQLiteParameter("@Filename"));
                        cmd.Parameters.Add(new SQLiteParameter("@Artist"));
                        cmd.Parameters.Add(new SQLiteParameter("@AlbumArtist"));
                        cmd.Parameters.Add(new SQLiteParameter("@Title"));
                        cmd.Parameters.Add(new SQLiteParameter("@Album"));
                        cmd.Parameters.Add(new SQLiteParameter("@Genre"));
                        cmd.Parameters.Add(new SQLiteParameter("@Key"));
                        cmd.Parameters.Add(new SQLiteParameter("@TrackNumber"));
                        cmd.Parameters.Add(new SQLiteParameter("@Length"));
                        cmd.Parameters.Add(new SQLiteParameter("@FullLength"));
                        cmd.Parameters.Add(new SQLiteParameter("@Bitrate"));
                        cmd.Parameters.Add(new SQLiteParameter("@LastModified"));
                        cmd.Parameters.Add(new SQLiteParameter("@Bpm"));
                        cmd.Parameters.Add(new SQLiteParameter("@StartBpm"));
                        cmd.Parameters.Add(new SQLiteParameter("@EndBpm"));
                        cmd.Parameters.Add(new SQLiteParameter("@Rank"));
                        cmd.Parameters.Add(new SQLiteParameter("@IsShufflerTrack"));
                        cmd.Parameters.Add(new SQLiteParameter("@CannotCalculateBpm"));
                        cmd.Parameters.Add(new SQLiteParameter("@PowerDown"));
                        cmd.Parameters.Add(new SQLiteParameter("@OriginalDescription"));

                        foreach (var track in tracks)
                        {
                            cmd.Parameters["@Filename"].Value = track.Filename;
                            cmd.Parameters["@Artist"].Value = track.Artist;
                            cmd.Parameters["@AlbumArtist"].Value = track.AlbumArtist;
                            cmd.Parameters["@Title"].Value = track.Title;
                            cmd.Parameters["@Album"].Value = track.Album;
                            cmd.Parameters["@Genre"].Value = track.Genre;
                            cmd.Parameters["@Key"].Value = track.Key;
                            cmd.Parameters["@TrackNumber"].Value = track.TrackNumber;
                            cmd.Parameters["@Length"].Value = (double)track.Length;
                            cmd.Parameters["@FullLength"].Value = (double)track.FullLength;
                            cmd.Parameters["@Bitrate"].Value = (double)track.Bitrate;
                            cmd.Parameters["@LastModified"].Value = track.LastModified.ToString("O");
                            cmd.Parameters["@Bpm"].Value = (double)track.Bpm;
                            cmd.Parameters["@StartBpm"].Value = (double)track.StartBpm;
                            cmd.Parameters["@EndBpm"].Value = (double)track.EndBpm;
                            cmd.Parameters["@Rank"].Value = track.Rank;
                            cmd.Parameters["@IsShufflerTrack"].Value = track.IsShufflerTrack ? 1 : 0;
                            cmd.Parameters["@CannotCalculateBpm"].Value = track.CannotCalculateBpm ? 1 : 0;
                            cmd.Parameters["@PowerDown"].Value = track.PowerDown ? 1 : 0;
                            cmd.Parameters["@OriginalDescription"].Value = track.OriginalDescription ?? "";
                            cmd.ExecuteNonQuery();
                        }
                    }

                    transaction.Commit();
                }
            }
        }

        private static void EnsureSchema()
        {
            using (var conn = new SQLiteConnection(ConnectionString))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = CreateTableSql;
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private static Track ReadTrack(SQLiteDataReader r)
        {
            return new Track
            {
                Filename            = r["Filename"] as string ?? "",
                Artist              = r["Artist"] as string ?? "",
                AlbumArtist         = r["AlbumArtist"] as string ?? "",
                Title               = r["Title"] as string ?? "",
                Album               = r["Album"] as string ?? "",
                Genre               = r["Genre"] as string ?? "",
                Key                 = r["Key"] as string ?? "",
                TrackNumber         = Convert.ToInt32(r["TrackNumber"]),
                Length              = Convert.ToDecimal(r["Length"]),
                FullLength          = Convert.ToDecimal(r["FullLength"]),
                Bitrate             = Convert.ToDecimal(r["Bitrate"]),
                LastModified        = ParseDateTime(r["LastModified"] as string),
                Bpm                 = Convert.ToDecimal(r["Bpm"]),
                StartBpm            = Convert.ToDecimal(r["StartBpm"]),
                EndBpm              = Convert.ToDecimal(r["EndBpm"]),
                Rank                = Convert.ToInt32(r["Rank"]),
                IsShufflerTrack     = Convert.ToInt32(r["IsShufflerTrack"]) != 0,
                CannotCalculateBpm  = Convert.ToInt32(r["CannotCalculateBpm"]) != 0,
                PowerDown           = Convert.ToInt32(r["PowerDown"]) != 0,
                OriginalDescription = r["OriginalDescription"] as string ?? ""
            };
        }

        private static DateTime ParseDateTime(string value)
        {
            if (string.IsNullOrEmpty(value)) return DateTime.MinValue;
            return DateTime.TryParse(value, null, System.Globalization.DateTimeStyles.RoundtripKind, out var dt)
                ? dt
                : DateTime.MinValue;
        }
    }
}
