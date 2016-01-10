using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ComponentFactory.Krypton.Toolkit;
using Halloumi.Common.Windows.Controls;
using Halloumi.Common.Windows.Helpers;

namespace Halloumi.Shuffler.Controls
{
    public class Slider : MediaSlider.MediaSlider
    {
        #region Events

        /// <summary>
        /// Occurs after the user slides the slider with the mouse
        /// </summary>
        public event EventHandler Slid;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the Slider class.
        /// </summary>
        public Slider()
            : base()
        {
            this.Style = PanelStyle.Custom;

            // KryptonManager.GlobalPaletteChanged += new EventHandler(KryptonManager_GlobalPaletteChanged);
            this.MouseDown += new MouseEventHandler(Slider_MouseDown);
            this.MouseUp += new MouseEventHandler(Slider_MouseUp);
            this.MouseWheel += new MouseEventHandler(Slider_MouseWheel);
            this.KeyUp += new KeyEventHandler(Slider_KeyUp);
            this.Resize += new EventHandler(Slider_Resize);
            base.ResizeRedraw = true;

            SetThemeState();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Sets the state of the control based on the current theme.
        /// </summary>
        private void SetThemeState()
        {
            if (this.DesignMode) return;

            PaletteMode palette = KryptonHelper.GetCurrentPalette();
            base.TrackProgressColor = KryptonManager.GetPaletteForMode(palette).GetBackColor1(PaletteBackStyle.ButtonStandalone, PaletteState.Pressed);
            base.TrackBorderColor = KryptonHelper.GetBorderColor();
            base.ButtonBorderColor = KryptonHelper.GetBorderColor();
            SetBackgroundColour();
            base.Invalidate();
        }

        /// <summary>
        /// Raises the slid event.
        /// </summary>
        private void RaiseSlidEvent()
        {
            _raisingSlidEvent = true;
            if (this.Slid != null) Slid(this, EventArgs.Empty);
            _raisingSlidEvent = false;
        }

        private bool _raisingSlidEvent = false;

        /// <summary>
        /// Sets the background colour.
        /// </summary>
        private void SetBackgroundColour()
        {
            if (this.Style == PanelStyle.Custom)
            {
                base.BackColor = _backColor;
            }
            else
            {
                base.BackColor = KryptonHelper.GetPanelColor(this.Style);
            }
            base.Invalidate();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Set to true while the user is sliding the slider
        /// </summary>
        private bool _slidingWithMouse = false;

        /// <summary>
        /// Gets a value indicating whether this Slider is sliding.
        /// </summary>
        [Browsable(false)]
        public bool Sliding { get { return _slidingWithMouse; } }

        /// <summary>
        /// Gets or sets the dock.
        /// </summary>
        [Browsable(true)]
        public new DockStyle Dock
        {
            get { return base.Dock; }
            set
            {
                base.Dock = value;
                base.Width = this.Width;
                base.Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        public new int Value
        {
            get
            {
                return _value;
            }
            set
            {
                if (_raisingSlidEvent) return;
                _value = value;
                if (!_slidingWithMouse)
                {
                    try
                    {
                        base.Value = value;
                    }
                    catch { }
                }
            }
        }

        private int _value = 0;

        /// <summary>
        /// Gets or sets the color of the track progress.
        /// </summary>
        [Browsable(false)]
        public new Color TrackProgressColor
        {
            get { return base.TrackProgressColor; }
            set { }
        }

        /// <summary>
        /// Gets or sets the color of the track border.
        /// </summary>
        [Browsable(false)]
        public new Color TrackBorderColor
        {
            get { return base.TrackBorderColor; }
            set { }
        }

        /// <summary>
        /// Gets or sets the color of the button border.
        /// </summary>
        [Browsable(false)]
        public new Color ButtonBorderColor
        {
            get { return base.ButtonBorderColor; }
            set { }
        }

        /// <summary>
        /// Gets or sets the background color of the control
        /// </summary>
        public new Color BackColor
        {
            get
            {
                return _backColor;
            }
            set
            {
                _backColor = value;
                SetBackgroundColour();
                if (this.DesignMode) this.Invalidate();
            }
        }

        private Color _backColor = SystemColors.Control;

        [Browsable(false)]
        public new bool ResizeRedraw
        {
            get { return base.ResizeRedraw; }
            set { }
        }

        /// <summary>
        /// Gets or sets the style.
        /// </summary>
        [DefaultValue(PanelStyle.Custom)]
        [Category("Apperance")]
        [Description("The display style of the panel")]
        public PanelStyle Style
        {
            get { return _style; }
            set
            {
                _style = value;
                SetBackgroundColour();
                if (this.DesignMode) this.Invalidate();
            }
        }

        private PanelStyle _style = PanelStyle.Custom;

        [Browsable(false)]
        public Image BackGroundImage
        {
            get { return null; }
            set { }
        }

        #endregion

        #region Event Handler

        /// <summary>
        /// Handles the MouseUp event of the Slider control.
        /// </summary>
        private void Slider_MouseUp(object sender, MouseEventArgs e)
        {
            this.Value = base.Value;
            RaiseSlidEvent();
            _slidingWithMouse = false;
        }

        /// <summary>
        /// Handles the MouseDown event of the Slider control.
        /// </summary>
        private void Slider_MouseDown(object sender, MouseEventArgs e)
        {
            _slidingWithMouse = true;
        }

        /// <summary>
        /// Handles the MouseWheel event of the Slider control.
        /// </summary>
        private void Slider_MouseWheel(object sender, MouseEventArgs e)
        {
            Slider_MouseUp(this, e);
        }

        /// <summary>
        /// Handles the KeyUp event of the Slider control.
        /// </summary>
        private void Slider_KeyUp(object sender, KeyEventArgs e)
        {
            Slider_MouseUp(this, null);
        }

        /// <summary>
        /// Handles the GlobalPaletteChanged event of the KryptonManager control.
        /// </summary>
        private void KryptonManager_GlobalPaletteChanged(object sender, EventArgs e)
        {
            // SetThemeState();
        }

        /// <summary>
        /// Handles the Resize event of the Slider control.
        /// </summary>
        private void Slider_Resize(object sender, EventArgs e)
        {
            SetThemeState();
        }

        /// <summary>
        /// Raises the Resize event.
        /// </summary>
        protected override void OnResize(EventArgs e)
        {
            try { base.OnResize(e); }
            catch { }
        }

        public int ScrollValue
        {
            get { return base.Value; }
        }

        #endregion
    }
}