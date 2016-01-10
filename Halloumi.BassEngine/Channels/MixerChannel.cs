using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Halloumi.Common.Helpers;
using Un4seen.Bass;
using Un4seen.Bass.AddOn.Mix;
using Un4seen.Bass.AddOn.Vst;

namespace Halloumi.BassEngine.Channels
{
    public class MixerChannel : Channel
    {
        internal MixerChannelOutputType OutputType { get; set; }

        private int _eqChannel = int.MinValue;

        public MixerChannel(IBMPProvider bpmProvider, MixerChannelOutputType outputType)
            : base(bpmProvider)
        {
            this.InternalChannel = BassHelper.IntialiseMixerChannel();
            OutputType = outputType;
            SetVolume(100);
        }

        public void CutBass()
        {
            if (!IsMixerEQIntialised())
                InitialiseMixerEQ();

            SetBassEQ(-15);
        }

        public void RestoreBass()
        {
            if (!IsMixerEQIntialised())
                InitialiseMixerEQ();

            SetBassEQ(0);
        }

        private void InitialiseMixerEQ()
        {
            _eqChannel = Bass.BASS_ChannelSetFX(this.InternalChannel, BASSFXType.BASS_FX_DX8_PARAMEQ, 0);
        }

        private bool IsMixerEQIntialised()
        {
            return (_eqChannel != int.MinValue);
        }

        private void SetBassEQ(decimal gain)
        {
            // 3-band EQ
            var eq = new BASS_DX8_PARAMEQ();
            eq.fBandwidth = 18f;
            eq.fCenter = 100f;
            eq.fGain = (float)gain;
            Bass.BASS_FXSetParameters(_eqChannel, eq);
        }
    }

    public enum MixerChannelOutputType
    {
        SingleOutput,
        MultipleOutputs
    }
}