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

        public MixerChannel(IBmpProvider bpmProvider, MixerChannelOutputType outputType)
            : base(bpmProvider)
        {
            InternalChannel = BassHelper.IntialiseMixerChannel();
            OutputType = outputType;
            SetVolume(100);
        }

        public void CutBass()
        {
            if (!IsMixerEqIntialised())
                InitialiseMixerEq();

            SetBassEq(-15);
        }

        public void RestoreBass()
        {
            if (!IsMixerEqIntialised())
                InitialiseMixerEq();

            SetBassEq(0);
        }

        private void InitialiseMixerEq()
        {
            _eqChannel = Bass.BASS_ChannelSetFX(InternalChannel, BASSFXType.BASS_FX_DX8_PARAMEQ, 0);
        }

        private bool IsMixerEqIntialised()
        {
            return (_eqChannel != int.MinValue);
        }

        private void SetBassEq(decimal gain)
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