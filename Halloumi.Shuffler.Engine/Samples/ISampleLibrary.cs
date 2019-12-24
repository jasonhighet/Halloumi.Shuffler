using System.Collections.Generic;
using Halloumi.Shuffler.AudioLibrary.Models;

namespace Halloumi.Shuffler.AudioLibrary.Samples
{
    public interface ISampleLibrary
    {
        /// <summary>
        ///     Saves the sample details to a cache file
        /// </summary>
        void SaveToCache();

        List<Sample> GetSamples();
        List<Sample> GetSamples(SearchCriteria searchCriteria);
        string GetSampleFileName(Sample sample);
        void CalculateSampleKey(Sample sample);

        Track GetTrackFromSample(Sample sample);

        void LoadFromCache();

        void LoadFromFiles();
    }
}