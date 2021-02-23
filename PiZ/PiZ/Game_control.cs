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
    public partial class Game_control : UserControl
    {
        public Game_control()
        {
            InitializeComponent();
        }

        private void Game_control_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.SendToBack();
           //this.Visible = false;
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            this.SendToBack();
        }
    }
}
