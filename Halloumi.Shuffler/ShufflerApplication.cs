using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Halloumi.Common.Helpers;
using Halloumi.Common.Windows.Forms;
using Halloumi.Common.Windows.Helpers;
using Halloumi.Shuffler.AudioEngine.BassPlayer;
using Halloumi.Shuffler.AudioEngine.Channels;
using Halloumi.Shuffler.AudioEngine.Helpers;
using Halloumi.Shuffler.AudioEngine.Midi;
using Halloumi.Shuffler.AudioEngine.Plugins;
using Halloumi.Shuffler.AudioLibrary;
using Halloumi.Shuffler.AudioLibrary.Helpers;
using Halloumi.Shuffler.AudioLibrary.Models;
using Halloumi.Shuffler.AudioLibrary.Samples;
using Halloumi.Shuffler.Forms;

namespace Halloumi.Shuffler
{
    public class ShufflerApplication
    {
        private bool _useConservativeFadeOut;

        private readonly TrackSelector _trackSelector = new TrackSelector();

        private static string PlaylistSettingsFilename
        {
            get
            {
                return Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    "Halloumi",
                    "Halloumi.Shuffler.PlaylistGenerationSettings.xml");
            }
        }

        public ShufflerApplication()
        {
            BassPlayer = new BassPlayer();
            Library = new Library(BassPlayer);

            CollectionHelper.Library = Library;


            LoopLibrary = new LoopLibrary(BassPlayer);
            LoadSettings();

            MixLibrary = new MixLibrary(Library.ShufflerFolder);
            TrackSampleLibrary = new TrackSampleLibrary(BassPlayer, Library);

            MidiManager = new MidiManager();
            MidiMapper = new BassPlayerMidiMapper(BassPlayer, MidiManager);


            //LoopLibrary = new LoopLibrary(BassPlayer, @"E:\OneDrive\Music\Samples\Hiphop\Future Loops Scratch Anthology");

            LoadFromDatabase();

            BassPlayer.OnTrackQueued += BassPlayer_OnTrackQueued;
            BassPlayer.OnTrackChange += BassPlayer_OnTrackChange;
            BassPlayer.OnSkipToEnd += BassPlayer_OnFadeEnded;
            BassPlayer.OnEndFadeIn += BassPlayer_OnFadeEnded;
        }

        /// <summary>
        /// Raised when BassPlayer changes the current track.
        /// </summary>
        public event EventHandler OnTrackChanged;

        /// <summary>
        /// Raised when a fade ends (BassPlayer.OnSkipToEnd or BassPlayer.OnEndFadeIn).
        /// </summary>
        public event EventHandler OnFadeEnded;

        private const int AutoGenerateTracksRemainingThreshold = 1;

        /// <summary>
        /// Raised when auto-generation should run because 5 or fewer tracks remain in
        /// the playlist (as reported by <see cref="PlaylistTrackCountProvider"/>).
        /// Only fires when <see cref="AutoGenerateEnabled"/> is true and
        /// <see cref="PlaylistTrackCountProvider"/> is set.
        /// </summary>
        public event EventHandler OnAutoGenerateRequired;

        /// <summary>
        /// Whether auto-generation is enabled.
        /// When true, ShufflerApplication checks the playlist track count on every fade
        /// end and fires <see cref="OnAutoGenerateRequired"/> when 5 or fewer tracks remain.
        /// </summary>
        public bool AutoGenerateEnabled { get; set; }

        /// <summary>
        /// Callback that returns the number of tracks remaining in the playlist.
        /// Set by the UI at startup so ShufflerApplication can evaluate the threshold
        /// without holding a WinForms reference.
        /// </summary>
        public Func<int> PlaylistTrackCountProvider { get; set; }

        private void BassPlayer_OnTrackChange(object sender, EventArgs e)
        {
            OnTrackChanged?.Invoke(this, EventArgs.Empty);
        }

        private void BassPlayer_OnFadeEnded(object sender, EventArgs e)
        {
            OnFadeEnded?.Invoke(this, EventArgs.Empty);
            if (!AutoGenerateEnabled) return;
            if (PlaylistTrackCountProvider == null) return;
            if (PlaylistTrackCountProvider() <= AutoGenerateTracksRemainingThreshold)
                OnAutoGenerateRequired?.Invoke(this, EventArgs.Empty);
        }

        public BaseMinimizeToTrayForm BaseForm { get; set; }

        public bool UseConservativeFadeOut
        {
            get { return _useConservativeFadeOut; }
            set
            {
                _useConservativeFadeOut = value;
                SetConservativeFadeOutSettings();
            }
        }

        public BassPlayerMidiMapper MidiMapper { get; }

        public MixLibrary MixLibrary { get; }

        public Library Library { get; }

        public TrackSampleLibrary TrackSampleLibrary { get; }

        public BassPlayer BassPlayer { get; }

        public MidiManager MidiManager { get; }
        public LoopLibrary LoopLibrary { get; internal set; }
        public bool ShuffleAfterShuffling { get; internal set; }

        private void LoadFromDatabase()
        {
            Library.LoadFromDatabase();
            TrackSampleLibrary.LoadFromCache();

            MixLibrary.AvailableTracks = Library.GetTracks();
            MixLibrary.LoadFromDatabase();

            ExtenedAttributesHelper.ShufflerFolder = Library.ShufflerFolder;
            ExtenedAttributesHelper.LoadFromDatabase();
            Library.LoadAllExtendedAttributes();

            AutomationAttributesHelper.ShufflerFolder = Library.ShufflerFolder;
            AutomationAttributesHelper.LoadFromDatabase();

            CollectionHelper.LoadFromDatabase();

            LoopLibrary.LoadFromCache();
        }

        public void Unload()
        {
            SaveSettings();
            Library.SaveToDatabase();
            MixLibrary.SaveToDatabase();
            LoopLibrary.SaveToCache();
            BassPlayer.Dispose();
            MidiManager.Dispose();
        }

        // ── Plugin load/save helpers ─────────────────────────────────────────

        /// <summary>
        /// Calls <paramref name="loader"/> with <paramref name="path"/> if the path is non-empty,
        /// swallowing any exception (plugin not found / incompatible version).
        /// </summary>
        private static void TryLoadPlugin(string path, Action<string> loader)
        {
            if (string.IsNullOrEmpty(path)) return;
            try { loader(path); } catch { /* ignored */ }
        }

        /// <summary>
        /// Loads a plugin via <paramref name="loader"/> and then applies stored
        /// <paramref name="parameters"/> to <paramref name="getPlugin"/>() when available.
        /// </summary>
        private static void TryLoadPluginWithParams(
            string path, Action<string> loader,
            string parameters, Func<VstPlugin> getPlugin)
        {
            TryLoadPlugin(path, loader);
            if (!string.IsNullOrEmpty(parameters))
            {
                var plugin = getPlugin();
                if (plugin != null)
                    try { PluginHelper.SetVstPluginParameters(plugin, parameters); } catch { /* ignored */ }
            }
        }

        /// <summary>
        /// Returns the plugin's location, or <see cref="string.Empty"/> if the plugin is null.
        /// </summary>
        private static string SavePlugin(VstPlugin plugin)
            => plugin?.Location ?? string.Empty;

        /// <summary>
        /// Returns the plugin's serialised parameter string, or <see cref="string.Empty"/> if null.
        /// </summary>
        private static string SavePluginParams(VstPlugin plugin)
            => plugin != null ? PluginHelper.GetVstPluginParameters(plugin) : string.Empty;

        // ── Settings ─────────────────────────────────────────────────────────

        /// <summary>
        ///     Loads the settings.
        /// </summary>
        public void LoadSettings()
        {
            var s = Settings.Default;
            Library.LibraryFolder = s.LibraryFolder;

            ExtenedAttributesHelper.ShufflerFolder = s.ShufflerFolder;
            PluginHelper.WaPluginsFolder            = s.WaPluginsFolder;
            PluginHelper.VstPluginsFolder           = s.VstPluginsFolder;
            BassPlayer.TrackFxAutomationEnabled     = s.EnableTrackFxAutomation;
            BassPlayer.SampleAutomationEnabled      = s.EnableSampleAutomation;
            KeyHelper.SetApplicationFolder(s.KeyFinderFolder);

            TryLoadPlugin(s.WaPlugin,                                  p => BassPlayer.LoadWaPlugin(p));
            TryLoadPluginWithParams(s.MainMixerVstPlugin,  p => BassPlayer.LoadMainVstPlugin(p, 0),        s.MainMixerVstPluginParameters,  () => BassPlayer.MainVstPlugin);
            TryLoadPluginWithParams(s.MainMixerVstPlugin2, p => BassPlayer.LoadMainVstPlugin(p, 1),        s.MainMixerVstPlugin2Parameters, () => BassPlayer.MainVstPlugin2);
            TryLoadPluginWithParams(s.SamplerVstPlugin,    p => BassPlayer.LoadSamplerVstPlugin(p, 0),    s.SamplerVstPluginParameters,    () => BassPlayer.SamplerVstPlugin);
            TryLoadPluginWithParams(s.SamplerVstPlugin2,   p => BassPlayer.LoadSamplerVstPlugin(p, 1),    s.SamplerVstPlugin2Parameters,   () => BassPlayer.SamplerVstPlugin2);
            TryLoadPluginWithParams(s.TrackVstPlugin,      p => BassPlayer.LoadTracksVstPlugin(p, 0),     s.TrackVstPluginParameters,      () => BassPlayer.TrackVstPlugin);
            TryLoadPluginWithParams(s.TrackFxvstPlugin,    p => BassPlayer.LoadTrackSendFxvstPlugin(p, 0), s.TrackFxvstPluginParameters,   () => BassPlayer.TrackSendFxVstPlugin);
            TryLoadPluginWithParams(s.TrackFxvstPlugin2,   p => BassPlayer.LoadTrackSendFxvstPlugin(p, 1), s.TrackFxvstPlugin2Parameters,  () => BassPlayer.TrackSendFxVstPlugin2);

            BassPlayer.LimitSongLength = s.LimitSongLength;
            BassPlayer.SetMonitorVolume(s.MonitorVolume);
            UseConservativeFadeOut = s.LimitSongLength;

            LoopLibrary.Initialize(s.LoopLibraryFolder);
            BassPlayer.LoopFolder = s.LoopLibraryFolder;

            ShuffleAfterShuffling = s.ShuffleAfterShuffling;
        }

        public void SaveSettings()
        {
            var s = Settings.Default;

            s.WaPlugin = BassPlayer.WaPlugin?.Location ?? string.Empty;

            s.MainMixerVstPlugin            = SavePlugin(BassPlayer.MainVstPlugin);
            s.MainMixerVstPlugin2           = SavePlugin(BassPlayer.MainVstPlugin2);
            s.MainMixerVstPluginParameters  = SavePluginParams(BassPlayer.MainVstPlugin);
            s.MainMixerVstPlugin2Parameters = SavePluginParams(BassPlayer.MainVstPlugin2);

            s.SamplerVstPlugin            = SavePlugin(BassPlayer.SamplerVstPlugin);
            s.SamplerVstPlugin2           = SavePlugin(BassPlayer.SamplerVstPlugin2);
            s.SamplerVstPluginParameters  = SavePluginParams(BassPlayer.SamplerVstPlugin);
            s.SamplerVstPlugin2Parameters = SavePluginParams(BassPlayer.SamplerVstPlugin2);

            s.TrackVstPlugin           = SavePlugin(BassPlayer.TrackVstPlugin);
            s.TrackVstPluginParameters = SavePluginParams(BassPlayer.TrackVstPlugin);

            s.TrackFxvstPlugin            = SavePlugin(BassPlayer.TrackSendFxVstPlugin);
            s.TrackFxvstPlugin2           = SavePlugin(BassPlayer.TrackSendFxVstPlugin2);
            s.TrackFxvstPluginParameters  = SavePluginParams(BassPlayer.TrackSendFxVstPlugin);
            s.TrackFxvstPlugin2Parameters = SavePluginParams(BassPlayer.TrackSendFxVstPlugin2);

            s.ShuffleAfterShuffling    = ShuffleAfterShuffling;
            s.LimitSongLength          = UseConservativeFadeOut;
            s.Volume                   = BassPlayer.GetMixerVolume();
            s.SamplerDelayNotes        = BassPlayer.SamplerDelayNotes;
            s.TrackFxDelayNotes        = BassPlayer.TrackSendFxDelayNotes;
            s.SamplerOutput            = BassPlayer.SamplerOutput;
            s.TrackOutput              = BassPlayer.TrackOutput;
            s.MonitorVolume            = Convert.ToInt32(BassPlayer.GetMonitorVolume());
            s.RawLoopOutput            = BassPlayer.RawLoopOutput;
            s.EnableTrackFxAutomation  = BassPlayer.TrackFxAutomationEnabled;
            s.EnableSampleAutomation   = BassPlayer.SampleAutomationEnabled;

            s.Save();
        }

        public List<Track> GeneratePlaylist(
            PlaylistGenerationRequest request,
            List<Track> availableTracks,
            List<Track> currentPlaylist)
        {
            Dictionary<string, Dictionary<string, Track>> excludedMixes = null;

            if (!string.IsNullOrEmpty(request.ExcludeFromPlaylistFile)
                && File.Exists(request.ExcludeFromPlaylistFile))
            {
                var excludeTracks = PlaylistHelper
                    .GetFilesInPlaylist(request.ExcludeFromPlaylistFile)
                    .Select(f => Library.GetTrackByFilename(f))
                    .Where(t => t != null)
                    .ToList();

                if (request.ExcludeMixesOnly)
                {
                    if (excludeTracks.Count > 1)
                        excludedMixes = MixLibrary.ConvertPlaylistToMixDictionary(excludeTracks);
                }
                else
                {
                    var excludeTitles = excludeTracks.Select(t => t.Title).ToList();
                    availableTracks.RemoveAll(t => excludeTitles.Contains(t.Title));
                }
            }

            return _trackSelector.GeneratePlayList(
                availableTracks,
                MixLibrary,
                currentPlaylist,
                request.BpmDirection,
                request.ApproximateLengthMinutes,
                request.AllowBearable,
                request.Strategy,
                request.UseExtendedMixes,
                excludedMixes,
                request.RestrictArtistClumping,
                request.RestrictGenreClumping,
                request.RestrictTitleClumping,
                request.ContinueMix,
                request.KeyMixStrategy,
                request.MaxTracksToAdd,
                request.Direction);
        }

        public void StopPlaylistGeneration()
        {
            _trackSelector.StopGeneratePlayList();
        }

        public void CancelPlaylistGeneration()
        {
            _trackSelector.CancelGeneratePlayList();
        }

        public string PlaylistGenerationStatus
        {
            get { return _trackSelector.GeneratePlayListStatus; }
        }

        public void SavePlaylistGenerationSettings(PlaylistGenerationRequest request)
        {
            request.AutoGenerateEnabled = AutoGenerateEnabled;

            Directory.CreateDirectory(Path.GetDirectoryName(PlaylistSettingsFilename));
            SerializationHelper<PlaylistGenerationRequest>.ToXmlFile(request, PlaylistSettingsFilename);
        }

        public PlaylistGenerationRequest LoadPlaylistGenerationSettings()
        {
            var request = GetPlaylistGenerationRequest();

            AutoGenerateEnabled = request.AutoGenerateEnabled;

            return request;
        }

        public PlaylistGenerationRequest GetPlaylistGenerationRequest()
        {
            return File.Exists(PlaylistSettingsFilename)
                ? SerializationHelper<PlaylistGenerationRequest>.FromXmlFile(PlaylistSettingsFilename)
                : PlaylistGenerationRequest.Default();
        }

        public void ResetMidi()
        {
            MidiManager.Reset();
        }

        private void SetConservativeFadeOutSettings()
        {
            if (!_useConservativeFadeOut) return;
            if (BassPlayer.CurrentTrack == null || BassPlayer.NextTrack == null) return;

            var track1 = Library.GetTrackByFilename(BassPlayer.CurrentTrack.Filename);
            var track2 = Library.GetTrackByFilename(BassPlayer.NextTrack.Filename);
            var mixRank = MixLibrary.GetMixLevel(track1, track2);
            var hasExtendedMix = MixLibrary.HasExtendedMix(track1, track2);

            if (mixRank <= 2 && !hasExtendedMix)
                BassPlayer.SetConservativeFadeOutSettings();
        }

        /// <summary>
        ///     Handles the OnTrackQueued event of the BassPlayer control.
        /// </summary>
        private void BassPlayer_OnTrackQueued(object sender, EventArgs e)
        {
            SetConservativeFadeOutSettings();
        }

        /// <summary>
        ///     Returns a filtered and sorted list of tracks.
        ///     Handles <see cref="TrackSortField.MixInCount"/> and <see cref="TrackSortField.MixOutCount"/>
        ///     sorts (which require MixLibrary) and the <see cref="TrackFilter.QueuedFilter"/> intersection
        ///     (which requires a playlist snapshot) — neither of which can be done in the Library layer.
        /// </summary>
        /// <param name="filter">Filter parameters.</param>
        /// <param name="sort">Optional sort; defaults to library default sort when null.</param>
        /// <param name="currentPlaylist">
        ///     Snapshot of the current playlist, used only when <paramref name="filter"/>.Queued is not Any.
        ///     Pass null or an empty list to skip the queued filter.
        /// </param>
        public List<Track> GetTracks(
            TrackFilter filter,
            TrackSort sort = null,
            List<Track> currentPlaylist = null)
        {
            List<Track> tracks;

            // MixInCount / MixOutCount sorts need MixLibrary — cannot be done in Library layer
            if (sort != null &&
                (sort.Field == TrackSortField.MixInCount || sort.Field == TrackSortField.MixOutCount))
            {
                tracks = Library.GetTracks(filter);
                tracks = sort.Field == TrackSortField.MixInCount
                    ? tracks.OrderByDescending(t => MixLibrary.GetMixInCount(t)).ToList()
                    : tracks.OrderByDescending(t => MixLibrary.GetMixOutCount(t)).ToList();

                if (sort.Descending) tracks.Reverse();
            }
            else
            {
                tracks = Library.GetTracks(filter, sort);
            }

            // Queued filter — intersect or exclude based on playlist membership
            if (currentPlaylist != null &&
                currentPlaylist.Count > 0 &&
                filter.Queued != TrackFilter.QueuedFilter.Any)
            {
                tracks = filter.Queued == TrackFilter.QueuedFilter.Queued
                    ? tracks.Where(t => currentPlaylist.Contains(t)).ToList()
                    : tracks.Except(currentPlaylist).ToList();
            }

            return tracks;
        }

        /// <summary>
        ///     Returns a filtered list of <see cref="MixableTrackResult"/> objects for a given parent track.
        ///     Handles: MixLibrary lookup, intersection with the candidate pool, key-rank filtering,
        ///     queued-track exclusion, deduplication, result model construction, and default sort.
        ///     Column-based re-sorting is a UI concern and is NOT done here.
        /// </summary>
        public List<MixableTrackResult> GetMixableTracks(
            Track parentTrack,
            MixableTrackFilter filter,
            IEnumerable<Track> candidateTracks,
            IEnumerable<Track> currentPlaylist = null)
        {
            if (parentTrack == null) return new List<MixableTrackResult>();
            if (filter == null) filter = new MixableTrackFilter();

            var isFrom = filter.Direction == MixableTrackFilter.TrackDirection.From;

            // Step 1 - get mixable track descriptions from MixLibrary
            var mixableDescriptions = new HashSet<string>(
                isFrom
                    ? MixLibrary.GetMixableFromTracks(parentTrack, filter.MixRankLevels).Select(t => t.Description)
                    : MixLibrary.GetMixableToTracks(parentTrack, filter.MixRankLevels).Select(t => t.Description));

            // Step 2 - intersect with candidate pool
            var candidates = (candidateTracks ?? Enumerable.Empty<Track>())
                .Where(t => mixableDescriptions.Contains(t.Description))
                .ToList();

            // Step 3 - apply key rank filter
            if (filter.MinKeyRank == 0)
                candidates = candidates.Where(t => KeyHelper.GetKeyMixRank(parentTrack.Key, t.Key) <= 1).ToList();
            else if (filter.MinKeyRank > 0)
                candidates = candidates.Where(t => KeyHelper.GetKeyMixRank(parentTrack.Key, t.Key) >= filter.MinKeyRank).ToList();

            // Step 4 - exclude queued tracks
            if (filter.ExcludeQueued && currentPlaylist != null)
            {
                var queued = new HashSet<string>(currentPlaylist.Where(t => t != null).Select(t => t.Description));
                if (queued.Count > 0)
                    candidates = candidates.Where(t => !queued.Contains(t.Description)).ToList();
            }

            // Step 5 - deduplicate, build result models
            var seen = new HashSet<string>();
            var results = new List<MixableTrackResult>();
            foreach (var track in candidates)
            {
                if (!seen.Add(track.Description)) continue;

                var mixRank = isFrom
                    ? MixLibrary.GetExtendedMixLevel(track, parentTrack)
                    : MixLibrary.GetExtendedMixLevel(parentTrack, track);

                var mixRankDescription = MixLibrary.GetRankDescription(Convert.ToInt32(Math.Floor(mixRank)));
                var hasExtended = isFrom ? MixLibrary.HasExtendedMix(parentTrack, track) : MixLibrary.HasExtendedMix(track, parentTrack);
                if (hasExtended) mixRankDescription += "*";

                results.Add(new MixableTrackResult
                {
                    Track              = track,
                    Description        = track.Description,
                    Bpm                = track.Bpm,
                    Diff               = BpmHelper.GetAbsoluteBpmPercentChange(parentTrack.EndBpm, track.StartBpm),
                    MixRank            = mixRank,
                    MixRankDescription = mixRankDescription,
                    Rank               = track.Rank,
                    RankDescription    = track.RankDescription,
                    Key                = KeyHelper.GetDisplayKey(track.Key),
                    KeyDiff            = KeyHelper.GetKeyDifference(parentTrack.Key, track.Key),
                    KeyRankDescription = KeyHelper.GetKeyMixRankDescription(track.Key, parentTrack.Key)
                });
            }

            // Step 6 - default sort
            return results
                .OrderByDescending(t => t.MixRank)
                .ThenBy(t => t.KeyDiff)
                .ThenBy(t => t.Diff)
                .ThenByDescending(t => t.Rank)
                .ThenBy(t => t.Description)
                .ToList();
        }

        // ── BassPlayer wrappers ──────────────────────────────────────────────

        public bool IsCurrentTrackNull => BassPlayer.CurrentTrack == null;

        public string GetCurrentTrackDescription() => BassPlayer.CurrentTrack?.Description;

        public string GetNextTrackDescription() => BassPlayer.NextTrack?.Description;

        public void QueueTrack(string filename) => BassPlayer.QueueTrack(filename);

        public void PreloadTrack(string filename) => BassPlayer.PreloadTrack(filename);

        public void ForcePlay(string filename) => BassPlayer.ForcePlay(filename);

        public void SkipToFadeOut() => BassPlayer.SkipToFadeOut();

        public void Play() => BassPlayer.Play();

        // ── Library wrappers ────────────────────────────────────────────────

        public void SetTrackRank(List<Track> tracks, int rank) => Library.SetRank(tracks, rank);

        public void CancelLibraryImport() => Library.CancelImport();

        public void LoadTrack(string filename) => Library.LoadTrack(filename);

        public void CalculateKeyForTrack(string filename)
        {
            KeyHelper.CalculateKey(filename);
            Library.LoadTrack(filename);
        }

        public void CalculateBpmForTrack(string filename)
        {
            BpmCalculator.CalculateBpm(filename);
            Library.LoadTrack(filename);
        }

        public List<Genre> GetGenresFromTracks(List<Track> tracks) => Library.GetGenresFromTracks(tracks);

        public List<Artist> GetAlbumArtistsFromTracks(List<Track> tracks) => Library.GetAlbumArtistsFromTracks(tracks);

        public List<Album> GetAlbumsFromTracks(List<Track> tracks) => Library.GetAlbumsFromTracks(tracks);

        public int GetLibraryTrackCount() => Library.TrackCount();

        public Track GetTrackByFilename(string filename) => Library.GetTrackByFilename(filename);

        public List<Track> GetTracksByDescription(string description) => Library.GetTracksByDescription(description);

        public Image GetAlbumCover(string albumName, List<Track> tracks = null) => Library.GetAlbumCover(albumName, tracks);

        public Track LoadNonLibraryTrack(string filename) => Library.LoadNonLibraryTrack(filename);

        public bool SaveNonLibraryTrack(Track track) => Library.SaveNonLibraryTrack(track);

        public void SetTrackAlbumCover(Track track, Image image) => Library.SetTrackAlbumCover(track, image);

        public List<Artist> GetAllArtists() => Library.GetAllArtists();

        public List<Artist> GetAllAlbumArtists() => Library.GetAllAlbumArtists();

        public List<Genre> GetAllGenres() => Library.GetAllGenres();

        public List<Album> GetAllAlbums() => Library.GetAllAlbums();

        public bool UpdateTrackDetails(Track track, string artist, string title, string album,
            string albumArtist, string genre, int trackNumber, bool updateAuxiliaryFiles)
            => Library.UpdateTrackDetails(track, artist, title, album, albumArtist, genre,
                                          trackNumber, updateAuxiliaryFiles);

        public string GetExportPlaylistFolder() => Settings.Default.ExportPlaylistFolder;

        public void SetExportPlaylistFolder(string folder)
        {
            Settings.Default.ExportPlaylistFolder = folder;
            Settings.Default.Save();
        }

        public void CopyAudioFromAnotherTrack(Track track, string sourceFilename) => Library.CopyAudioFromAnotherTrack(track, sourceFilename);

        public string GetLibraryFolder() => Library.LibraryFolder;

        public void ImportExternalShufflerTracks(string folder) => Library.ImportExternalShufflerTracks(folder);

        public void ImportAndCleanLibrary() { Library.ImportTracks(); Library.CleanLibrary(); }

        public void CleanLibrary() => Library.CleanLibrary();

        public void ImportTracks(string folder) => Library.ImportTracks(folder);

        public void RemoveShufflerDetails(Track track) => Library.RemoveShufflerDetails(track);

        public void UpdateTitle(Track track, string title, bool updateAxillaryFiles)
            => Library.UpdateTitle(track, title, updateAxillaryFiles);

        public List<Track> GetAllTracksForAlbum(string albumName)
            => Library.GetAllTracksForAlbum(albumName);

        public void UpdateAlbumArtist(string album, string newAlbumArtist)
            => Library.UpdateAlbumArtist(album, newAlbumArtist);

        public void UpdateArtist(List<Track> tracks, string newArtist)
            => Library.UpdateArtist(tracks, newArtist);

        public void RenameArtist(string oldArtist, string newArtist)
            => Library.RenameArtist(oldArtist, newArtist);

        public void UpdateGenre(List<Track> tracks, string newGenre)
            => Library.UpdateGenre(tracks, newGenre);

        public void RenameGenre(string oldGenre, string newGenre)
            => Library.RenameGenre(oldGenre, newGenre);

        public void UpdateAlbum(List<Track> tracks, string newAlbum)
            => Library.UpdateAlbum(tracks, newAlbum);

        public void RenameAlbum(string oldAlbum, string newAlbum)
            => Library.RenameAlbum(oldAlbum, newAlbum);

        public List<Track> GetDuplicateButDifferentShufflerTracks()
            => Library.GetDuplicateButDifferentShufflerTracks();

        public List<Track> GetAllShufflerTracks()
            => Library.GetTracks(shufflerFilter: Library.ShufflerFilter.ShufflerTracks);

        public List<Track> GetAllSampleTracks()
            => TrackSampleLibrary.GetAllTracks();

        // ── MixLibrary wrappers ─────────────────────────────────────────────

        public int GetMixInCount(Track track) => MixLibrary.GetMixInCount(track);

        public int GetMixOutCount(Track track) => MixLibrary.GetMixOutCount(track);

        public string GetMixRankDescription(int rank) => MixLibrary.GetRankDescription(rank);

        public MixLibrary.MixRank GetMixRankFromDescription(string description) => MixLibrary.GetRankFromDescription(description);

        public void SetMixLibraryAvailableTracks(List<Track> tracks) { MixLibrary.AvailableTracks = tracks; }

        public string GetExtendedMixDescription(Track track1, Track track2)
            => MixLibrary.GetExtendedMixDescription(track1, track2);

        public int GetMixLevel(Track track1, Track track2)
            => MixLibrary.GetMixLevel(track1, track2);

        public void SetMixLevel(Track track1, Track track2, int level)
            => MixLibrary.SetMixLevel(track1, track2, level);

        // ── TrackSampleLibrary wrappers ─────────────────────────────────────

        public void ExportMixSectionsAsSamples(Track track) => TrackSampleLibrary.ExportMixSectionsAsSamples(track);

        public void ExportShufflerTracks(
            IEnumerable<Track> tracks,
            string destinationFolder,
            bool createSubfolder,
            Func<bool> isCancelled,
            Action<Track> onTrackStarted,
            Action<Track, string, string> onTrackError)
        {
            var exportFolder = destinationFolder;

            if (createSubfolder)
            {
                exportFolder = Path.Combine(destinationFolder,
                    FileSystemHelper.StripInvalidFileNameChars("Library"));

                if (!Directory.Exists(exportFolder))
                    Directory.CreateDirectory(exportFolder);
            }

            try
            {
                FileSystemHelper.DeleteFiles(exportFolder, "*.mp3;*.jpg", true);
            }
            catch
            {
                // ignored
            }

            foreach (var track in tracks.TakeWhile(_ => !isCancelled()))
            {
                onTrackStarted(track);

                var destinationFile = track.Filename.Replace(GetLibraryFolder(), exportFolder);
                var destinationSubFolder = Path.GetDirectoryName(destinationFile) + "";

                var albumArt = Path.Combine(Path.GetDirectoryName(track.Filename) + "", "folder.jpg");
                var destinationAlbumArt = Path.Combine(destinationSubFolder, "folder.jpg");

                try
                {
                    if (!Directory.Exists(destinationSubFolder))
                        Directory.CreateDirectory(destinationSubFolder);

                    if (!File.Exists(destinationAlbumArt) && File.Exists(albumArt))
                        FileSystemHelper.Copy(albumArt, destinationAlbumArt);

                    FileSystemHelper.Copy(track.Filename, destinationFile);
                    if (isCancelled()) break;
                }
                catch (Exception exception)
                {
                    onTrackError(track, exportFolder, exception.Message);

                    if (File.Exists(destinationFile))
                    {
                        try
                        {
                            File.Delete(destinationFile);
                        }
                        catch
                        {
                            // ignored
                        }
                    }
                }
            }

            SetExportPlaylistFolder(destinationFolder);
        }

        // ── Playlist file I/O ────────────────────────────────────────────────

        private static string WorkingPlaylistFilename
        {
            get { return Path.Combine(ApplicationHelper.GetUserDataPath(), "Halloumi.Shuffler.WorkingPlaylist.xml"); }
        }

        public void SaveWorkingPlaylist(List<string> trackDescriptions)
        {
            SerializationHelper<List<string>>.ToXmlFile(trackDescriptions, WorkingPlaylistFilename);
        }

        public List<string> LoadWorkingPlaylist()
        {
            if (!File.Exists(WorkingPlaylistFilename))
                return new List<string>();

            return SerializationHelper<List<string>>
                .FromXmlFile(WorkingPlaylistFilename)
                .Select(x => Library.GetTrackByDescription(x))
                .Where(x => x != null)
                .Select(x => x.Filename)
                .ToList();
        }

        public List<Track> GetTracksInPlaylist(string playlistName)
        {
            return CollectionHelper.GetTracksInPlaylistFile(playlistName);
        }

        public void SavePlaylist(string playlistFile, List<Track> tracks)
        {
            CollectionHelper.ExportPlaylist(playlistFile, tracks);
        }

        // ── Collection helpers ───────────────────────────────────────────────

        public IEnumerable<string> GetCollectionsTracksArentIn(List<Track> tracks)
            => CollectionHelper.GetCollectionsTracksArentIn(tracks);

        public IEnumerable<string> GetCollectionsForTracks(List<Track> tracks)
            => CollectionHelper.GetCollectionsForTracks(tracks);

        public void AddTracksToCollection(string collection, List<Track> tracks)
            => CollectionHelper.AddTracksToCollection(collection, tracks);

        public void RemoveTracksFromCollection(string collection, List<Track> tracks)
            => CollectionHelper.RemoveTracksFromCollection(collection, tracks);

        // ── MixableTracks display settings ───────────────────────────────────

        public MixableTracksDisplaySettings LoadMixableTracksSettings()
        {
            var s = Settings.Default;
            return new MixableTracksDisplaySettings
            {
                RankFilterIndex    = s.MixableRankFilterIndex,
                KeyRankFilterIndex = s.MixableKeyRankFilterIndex,
                ViewIndex          = s.MixableViewIndex,
                ExcludeQueued      = s.MixableTracksExcludeQueued
            };
        }

        public void SaveMixableTracksSettings(MixableTracksDisplaySettings settings)
        {
            Settings.Default.MixableRankFilterIndex     = settings.RankFilterIndex;
            Settings.Default.MixableKeyRankFilterIndex  = settings.KeyRankFilterIndex;
            Settings.Default.MixableViewIndex           = settings.ViewIndex;
            Settings.Default.MixableTracksExcludeQueued = settings.ExcludeQueued;
        }

        // ── Settings accessors ───────────────────────────────────────────────

        public decimal GetLoopVolume() => Settings.Default.LoopVolume;
        public void SetLoopVolume(decimal volume) { Settings.Default.LoopVolume = volume; Settings.Default.Save(); }

        public decimal GetVolume() => Settings.Default.Volume;
        public void SetVolume(decimal volume) { Settings.Default.Volume = volume; Settings.Default.Save(); }

        public decimal GetTrackFxDelayNotes() => Settings.Default.TrackFxDelayNotes;
        public bool GetEnableTrackFxAutomation() => Settings.Default.EnableTrackFxAutomation;

        public string GetAnalogXScratchFolder() => Settings.Default.AnalogXScratchFolder;
        public decimal GetSamplerDelayNotes() => Settings.Default.SamplerDelayNotes;

        public SoundOutput GetRawLoopOutput() => Settings.Default.RawLoopOutput;
    }
}