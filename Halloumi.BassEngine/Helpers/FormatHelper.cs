using System;

namespace Halloumi.BassEngine.Helpers
{
    public static class FormatHelper
    {
        /// <summary>
        ///     Gets a seconds value formatted as mm:ss.ttt
        /// </summary>
        /// <returns>The formatted text</returns>
        public static string GetFormattedSeconds(decimal seconds)
        {
            return GetFormattedSeconds((double) seconds);
        }
        
        /// <summary>
        ///     Gets a seconds value formatted as mm:ss.ttt
        /// </summary>
        /// <returns>The formatted text</returns>
        public static string GetFormattedSeconds(double seconds)
        {
            if (double.IsNaN(seconds)) return "";

            var timeSpan = TimeSpan.FromSeconds(seconds);

            return $"{(timeSpan.Hours*60) + timeSpan.Minutes:D2}:{timeSpan.Seconds:D2}.{timeSpan.Milliseconds:D3}";
        }

        /// <summary>
        ///     Gets the formatted length
        /// </summary>
        /// <param name="length">The length.</param>
        /// <returns>The formatted length</returns>
        public static string GetFormattedHours(decimal length)
        {
            return GetFormattedHours((double) length);
        }

        /// <summary>
        ///     Gets the formatted length
        /// </summary>
        /// <param name="length">The length.</param>
        /// <returns>The formatted length</returns>
        public static string GetFormattedHours(double length)
        {
            var timeSpan = TimeSpan.FromSeconds(length);

            return timeSpan.Hours < 1
                ? $"{timeSpan.Minutes:D2}:{timeSpan.Seconds:D2}"
                : $"{timeSpan.Hours}:{timeSpan.Minutes:D2}:{timeSpan.Seconds:D2}";
        }
    }
}