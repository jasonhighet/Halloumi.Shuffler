using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using ComponentFactory.Krypton.Toolkit;

namespace Halloumi.Shuffler.Controls
{
    public class SecondsComboBox : KryptonComboBox
    {
        /// <summary>
        ///     The value of the combobox, in seconds
        /// </summary>
        private double _seconds;

        /// <summary>
        ///     Initializes a new instance of the SecondsComboBox class.
        /// </summary>
        public SecondsComboBox()
        {
            SelectedIndexChanged += SecondsComboBox_SelectedIndexChanged;
        }

        /// <summary>
        ///     Gets or sets the value of the combobox in seconds
        /// </summary>
        [Category("Value")]
        [DefaultValue(0)]
        [Description("Gets or sets the value of the combobox in seconds")]
        public double Seconds
        {
            get { return _seconds; }
            set
            {
                _seconds = value;
                SetTextFromSeconds();
            }
        }

        /// <summary>
        ///     Handles the SelectedIndexChanged event of the SecondsComboBox control.
        /// </summary>
        private void SecondsComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetSecondsFromText();
        }

        /// <summary>
        ///     Populates the items from a list of seconds values.
        /// </summary>
        /// <param name="secondsList">The list of values in seconds.</param>
        public void PopulateItemsFromSecondsList(List<double> secondsList)
        {
            Items.Clear();
            foreach (var value in secondsList)
            {
                Items.Add(GetFormattedSeconds(value));
            }
        }

        /// <summary>
        ///     Sets the text value based on the current seconds value
        /// </summary>
        private void SetTextFromSeconds()
        {
            var formattedValue = GetFormattedSeconds();
            if (Text != formattedValue)
            {
                Text = formattedValue;
            }
        }

        /// <summary>
        ///     Sets the seconds value based on the current text
        /// </summary>
        private void SetSecondsFromText()
        {
            var value = GetSecondsFromText();
            if (value != Seconds)
            {
                Seconds = value;
            }
        }

        /// <summary>
        ///     Gets the current seconds value as formatted text
        /// </summary>
        /// <returns></returns>
        private string GetFormattedSeconds()
        {
            return GetFormattedSeconds(_seconds);
        }

        /// <summary>
        ///     Gets the current seconds value as formatted text
        /// </summary>
        /// <returns></returns>
        private string GetFormattedSeconds(double seconds)
        {
            var timeSpan = TimeSpan.FromSeconds(seconds);
            return string.Format("{0:D2}:{1:D2}:{2:D2}.{3:D3}",
                timeSpan.Hours,
                timeSpan.Minutes,
                timeSpan.Seconds,
                timeSpan.Milliseconds);
        }

        /// <summary>
        ///     Converts the current text in a value in seconds
        /// </summary>
        /// <returns></returns>
        private double GetSecondsFromText()
        {
            try
            {
                var textArray = Text.Split(':');
                var hoursValue = "0";
                var minutesValue = "0";
                var secondsValue = "0";

                if (textArray.Length == 1)
                {
                    secondsValue = textArray[0];
                }
                else if (textArray.Length == 2)
                {
                    minutesValue = textArray[0];
                    secondsValue = textArray[1];
                }
                else if (textArray.Length == 3)
                {
                    hoursValue = textArray[0];
                    minutesValue = textArray[1];
                    secondsValue = textArray[2];
                }
                else
                {
                    throw new Exception("Text not in hh:mm:ss.tttt format");
                }

                return double.Parse(secondsValue.Trim())
                       + (double.Parse(minutesValue.Trim()) * 60)
                       + (double.Parse(hoursValue.Trim()) * 60 * 60);
            }
            catch
            {
                return _seconds;
            }
        }

        /// <summary>
        ///     Raises the Validating event.
        ///     Ensures the Seconds value has the same value as the text, and also formats the text
        /// </summary>
        protected override void OnValidating(CancelEventArgs e)
        {
            SetSecondsFromText();
            base.OnValidating(e);
        }

        /// <summary>
        ///     Overrides the key press and filters out non-numeric keys
        /// </summary>
        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != ':' && e.KeyChar != '.')
            {
                e.Handled = true;
            }
            base.OnKeyPress(e);
        }
    }
}