using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Halloumi.Common.Windows.Controls;
using Halloumi.Shuffler.AudioEngine.ModulePlayer;
using Halloumi.Shuffler.AudioLibrary;

namespace Halloumi.Shuffler.Controls.ModulePlayerControls
{
    public class ModulePlayerControl : BaseUserControl {

        public ModulePlayer ModulePlayer { get; set; }

        public SampleLibrary SampleLibrary { get; set; }
    }
}
