using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Halloumi.Shuffler.TestHarness
{
    public partial class ListBuilder : UserControl
    {
        private List<string> SourceList { get; set; }

        private List<string> DestinationList { get; set; }


        public ListBuilder()
        {
            InitializeComponent();
        }

        public void SetSourceList(List<string> sourceList)
        {
            SourceList = sourceList;
            BindData();
        }

        private void BindData()
        {
           
        }

        public List<string> GetDestinationList()
        {
            return DestinationList.ToList();
        }

        public bool AllowMultipleSourceItemsInDestination { get; set; }

        public event EventHandler OnDestinationListChanged;

        private void btnClear_Click(object sender, EventArgs e)
        {
            DestinationList.Clear();
            BindData();
        }
    }
}
