using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace New_DIrection
{
    public partial class RunGeneration : Form
    {
        //generation for runto form
        public int generations { get; set; } = 0;
        //stopat for runto form
        public int StopAt { get; private set; }
        public RunGeneration()
        {
            InitializeComponent();
        }
        //when first shown will get initial values for numeric updown
        private void RunToDialogue_Shown(object sender, EventArgs e)
        {
            RunGener.Value = generations + 1;
            RunGener.Minimum = generations;
            RunGener.Maximum = int.MaxValue;
        }

        //clicking ok stores the numeric value to property
        private void OK_Click(object sender, EventArgs e)
        {
            StopAt = (int)RunGener.Value;
        }
    }
}
