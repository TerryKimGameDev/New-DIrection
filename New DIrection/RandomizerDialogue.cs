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
    public partial class RandomizerDialogue : Form
    {
        public int seed { get; set; }
        public RandomizerDialogue()
        {
            InitializeComponent();
            //setup for seed numeric
            SeedUpDown.Maximum = int.MaxValue;
            SeedUpDown.Minimum = int.MinValue;
        }

        private void RandomizerDialogue_Shown(object sender, EventArgs e)
        {
            SeedUpDown.Value = seed;
        }

        private void OK_Button_Click(object sender, EventArgs e)
        {
            seed = (int)SeedUpDown.Value;
        }

        private void Randomize_Click(object sender, EventArgs e)
        {
            //generate time
            Random time = new Random(DateTime.Now.Millisecond);

            //get seed value
            SeedUpDown.Value = time.Next(int.MinValue, int.MaxValue);
        }
    }
}
