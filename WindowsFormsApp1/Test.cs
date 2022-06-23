using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace WindowsFormsApp1
{
    public partial class mygame : Form
    {
        List<int> numbers = new List<int> { 1, 1, 2, 2, 3, 3, 4, 4, 5, 5, 6, 6 };
        string firstChoice;
        string secondChoice;
        int tries;
        List<PictureBox> pictures = new List<PictureBox>();
        PictureBox picA;
        PictureBox picB;

        int totalTime = 5;
        int countDownTime;
        bool gameOver = false;
        public mygame()
        {
            InitializeComponent();
        }
        private void Start_btn(object sender, EventArgs e)
        {
            LoadPictures();
            startButton.Visible = false;
        }
        private void TimerEvent(object sender, EventArgs e)
        {
            countDownTime--;

            lblTimeLeft.Text = "Time left: " + countDownTime;
            if (countDownTime == 0)
            {
                GameOver("GAME OVER");
                foreach (PictureBox pics in pictures)
                {
                    if (pics.Tag != null)
                    {
                        pics.Image = Image.FromFile("pics/" + (string)pics.Tag + ".png"); 
                    }
                }
            }
        }

        private void RestartGameEvent(object sender, EventArgs e)
        {
            RestartGame();
        }
        private void LoadPictures()
        {
            
            int leftPos = 20;
            int topPos = 20;
            int rows = 0;

            for(int i = 0; i < 12; i++)
            {
                PictureBox newPic = new PictureBox();
                newPic.Height = 50;
                newPic.Width = 50;
                newPic.BackColor = Color.LightGray;
                newPic.SizeMode = PictureBoxSizeMode.StretchImage;
                newPic.Click += NewPic_Click;
                pictures.Add(newPic);
                if(rows < 3)
                {
                    rows++;
                    newPic.Left = leftPos;
                    newPic.Top = topPos;
                    this.Controls.Add(newPic);
                    leftPos = leftPos + 60;
                }
                if(rows == 3)
                {
                    leftPos = 20;
                    topPos += 60;
                    rows = 0;
                }
            }

            RestartGame();
        }

        private void NewPic_Click(object sender, EventArgs e)
        {
            if (gameOver)
            {
                return;
            }
            if (firstChoice == null)
            {
                picA = sender as PictureBox;
                if (picA.Tag != null && picA.Image == null)
                {
                    picA.Image = Image.FromFile("pics/"+(string)picA.Tag+".png");
                    firstChoice = (string)picA.Tag;
                }
            }
            else if(secondChoice == null)
            {
                picB = sender as PictureBox;
                if (picB.Tag != null && picB.Image == null)
                {
                    picB.Image = Image.FromFile("pics/" + (string)picB.Tag + ".png");
                    secondChoice = (string)picB.Tag;
                }
                timer1.Start();       
            }
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Stop();
            CheckPictures(picA, picB);
        }

        private void CheckPictures(PictureBox A, PictureBox B)
        {
            if (firstChoice == secondChoice)
            {
                A.Tag = null;
                B.Tag = null;
            }
            else
            { 
                tries++;
                lblStatus.Text = "Mismatched " + tries + " times";
            }

            firstChoice = null;
            secondChoice = null;

            foreach (PictureBox pics in pictures)
            {
                if (pics.Tag != null)
                {
                    pics.Image = null;
                }
            }

            if (pictures.All(o => o.Tag == null))
            {
                GameOver("Great! You are winner!");
            }
        }

        private void RestartGame()
        {
            var randomList = numbers.OrderBy(x => Guid.NewGuid()).ToList();

            numbers = randomList;

            for(int i = 0; i < pictures.Count; i++)
            {
                pictures[i].Image = null;
                pictures[i].Tag = numbers[i].ToString();
            }

            tries = 0;
            lblStatus.Text = "Mismatched: " + tries + " times";
            lblTimeLeft.Text = "Time left: " + totalTime;
            gameOver = false;
            GameTimer.Start();
            countDownTime = totalTime;

        }
       
        private void GameOver(string msg)
        {
            GameTimer.Stop();
            gameOver = true;
            MessageBox.Show(msg + "\nClick Restart to Play again");
        }

        
    }
}
