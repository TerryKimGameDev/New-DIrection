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
    public partial class OptionsDialogue : Form
    {
        public int width { get; set; }
        public int height { get; set; }
        public int interval { get; set; }
        public OptionsDialogue()
        {
            InitializeComponent();
        }

        private void OptionsDialogue_Shown(object sender, EventArgs e)
        {
            TimeInterval.Value = interval;
            UniWidth.Value = width;
            UniHeight.Value = height;
        }

        private void OK_Button_Click(object sender, EventArgs e)
        {
            interval = (int)TimeInterval.Value;
            width = (int)UniWidth.Value;
            height = (int)UniHeight.Value;
        }
    }
}
