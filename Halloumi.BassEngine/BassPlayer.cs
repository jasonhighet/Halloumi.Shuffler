using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using Halloumi.BassEngine.Channels;
using Halloumi.Common.Helpers;
using Un4seen.Bass;
using Un4seen.Bass.AddOn.Fx;
using Un4seen.Bass.AddOn.Mix;
using Un4seen.Bass.AddOn.Tags;

namespace Halloumi.BassEngine
{
    /// <summary>
    /// Track mixing engine utilizing the Bass.Net audio engine
    /// </summary>
    public partial class BassPlayer : IDisposable, IBmpProvider
    {
        #region Private Variables

        private MixerChannel _trackMixer;

        private MixerChannel _trackSendMixer;

        private MixerChannel _trackSendFxMixer;

        private SpeakerOutputChannel _speakerOutput;

        private MonitorOutputChannel _monitorOutput;

        private OutputSplitter _trackOutputSplitter;

        /// <summary>
        /// A collection of all loaded tracks
        /// </summary>
        private static List<Track> _cachedTracks = new List<Track>();

        /// <summary>
        /// The next available track Id
        /// </summary>
        private static int _nextTrackId = 0;

        private bool _locked = false;

        private static IntPtr _windowHandle = IntPtr.Zero;

        private static List<Track> _recentTracks = new List<Track>();

        private static object _mixerLock = new object();

        #endregion

        #region Events

        /// <summary>
        /// Event raised when the currently playing track changes
        /// </summary>
        public event EventHandler OnTrackChange;

        /// <summary>
        /// Event raised when a track is queued
        /// </summary>
        public event EventHandler OnTrackQueued;

        /// <summary>
        /// Event raised when the fade in on the current track ends
        /// </summary>
        public event EventHandler OnEndFadeIn;

        /// <summary>
        /// Event raised when the fade in on the current track ends
        /// </summary>
        public event EventHandler OnSkipToEnd;

        public event EventHandler TrackTagsLoaded;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the BassPlayer class.
        /// </summary>
        public BassPlayer()
            : this(IntPtr.Zero)
        { }

        /// <summary>
        /// Initializes a new instance of the BassPlayer class.
        /// </summary>
        public BassPlayer(IntPtr windowHandle)
        {
            this.LimitSongLength = false;
            this.MaxSongLength = 5 * 60;            // 5 mins (if LimitSongLength set to true)
            this.DefaultFadeLength = 10;            // 10 seconds
            this.DefaultFadeInStartVolume = 50;     // 50 %
            this.DefaultFadeInEndVolume = 100;      // 100%
            this.DefaultFadeOutStartVolume = 80;   // 100%
            this.DefaultFadeOutEndVolume = 0;       // 0%

            this.TrackFxAutomationEnabled = false;

            BassNet.Registration("jason.highet@gmail.com", "2X1931822152222");

            // start audio engine
            StartAudioEngine(windowHandle);

            this._speakerOutput = new SpeakerOutputChannel();
            this._monitorOutput = new MonitorOutputChannel();

            this.PlayState = PlayState.Stopped;

            InitialiseTrackMixer();
            InitialiseSampler();
            InitialiseRawLoopMixer();
            InitialiseManualMixer();

            this.ExtendedAttributeFolder = "";

            if (string.IsNullOrEmpty(this.WaPluginsFolder)) this.WaPluginsFolder = @"C:\Program Files\Winamp\Plugins\";
            if (string.IsNullOrEmpty(this.VstPluginsFolder)) this.VstPluginsFolder = @"C:\Program Files\Steinberg\VstPlugins\";
        }

        /// <summary>
        /// Initialises the track mixer.
        /// </summary>
        private void InitialiseTrackMixer()
        {
            DebugHelper.WriteLine("InitialiseTrackMixer");

            // create mixer channel
            this._trackMixer = new MixerChannel(this, MixerChannelOutputType.MultipleOutputs);
            this._trackOutputSplitter = new OutputSplitter(this._trackMixer, this._speakerOutput, this._monitorOutput);

            // create clone of mixer channel and mute it
            this._trackSendMixer = new MixerChannel(this, MixerChannelOutputType.SingleOutput);
            this._trackSendMixer.AddInputChannel(this._trackMixer);
            this._trackSendMixer.SetVolume(0);

            // add the track FX to the track FX mixer,
            this._trackSendFxMixer = new MixerChannel(this, MixerChannelOutputType.SingleOutput);
            this._trackSendFxMixer.AddInputChannel(this._trackSendMixer);
            this._trackSendFxMixer.CutBass();

            // then that to the main mixer
            this._speakerOutput.AddInputChannel(this._trackSendFxMixer);

            DebugHelper.WriteLine("END InitialiseTrackMixer");
        }

        /// <summary>
        /// Deconstructor - performs application-defined tasks associated
        /// with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            DebugHelper.WriteLine("Destroying Bass Engine");

            this.Stop();

            UnloadAllWaPlugins();
            UnloadAllVstPlugins();

            // unload all tracks
            foreach (var track in _cachedTracks)
            {
                UnloadTrackAudioData(track);
            }
            _cachedTracks.Clear();

            StopAudioEngine();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the mixer lock.
        /// </summary>
        public object MixerLock { get { return _mixerLock; } }

        /// <summary>
        /// Gets the current track.
        /// </summary>
        public Track CurrentTrack
        {
            get;
            private set;
        }

        public int MixerChanel { get { return this._speakerOutput.InternalChannel; } }

        /// <summary>
        /// Gets the preloaded track.
        /// </summary>
        public Track PreloadedTrack
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the track queued to play next.
        /// </summary>
        public Track NextTrack
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the last track that was previously played.
        /// </summary>
        public Track PreviousTrack
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the active track - usually the current, unless the current
        /// has finished playing and there was no track queued to play next.
        /// </summary>
        public Track ActiveTrack
        {
            get
            {
                if (this.CurrentTrack != null)
                {
                    return this.CurrentTrack;
                }
                else
                {
                    return this.PreviousTrack;
                }
            }
        }

        /// <summary>
        /// Gets the state of the play - playing, paused, or stopped.
        /// </summary>
        public PlayState PlayState
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets the volume of the bass player as decimal 0 - 99.99.
        /// </summary>
        public decimal Volume
        {
            get
            {
                return (decimal)(Bass.BASS_GetVolume() * 100);
            }
            set
            {
                if (value >= 0 && value < 100)
                {
                    Bass.BASS_SetVolume((float)(value / 100));
                }
            }
        }

        /// <summary>
        /// Gets or sets the maximun length of songs (in seconds)
        /// </summary>
        public double MaxSongLength { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether song lengths should be limited
        /// </summary>
        public bool LimitSongLength { get; set; }

        /// <summary>
        /// Gets or sets the default cross fade length (in seconds)
        /// </summary>
        public double DefaultFadeLength { get; set; }

        /// <summary>
        /// Gets or sets the default fade-in start volume.
        /// </summary>
        public double DefaultFadeInStartVolume { get; set; }

        /// <summary>
        /// Gets or sets the default fade-in end volume.
        /// </summary>
        public double DefaultFadeInEndVolume { get; set; }

        /// <summary>
        /// Gets or sets the default fade-out start volume.
        /// </summary>
        public double DefaultFadeOutStartVolume { get; set; }

        /// <summary>
        /// Gets or sets the default fade-out end volume.
        /// </summary>
        public double DefaultFadeOutEndVolume { get; set; }

        /// <summary>
        /// Gets or sets the track extended attribute folder.
        /// </summary>
        public string ExtendedAttributeFolder { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Sets the previous track.
        /// </summary>
        /// <param name="track">The track.</param>
        public void SetPreviousTrack(string filename)
        {
            var track = LoadTrack(filename);
            this.PreviousTrack = track;
        }

        /// <summary>
        /// Enques a track for playing.
        /// </summary>
        /// <param name="filename">The filename of the track to play.</param>
        /// <returns>A track object for the specified file</returns>
        public Track QueueTrack(string filename)
        {
            var track = LoadTrack(filename);
            this.QueueTrack(track);
            return track;
        }

        public void PreloadTrack(string filename)
        {
            var track = LoadTrack(filename);

            if (track == null) return;
            if (BassHelper.IsSameTrack(track, this.PreloadedTrack)) return;
            if (track.IsAudioLoaded()) return;

            // load audio data if not loaded

            WaitForLock();
            Lock();

            DebugHelper.WriteLine("Preloading track " + track.Description);
            this.PreloadedTrack = track;

            LoadTrackAudioData(track);
            LoadSamples(track);

            DebugHelper.WriteLine("Preloading track " + track.Description + "...Done");

            Unlock();
        }

        /// <summary>
        /// Enques a track for playing as the next track, or current track if there is no current one.
        /// </summary>
        /// <param name="track">The track to enque.</param>
        public void QueueTrack(Track track)
        {
            if (track == null) return;

            DebugHelper.WriteLine("Enqueing track " + track.Description);

            this.IsForceFadeNowMode = false;

            AddTrackToMixer(track);

            if (this.CurrentTrack == null)
            {
                this.CurrentTrack = track;
                StopSamples();
                RaiseOnTrackChange();
            }
            else
            {
                var oldNextTrack = this.NextTrack;
                this.NextTrack = track;
                SetTrackSyncPositions();
                if (!this.IsTrackInUse(oldNextTrack)) this.RemoveTrackFromMixer(oldNextTrack);
            }

            RaiseOnTrackQueued();
        }

        /// <summary>
        /// Clears the next track.
        /// </summary>
        public void ClearNextTrack()
        {
            if (this.NextTrack == null) return;
            var oldNextTrack = this.NextTrack;
            this.NextTrack = null;
            if (!this.IsTrackInUse(oldNextTrack)) this.RemoveTrackFromMixer(oldNextTrack);

            this.SetTrackSyncPositions(this.CurrentTrack);
        }

        public bool IsTrackDataEntered(Track track)
        {
            return (track.FadeInStart != 0);
        }

        /// <summary>
        /// Loads the details and meta data about a track .
        /// </summary>
        /// <param name="filename">The filename of the track to load.</param>
        /// <returns>A track object</returns>
        public Track LoadTrack(string filename)
        {
            return LoadTrack(filename, "", "");
        }

        public Track LoadTrackAndAudio(string filename)
        {
            var track = this.LoadTrack(filename);

            if (!track.IsAudioLoaded())
            {
                this.LoadTrackAudioData(track);
                this.LoadExtendedAttributes(track);
            }

            return track;
        }

        /// <summary>
        /// Loads the details and meta data about a track .
        /// </summary>
        /// <param name="filename">The filename of the track to load.</param>
        /// <param name="artist">The artist of the track.</param>
        /// <param name="title">The title of the track.</param>
        /// <returns>
        /// A track object
        /// </returns>
        public Track LoadTrack(string filename, string artist, string title)
        {
            if (_cachedTracks.Exists(t => t.Filename.ToLower() == filename.ToLower()))
            {
                return _cachedTracks.Where(t => t.Filename.ToLower() == filename.ToLower()).FirstOrDefault();
            }

            DebugHelper.WriteLine("Loading track " + filename);

            if (!File.Exists(filename)) throw new Exception("Cannot find file " + filename);

            var track = new Track();
            track.Id = _nextTrackId++;
            track.Filename = filename;

            SetArtistAndTitle(track, title, artist);

            LoadExtendedAttributes(track);

            _cachedTracks.Add(track);

            LoadTagData(track);

            return track;
        }

        /// <summary>
        /// Loads tag data for all tracks with unloaded tag data
        /// </summary>
        private void LoadAllTagData()
        {
            if (_loadingTagData)
            {
                return;
            }

            _loadingTagData = true;

            var trackCount = _cachedTracks.Count;
            var tagLoaded = false;

            for (var i = 0; i < trackCount; i++)
            {
                if (!_cachedTracks[i].TagDataLoaded)
                {
                    LoadTagData(_cachedTracks[i]);
                    tagLoaded = true;
                }
                trackCount = _cachedTracks.Count;
            }

            _loadingTagData = false;

            // call recursively until no tags loaded
            if (tagLoaded) LoadAllTagData();
        }

        private bool _loadingTagData = false;

        /// <summary>
        /// Determines whether this instance is playing.
        /// </summary>
        public bool IsPlaying()
        {
            if ((this.CurrentTrack != null && Bass.BASS_ChannelIsActive(this.CurrentTrack.Channel) == BASSActive.BASS_ACTIVE_STOPPED)
                || (this.NextTrack != null && Bass.BASS_ChannelIsActive(this.NextTrack.Channel) == BASSActive.BASS_ACTIVE_STOPPED)
                || (this.PreviousTrack != null && Bass.BASS_ChannelIsActive(this.PreviousTrack.Channel) == BASSActive.BASS_ACTIVE_STOPPED))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Sets the artist and title for a track.
        /// </summary>
        /// <param name="track">The track.</param>
        private void SetArtistAndTitle(Track track, string title, string artist)
        {
            if (title == "" || artist == "")
            {
                GuessArtistAndTitleFromFilename(track);
            }
            else
            {
                track.Artist = artist;
                track.Title = title;
                if (title.Contains("/"))
                {
                    track.Artist = title.Split('/')[0].Trim();
                    track.Title = title.Split('/')[1].Trim();
                }
            }
        }

        /// <summary>
        /// Guesses the artist and title of a track from its filename.
        /// </summary>
        /// <param name="track">The track.</param>
        private void GuessArtistAndTitleFromFilename(Track track)
        {
            var trackDetails = BassHelper.GuessTrackDetailsFromFilename(track.Filename);
            track.Title = trackDetails.Title;
            track.Artist = trackDetails.Artist;
        }

        /// <summary>
        /// Loads data for a track from its mp3 tags
        /// </summary>
        /// <param name="track">The track.</param>
        public void LoadTagData(Track track)
        {
            if (track == null) return;

            if (Path.GetExtension(track.Filename).ToLower() == "wav") return;

            lock (track)
            {
                if (track.TagDataLoaded) return;

                DebugHelper.WriteLine("Loading track tags - " + track.Description);
                var tags = BassTags.BASS_TAG_GetFromFile(track.Filename);
                if (tags == null) throw new Exception("Cannot load tags for track " + track.Filename);

                track.Title = tags.title;
                track.Artist = tags.artist;
                if (track.Title.Contains("/"))
                {
                    track.Artist = tags.title.Split('/')[0].Trim();
                    track.Title = tags.title.Split('/')[1].Trim();
                }

                if (track.Title == "" || track.Artist == "")
                {
                    GuessArtistAndTitleFromFilename(track);
                }

                track.Gain = tags.replaygain_track_peak;
                track.Comment = tags.comment;

                var key = tags.NativeTag("InitialKey");
                if (key != "") track.Key = key;

                decimal bpm = 0;
                decimal.TryParse(tags.bpm, out bpm);
                track.TagBpm = BassHelper.NormaliseBpm(bpm);

                var duration = TimeSpan.FromSeconds(tags.duration);
                track.Length = (long)duration.TotalMilliseconds;
                track.TagDataLoaded = true;

                if (this.TrackTagsLoaded != null) this.TrackTagsLoaded(track, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Unloads a track from the cached tracks
        /// </summary>
        /// <param name="track">The track to unload.</param>
        public void UnloadTrack(Track track)
        {
            if (track == null) return;
            DebugHelper.WriteLine("Unloading track " + track.Description);

            BassHelper.TrackPause(track);
            if (this.IsTrackInUse(track))
            {
                this.RemoveTrackFromMixer(track);
            }
            UnloadTrackAudioData(track);
            _cachedTracks.Remove(track);
        }

        public enum AudioDataMode
        {
            StreamFromFile,
            LoadIntoMemory
        }

        public Track LoadTrackAudioData(Track track)
        {
            return LoadTrackAudioData(track, AudioDataMode.LoadIntoMemory);
        }

        /// <summary>
        /// Loads the track audio data.
        /// </summary>
        /// <param name="track">The track to load.</param>
        /// <param name="mode">The mode.</param>
        /// <returns>
        /// The loaded track
        /// </returns>
        public Track LoadTrackAudioData(Track track, AudioDataMode mode)
        {
            // abort if audio data already loaded
            if (track.IsAudioLoaded()) return track;

            // ensure mp3 tag data is loaded
            if (!track.TagDataLoaded) LoadTagData(track);

            DebugHelper.WriteLine("Loading track Audio Data " + track.Description);
            lock (track)
            {
                // abort if audio data already loaded
                if (track.IsAudioLoaded()) return track;

                if (mode == AudioDataMode.LoadIntoMemory)
                {
                    track.AudioData = File.ReadAllBytes(track.Filename);
                    track.AudioDataHandle = GCHandle.Alloc(track.AudioData, GCHandleType.Pinned);
                    track.AddChannel(Bass.BASS_StreamCreateFile(track.AudioDataPointer, 0, track.AudioData.Length,
                        BASSFlag.BASS_SAMPLE_FLOAT | BASSFlag.BASS_STREAM_DECODE | BASSFlag.BASS_STREAM_PRESCAN));
                }
                else
                {
                    track.AddChannel(Bass.BASS_StreamCreateFile(track.Filename, 0, 0,
                        BASSFlag.BASS_SAMPLE_FLOAT | BASSFlag.BASS_STREAM_DECODE | BASSFlag.BASS_STREAM_PRESCAN));
                }

                if (track.Channel == 0)
                {
                    var errorCode = Bass.BASS_ErrorGetCode();
                    throw new Exception("Cannot load track " + track.Filename + ". Error code: " + errorCode.ToString());
                }

                DebugHelper.WriteLine("Creating reverse FX stream " + track.Description + "...");
                track.AddChannel(BassFx.BASS_FX_ReverseCreate(track.Channel, 1, BASSFlag.BASS_STREAM_DECODE));
                if (track.Channel == 0) throw new Exception("Cannot load track " + track.Filename);
                Bass.BASS_ChannelSetAttribute(track.Channel, BASSAttribute.BASS_ATTRIB_REVERSE_DIR, (float)BASSFXReverse.BASS_FX_RVS_FORWARD);
                DebugHelper.WriteLine("done");

                DebugHelper.WriteLine("Creating tempo FX stream " + track.Description + "...");
                track.AddChannel(BassFx.BASS_FX_TempoCreate(track.Channel, BASSFlag.BASS_FX_FREESOURCE | BASSFlag.BASS_STREAM_DECODE));
                if (track.Channel == 0) throw new Exception("Cannot load track " + track.Filename);
                DebugHelper.WriteLine("done");

                DebugHelper.WriteLine("Calculating track length " + track.Description + "...");
                track.Length = Bass.BASS_ChannelGetLength(track.Channel);
                DebugHelper.WriteLine("done");

                track.FadeInStart = 0;
                track.FadeInStartVolume = (float)(this.DefaultFadeInStartVolume / 100);
                track.FadeInEndVolume = (float)(this.DefaultFadeInEndVolume / 100);

                track.FadeOutEnd = 0;
                track.FadeOutStartVolume = (float)(this.DefaultFadeOutStartVolume / 100);
                track.FadeOutEndVolume = (float)(this.DefaultFadeOutEndVolume / 100);

                LoadExtendedAttributes(track);

                if (track.FadeOutStart == 0)
                {
                    if (this.LimitSongLength && track.LengthSeconds > this.MaxSongLength)
                    {
                        track.FadeOutStart = track.SecondsToSamples(this.MaxSongLength - this.DefaultFadeLength - 1);
                    }
                    else
                    {
                        track.FadeOutStart = track.SecondsToSamples(track.LengthSeconds - this.DefaultFadeLength - 1);
                    }
                }

                if (track.FadeInEnd == track.FadeInStart || track.FadeInEnd == 0)
                {
                    track.FadeInEnd = track.FadeInStart + track.SecondsToSamples(BassHelper.GetBestFitLoopLength(track.StartBpm, this.DefaultFadeLength));
                }

                if (track.FadeOutEnd == track.FadeInStart || track.FadeOutEnd == 0)
                {
                    track.FadeOutEnd = track.FadeOutStart + track.SecondsToSamples(BassHelper.GetBestFitLoopLength(track.EndBpm, this.DefaultFadeLength));
                }

                if (!track.UsePreFadeIn)
                {
                    track.PreFadeInStart = track.FadeInStart;
                    track.PreFadeInStartVolume = 0;
                }

                track.DefaultSampleRate = BassHelper.GetTrackSampleRate(track);

                BassHelper.SetTrackPosition(track, 0);
            }

            AddToRecentTracks(track);

            var sleepLength = Convert.ToInt32(track.LengthSeconds * 10);
            if (sleepLength > 480) sleepLength = 480;
            if (sleepLength < 120) sleepLength = 120;
            Thread.Sleep(sleepLength);

            DebugHelper.WriteLine("Finished loading track Audio Data " + track.Description);

            return track;
        }

        /// <summary>
        /// Loads any attributes stored in a the track comment tag.
        /// </summary>
        /// <param name="track">The track.</param>
        public void LoadExtendedAttributes(Track track)
        {
            if (track == null) return;
            if (track.Artist == "" || track.Title == "") return;

            DebugHelper.WriteLine("Loading Extended Attributes " + track.Description);

            var attributes = GetExtendedAttributes(track);
            if (attributes.ContainsKey("FadeIn"))
            {
                track.FadeInStart = track.SecondsToSamples(ConversionHelper.ToDouble(attributes["FadeIn"]));
            }
            if (attributes.ContainsKey("FadeOut"))
            {
                track.FadeOutStart = track.SecondsToSamples(ConversionHelper.ToDouble(attributes["FadeOut"]));
            }
            if (attributes.ContainsKey("BPMAdjust"))
            {
                track.BpmAdjustmentRatio = ConversionHelper.ToDecimal(attributes["BPMAdjust"]);
            }
            if (attributes.ContainsKey("FadeInLengthInSeconds"))
            {
                track.FadeInEnd = track.FadeInStart + track.SecondsToSamples(ConversionHelper.ToDouble(attributes["FadeInLengthInSeconds"]));
            }
            if (attributes.ContainsKey("FadeOutLengthInSeconds"))
            {
                track.FadeOutEnd = track.FadeOutStart + track.SecondsToSamples(ConversionHelper.ToDouble(attributes["FadeOutLengthInSeconds"]));
            }
            if (attributes.ContainsKey("PreFadeInStartVolume"))
            {
                track.PreFadeInStartVolume = ConversionHelper.ToFloat(attributes["PreFadeInStartVolume"]) / 100;
                track.UsePreFadeIn = true;
            }
            if (attributes.ContainsKey("PreFadeInPosition"))
            {
                track.PreFadeInStart = track.SecondsToSamples(ConversionHelper.ToDouble(attributes["PreFadeInPosition"]));
                track.UsePreFadeIn = true;
            }
            if (attributes.ContainsKey("PreFadeInStart"))
            {
                track.PreFadeInStart = track.SecondsToSamples(ConversionHelper.ToDouble(attributes["PreFadeInStart"]));
                track.UsePreFadeIn = true;
            }
            if (attributes.ContainsKey("ChangeTempoToMatchNextTrack"))
            {
                track.ChangeTempoOnFadeOut = ConversionHelper.ToBoolean(attributes["ChangeTempoToMatchNextTrack"]);
            }
            if (attributes.ContainsKey("StartBPM"))
            {
                track.StartBpm = BassHelper.NormaliseBpm(ConversionHelper.ToDecimal(attributes["StartBPM"]));
            }
            if (attributes.ContainsKey("EndBPM"))
            {
                track.EndBpm = BassHelper.NormaliseBpm(ConversionHelper.ToDecimal(attributes["EndBPM"]));
            }
            if (attributes.ContainsKey("Duration"))
            {
                if (track.Length == 0) track.Length = (long)(ConversionHelper.ToDouble(attributes["Duration"]) * 1000);
            }
            if (attributes.ContainsKey("PowerDown"))
            {
                track.PowerDownOnEnd = ConversionHelper.ToBoolean(attributes["PowerDown"]);
                track.PowerDownOnEndOriginal = track.PowerDownOnEnd;
            }
            if (attributes.ContainsKey("StartLoopCount"))
            {
                track.StartLoopCount = ConversionHelper.ToInt(attributes["StartLoopCount"]);
            }
            if (attributes.ContainsKey("EndLoopCount"))
            {
                track.EndLoopCount = ConversionHelper.ToInt(attributes["EndLoopCount"]);
            }
            if (attributes.ContainsKey("SkipStart"))
            {
                track.SkipStart = track.SecondsToSamples(ConversionHelper.ToDouble(attributes["SkipStart"]));
            }
            if (attributes.ContainsKey("SkipLengthInSeconds"))
            {
                track.SkipEnd = track.SkipStart + track.SecondsToSamples(ConversionHelper.ToDouble(attributes["SkipLengthInSeconds"]));
            }
            if (attributes.ContainsKey("Rank"))
            {
                track.Rank = ConversionHelper.ToInt(attributes["Rank"], 1);
            }
            if (attributes.ContainsKey("Key"))
            {
                track.Key = attributes["Key"];
            }

            if (attributes.ContainsKey("Sample1Start")
                || attributes.ContainsKey("Sample2Start")
                || attributes.ContainsKey("Sample3Start")
                || attributes.ContainsKey("Sample4Start"))
            {
                LoadSamplesFromExtenedAttributes(track, attributes);
            }
        }

        private void LoadSamplesFromExtenedAttributes(Track track, Dictionary<string, string> attributes)
        {
            var xmlAttributes = AutomationAttributes.GetAutomationAttributes(track, this.ExtendedAttributeFolder);

            if (attributes.ContainsKey("Sample1Start") && xmlAttributes.GetTrackSampleByKey("Sample1") == null)
            {
                var sample1 = new TrackSample();
                sample1.Start = ConversionHelper.ToDouble(attributes["Sample1Start"]);
                sample1.Length = ConversionHelper.ToDouble(attributes["Sample1LengthInSeconds"]);
                if (attributes.ContainsKey("Sample1IsLoop"))
                {
                    sample1.IsLooped = ConversionHelper.ToBoolean(attributes["Sample1IsLoop"]);
                }
                sample1.Key = "Sample1";
                sample1.Description = "Sample #1";
                xmlAttributes.TrackSamples.Add(sample1);
            }

            if (attributes.ContainsKey("Sample2Start") && xmlAttributes.GetTrackSampleByKey("Sample2") == null)
            {
                var sample2 = new TrackSample();
                sample2.Start = ConversionHelper.ToDouble(attributes["Sample2Start"]);
                sample2.Length = ConversionHelper.ToDouble(attributes["Sample2LengthInSeconds"]);
                if (attributes.ContainsKey("Sample2IsLoop"))
                {
                    sample2.IsLooped = ConversionHelper.ToBoolean(attributes["Sample2IsLoop"]);
                }
                sample2.Key = "Sample2";
                sample2.Description = "Sample #2";
                xmlAttributes.TrackSamples.Add(sample2);
            }
            if (attributes.ContainsKey("Sample3Start") && xmlAttributes.GetTrackSampleByKey("Sample3") == null)
            {
                var sample3 = new TrackSample();
                sample3.Start = ConversionHelper.ToDouble(attributes["Sample3Start"]);
                sample3.Length = ConversionHelper.ToDouble(attributes["Sample3LengthInSeconds"]);
                if (attributes.ContainsKey("Sample3IsLoop"))
                {
                    sample3.IsLooped = ConversionHelper.ToBoolean(attributes["Sample3IsLoop"]);
                }
                sample3.Key = "Sample3";
                sample3.Description = "Sample #3";
                xmlAttributes.TrackSamples.Add(sample3);
            }

            if (attributes.ContainsKey("Sample4Start") && xmlAttributes.GetTrackSampleByKey("Sample4") == null)
            {
                var sample4 = new TrackSample();
                sample4.Start = ConversionHelper.ToDouble(attributes["Sample4Start"]);
                sample4.Length = ConversionHelper.ToDouble(attributes["Sample4LengthInSeconds"]);
                if (attributes.ContainsKey("Sample4IsLoop"))
                {
                    sample4.IsLooped = ConversionHelper.ToBoolean(attributes["Sample4IsLoop"]);
                }
                sample4.Key = "Sample4";
                sample4.Description = "Sample #4";
                xmlAttributes.TrackSamples.Add(sample4);
            }
        }

        /// <summary>
        /// Gets the path of extended attribute file for the specified track
        /// </summary>
        /// <param name="track">The track.</param>
        /// <returns>A filename, including the full path</returns>
        private string GetExtendedAttributeFile(Track track)
        {
            var filename = track.Description
                + ".ExtendedAttributes.txt";
            filename = FileSystemHelper.StripInvalidFileNameChars(filename);
            return Path.Combine(this.ExtendedAttributeFolder, filename);
        }

        /// <summary>
        /// Gets the shuffler attributes.
        /// </summary>
        /// <param name="track">The track.</param>
        /// <returns>
        /// A collection of shuffler attributes
        /// </returns>
        private Dictionary<string, string> GetExtendedAttributes(Track track)
        {
            var file = GetExtendedAttributeFile(track);
            return GetExtendedAttributes(file);
        }

        /// <summary>
        /// Gets the shuffler attributes.
        /// </summary>
        /// <param name="extendedAttributeFile">The shuffler attributes file</param>
        /// <returns>
        /// A collection of shuffler attributes
        /// </returns>
        private Dictionary<string, string> GetExtendedAttributes(string extendedAttributeFile)
        {
            var attributes = new Dictionary<string, string>();
            if (!File.Exists(extendedAttributeFile)) return attributes;

            foreach (var element in File.ReadAllText(extendedAttributeFile).Split(';').ToList())
            {
                var items = element.Split('=').ToList();
                if (items.Count > 1 && !attributes.ContainsKey(items[0].Trim()))
                {
                    attributes.Add(items[0].Trim(), items[1].Trim());
                }
            }
            return attributes;
        }

        public void SaveExtendedAttributes(Dictionary<string, string> attributes, string extendedAttributeFile)
        {
            if (attributes.Count == 0)
            {
                if (File.Exists(extendedAttributeFile))
                    File.Delete((extendedAttributeFile));
            }
            else
            {
                var extendedAttributeData = attributes.Aggregate("", (current, keyvalue) => current + string.Format("{0}={1};", keyvalue.Key, keyvalue.Value));
                File.WriteAllText(extendedAttributeFile, extendedAttributeData, Encoding.Unicode);
            }
        }

        /// <summary>
        /// Determines whether the specified track has an extended attribute file.
        /// </summary>
        /// <param name="track">The track.</param>
        /// <returns>True if the specified track has an extended attribute file; otherwise, false.
        /// </returns>
        public bool HasExtendedAttributeFile(Track track)
        {
            return File.Exists(GetExtendedAttributeFile(track));
        }

        /// <summary>
        /// Saves the track details to the track comment tage
        /// </summary>
        /// <param name="track">The track.</param>
        public void SaveExtendedAttributes(Track track)
        {
            var attributes = new Dictionary<string, string>();

            if (track.StartBpm != 0) attributes.Add("StartBPM", String.Format("{0}", track.StartBpm));
            if (track.EndBpm != 0) attributes.Add("EndBPM", string.Format("{0}", track.EndBpm));
            attributes.Add("Duration", string.Format("{0}", track.LengthSeconds));

            if (track.FadeInStart != 0) attributes.Add("FadeIn", string.Format("{0:0.000}", track.SamplesToSeconds(track.FadeInStart)));
            if (track.FadeOutStart != 0) attributes.Add("FadeOut", string.Format("{0:0.000}", track.SamplesToSeconds(track.FadeOutStart)));
            if (track.BpmAdjustmentRatio != 1) attributes.Add("BPMAdjust", string.Format("{0}", track.BpmAdjustmentRatio));

            if (track.FadeInLength != 0) attributes.Add("FadeInLengthInSeconds", string.Format("{0:0.000}", track.FadeInLengthSeconds));
            if (track.StartLoopCount > 0)
            {
                attributes.Add("StartLoopCount", string.Format("{0}", track.StartLoopCount));
            }

            if (track.FadeOutLength != 0) attributes.Add("FadeOutLengthInSeconds", string.Format("{0:0.000}", track.SamplesToSeconds(track.FadeOutLength)));
            if (track.EndLoopCount > 0)
            {
                attributes.Add("EndLoopCount", string.Format("{0}", track.EndLoopCount));
            }

            if (track.UsePreFadeIn)
            {
                attributes.Add("PreFadeInStartVolume", string.Format("{0:00}", track.PreFadeInStartVolume * 100));
                attributes.Add("PreFadeInPosition", string.Format("{0:0.000}", track.SamplesToSeconds(track.PreFadeInStart)));
            }

            if (track.PowerDownOnEnd) attributes.Add("PowerDown", string.Format("{0}", track.PowerDownOnEnd));

            if (track.HasSkipSection)
            {
                attributes.Add("SkipStart", string.Format("{0:0.000}", track.SamplesToSeconds(track.SkipStart)));
                attributes.Add("SkipLengthInSeconds", string.Format("{0:0.000}", track.SamplesToSeconds(track.SkipLength)));
            }
            if (track.Rank != 1)
            {
                attributes.Add("Rank", string.Format("{0}", track.Rank));
            }

            if (track.Key != "")
            {
                attributes.Add("Key", track.Key);
            }

            var extendedAttributeFile = GetExtendedAttributeFile(track);
            SaveExtendedAttributes(attributes, extendedAttributeFile);
        }

        /// <summary>
        /// Unloads the track audio data.
        /// </summary>
        /// <param name="track">The track.</param>
        public void UnloadTrackAudioData(Track track)
        {
            if (!track.IsAudioLoaded()) return;

            DebugHelper.WriteLine("Unloading track Audio Data " + track.Description);

            BassHelper.RemoveTrackFromMixer(track, this._trackMixer.InternalChannel);

            BassHelper.UnloadTrackAudio(track);
            track.TrackSync = null;
        }

        /// <summary>
        /// Unloads all unused audio data.
        /// </summary>
        private void UnloadUnusedAudioData()
        {
            try
            {
                List<Track> unusedTracks;
                lock (_recentTracks)
                {
                    unusedTracks = _cachedTracks
                        .Where(t => t.IsAudioLoaded() && !IsTrackInUse(t))
                        .Where(t => !_recentTracks.Exists(rt => rt.Description == t.Description))
                        .ToList();
                }

                foreach (var track in unusedTracks)
                {
                    UnloadTrackAudioData(track);
                }
            }
            catch
            {
            }
        }

        private void AddToRecentTracks(Track track)
        {
            const int length = 5;
            if (track == null) return;

            lock (_recentTracks)
            {
                if (_recentTracks.Exists(rt => rt.Description == track.Description))
                {
                    _recentTracks.RemoveAll(rt => rt.Description == track.Description);
                }

                _recentTracks.Insert(0, track);
                if (_recentTracks.Count > length)
                {
                    _recentTracks = _recentTracks.Take(length).ToList();
                }
            }
        }

        /// <summary>
        /// Plays the current track (or resumes playing if paused)
        /// </summary>
        public void Play()
        {
            if (this.CurrentTrack != null)
            {
                DebugHelper.WriteLine("Play");

                if (!CurrentTrack.IsAudioLoaded()) LoadTrackAudioData(this.CurrentTrack);

                BassHelper.SetTrackPitch(this.CurrentTrack, 100F);
                BassHelper.SetTrackVolume(this.CurrentTrack, 1F);
                BassHelper.TrackPlay(this.CurrentTrack);

                this.PlayState = PlayState.Playing;
            }

            SetDelayByBpm();
        }

        /// <summary>
        /// Stops the current track and starts the next one
        /// </summary>
        public void ForcePlayNext()
        {
            DebugHelper.WriteLine("ForceNextPrevious");
            if (this.NextTrack == null) return;
            var trackName = this.NextTrack.Filename;
            ForcePlay(trackName);
        }

        /// <summary>
        /// Stops the current track and starts the next one
        /// </summary>
        public void ForceCueNext()
        {
            DebugHelper.WriteLine("ForcePlayNext");
            if (this.NextTrack == null) return;
            var trackName = this.NextTrack.Filename;
            ForcePlay(trackName);
        }

        /// <summary>
        /// Stops the current track and replays the previous one
        /// </summary>
        public void ForcePlayPrevious()
        {
            DebugHelper.WriteLine("ForcePlayPrevious");
            if (this.PreviousTrack == null) return;
            var trackName = this.PreviousTrack.Filename;
            ForcePlay(trackName);
        }

        /// <summary>
        /// Forces the play.
        /// </summary>
        /// <param name="filename">The filename of the track to play.</param>
        public void ForcePlay(string filename)
        {
            this.Stop();
            this.StopSamples();
            ClearTracks();
            this.QueueTrack(filename);
            RaiseOnTrackChange();
            this.Play();
        }

        /// <summary>
        /// Clears the next/current/previous tracks.
        /// </summary>
        public void ClearTracks()
        {
            this.RemoveTrackFromMixer(this.PreviousTrack);
            this.PreviousTrack = null;
            this.RemoveTrackFromMixer(this.NextTrack);
            this.NextTrack = null;
            this.RemoveTrackFromMixer(this.CurrentTrack);
            this.CurrentTrack = null;
        }

        /// <summary>
        /// Forces a track to be played
        /// </summary>
        /// <param name="track">The track.</param>
        public void ForcePlay(Track track)
        {
            ForcePlay(track.Filename);
        }

        /// <summary>
        /// Stops the current playback
        /// </summary>
        public void Stop()
        {
            DebugHelper.WriteLine("Stop");

            this.Pause();
            if (this.CurrentTrack != null)
            {
                BassHelper.SetTrackPosition(this.CurrentTrack, 0);
            }
            this.PlayState = PlayState.Stopped;
        }

        /// <summary>
        /// Pauses the current playback
        /// </summary>
        public void Pause()
        {
            DebugHelper.WriteLine("Pause");

            if (this.CurrentTrack != null)
            {
                BassHelper.TrackSmoothPause(this.CurrentTrack);
            }

            if (this.NextTrack != null)
            {
                BassHelper.TrackSmoothPause(this.NextTrack);
            }

            if (this.PreviousTrack != null)
            {
                BassHelper.TrackSmoothPause(this.PreviousTrack);
            }

            this.StopTrackFxSend();
            this.PlayState = PlayState.Paused;
            Thread.Sleep(150);
        }

        /// <summary>
        /// Gets the current volume levels.
        /// </summary>
        /// <returns>A VolumeLevels object containing the left and right volume levels (0 - 32768)</returns>
        public VolumeLevels GetVolumeLevels()
        {
            WaitForLock();

            lock (_mixerLock)
            {
                return this._speakerOutput.GetVolumeLevels();
            }
        }

        /// <summary>
        /// Gets the mixer volume.
        /// </summary>
        /// <returns>A value between 0 and 100</returns>
        public decimal GetMixerVolume()
        {
            return this._speakerOutput.GetVolume();
        }

        /// <summary>
        /// Sets the mixer volume.
        /// </summary>
        /// <param name="volume">The volume as a value between 0 and 100.</param>
        public void SetMixerVolume(decimal volume)
        {
            this._speakerOutput.SetVolume(volume);
        }

        /// <summary>
        /// Gets the monitor volume.
        /// </summary>
        /// <returns>A value between 0 and 100</returns>
        public decimal GetMonitorVolume()
        {
            return this._monitorOutput.GetVolume();
        }

        /// <summary>
        /// Sets the monitor volume.
        /// </summary>
        /// <param name="volume">The volume as a value between 0 and 100.</param>
        public void SetMonitorVolume(decimal volume)
        {
            this._monitorOutput.SetVolume(volume);
        }

        /// <summary>
        /// Gets the position of the active track, including elapsed/remaining times
        /// </summary>
        /// <returns>A track position object</returns>
        public TrackPosition GetTrackPosition()
        {
            WaitForLock();

            lock (_mixerLock)
            {
                return GetPositionNoLock();
            }
        }

        private TrackPosition GetPositionNoLock()
        {
            var position = new TrackPosition();
            var track = this.ActiveTrack;
            if (track != null)
            {
                position.Track = track;

                position.ChannelPosition = BassHelper.GetTrackPosition(track);
                position.Positition = GetAdjustedPosition(position.ChannelPosition, track);

                if (this.NextTrack != null)
                {
                    position.Length = track.ActiveLength;
                }
                else
                {
                    position.Length = (track.FadeOutEnd - track.FadeInStart) + track.AdditionalEndLoopLength;
                }
            }
            return position;
        }

        /// <summary>
        /// Gets the adjusted position.
        /// </summary>
        /// <param name="channelPosition">The channel position.</param>
        /// <param name="track">The track.</param>
        /// <returns>The adjusted position</returns>
        private long GetAdjustedPosition(long channelPosition, Track track)
        {
            var position = channelPosition - track.FadeInStart;

            if (track.IsLoopedAtStart)
            {
                if (track.CurrentStartLoop < track.StartLoopCount)
                {
                    position += (track.CurrentStartLoop) * track.FadeInLength;
                }
                else
                {
                    position += track.AdditionalStartLoopLength;
                }
            }

            if (track.HasSkipSection && channelPosition > track.SkipStart)
            {
                position -= track.SkipLength;
            }

            return position;
        }

        private long GetAdjustedPosition(Track track)
        {
            if (track == null) return 0;
            var channelPosition = BassHelper.GetTrackPosition(track);
            return GetAdjustedPosition(channelPosition, track);
        }

        private double GetAdjustedPositionSeconds(Track track)
        {
            if (track == null) return 0;
            return track.SamplesToSeconds(GetAdjustedPosition(track));
        }

        /// <summary>
        /// Sets the current track position.
        /// </summary>
        /// <param name="position">The position.</param>
        public void SetAdjustedTrackPosition(long position)
        {
            DebugHelper.WriteLine("Set track position");
            var track = this.ActiveTrack;
            if (track == null) return;

            this.StopTrackFxSend();

            track.CurrentEndLoop = 0;

            var actualPosition = position;

            var newCurrentStartLoop = track.CurrentStartLoop;
            if (track.IsLoopedAtStart)
            {
                if (actualPosition >= track.FullStartLoopLength)
                {
                    newCurrentStartLoop = track.StartLoopCount;
                    actualPosition = actualPosition - track.AdditionalStartLoopLength;
                }
                else
                {
                    newCurrentStartLoop = (int)(actualPosition / track.FadeInLength);
                    actualPosition = actualPosition - (newCurrentStartLoop * track.FadeInLength);
                    if (newCurrentStartLoop > 0 && actualPosition == 0) actualPosition = 1;  // avoid start-fade event when looping
                }
            }

            actualPosition = actualPosition + track.FadeInStart;

            if (track.HasSkipSection && actualPosition > track.SkipStart)
            {
                actualPosition = actualPosition + track.SkipLength;
            }

            if (actualPosition >= track.FadeInStart && actualPosition <= track.FadeOutStart)
            {
                track.CurrentStartLoop = newCurrentStartLoop;
                BassHelper.SetTrackPosition(track, actualPosition);

                if (this.PreviousTrack != null) BassHelper.TrackPause(this.PreviousTrack);
            }
        }

        /// <summary>
        /// Sets the current track position.
        /// </summary>
        /// <param name="double">The new position in seconds.</param>
        public void SetTrackPosition(double seconds)
        {
            DebugHelper.WriteLine("Set track position");

            var track = this.ActiveTrack;
            if (track == null) return;

            var posistion = track.SecondsToSamples(seconds);

            this.SetAdjustedTrackPosition(posistion);
        }

        /// <summary>
        /// Sets the default fade out settings.
        /// </summary>
        public void SetConservativeFadeOutSettings()
        {
            if (this.CurrentTrack == null || this.NextTrack == null) return;

            if (!BassHelper.IsBpmInRange(this.CurrentTrack.EndBpm, this.NextTrack.StartBpm, 10))
            {
                this.CurrentTrack.PowerDownOnEnd = true;
            }
            else if (KeyHelper.GetKeyMixRank(this.CurrentTrack.Key, this.NextTrack.Key) < 3)
            {
                this.CurrentTrack.PowerDownOnEnd = true;
            }
            else
            {
                var newFadeOutLength = GetQuickFadeLength(this.CurrentTrack);
                if (newFadeOutLength < this.CurrentTrack.FadeOutLengthSeconds)
                {
                    this.CurrentTrack.FadeOutEnd = this.CurrentTrack.FadeOutStart + this.CurrentTrack.SecondsToSamples(newFadeOutLength);
                }
                this.CurrentTrack.EndLoopCount = 0;
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Starts the Bass audio engine.
        /// </summary>
        private static void StartAudioEngine(IntPtr windowHandle)
        {
            if (_engineStarted) return;
            DebugHelper.WriteLine("Start Engine");
            BassHelper.InitialiseBassEngine(windowHandle);
            _windowHandle = windowHandle;
            _engineStarted = true;
        }

        private static bool _engineStarted = false;

        /// <summary>
        /// Stops the Bass audio engine.
        /// </summary>
        private static void StopAudioEngine()
        {
            DebugHelper.WriteLine("Stop Engine");
            Bass.BASS_Stop();
            Bass.BASS_Free();
        }

        /// <summary>
        /// Adds to the mixer channel, and sets the sync points.
        /// </summary>
        /// <param name="track">The track to sync.</param>
        private void AddTrackToMixer(Track track)
        {
            if (track == null) return;

            DebugHelper.WriteLine("Add track to mixer " + track.Description);

            WaitForLock();
            Lock();

            track.ResetPowerDownOnEnd();

            //LoadTagData(track);

            // load audio data if not loaded
            if (!track.IsAudioLoaded())
            {
                LoadTrackAudioData(track);
            }

            if (track != this.PreviousTrack && track != this.CurrentTrack)
            {
                lock (_mixerLock)
                {
                    // add the new track to the mixer (in paused mode)
                    BassHelper.AddTrackToMixer(track, this._trackMixer.InternalChannel);

                    BassHelper.SetTrackReplayGain(track);

                    // set track sync event
                    track.TrackSync = new SYNCPROC(OnTrackSync);
                    track.CurrentStartLoop = 0;
                    track.CurrentEndLoop = 0;

                    if (this.PreviousTrack == null)
                    {
                        BassHelper.SetTrackPosition(track, 0);
                        BassHelper.SetTrackVolume(track, 1F);
                    }
                    else if (track.UsePreFadeIn)
                    {
                        BassHelper.SetTrackPosition(track, track.PreFadeInStart);
                        BassHelper.SetTrackVolume(track, track.PreFadeInStartVolume);
                    }
                    else
                    {
                        BassHelper.SetTrackPosition(track, track.FadeInStart);
                        BassHelper.SetTrackVolume(track, track.FadeInStartVolume);
                    }
                }
            }

            Unlock();
        }

        private void WaitForLock()
        {
            while (IsLocked())
            {
                DebugHelper.WriteLine("waiting for lock");
                Thread.Sleep(50);
            }
        }

        public bool IsLocked()
        {
            return _locked;
        }

        /// <summary>
        /// Resets the track sync positions.
        /// </summary>
        /// <param name="track">The track.</param>
        public void ResetTrackSyncPositions(Track track)
        {
            SetTrackSyncPositions(track);
        }

        /// <summary>
        /// Resets the track sync positions.
        /// </summary>
        public void ResetTrackSyncPositions()
        {
            LoadExtendedAttributes(this.PreviousTrack);
            LoadExtendedAttributes(this.CurrentTrack);
            LoadExtendedAttributes(this.NextTrack);

            SetTrackSyncPositions(this.PreviousTrack);
            SetTrackSyncPositions(this.CurrentTrack);
            SetTrackSyncPositions(this.NextTrack);
        }

        /// <summary>
        /// Reloads the track.
        /// </summary>
        /// <param name="filename">The filename.</param>
        public void ReloadTrack(string filename)
        {
            var track = _cachedTracks.Where(t => t.Filename.ToLower() == filename.ToLower()).FirstOrDefault();
            if (track == null) return;

            LoadTagData(track);
            LoadExtendedAttributes(track);

            if (this.IsTrackInUse(track))
                SetTrackSyncPositions(track);
        }

        /// <summary>
        /// Sets the track sync positions.
        /// </summary>
        /// <param name="track">The track.</param>
        private void SetTrackSyncPositions(Track track)
        {
            if (track == null) return;

            ClearTrackSyncPositions(track);

            DebugHelper.WriteLine("Set track sync postions " + track.Description);

            // set end track sync
            SetTrackSync(track, 0, SyncType.TrackEnd);

            // set fade-in sync events
            SetTrackSync(track, track.FadeInStart, SyncType.StartFadeIn);
            SetTrackSync(track, track.FadeInEnd, SyncType.EndFadeIn);

            // set fade-out sync events
            SetTrackSync(track, track.FadeOutStart, SyncType.StartFadeOut);
            SetTrackSync(track, track.FadeOutEnd, SyncType.EndFadeOut);

            // set pre-fade-in start (of next track) sync event
            if (this.NextTrack != null && this.NextTrack.UsePreFadeIn && track == this.CurrentTrack)
            {
                var preFadeInLength = track.SecondsToSamples(this.NextTrack.PreFadeInLengthSeconds);
                SetTrackSync(track, track.FadeOutStart - preFadeInLength, SyncType.StartPreFadeIn);
            }

            // set pre-fade-in start (of previous track) sync event
            if (track == this.NextTrack && track.UsePreFadeIn)
            {
                var preFadeInLength = this.CurrentTrack.SecondsToSamples(track.PreFadeInLengthSeconds);
                SetTrackSync(this.CurrentTrack, this.CurrentTrack.FadeOutStart - preFadeInLength, SyncType.StartPreFadeIn);
            }

            if (track == this.CurrentTrack)
            {
                SetAutomationSyncPositions(track);
                if (this.HasExtendedMixAttributes())
                {
                    SetTrackSync(this.PreviousTrack, this.GetExtendedMixAttributes().FadeEnd, SyncType.EndExtendedMix);
                }

                if (track.HasSkipSection)
                {
                    SetTrackSync(track, track.SkipStart, SyncType.StartSkipSection);
                }
            }
        }

        /// <summary>
        /// Sets the track sync positions.
        /// </summary>
        private void SetTrackSyncPositions()
        {
            // set track sync points
            SetTrackSyncPositions(this.CurrentTrack);
            SetTrackSyncPositions(this.NextTrack);
        }

        /// <summary>
        /// Sets a track sync.
        /// </summary>
        /// <param name="track">The track.</param>
        /// <param name="position">The position.</param>
        /// <param name="syncType">Type of the sync.</param>
        /// <returns>The sync Id</returns>
        private int SetTrackSync(Track track, long position, SyncType syncType)
        {
            if (!track.IsAudioLoaded()) throw new Exception("Track audio not loaded");

            var flags = BASSSync.BASS_SYNC_POS | BASSSync.BASS_SYNC_MIXTIME;

            if (syncType == SyncType.TrackEnd)
            {
                flags = BASSSync.BASS_SYNC_END;
                position = 0;
            }

            lock (track)
            {
                var syncId = BassMix.BASS_Mixer_ChannelSetSync(track.Channel,
                    flags,
                    position,
                    track.TrackSync,
                    new IntPtr((int)syncType));

                if (syncType == SyncType.StartPreFadeIn) track.PreFadeInStartSyncId = syncId;
                else if (syncType == SyncType.StartFadeIn) track.FadeInStartSyncId = syncId;
                else if (syncType == SyncType.EndFadeIn) track.FadeInEndSyncId = syncId;
                else if (syncType == SyncType.StartFadeOut) track.FadeOutStartSyncId = syncId;
                else if (syncType == SyncType.EndFadeOut) track.FadeOutEndSyncId = syncId;
                else if (syncType == SyncType.TrackEnd) track.TrackEndSyncId = syncId;
                else if (syncType == SyncType.EndRawLoop) track.RawLoopEndSyncId = syncId;
                else if (syncType == SyncType.EndExtendedMix) track.ExtendedMixEndSyncId = syncId;

                return syncId;
            }
        }

        /// <summary>
        /// Clears the track sync positions.
        /// </summary>
        /// <param name="track">The track.</param>
        private void ClearTrackSyncPositions(Track track)
        {
            DebugHelper.WriteLine("Clear track sync postions " + track.Description);

            if (track.IsAudioLoaded())
            {
                if (track.FadeInStartSyncId != int.MinValue)
                {
                    BassMix.BASS_Mixer_ChannelRemoveSync(track.Channel, track.FadeInStartSyncId);
                    track.FadeInStartSyncId = int.MinValue;
                }
                if (track.FadeInEndSyncId != int.MinValue)
                {
                    BassMix.BASS_Mixer_ChannelRemoveSync(track.Channel, track.FadeInEndSyncId);
                    track.FadeInEndSyncId = int.MinValue;
                }
                if (track.FadeOutStartSyncId != int.MinValue)
                {
                    BassMix.BASS_Mixer_ChannelRemoveSync(track.Channel, track.FadeOutStartSyncId);
                    track.FadeOutStartSyncId = int.MinValue;
                }
                if (track.FadeOutEndSyncId != int.MinValue)
                {
                    BassMix.BASS_Mixer_ChannelRemoveSync(track.Channel, track.FadeOutEndSyncId);
                    track.FadeInStartSyncId = int.MinValue;
                }
                if (track.PreFadeInStartSyncId != int.MinValue)
                {
                    BassMix.BASS_Mixer_ChannelRemoveSync(track.Channel, track.PreFadeInStartSyncId);
                    track.PreFadeInStartSyncId = int.MinValue;
                }
                if (track.TrackEndSyncId != int.MinValue)
                {
                    BassMix.BASS_Mixer_ChannelRemoveSync(track.Channel, track.TrackEndSyncId);
                    track.TrackEndSyncId = int.MinValue;
                }
                if (track.RawLoopEndSyncId != int.MinValue)
                {
                    BassMix.BASS_Mixer_ChannelRemoveSync(track.Channel, track.RawLoopEndSyncId);
                    track.TrackEndSyncId = int.MinValue;
                }
                if (track.ExtendedMixEndSyncId != int.MinValue)
                {
                    BassMix.BASS_Mixer_ChannelRemoveSync(track.Channel, track.ExtendedMixEndSyncId);
                    track.ExtendedMixEndSyncId = int.MinValue;
                }
                if (track.SkipSyncId != int.MinValue)
                {
                    BassMix.BASS_Mixer_ChannelRemoveSync(track.Channel, track.SkipSyncId);
                    track.SkipSyncId = int.MinValue;
                }
            }

            ClearAutomationSyncPositions(track);
        }

        /// <summary>
        /// Unloads the track stream data.
        /// </summary>
        /// <param name="track">The track to unload.</param>
        private void RemoveTrackFromMixer(Track track)
        {
            if (track == null) return;
            if (!track.IsAudioLoaded()) return;

            BassHelper.TrackPause(track);

            DebugHelper.WriteLine("Remove track from mixer " + track.Description);

            ClearTrackSyncPositions(track);

            UnloadTrackAudioData(track);

            track.TrackSync = null;
        }

        /// <summary>
        /// Determines whether a track is currently in use as the next/current/previous track
        /// </summary>
        /// <param name="track">The track to check.</param>
        /// <returns>True if the track is in use; otherwise, false.</returns>
        public bool IsTrackInUse(Track track)
        {
            // no track, so not in use
            if (track == null) return false;

            return GetInUseTracks().Any(t => BassHelper.IsSameTrack(track, t));
        }

        private List<Track> GetInUseTracks()
        {
            var tracks = new List<Track>();

            if (this.CurrentTrack != null) tracks.Add(this.CurrentTrack);
            if (this.NextTrack != null) tracks.Add(this.NextTrack);
            if (this.PreviousTrack != null) tracks.Add(this.PreviousTrack);
            if (this.PreloadedTrack != null) tracks.Add(this.PreloadedTrack);

            return tracks;
        }

        /// <summary>
        /// Called when the StartFadeIn sync is fired
        /// </summary>
        private void StartFadeIn()
        {
            if (this.CurrentTrack == null) return;

            DebugHelper.WriteLine("Start fade-in:" + this.CurrentTrack.Description);

            Lock();

            SetTrackSyncPositions();

            if (!CurrentTrack.IsAudioLoaded()) LoadTrackAudioData(this.CurrentTrack);

            // set loop count
            this.CurrentTrack.CurrentStartLoop = 0;

            // set position
            BassHelper.SetTrackPosition(this.CurrentTrack, this.CurrentTrack.FadeInStart);

            var startVolume = this.CurrentTrack.FadeInStartVolume;
            var endVolume = this.CurrentTrack.FadeInEndVolume;

            if (this.PreviousTrack != null && this.PreviousTrack.PowerDownOnEnd)
            {
                startVolume = this.CurrentTrack.FadeInEndVolume;
                endVolume = 1F;
            }

            // set the volume slide
            BassHelper.SetTrackVolumeSlide(this.CurrentTrack,
                startVolume,
                endVolume,
                GetFadeLength(this.PreviousTrack, this.CurrentTrack));

            // set to standard pitch
            BassHelper.SetTrackPitch(this.CurrentTrack, 100);

            // start playing the track
            BassHelper.TrackPlay(this.CurrentTrack);

            SetDelayByBpm();

            Unlock();

            DebugHelper.WriteLine("End Start fade-in:" + this.CurrentTrack.Description);
        }

        /// <summary>
        /// Called when the EndFadeIn sync is fired
        /// </summary>
        private void EndFadeIn()
        {
            DebugHelper.WriteLine("End fade-in:" + this.CurrentTrack.Description);

            var position = GetTrackPosition();

            Lock();

            var isLooped = this.CurrentTrack.IsLoopedAtStart;
            var maxLoops = this.CurrentTrack.StartLoopCount;
            var currentLoop = this.CurrentTrack.CurrentStartLoop;
            var loopForever = this.CurrentTrack.LoopFadeInIndefinitely;

            // set to loop start if neccesary
            if ((isLooped && currentLoop < maxLoops - 1) || loopForever)
            {
                var message = String.Format("Looping fade-in section - starting loop {0} of {1}", currentLoop + 1, maxLoops);
                DebugHelper.WriteLine(message);

                this.CurrentTrack.CurrentStartLoop++;
                if (loopForever && this.CurrentTrack.CurrentStartLoop == maxLoops) this.CurrentTrack.CurrentStartLoop = 0;

                // set position (add one to void StartFadeIn firing)
                BassHelper.SetTrackPosition(this.CurrentTrack, this.CurrentTrack.FadeInStart + 1);
                Unlock();
            }
            else
            {
                // set volume to full
                BassHelper.SetTrackVolume(this.CurrentTrack, 1F);
                Unlock();

                if ((!this.ManualFadeOut)
                    || (this.ManualFadeOut && this.PreviousManaulExtendedFadeType == ExtendedFadeType.PowerDown))
                {
                    RaiseOnEndFadeIn();
                }

                UnloadUnusedAudioData();
            }

            DebugHelper.WriteLine("END End fade-in");
        }

        private void Unlock()
        {
            _locked = false;
        }

        private void Lock()
        {
            WaitForLock();
            _locked = true;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the volume fade out is manual or automatic
        /// </summary>
        public bool ManualFadeOut { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether automatic track FX is enabled
        /// </summary>
        public bool TrackFxAutomationEnabled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether automatic track FX is enabled
        /// </summary>
        public bool SampleAutomationEnabled { get; set; }

        /// <summary>
        /// Called when the StartFadeOut sync is fired
        /// </summary>
        private void StartFadeOut()
        {
            DebugHelper.WriteLine("Start fade-out");

            Lock();

            var oldPreviousTrack = this.PreviousTrack;

            this.PreviousTrack = this.CurrentTrack;
            this.CurrentTrack = this.NextTrack;
            this.NextTrack = null;

            this.PreviousManaulExtendedFadeType = this.CurrentManualExtendedFadeType;
            this.CurrentManualExtendedFadeType = this.GetCurrentExtendedFadeType();

            if (!this.IsTrackInUse(oldPreviousTrack)) this.RemoveTrackFromMixer(oldPreviousTrack);

            if (this.CurrentTrack != null)
            {
                this.StopSamples();
                RaiseOnTrackChange();
            }
            else
            {
                // if no next track, we are at the end, so play is stopped
                this.PlayState = PlayState.Stopped;
            }

            if (this.PreviousTrack != null)
            {
                // set loop count
                this.PreviousTrack.CurrentEndLoop = 0;

                if (this.PreviousTrack.ChangeTempoOnFadeOut)
                {
                    // change tempo if required
                    BassHelper.SetTrackTempoToMatchAnotherTrack(this.PreviousTrack, this.CurrentTrack);
                }

                if (this.ManualFadeOut)
                {
                    if (this.PreviousManaulExtendedFadeType == ExtendedFadeType.Cut)
                    {
                        // set the volume slide
                        BassHelper.SetTrackVolumeSlide(this.PreviousTrack,
                            this.PreviousTrack.FadeOutStartVolume,
                            0,
                            GetCutFadeLength(this.CurrentTrack));

                        StopRecordingAutoExtendedMix();
                    }
                    else if (this.PreviousManaulExtendedFadeType == ExtendedFadeType.PowerDown)
                    {
                        BassHelper.TrackPowerDown(this.PreviousTrack);
                        StopRecordingAutoExtendedMix();
                    }
                }
                else
                {
                    if (this.PreviousTrack.PowerDownOnEnd)
                    {
                        BassHelper.TrackPowerDown(this.PreviousTrack);
                    }
                    else if (this.IsForceFadeNowMode)
                    {
                        BassHelper.SetTrackVolumeSlide(this.PreviousTrack,
                            this.PreviousTrack.FadeOutStartVolume,
                            this.PreviousTrack.FadeOutEndVolume,
                            this.PreviousTrack.FadeOutLengthSeconds);
                    }
                    else if (this.HasExtendedMixAttributes())
                    {
                        var mixAttributes = this.GetExtendedMixAttributes();
                        if (mixAttributes.ExtendedFadeType == ExtendedFadeType.Default)
                        {
                            // set the volume slide
                            BassHelper.SetTrackVolumeSlide(this.PreviousTrack,
                                this.PreviousTrack.FadeOutStartVolume,
                                mixAttributes.FadeEndVolume,
                                mixAttributes.FadeLength);
                        }
                        else if (mixAttributes.ExtendedFadeType == ExtendedFadeType.PowerDown)
                        {
                            BassHelper.TrackPowerDown(this.PreviousTrack);
                        }
                        else if (mixAttributes.ExtendedFadeType == ExtendedFadeType.Cut)
                        {
                            // set the volume slide
                            BassHelper.SetTrackVolumeSlide(this.PreviousTrack,
                                this.PreviousTrack.FadeOutStartVolume,
                                0,
                                GetCutFadeLength(this.CurrentTrack));
                        }
                    }
                    else
                    {
                        // set the volume slide
                        BassHelper.SetTrackVolumeSlide(this.PreviousTrack,
                            this.PreviousTrack.FadeOutStartVolume,
                            this.PreviousTrack.FadeOutEndVolume,
                            GetFadeLength());
                    }
                }
            }

            Unlock();

            if (this.CurrentTrack != null)
            {
                this.StartFadeIn();
            }
        }

        /// <summary>
        /// Determines whether the mix between the previous/current tracks has extended mix attributes
        /// </summary>
        public bool HasExtendedMixAttributes(string fadeOutTrackDescription, string fadeInTrackDescription)
        {
            return (GetExtendedMixAttributes(fadeOutTrackDescription, fadeInTrackDescription) != null);
        }

        /// <summary>
        /// Determines whether the mix between the previous/current tracks has extended mix attributes
        /// </summary>
        public bool HasExtendedMixAttributes(Track fadeOutTrack, Track fadeInTrack)
        {
            return (GetExtendedMixAttributes(fadeOutTrack, fadeInTrack) != null);
        }

        /// <summary>
        /// Determines whether the mix between the previous/current tracks has extended mix attributes
        /// </summary>
        private bool HasExtendedMixAttributes()
        {
            return (this.GetExtendedMixAttributes() != null
                && this.GetExtendedMixAttributes().FadeLength != 0);
        }

        /// <summary>
        /// Gets the extended mix attributes.
        /// </summary>
        /// <returns>The extended mix attributes.</returns>
        private ExtendedMixAttributes GetExtendedMixAttributes()
        {
            return GetExtendedMixAttributes(this.PreviousTrack, this.CurrentTrack);
        }

        /// <summary>
        /// Gets the extended mix attributes.
        /// </summary>
        /// <param name="fadeOutTrack">The fade out track.</param>
        /// <param name="fadeInTrack">The fade in track.</param>
        /// <returns>The extended mix attributes.</returns>
        public ExtendedMixAttributes GetExtendedMixAttributes(Track fadeOutTrack, Track fadeInTrack)
        {
            if (fadeOutTrack == null || fadeInTrack == null) return null;
            return GetExtendedMixAttributes(fadeOutTrack.Description, fadeInTrack.Description);
        }

        /// <summary>
        /// Gets the extended mix attributes.
        /// </summary>
        /// <param name="fadeOutTrackDescription">The fade out track description.</param>
        /// <param name="fadeInTrackDescription">The fade in track description.</param>
        /// <returns>The extended mix attributes.</returns>
        private ExtendedMixAttributes GetExtendedMixAttributes(string fadeOutTrackDescription, string fadeInTrackDescription)
        {
            var attributes = GetAutomationAttributes(fadeOutTrackDescription);
            if (attributes == null) return null;
            return attributes.GetExtendedMixAttributes(fadeInTrackDescription);
        }

        private double GetCutFadeLength(Track track)
        {
            if (track == null) return 0D;
            return BassHelper.GetDefaultLoopLength(track.StartBpm) / 16D;
        }

        private double GetQuickFadeLength(Track track)
        {
            if (track == null) return 0D;
            return (BassHelper.GetDefaultLoopLength(track.EndBpm) / 2D);
        }

        private double GetPowerDownFadeLength(Track track)
        {
            if (track == null) return 0D;
            return BassHelper.GetDefaultLoopLength(track.EndBpm) / 4D;
        }

        private double GetFadeLength()
        {
            return GetFadeLength(this.PreviousTrack, this.CurrentTrack);
        }

        private double GetFadeLength(Track fromTrack, Track toTrack)
        {
            if (fromTrack == null && toTrack == null) return 0F;

            if (toTrack == null) return fromTrack.SamplesToSeconds(fromTrack.FullEndLoopLength);
            if (fromTrack == null) return BassHelper.GetDefaultLoopLength(toTrack.StartBpm) / 4D;

            var fadeInLength = toTrack.FullStartLoopLengthSeconds;
            if (fadeInLength == 0)
                fadeInLength = BassHelper.GetDefaultLoopLength(toTrack.StartBpm);

            var fadeOutLength = GetExtendedFadeOutLength(fromTrack, toTrack);

            if (!HasExtendedMixAttributes(fromTrack, toTrack))
            {
                if (fromTrack.PowerDownOnEnd)
                {
                    fadeOutLength = GetPowerDownFadeLength(fromTrack);
                    fadeOutLength = BassHelper.GetLengthAdjustedToMatchAnotherTrack(fromTrack, toTrack, fadeOutLength);
                }
            }

            var fadeLength = fadeOutLength;
            if (fadeInLength < fadeLength) fadeLength = fadeInLength;
            return fadeLength;
        }

        /// <summary>
        /// Completes the fade-out.
        /// </summary>
        private void EndFadeOut()
        {
            DebugHelper.WriteLine("End fade-out");

            this.IsForceFadeNowMode = false;

            if (this.PreviousTrack == null) return;

            var maxLoops = GetActiveEndLoopCount();
            var isLooped = this.PreviousTrack.IsLoopedAtEnd;
            var currentLoop = this.PreviousTrack.CurrentEndLoop;
            var loopForever = isLooped && (this.ManualFadeOut || this.HasExtendedMixAttributes());

            // set to loop start if neccesary
            if ((isLooped && currentLoop < maxLoops - 1) || loopForever)
            {
                var message = String.Format("Looping fade-out section - starting loop {0} of {1}", currentLoop + 1, maxLoops);
                DebugHelper.WriteLine(message);

                this.PreviousTrack.CurrentEndLoop++;

                // set position (add one to void StartFadeIn firing)
                BassHelper.SetTrackPosition(this.PreviousTrack, this.PreviousTrack.FadeOutStart + 1);
            }
            else if (!this.ManualFadeOut && !this.HasExtendedMixAttributes())
            {
                // stop track
                BassHelper.TrackPause(this.PreviousTrack);
            }
        }

        private void EndExtendedMix()
        {
            if (this.ManualFadeOut) return;
            if (this.PreviousTrack == null) return;
            if (!this.HasExtendedMixAttributes()) return;

            DebugHelper.WriteLine("Current End Loop: " + this.PreviousTrack.CurrentEndLoop.ToString() + "  Fade End Loop: " + this.GetExtendedMixAttributes().FadeEndLoop.ToString());

            var mixAttributes = this.GetExtendedMixAttributes();
            if (mixAttributes.FadeEndLoop == 0 || this.PreviousTrack.CurrentEndLoop == mixAttributes.FadeEndLoop)
            {
                if (this.GetExtendedMixAttributes().PowerDownAfterFade)
                {
                    BassHelper.TrackPowerDown(this.PreviousTrack);
                }
                else
                {
                    BassHelper.TrackPause(this.PreviousTrack);
                }
            }
        }

        private int GetActiveEndLoopCount()
        {
            return this.PreviousTrack.EndLoopCount;
        }

        private double GetActiveEndLoopLength()
        {
            return this.PreviousTrack.FullEndLoopLength;
        }

        /// <summary>
        /// Starts the pre-fade.
        /// </summary>
        private void StartPreFadeIn()
        {
            DebugHelper.WriteLine("Start pre-fade-in");

            if (this.NextTrack == null || !this.NextTrack.UsePreFadeIn) return;

            if (!this.NextTrack.IsAudioLoaded()) LoadTrackAudioData(this.NextTrack);

            // set position
            BassHelper.SetTrackPosition(this.NextTrack, this.NextTrack.PreFadeInStart);

            // set the volume slide
            BassHelper.SetTrackVolumeSlide(this.NextTrack,
                this.NextTrack.PreFadeInStartVolume * this.NextTrack.FadeInStartVolume,
                this.NextTrack.FadeInStartVolume,
                this.NextTrack.PreFadeInLength);

            // set to standard pitch
            BassHelper.SetTrackPitch(this.NextTrack, 100);

            // start playing the track
            BassHelper.TrackPlay(this.NextTrack);

            DebugHelper.WriteLine("End Start pre-fade-in");
        }

        /// <summary>
        /// Raises the on-track-change event.
        /// </summary>
        private void RaiseOnTrackChange()
        {
            DebugHelper.WriteLine("Track change event");
            if (OnTrackChange != null)
            {
                OnTrackChange(this.CurrentTrack, EventArgs.Empty);
                SetDelayByBpm();
            }
        }

        /// <summary>
        /// Raises the OnEndFadeIn
        /// </summary>
        private void RaiseOnEndFadeIn()
        {
            DebugHelper.WriteLine("End fade in event");
            if (OnEndFadeIn != null)
            {
                OnEndFadeIn(this.CurrentTrack, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Raises the OnSkipToEnd
        /// </summary>
        private void RaiseOnSkipToEnd()
        {
            DebugHelper.WriteLine("Skip to end event");
            if (OnSkipToEnd != null)
            {
                OnSkipToEnd(this.CurrentTrack, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Raises the on-track-qued
        /// </summary>
        private void RaiseOnTrackQueued()
        {
            DebugHelper.WriteLine("Track queued event");
            if (OnTrackQueued != null)
            {
                OnTrackQueued(this.CurrentTrack, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Skips to the start of the current track
        /// </summary>
        public void SkipToStart()
        {
            if (this.CurrentTrack == null) return;
            this.SetAdjustedTrackPosition(0);
            if (this.PreviousTrack != null)
            {
                BassHelper.TrackPause(this.PreviousTrack);
                BassHelper.ResetTrackTempo(this.PreviousTrack);
            }
            if (this.NextTrack != null)
            {
                BassHelper.TrackPause(this.NextTrack);
                BassHelper.ResetTrackTempo(this.NextTrack);
            }
            BassHelper.ResetTrackTempo(this.CurrentTrack);
        }

        /// <summary>
        /// Skips to the end of the current track
        /// </summary>
        public void SkipToEnd()
        {
            if (this.CurrentTrack == null) return;

            BassHelper.ResetTrackTempo(this.CurrentTrack);
            if (this.PreviousTrack != null)
            {
                BassHelper.ResetTrackTempo(this.PreviousTrack);
                BassHelper.TrackPause(this.PreviousTrack);
            }
            if (this.NextTrack != null)
            {
                BassHelper.ResetTrackTempo(this.NextTrack);
            }

            this.SetTrackPosition(this.CurrentTrack.ActiveLengthSeconds - (BassHelper.GetDefaultLoopLength(this.CurrentTrack.EndBpm) / 2));

            RaiseOnSkipToEnd();
        }

        /// <summary>
        /// Skips to the end of the current track
        /// </summary>
        public void SkipToFadeOut()
        {
            if (this.CurrentTrack == null) return;

            BassHelper.ResetTrackTempo(this.CurrentTrack);
            if (this.PreviousTrack != null)
            {
                BassHelper.ResetTrackTempo(this.PreviousTrack);
                BassHelper.TrackPause(this.PreviousTrack);
            }
            if (this.NextTrack != null)
            {
                BassHelper.ResetTrackTempo(this.NextTrack);
            }

            this.SetTrackPosition(this.CurrentTrack.ActiveLengthSeconds - 0.05);
            //this.ResetTrackSyncPositions();
        }

        /// <summary>
        /// Skips the skip section.
        /// </summary>
        private void StartSkipSection()
        {
            DebugHelper.WriteLine("Start skip section");

            if (this.CurrentTrack == null || !this.CurrentTrack.HasSkipSection) return;

            // set position
            BassHelper.SetTrackPosition(this.CurrentTrack, this.CurrentTrack.SkipEnd);
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Called when when the track sync event is fired
        /// </summary>
        private void OnTrackSync(int handle, int channel, int data, IntPtr pointer)
        {
            var syncType = (SyncType)(pointer.ToInt32());
            var track = GetInUseTracks().FirstOrDefault(x => x.Channel == channel);
            var description = track != null ? "Track: " + track.Description : "";

            DebugHelper.WriteLine("Event Fired: " + syncType.ToString() + " " + description);

            if (syncType == SyncType.StartPreFadeIn)
            {
                this.StartPreFadeIn();
            }
            else if (syncType == SyncType.StartFadeOut)
            {
                // Start-Fade-Out also calls start fade in.
                // Old track fade out & new track fade-in
                // always start at same time.
                if (BassHelper.IsSameTrack(track, this.CurrentTrack))
                    this.StartFadeOut();
            }
            else if (syncType == SyncType.EndFadeOut)
            {
                this.EndFadeOut();
            }
            else if (syncType == SyncType.EndFadeIn)
            {
                this.EndFadeIn();
            }
            else if (syncType == SyncType.StartPreFadeIn)
            {
                this.StartPreFadeIn();
            }
            else if (syncType == SyncType.EndRawLoop)
            {
                this.PlayRawLoop();
            }
            else if (syncType == SyncType.StartTrackFxTrigger)
            {
                this.StartTrackFxTrigger();
            }
            else if (syncType == SyncType.EndTrackFxTrigger)
            {
                this.StopTrackFxTrigger();
            }
            else if (syncType == SyncType.StartSampleTrigger)
            {
                this.StartSampleTrigger();
            }
            else if (syncType == SyncType.EndSampleTrigger)
            {
                this.StopSampleTrigger();
            }
            else if (syncType == SyncType.EndExtendedMix)
            {
                this.EndExtendedMix();
            }
            else if (syncType == SyncType.StartSkipSection)
            {
                this.StartSkipSection();
            }
        }

        /// <summary>
        /// Enum style representing different types of sync events.
        /// </summary>
        private enum SyncType
        {
            /// <summary>
            /// track end sync event type
            /// </summary>
            TrackEnd = 0,

            /// <summary>
            /// Start fade-in sync event type
            /// </summary>
            StartFadeIn = 1,

            /// <summary>
            /// End fade-in sync event type
            /// </summary>
            EndFadeIn = 2,

            /// <summary>
            /// Start fade-out sync event type
            /// </summary>
            StartFadeOut = 3,

            /// <summary>
            /// End fade-out sync event type
            /// </summary>
            EndFadeOut = 4,

            /// <summary>
            /// Start pre-fade-in sync event type
            /// </summary>
            StartPreFadeIn = 5,

            /// <summary>
            /// End the loop section in raw-loop mode
            /// </summary>
            EndRawLoop = 6,

            /// <summary>
            /// Start trax fx sync event type
            /// </summary>
            StartTrackFxTrigger = 7,

            /// <summary>
            /// End trax fx sync event type
            /// </summary>
            EndTrackFxTrigger = 8,

            /// <summary>
            /// Start trax fx sync event type
            /// </summary>
            StartSampleTrigger = 9,

            /// <summary>
            /// End trax fx sync event type
            /// </summary>
            EndSampleTrigger = 10,

            /// <summary>
            /// End of the extended mix
            /// </summary>
            EndExtendedMix = 11,

            /// <summary>
            /// Start of the skip section
            /// </summary>
            StartSkipSection = 12
        }

        #endregion
    }
}