using System.Windows.Forms;

namespace Halloumi.Shuffler.Controls
{
    public partial class VolumeLevels : UserControl
    {
        public VolumeLevels()
        {
            InitializeComponent();

            volLeft.Max = 32768;
            volRight.Max = 32768;
            volLeft.Min = 0;
            volRight.Min = 0;
        }

        public void SetLevels(int left, int right)
        {
            volLeft.Value = left;
            volRight.Value = right;
        }
    }
}