using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PiZ
{   
    public partial class Form1 : Form
    {
        static int BoardSize = 20;       
        public int gameMode = 1;
        public int policeman1 = 1;
        public int thief = 1;
        public int policeman2 = 1;
        public PictureBox[,] imageBoard = new PictureBox[BoardSize, BoardSize];
       
        public Form1()
        {            
            InitializeComponent();           
            FillPictureBoard();            
        }       

        private void FillPictureBoard()
        {
            int pictureSize = panel2.Width / BoardSize;
            int counter = 0;
            for (int i = 0; i < BoardSize; i++)
            {
                for(int j = 0; j < BoardSize; j++)
                {
                    imageBoard[i, j] = new PictureBox();
                    imageBoard[i, j].Height = pictureSize;
                    imageBoard[i, j].Width = pictureSize;
                    if (Properties.Resources.Board1[counter] == '1')
                    {
                        imageBoard[i, j].Image = Properties.Resources.Budynek_kwadrat_ciemny;                                             
                    }
                    else 
                    {
                        imageBoard[i, j].Image = Properties.Resources.Droga_kwadrat_biały;
                    }
                    imageBoard[i, j].SizeMode = PictureBoxSizeMode.Zoom;
                    panel2.Controls.Add(imageBoard[i, j]);
                    imageBoard[i, j].Location = new Point(j * pictureSize, i * pictureSize);
                    counter++;
                }
                counter += 2;
            }           
            Form5 VILAS = new Form5();
            VILAS.Show();
        }
      
        private void button1_Click(object sender, EventArgs e)
        {
            Form2 VILAS = new Form2(policeman1, policeman2, thief, gameMode);
            VILAS.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (label4.Text == "Drużyna Złodziei" && gameMode == 1)
            {
                label4.Text = "Drużyna Policjantów";
                gameMode = 2;
            }              
            else
            {
                label4.Text = "Drużyna Złodziei";
                gameMode = 1;
            }                
        }

        private void button10_Click(object sender, EventArgs e)
        {
            this.Close();           
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            if (label4.Text == "Drużyna Złodziei" && gameMode == 1)
            {
                label4.Text = "Drużyna Policjantów";
                gameMode = 2;
            }
            else
            {
                label4.Text = "Drużyna Złodziei";
                gameMode = 1;
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (pictureBox3.Visible == true)
            {
                pictureBox6.Visible = true;
                pictureBox3.Visible = false;
                thief = 2;
            }
            else if(pictureBox6.Visible == true)
            {
                pictureBox3.Visible = true;
                pictureBox6.Visible = false;
                thief = 1;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (pictureBox3.Visible == true)
            {
                pictureBox6.Visible = true;
                pictureBox3.Visible = false;
                thief = 2;
            }
            else if (pictureBox6.Visible == true)
            {
                pictureBox3.Visible = true;
                pictureBox6.Visible = false;
                thief = 1;
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (pictureBox4.Visible == true)
            {
                pictureBox7.Visible = true;
                pictureBox4.Visible = false;
                policeman2 = 2;
            }
            else if (pictureBox7.Visible == true)
            {
                pictureBox4.Visible = true;
                pictureBox7.Visible = false;
                policeman2 = 1;
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (pictureBox4.Visible == true)
            {
                pictureBox7.Visible = true;
                pictureBox4.Visible = false;
                policeman2 = 2;
            }
            else if (pictureBox7.Visible == true)
            {
                pictureBox4.Visible = true;
                pictureBox7.Visible = false;
                policeman2 = 1;
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (pictureBox2.Visible == true)
            {
                pictureBox8.Visible = true;
                pictureBox2.Visible = false;
                policeman1 = 2;
            }
            else if (pictureBox8.Visible == true)
            {
                pictureBox2.Visible = true;
                pictureBox8.Visible = false;
                policeman1 = 1;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (pictureBox2.Visible == true)
            {
                pictureBox8.Visible = true;
                pictureBox2.Visible = false;
                policeman1 = 2;
            }
            else if (pictureBox8.Visible == true)
            {
                pictureBox2.Visible = true;
                pictureBox8.Visible = false;
                policeman1 = 1;
            }
        }

        private void button13_Click(object sender, EventArgs e)
        {
            Form4 VILAS4 = new Form4();
            VILAS4.Show();
        }       
    }
}
