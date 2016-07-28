using System.Collections.Generic;
using System.Drawing;
using Halloumi.Shuffler.AudioLibrary.Models;

namespace Halloumi.Shuffler.AudioLibrary
{
    public interface ILibrary
    {
        #region Properties

        /// <summary>
        ///     Gets or sets the folder where the mp3 files for the library are kept
        /// </summary>
        string LibraryFolder { get; set; }

        /// <summary>
        ///     Gets or sets the folder where the m3u play-list files for the library are kept
        /// </summary>
        string PlaylistFolder { get; set; }

        /// <summary>
        ///     Gets or sets the folder where the shuffler extended attribute files for the library are kept
        /// </summary>
        string ShufflerFolder { get; }

        #endregion

        #region Search

        Track GetTrack(string artist, string title, decimal length);

        /// <summary>
        ///     Gets a list of genres.
        /// </summary>
        /// <param name="searchFilter">The search filter.</param>
        /// <param name="playlistFilter">The play-list filter.</param>
        /// <param name="shufflerFilter">The shuffler filter.</param>
        /// <param name="minBpm">The min BPM.</param>
        /// <param name="maxBpm">The max BPM.</param>
        /// <param name="trackRankFilter">The track rank filter.</param>
        /// <param name="excludePlaylistFilter">The exclude play-list filter.</param>
        /// <returns>
        ///     A list of genres.
        /// </returns>
        List<Genre> GetGenres(string searchFilter,
            string playlistFilter,
            Library.ShufflerFilter shufflerFilter,
            int minBpm,
            int maxBpm,
            Library.TrackRankFilter trackRankFilter,
            string excludePlaylistFilter);

        /// <summary>
        ///     Gets a list of genres.
        /// </summary>
        /// <returns>
        ///     A list of genres.
        /// </returns>
        List<Genre> GetAllGenres();

        /// <summary>
        ///     Gets all albums.
        /// </summary>
        /// <returns>A list of all albums</returns>
        List<Album> GetAllAlbums();

        /// <summary>
        ///     Gets a list of albums.
        /// </summary>
        /// <param name="genreFilters">The genre filters.</param>
        /// <param name="albumArtistFilters">The album artist filters.</param>
        /// <param name="searchFilter">The search filter.</param>
        /// <param name="playlistFilter">The play-list filter.</param>
        /// <param name="shufflerFilter">The shuffler filter.</param>
        /// <param name="minBpm">The minimum BPM.</param>
        /// <param name="maxBpm">The maximum BPM.</param>
        /// <param name="trackRankFilter">The track rank filter.</param>
        /// <param name="excludePlaylistFilter">The exclude play-list filter.</param>
        /// <returns>
        ///     A list of albums matching the criteria.
        /// </returns>
        List<Album> GetAlbums(List<string> genreFilters,
            List<string> albumArtistFilters,
            string searchFilter,
            string playlistFilter,
            Library.ShufflerFilter shufflerFilter,
            int minBpm,
            int maxBpm,
            Library.TrackRankFilter trackRankFilter,
            string excludePlaylistFilter);

        /// <summary>
        ///     Gets all album artists.
        /// </summary>
        /// <returns> A list of all album artists.</returns>
        List<Artist> GetAllAlbumArtists();

        /// <summary>
        ///     Gets a filtered list of album artists.
        /// </summary>
        /// <param name="genreFilters">The genre filters.</param>
        /// <param name="searchFilter">The search filter.</param>
        /// <param name="playlistFilter">The play-list filter.</param>
        /// <param name="shufflerFilter">The shuffler filter.</param>
        /// <param name="minBpm">The minimum BPM.</param>
        /// <param name="maxBpm">The maximum BPM.</param>
        /// <param name="trackRankFilter">The track rank filter.</param>
        /// <param name="excludePlaylistFilter">The exclude play-list filter.</param>
        /// <returns>
        ///     A list of album artists matching the criteria.
        /// </returns>
        List<Artist> GetAlbumArtists(List<string> genreFilters,
            string searchFilter,
            string playlistFilter,
            Library.ShufflerFilter shufflerFilter,
            int minBpm,
            int maxBpm,
            Library.TrackRankFilter trackRankFilter,
            string excludePlaylistFilter);

        /// <summary>
        ///     Gets all tracks for an album.
        /// </summary>
        /// <param name="albumName">Name of the album.</param>
        /// <returns>A list of tracks</returns>
        List<Track> GetAllTracksForAlbum(string albumName);

        /// <summary>
        ///     Gets a filtered list of artists.
        /// </summary>
        /// <param name="genreFilter">The genre filter.</param>
        /// <param name="searchFilter">The search filter.</param>
        /// <param name="playlistFilter">The play-list filter.</param>
        /// <param name="shufflerFilter">The shuffler filter.</param>
        /// <param name="minBpm">The minimum BPM.</param>
        /// <param name="maxBpm">The maximum BPM.</param>
        /// <param name="trackRankFilter">The track rank filter.</param>
        /// <param name="excludePlaylistFilter">The exclude play-list filter.</param>
        /// <returns>
        ///     A list of artists matching the criteria.
        /// </returns>
        List<Artist> GetArtists(string genreFilter,
            string searchFilter,
            string playlistFilter,
            Library.ShufflerFilter shufflerFilter,
            int minBpm,
            int maxBpm,
            Library.TrackRankFilter trackRankFilter,
            string excludePlaylistFilter);

        /// <summary>
        ///     Gets a list of all artists.
        /// </summary>
        /// <returns>
        ///     A list of all artists
        /// </returns>
        List<Artist> GetAllArtists();

        /// <summary>
        ///     Gets a list of tracks.
        /// </summary>
        /// <returns>
        ///     A list tracks
        /// </returns>
        List<Track> GetTracks();

        /// <summary>
        ///     Gets a filtered list of tracks.
        /// </summary>
        /// <param name="genreFilters">The genre filters.</param>
        /// <param name="artistFilters">The artist filters.</param>
        /// <param name="albumFilters">The album filters.</param>
        /// <param name="searchFilter">The search filter.</param>
        /// <param name="playlistFilter">The play-list filter.</param>
        /// <param name="shufflerFilter">The shuffler filter.</param>
        /// <param name="minBpm">The minimum BPM.</param>
        /// <param name="maxBpm">The maximum BPM.</param>
        /// <param name="trackRankFilter">The track rank filter.</param>
        /// <param name="excludePlaylistFilter">The exclude play-list filter.</param>
        /// <returns>
        ///     A list tracks matching the criteria.
        /// </returns>
        List<Track> GetTracks(List<string> genreFilters,
            List<string> artistFilters,
            List<string> albumFilters,
            string searchFilter,
            string playlistFilter,
            Library.ShufflerFilter shufflerFilter,
            int minBpm,
            int maxBpm,
            Library.TrackRankFilter trackRankFilter,
            string excludePlaylistFilter);

        /// <summary>
        ///     Gets a filtered list of tracks.
        /// </summary>
        /// <param name="genreFilter">The genre filter.</param>
        /// <param name="artistFilter">The artist filter.</param>
        /// <param name="albumArtistFilter">The album artist filter.</param>
        /// <param name="albumFilter">The album filter.</param>
        /// <param name="searchFilter">The search filter.</param>
        /// <param name="playlistFilter">The play-list filter.</param>
        /// <param name="shufflerFilter">The shuffler filter.</param>
        /// <param name="minBpm">The minimum BPM.</param>
        /// <param name="maxBpm">The maximum BPM.</param>
        /// <param name="trackRankFilter">The track rank filter.</param>
        /// <param name="excludePlaylistFilter">The exclude play-list filter.</param>
        /// <returns>
        ///     A list tracks matching the criteria.
        /// </returns>
        List<Track> GetTracks(string genreFilter,
            string artistFilter,
            string albumArtistFilter,
            string albumFilter,
            string searchFilter,
            string playlistFilter,
            Library.ShufflerFilter shufflerFilter,
            int minBpm,
            int maxBpm,
            Library.TrackRankFilter trackRankFilter,
            string excludePlaylistFilter);

        /// <summary>
        ///     Gets the tracks by description.
        /// </summary>
        /// <param name="description">The description.</param>
        /// <returns>
        ///     A list of tracks matching the description
        /// </returns>
        List<Track> GetTracksByDescription(string description);

        /// <summary>
        ///     Gets a track from the library matching the specified filename.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <returns>The associated track, or null if it doesn't exist</returns>
        Track GetTrackByFilename(string filename);

        #endregion

        #region Album Covers

        /// <summary>
        ///     Gets a album cover.
        /// </summary>
        /// <param name="albumName">Name of the album.</param>
        /// <returns>The album cover</returns>
        Image GetAlbumCover(string albumName);

        /// <summary>
        ///     Sets the track album cover.
        /// </summary>
        /// <param name="track">The track.</param>
        /// <param name="image">The image.</param>
        void SetTrackAlbumCover(Track track, Image image);

        #endregion

        #region Update Library

        /// <summary>
        ///     Loads the library from the cache.
        /// </summary>
        void LoadFromDatabase();

        /// <summary>
        ///     Imports the tracks.
        /// </summary>
        /// <param name="folder">The folder.</param>
        void ImportTracks(string folder);

        /// <summary>
        ///     Imports the details of all tracks from the library folder into the library
        /// </summary>
        void ImportTracks();

        /// <summary>
        ///     Cancels the import.
        /// </summary>
        void CancelImport();

        /// <summary>
        ///     Saves the track details to a cache file
        /// </summary>
        void SaveToDatabase();

        /// <summary>
        ///     Cleans the folder images.
        /// </summary>
        void CleanLibrary();

        /// <summary>
        ///     Imports the shuffler details.
        /// </summary>
        /// <param name="importFolder">The import folder.</param>
        /// <param name="deleteAfterImport">If set to true, will delete Shuffler files after importing them</param>
        void ImportShufflerDetails(string importFolder, bool deleteAfterImport);

        #endregion

        #region Update track

        /// <summary>
        ///     Reloads a track.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <returns></returns>
        Track LoadTrack(string filename);

        void SaveRank(Track track);

        /// <summary>
        ///     Loads the track details from the tags in the associate mp3
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <returns></returns>
        Track LoadNonLibraryTrack(string filename);

        bool SaveNonLibraryTrack(Track track);

        /// <summary>
        ///     Saves a track.
        /// </summary>
        /// <param name="track">The track.</param>
        /// <param name="updateAxillaryFiles">If set to true, update axillary files.</param>
        /// <returns></returns>
        bool SaveTrack(Track track, bool updateAxillaryFiles = true);

        /// <summary>
        ///     Removes the shuffler details for a track
        /// </summary>
        /// <param name="track">The track.</param>
        void RemoveShufflerDetails(Track track);

        void CopyAudioFromAnotherTrack(Track destinationTrack, Track sourceTrack);

        /// <summary>
        ///     Updates the track title
        /// </summary>
        /// <param name="track">The track.</param>
        /// <param name="artist">The artist.</param>
        /// <param name="title">The title.</param>
        /// <param name="album">The album.</param>
        /// <param name="albumArtist">The album artist.</param>
        /// <param name="genre">The genre.</param>
        /// <param name="trackNumber">The title.</param>
        /// <param name="updateAxillaryFiles">If set to true, will update axillary files.</param>
        /// <returns></returns>
        bool UpdateTrackDetails(Track track, string artist, string title, string album, string albumArtist,
            string genre, int trackNumber, bool updateAxillaryFiles);

        #endregion

        #region Update or Rename

        /// <summary>
        ///     Renames a genre.
        /// </summary>
        /// <param name="oldGenre">The old genre name.</param>
        /// <param name="newGenre">The new genre name.</param>
        void RenameGenre(string oldGenre, string newGenre);

        /// <summary>
        ///     Updates the genre for a list of tracks
        /// </summary>
        /// <param name="tracks">The tracks.</param>
        /// <param name="newGenre">The new genre name.</param>
        void UpdateGenre(List<Track> tracks, string newGenre);

        /// <summary>
        ///     Renames an album.
        /// </summary>
        /// <param name="oldAlbum">The old album name.</param>
        /// <param name="newAlbum">The new album name.</param>
        void RenameAlbum(string oldAlbum, string newAlbum);

        /// <summary>
        ///     Updates the album for a list of tracks
        /// </summary>
        /// <param name="tracks">The tracks.</param>
        /// <param name="newAlbum">The new album name.</param>
        void UpdateAlbum(List<Track> tracks, string newAlbum);

        /// <summary>
        ///     Renames an artist.
        /// </summary>
        /// <param name="oldArtist">The old artist name.</param>
        /// <param name="newArtist">The new artist name.</param>
        void RenameArtist(string oldArtist, string newArtist);

        /// <summary>
        ///     Updates the artist for a list of tracks
        /// </summary>
        /// <param name="tracks">The tracks.</param>
        /// <param name="newArtist">The new artist name.</param>
        void UpdateArtist(List<Track> tracks, string newArtist);

        /// <summary>
        ///     Updates the album artist for a list of tracks
        /// </summary>
        /// <param name="album">The album.</param>
        /// <param name="newAlbumArtist">The new album name.</param>
        void UpdateAlbumArtist(string album, string newAlbumArtist);

        /// <summary>
        ///     Updates the track number.
        /// </summary>
        /// <param name="track">The track.</param>
        /// <param name="trackNumber">The track number.</param>
        void UpdateTrackNumber(Track track, int trackNumber);

        /// <summary>
        ///     Updates the track title
        /// </summary>
        /// <param name="track">The track.</param>
        /// <param name="title">The title.</param>
        /// <param name="updateAxillaryFiles">If set to true, will update axillary files.</param>
        void UpdateTitle(Track track, string title, bool updateAxillaryFiles);

        #endregion

        #region Play-lists

        /// <summary>
        ///     Gets all play-lists.
        /// </summary>
        /// <returns>A list of all play-lists</returns>
        List<Playlist> GetAllPlaylists();

        /// <summary>
        ///     Gets a play-list by name
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>
        ///     A play-list matching the specified name, or null if not found
        /// </returns>
        Playlist GetPlaylistByName(string name);

        /// <summary>
        ///     Loads all the play-lists in the play-list folder
        /// </summary>
        void LoadPlaylists();

        /// <summary>
        ///     Adds tracks to play-list.
        /// </summary>
        /// <param name="playlist">The play-list.</param>
        /// <param name="tracks">The tracks.</param>
        void AddTracksToPlaylist(Playlist playlist, List<Track> tracks);

        /// <summary>
        ///     Removes tracks from a play-list.
        /// </summary>
        /// <param name="playlist">The play-list.</param>
        /// <param name="tracks">The tracks to remove.</param>
        void RemoveTracksFromPlaylist(Playlist playlist, List<Track> tracks);

        /// <summary>
        ///     Removes a track from all play-lists.
        /// </summary>
        /// <param name="track">The track.</param>
        void RemoveTrackFromAllPlaylists(Track track);

        /// <summary>
        ///     Gets all the play-lists that contain a specific track.
        /// </summary>
        /// <param name="track">The track.</param>
        /// <returns>A list of play-lists that contain the track</returns>
        List<Playlist> GetPlaylistsForTrack(Track track);

        /// <summary>
        ///     Gets a distinct list of all play-lists that the specified tracks are in
        /// </summary>
        /// <param name="tracks">The tracks.</param>
        /// <returns>A distinct list of all play-lists that the specified tracks are in.</returns>
        List<Playlist> GetPlaylistsForTracks(List<Track> tracks);

        /// <summary>
        ///     Creates the new play-list.
        /// </summary>
        /// <param name="playlistName">Name of the play-list.</param>
        /// <returns></returns>
        Playlist CreateNewPlaylist(string playlistName);

        /// <summary>
        ///     Loads a play-list.
        /// </summary>
        /// <param name="playlistFile">The play-list file.</param>
        /// <returns>A play-list object</returns>
        Playlist LoadPlaylist(string playlistFile);

        /// <summary>
        ///     Saves the play-list.
        /// </summary>
        /// <param name="playlist">The play-list.</param>
        void SavePlaylist(Playlist playlist);

#endregion
    }
}