using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PiZ
{
    public partial class Instruction_s_control : UserControl
    {
        public Instruction_s_control()
        {
            InitializeComponent();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            //this.Visible = false;
            this.SendToBack();
        }
    }
}
