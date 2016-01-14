using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Halloumi.Common.Windows.Controls;
using Halloumi.Shuffler.Engine;
using Un4seen.Bass;
using Un4seen.Bass.Misc;
using BE = Halloumi.BassEngine;

namespace Halloumi.Shuffler.Controls
{
    public partial class TrackWave : BaseUserControl
    {
        #region Private Variables

        private BASSTimer _timer;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TrackWave"/> class.
        /// </summary>
        public TrackWave()
        {
            InitializeComponent();

            // create a secure timer
            _timer = new BASSTimer(50);
            _timer.Tick += new EventHandler(timer_Tick);

            this.Mode = TrackWaveMode.Shuffler;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Loads a track.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        public BE.Track LoadTrack(string fileName)
        {
            this.Filename = fileName;
            this.BassTrack = this.BassPlayer.LoadRawLoopTrack(this.Filename);

            LoadTrackWaveData();
            DrawWave();

            return this.BassTrack;
        }

        /// <summary>
        /// Unloads this instance.
        /// </summary>
        public void Unload()
        {
            this.BassPlayer.UnloadRawLoopTrack();
            _timer.Stop();
        }

        /// <summary>
        /// Refreshes the track positions.
        /// </summary>
        public void RefreshPositions()
        {
            if (this.Wave == null) return;
            SetMarkers();
            DrawWave();
        }

        /// <summary>
        /// Zooms the view to the specified position
        /// </summary>
        /// <param name="startSeconds">The start seconds.</param>
        /// <param name="lengthSeconds">The length seconds.</param>
        /// <param name="currentPositionSeconds">The current position seconds.</param>
        public void Zoom(double startSeconds, double lengthSeconds, double currentPositionSeconds)
        {
            var start = this.BassTrack.SecondsToSamples(startSeconds);
            var end = this.BassTrack.SecondsToSamples(startSeconds + lengthSeconds);
            var currentPosition = start;
            if (currentPositionSeconds != 0D)
                currentPosition = this.BassTrack.SecondsToSamples(currentPositionSeconds);

            Zoom(start, end, currentPosition);
        }

        /// <summary>
        /// Zooms the view to the specified position
        /// </summary>
        /// <param name="startSeconds">The start seconds.</param>
        /// <param name="lengthSeconds">The length seconds.</param>
        /// <param name="currentPositionSeconds">The current position seconds.</param>
        public void Zoom(double startSeconds, double lengthSeconds)
        {
            Zoom(startSeconds, lengthSeconds, startSeconds);
        }

        /// <summary>
        /// Zooms the view to the specified position
        /// </summary>
        /// <param name="start">The start sample.</param>
        /// <param name="end">The end sample.</param>
        /// <param name="currentPosition">The current position.</param>
        public void Zoom(long start, long end)
        {
            Zoom(start, end, start);
        }

        /// <summary>
        /// Zooms the view to the specified position
        /// </summary>
        /// <param name="start">The start sample.</param>
        /// <param name="end">The end sample.</param>
        /// <param name="currentPosition">The current position.</param>
        public void Zoom(long start, long end, long currentPosition)
        {
            if (start < 0) return;
            if (end > this.BassTrack.Length) return;

            if (start > end)
            {
                var oldStart = start;
                var oldEnd = end;
                end = oldStart;
                start = oldEnd;
            }

            if (end - start <= 0) return;

            if (currentPosition < start || currentPosition > end)
                currentPosition = start;

            this.ZoomStart = start;
            this.ZoomEnd = end;
            this.CurrentPosition = currentPosition;
            DrawWave();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Draws the wave.
        /// </summary>
        private void DrawWave()
        {
            if (this.Wave == null) return;
            if (this.ZoomLength == 0) return;

            this.BeginInvoke((MethodInvoker)delegate
            {
                var oldBitmap = picWaveForm.BackgroundImage;
                var bitmap = this.Wave.CreateBitmap(picWaveForm.Width,
                    picWaveForm.Height,
                    this.Wave.Position2Frames(this.BassTrack.SamplesToSeconds(this.ZoomStart)),
                    this.Wave.Position2Frames(this.BassTrack.SamplesToSeconds(this.ZoomEnd)),
                    true);

                if (this.CurrentPosition >= this.ZoomStart && this.CurrentPosition <= this.ZoomEnd)
                {
                    var positionPercent = ((double)(this.CurrentPosition - this.ZoomStart) / (double)this.ZoomLength);
                    var x = Convert.ToInt32(Math.Round(positionPercent * picWaveForm.Width, 0));

                    using (var pen = new Pen(Color.Red))
                    using (var graphics = Graphics.FromImage(bitmap))
                    {
                        graphics.DrawLine(pen, x, 0, x, picWaveForm.Height - 1);
                    }
                }

                picWaveForm.BackgroundImage = bitmap;
                if (oldBitmap != null) oldBitmap.Dispose();

                scrollBar.Maximum = (int)(this.BassTrack.Length - this.ZoomLength);
                scrollBar.SmallChange = scrollBar.Maximum / 400;
                scrollBar.LargeChange = scrollBar.Maximum / 50;

                if (!_scrolling) scrollBar.Value = (int)this.ZoomStart;

                UpdateViewText();
            });
        }

        /// <summary>
        /// Loads the wave data for the current track
        /// </summary>
        private void LoadTrackWaveData()
        {
            _waveRendered = false;

            if (!File.Exists(this.Filename)) return;

            this.Wave = new WaveForm(this.Filename, new WAVEFORMPROC(WaveForm_Callback), this);
            this.Wave.FrameResolution = 0.001f; // 10ms are nice
            this.Wave.CallbackFrequency = 30000; // every 5min.
            this.Wave.ColorBackground = Color.Black;  //Color.FromArgb(20, 20, 20);
            this.Wave.ColorLeft = Color.Navy;
            this.Wave.ColorLeftEnvelope = Color.LightGray;
            this.Wave.ColorRight = Color.Navy;
            this.Wave.ColorRightEnvelope = Color.LightGray;
            this.Wave.ColorMarker = Color.Gold;
            this.Wave.ColorBeat = Color.LightSkyBlue;
            this.Wave.ColorVolume = Color.White;
            this.Wave.DrawEnvelope = false;
            this.Wave.DrawWaveForm = WaveForm.WAVEFORMDRAWTYPE.Mono;

            this.Wave.DrawMarker = WaveForm.MARKERDRAWTYPE.Line | WaveForm.MARKERDRAWTYPE.Name | WaveForm.MARKERDRAWTYPE.NameBoxFilled;

            this.Wave.MarkerLength = 0.9f;
            this.Wave.RenderStart(true, BASSFlag.BASS_DEFAULT);

            SetMarkers();

            while (!_waveRendered)
            {
                Application.DoEvents();
                Thread.Sleep(100);
            }
        }

        private void UpdateViewText()
        {
            lblViewDetails.Text = String.Format("View: {0} to {1} ({2})     Cursor: {3}",
                BE.BassHelper.GetFormattedSecondsNoHours(this.BassTrack.SamplesToSeconds(this.ZoomStart)),
                BE.BassHelper.GetFormattedSecondsNoHours(this.BassTrack.SamplesToSeconds(this.ZoomEnd)),
                BE.BassHelper.GetFormattedSecondsNoHours(this.BassTrack.SamplesToSeconds(this.ZoomLength)),
                BE.BassHelper.GetFormattedSecondsNoHours(this.BassTrack.SamplesToSeconds(this.CurrentPosition)));
        }

        /// <summary>
        /// Sets the markers.
        /// </summary>
        private void SetMarkers()
        {
            if (this.Mode == TrackWaveMode.Shuffler)
                SetShufflerMarkers();
            else
                SetSamplerMarkers();
        }

        private void SetSamplerMarkers()
        {
            if (this.Wave == null) return;

            for (var i = 1; i <= 2000; i++)
            {
                this.Wave.RemoveMarker("S" + i.ToString() + "S");
                this.Wave.RemoveMarker("S" + i.ToString() + "E");
            }

            if (this.Samples != null)
            {
                for (var i = 1; i <= this.Samples.Count; i++)
                {
                    var sample = this.Samples[i - 1];

                    if (sample == this.CurrentSample)
                        continue;

                    if (sample.Start == 0 && sample.Length == 0) continue;

                    this.Wave.AddMarker("S" + i.ToString() + "S", sample.Start);
                    this.Wave.AddMarker("S" + i.ToString() + "E", sample.Start + sample.Length);
                }
            }

            this.Wave.RemoveMarker("CSS");
            this.Wave.RemoveMarker("CSE");
            this.Wave.RemoveMarker("CSO");

            if (this.CurrentSample != null)
            {
                this.Wave.AddMarker("CSS", this.CurrentSample.Start);

                if (this.CurrentSample.Offset != 0D)
                    this.Wave.AddMarker("CSO", this.CurrentSample.Offset);

                this.Wave.AddMarker("CSE", this.CurrentSample.Start + this.CurrentSample.Length);
            }
        }

        private void SetShufflerMarkers()
        {
            if (this.Wave == null) return;

            this.Wave.RemoveMarker("PFI");
            this.Wave.RemoveMarker("FIS");
            this.Wave.RemoveMarker("FIE");
            this.Wave.RemoveMarker("FOS");
            this.Wave.RemoveMarker("FOE");
            this.Wave.RemoveMarker("SKS");
            this.Wave.RemoveMarker("SKE");

            var attributes = this.BassPlayer.GetAutomationAttributes(this.BassTrack);
            foreach (var trigger in attributes.TrackFxTriggers)
            {
                this.Wave.RemoveMarker("TS" + attributes.TrackFxTriggers.IndexOf(trigger).ToString());
                this.Wave.RemoveMarker("TE" + attributes.TrackFxTriggers.IndexOf(trigger).ToString());
            }

            for (var i = 1; i <= 2000; i++)
            {
                this.Wave.RemoveMarker("S" + i.ToString() + "S");
                this.Wave.RemoveMarker("S" + i.ToString() + "E");
            }

            this.Wave.AddMarker("PFI", this.BassTrack.SamplesToSeconds(this.BassTrack.PreFadeInStart));
            this.Wave.AddMarker("FIS", this.BassTrack.SamplesToSeconds(this.BassTrack.FadeInStart));
            this.Wave.AddMarker("FIE", this.BassTrack.SamplesToSeconds(this.BassTrack.FadeInEnd));
            this.Wave.AddMarker("FOS", this.BassTrack.SamplesToSeconds(this.BassTrack.FadeOutStart));
            this.Wave.AddMarker("FOE", this.BassTrack.SamplesToSeconds(this.BassTrack.FadeOutEnd));

            if (this.BassTrack.HasSkipSection)
            {
                this.Wave.AddMarker("SKS", this.BassTrack.SamplesToSeconds(this.BassTrack.SkipStart));
                this.Wave.AddMarker("SKE", this.BassTrack.SamplesToSeconds(this.BassTrack.SkipEnd));
            }

            if (this.TrackSamples != null)
            {
                for (var i = 1; i <= this.TrackSamples.Count; i++)
                {
                    var trackSample = this.TrackSamples[i - 1];

                    if (trackSample.Start == 0 && trackSample.Length == 0) continue;

                    this.Wave.AddMarker("S" + i.ToString() + "S", trackSample.Start);
                    this.Wave.AddMarker("S" + i.ToString() + "E", trackSample.Start + trackSample.Length);
                }
            }

            if (this.ShowTrackFx)
            {
                foreach (var trackFx in attributes.TrackFxTriggers)
                {
                    this.Wave.AddMarker("TS" + attributes.TrackFxTriggers.IndexOf(trackFx).ToString(), trackFx.Start);
                    this.Wave.AddMarker("TE" + attributes.TrackFxTriggers.IndexOf(trackFx).ToString(), trackFx.Start + trackFx.Length);
                }
            }
        }

        /// <summary>
        /// Zooms in.
        /// </summary>
        private void ZoomIn()
        {
            var zoomLength = this.ZoomLength;
            zoomLength = (int)((decimal)zoomLength * _zoomRatio);
            Zoom(zoomLength);
        }

        /// <summary>
        /// Zooms to the specified zoom length.
        /// </summary>
        /// <param name="zoomLength">Length of the zoom.</param>
        private void Zoom(long zoomLength)
        {
            this.ZoomEnd = this.ZoomStart + zoomLength;
            if (this.ZoomEnd > this.BassTrack.Length)
            {
                this.ZoomEnd = this.BassTrack.Length;
                this.ZoomStart = this.BassTrack.Length - zoomLength;
            }
            if (this.ZoomEnd < 0) this.ZoomEnd = this.BassTrack.Length;
            if (this.ZoomStart < 0) this.ZoomStart = 0;
            DrawWave();
        }

        private decimal _zoomRatio = 0.5M;

        /// <summary>
        /// Zooms out.
        /// </summary>
        private void ZoomOut()
        {
            var zoomLength = this.ZoomLength;
            zoomLength = (int)((decimal)zoomLength / _zoomRatio);
            Zoom(zoomLength);
        }

        /// <summary>
        /// Zooms to full.
        /// </summary>
        private void ZoomFull()
        {
            this.ZoomStart = 0;
            this.ZoomEnd = this.BassTrack.Length;
            DrawWave();
        }

        /// <summary>
        /// Draws the current selected position.
        /// </summary>
        private void DrawCurrentPosition()
        {
            var position = BE.BassHelper.GetTrackPosition(this.BassPlayer.RawLoopTrack);

            this.BeginInvoke((MethodInvoker)delegate
            {
                var oldBitmap = picWaveForm.Image;
                var bitmap = new Bitmap(this.picWaveForm.Width, this.picWaveForm.Height);

                if (position >= this.ZoomStart && position <= this.ZoomEnd)
                {
                    var positionPercent = ((double)(position - this.ZoomStart) / (double)this.ZoomLength);
                    var x = Convert.ToInt32(Math.Round(positionPercent * picWaveForm.Width, 0));

                    using (var pen = new Pen(Color.Gray))
                    using (var graphics = Graphics.FromImage(bitmap))
                    {
                        graphics.Clear(Color.Black);
                        graphics.DrawLine(pen, x, 0, x, picWaveForm.Height - 1);
                    }
                }
                bitmap.MakeTransparent(Color.Black);
                picWaveForm.Image = bitmap;

                if (oldBitmap != null) oldBitmap.Dispose();
            });
        }

        private bool _ticking = false;

        /// <summary>
        /// Updates the fade positions.
        /// </summary>
        private void UpdatePositions()
        {
            SetMarkers();
            DrawWave();

            // raise track changed event
            if (this.PositionsChanged != null) PositionsChanged(this, EventArgs.Empty);
        }

        private void Play()
        {
            this.BeginInvoke((MethodInvoker)delegate
            {
                this.BassPlayer.SetRawLoopPositions(this.ZoomStart, this.ZoomEnd, this.CurrentPosition);
                this.BassPlayer.PlayRawLoop();
                _timer.Enabled = true;
            });
        }

        #endregion

        #region Properties

        public TrackWaveMode Mode { get; set; }

        public enum TrackWaveMode
        {
            Shuffler,
            Sampler
        }

        /// <summary>
        /// Gets or sets the filename.
        /// </summary>
        public string Filename { get; set; }

        /// <summary>
        /// Gets or sets the wave form for the track.
        /// </summary>
        private WaveForm Wave { get; set; }

        /// <summary>
        /// Gets or sets the zoom start.
        /// </summary>
        public long ZoomStart { get; private set; }

        /// <summary>
        /// Gets or sets the zoom end.
        /// </summary>
        public long ZoomEnd { get; private set; }

        /// <summary>
        /// Gets or sets the length of the zoom.
        /// </summary>
        public long ZoomLength
        {
            get { return this.ZoomEnd - this.ZoomStart; }
        }

        /// <summary>
        /// Gets or sets the bass player.
        /// </summary>
        public BE.BassPlayer BassPlayer { get; set; }

        /// <summary>
        /// Gets or sets the bass track.
        /// </summary>
        public BE.Track BassTrack { get; private set; }

        /// <summary>
        /// Gets or sets the current position.
        /// </summary>
        private long CurrentPosition { get; set; }

        public bool ShowTrackFx
        {
            get { return _showTrackFx; }
            set
            {
                _showTrackFx = value;
                RefreshPositions();
            }
        }

        private bool _showTrackFx = false;

        public List<BE.TrackSample> TrackSamples { get; set; }

        public List<Sample> Samples { get; set; }

        public Sample CurrentSample { get; set; }

        public float GetNormalizationGain(double start, double length)
        {
            var end = start + length;

            var startFrame = this.Wave.Position2Frames(start);
            var endFrame = this.Wave.Position2Frames(end);

            var peak = 0F;
            var gain = this.Wave.GetNormalizationGain(startFrame, endFrame, ref peak);

            return gain;
        }

        #endregion

        #region Events

        public EventHandler PositionsChanged;

        #endregion

        #region Event Handlers

        /// <summary>
        /// Handles the Callback event of a WaveFormObject.
        /// </summary>
        /// <param name="framesDone">The frames done.</param>
        /// <param name="framesTotal">The frames total.</param>
        /// <param name="elapsedTime">The elapsed time.</param>
        /// <param name="finished">If true, rendering has finished.</param>
        private void WaveForm_Callback(int framesDone, int framesTotal, TimeSpan elapsedTime, bool finished)
        {
            if (finished)
            {
                this.ZoomStart = 0;
                this.ZoomEnd = this.BassTrack.Length;
                _waveRendered = true;
            }
            DrawWave();
        }

        private bool _waveRendered = true;

        /// <summary>
        /// Handles the Resize event of the picWaveForm control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void picWaveForm_Resize(object sender, EventArgs e)
        {
            DrawWave();
        }

        /// <summary>
        /// Handles the Click event of the btnZoomIn control.
        /// </summary>
        private void btnZoomIn_Click(object sender, EventArgs e)
        {
            ZoomIn();
        }

        /// <summary>
        /// Handles the Click event of the btnZoomFull control.
        /// </summary>
        private void btnZoomFull_Click(object sender, EventArgs e)
        {
            ZoomFull();
        }

        /// <summary>
        /// Handles the Click event of the btnZoomOut control.
        /// </summary>
        private void btnZoomOut_Click(object sender, EventArgs e)
        {
            ZoomOut();
        }

        /// <summary>
        /// Handles the ValueChanged event of the scrollBar control.
        /// </summary>
        private void scrollBar_ValueChanged(object sender, EventArgs e)
        {
            if (_scrolling) return;
            _scrolling = true;

            var zoomLength = this.ZoomLength;
            this.ZoomStart = scrollBar.Value;
            this.ZoomEnd = this.ZoomStart + zoomLength;
            if (this.ZoomEnd < 0) this.ZoomEnd = this.BassTrack.Length;
            if (this.ZoomStart < 0) this.ZoomStart = 0;
            DrawWave();

            _scrolling = false;
        }

        private bool _scrolling = false;

        /// <summary>
        /// Handles the MouseUp event of the picWaveForm control.
        /// </summary>
        private void picWaveForm_MouseUp(object sender, MouseEventArgs e)
        {
            var percent = (double)e.X / (double)picWaveForm.Width;
            var viewPercent = percent * (double)this.ZoomLength;
            var position = this.ZoomStart + Convert.ToInt32(Math.Round(viewPercent, 0));

            if (Control.ModifierKeys == Keys.Shift)
            {
                Zoom(this.CurrentPosition, position);
            }
            else
            {
                this.CurrentPosition = position;
                DrawWave();
            }
        }

        /// <summary>
        /// Handles the Click event of the mnuSetPreFadeInStart control.
        /// </summary>
        private void mnuSetPreFadeInStart_Click(object sender, EventArgs e)
        {
            this.BassTrack.PreFadeInStart = this.CurrentPosition;
            UpdatePositions();
        }

        /// <summary>
        /// Handles the Click event of the mnuSetFadeInStart control.
        /// </summary>
        private void mnuSetFadeInStart_Click(object sender, EventArgs e)
        {
            this.BassTrack.FadeInStart = this.CurrentPosition;
            UpdatePositions();
        }

        /// <summary>
        /// Handles the Click event of the mnuSetFadeInEnd control.
        /// </summary>
        private void mnuSetFadeInEnd_Click(object sender, EventArgs e)
        {
            this.BassTrack.FadeInEnd = this.CurrentPosition;
            UpdatePositions();
        }

        /// <summary>
        /// Handles the Click event of the mnuSetFadeOutStart control.
        /// </summary>
        private void mnuSetFadeOutStart_Click(object sender, EventArgs e)
        {
            this.BassTrack.FadeOutStart = this.CurrentPosition;
            UpdatePositions();
        }

        /// <summary>
        /// Handles the Click event of the mnuSetFadeOutEnd control.
        /// </summary>
        private void mnuSetFadeOutEnd_Click(object sender, EventArgs e)
        {
            this.BassTrack.FadeOutEnd = this.CurrentPosition;
            UpdatePositions();
        }

        /// <summary>
        /// Handles the Click event of the mnuSetSkipStart control.
        /// </summary>
        private void mnuSetSkipStart_Click(object sender, EventArgs e)
        {
            this.BassTrack.SkipStart = this.CurrentPosition;
            UpdatePositions();
        }

        /// <summary>
        /// Handles the Click event of the mnuSetSkipEnd control.
        /// </summary>
        private void mnuSetSkipEnd_Click(object sender, EventArgs e)
        {
            this.BassTrack.SkipEnd = this.CurrentPosition;
            UpdatePositions();
        }

        /// <summary>
        /// Handles the Click event of the btnStop control.
        /// </summary>
        private void btnStop_Click(object sender, EventArgs e)
        {
            Stop();
        }

        public void Stop()
        {
            picWaveForm.Image = null;
            _timer.Enabled = false;
            this.BassPlayer.StopRawLoop();
        }

        /// <summary>
        /// Handles the Click event of the btnPlay control.
        /// </summary>
        private void btnPlay_Click(object sender, EventArgs e)
        {
            Play();
        }

        /// <summary>
        /// Handles the Tick event of the timer control.
        /// </summary>
        private void timer_Tick(object sender, EventArgs e)
        {
            if (_ticking) return;
            try
            {
                DrawCurrentPosition();
            }
            catch
            { }
            finally
            {
                _ticking = false;
            }
        }

        /// <summary>
        /// Handles the Click event of the btnEdit control.
        /// </summary>
        private void btnEdit_Click(object sender, EventArgs e)
        {
            var editPath = @"C:\Program Files\coolpro2\coolpro2.exe";
            var output = Path.Combine(Path.GetTempPath(), @"Shuffler.wav");

            if (!File.Exists(editPath)) return;

            BE.BassHelper.SaveAsWave(this.BassTrack.Filename, output);

            if (!File.Exists(output)) return;
            output = string.Format("\"{0}\"", output);

            Process.Start(editPath, output);
        }

        #endregion

        private void picWaveForm_DoubleClick(object sender, EventArgs e)
        {
            Play();
        }

        private void contextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            mnuSetFadeInEnd.Visible = (this.Mode == TrackWaveMode.Shuffler);
            mnuSetFadeInStart.Visible = (this.Mode == TrackWaveMode.Shuffler);
            mnuSetFadeOutEnd.Visible = (this.Mode == TrackWaveMode.Shuffler);
            mnuSetFadeOutStart.Visible = (this.Mode == TrackWaveMode.Shuffler);
            mnuSetSkipStart.Visible = (this.Mode == TrackWaveMode.Shuffler);
            mnuSetSkipEnd.Visible = (this.Mode == TrackWaveMode.Shuffler);
            mnuSetPreFadeInStart.Visible = (this.Mode == TrackWaveMode.Shuffler);

            mnuSetSampleEnd.Visible = (this.Mode == TrackWaveMode.Sampler);
            mnuSetSampleStart.Visible = (this.Mode == TrackWaveMode.Sampler);
            mnuSetSampleOffset.Visible = (this.Mode == TrackWaveMode.Sampler);

            mnuSetSampleEnd.Enabled = (this.CurrentSample != null);
            mnuSetSampleStart.Enabled = (this.CurrentSample != null);
            mnuSetSampleOffset.Enabled = (this.CurrentSample != null);
        }

        private void mnuSetSampleStart_Click(object sender, EventArgs e)
        {
            if (this.CurrentSample == null) return;

            this.CurrentSample.Start = this.BassTrack.SamplesToSeconds(this.CurrentPosition);
            UpdatePositions();
        }

        private void mnuSetSampleEnd_Click(object sender, EventArgs e)
        {
            if (this.CurrentSample == null) return;

            var end = this.BassTrack.SamplesToSeconds(this.CurrentPosition);
            var start = this.CurrentSample.Start;

            if (start > end)
            {
                var temp = start;
                start = end;
                end = temp;
            }
            var length = end - start;

            this.CurrentSample.Start = start;
            this.CurrentSample.Length = length;

            UpdatePositions();
        }

        private void mnuSetSampleOffset_Click(object sender, EventArgs e)
        {
            if (this.CurrentSample == null) return;

            this.CurrentSample.Offset = this.BassTrack.SamplesToSeconds(this.CurrentPosition);
            UpdatePositions();
        }
    }
}