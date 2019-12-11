using Halloumi.Shuffler.AudioEngine.Helpers;
using Un4seen.Bass;

namespace Halloumi.Shuffler.AudioEngine.Channels
{
    public class MixerChannel : Channel
    {
        internal MixerChannelOutputType OutputType { get; set; }

        private int _eqChannel = int.MinValue;

        public MixerChannel(IBmpProvider bpmProvider = null, MixerChannelOutputType outputType = MixerChannelOutputType.SingleOutput)
            : base(bpmProvider)
        {
            ChannelId = ChannelHelper.InitializeMixerChannel();
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
            _eqChannel = Bass.BASS_ChannelSetFX(ChannelId, BASSFXType.BASS_FX_DX8_PARAMEQ, 0);
        }

        private bool IsMixerEqIntialised()
        {
            return (_eqChannel != int.MinValue);
        }

        private void SetBassEq(decimal gain)
        {
            // 3-band EQ
            var eq = new BASS_DX8_PARAMEQ
            {
                fBandwidth = 18f,
                fCenter = 100f,
                fGain = (float) gain
            };
            Bass.BASS_FXSetParameters(_eqChannel, eq);
        }
    }

    public enum MixerChannelOutputType
    {
        SingleOutput,
        MultipleOutputs
    }
}