using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Halloumi.Common.Helpers;
using Halloumi.Shuffler.AudioEngine;
using Halloumi.Shuffler.AudioEngine.Helpers;
using Halloumi.Shuffler.AudioEngine.ModulePlayer;
using Halloumi.Shuffler.AudioLibrary;
using Halloumi.Shuffler.AudioLibrary.Models;

namespace Halloumi.Shuffler.TestHarness
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            listBuilder1.OnDestinationListChanged += ListBuilder1_OnDestinationListChanged;

            var items = new List<string>()
            {
                "Item1",
                "Item2",
                "Item3",
                "Item4",
                "Item5"
            };

            listBuilder1.SetSourceList(items);

            listBuilder1.SetDestinationList(items);
        }

        private void ListBuilder1_OnDestinationListChanged(object sender, EventArgs e)
        {
            var items = listBuilder1.GetDestinationList();
            foreach (var item in items)
            {
                DebugHelper.WriteLine(item);
            }
        }
    }
}