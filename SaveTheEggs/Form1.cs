using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SaveTheEggs
{
    public partial class Form1 : Form
    {
        bool goLeft, goRight;
        int speed = 8;
        int score = 0;
        int missed = 0;

        Random randX = new Random();
        Random randY = new Random();

        PictureBox splash = new PictureBox();

        public Form1()
        {
            InitializeComponent();
            RestartGame();
        }


        private void MainGameTimerEvent(object sender, EventArgs e)
        {
            txtScore.Text = "Score: " + score;
            txtMissed.Text = "Missed: " + missed;

            if (goLeft == true && Player.Left > 0) // if left key is pushed and player isnt at the end of board  
            {
                Player.Left -= 12; //speed set to 12
                Player.Image = Properties.Resources.chicken_normal2; //image of player switches to chicken facing left
            }
            if (goRight == true && Player.Left + Player.Width < this.ClientSize.Width)// if right key is pushed and player isnt at the end of board 
            {
                Player.Left += 12; //speed set to 12
                Player.Image = Properties.Resources.chicken_normal; //image of player switches to chicken facing right
            }

            foreach (Control x in this.Controls)
            {
                if (x is PictureBox && (string)x.Tag =="eggs") //pushes eggs down
                {
                    x.Top += speed;
                    
                    if(x.Top + x.Height >this.ClientSize.Height) // if egg left the scene it resets them back to the top
                    {
                        splash.Image = Properties.Resources.splash; //splash image of cracked egg if it leaves client
                        splash.Location = x.Location; //splash image will be at same location as the egg
                        splash.Height = 60;
                        splash.Width = 60;
                        splash.BackColor = Color.Transparent;

                        this.Controls.Add(splash);

                        x.Top = randY.Next(80, 300) * -1; 
                        x.Left = randX.Next(5, this.ClientSize.Width - x.Width);
                        missed += 1; //missed score increments
                        Player.Image = Properties.Resources.chicken_hurt;  // player image turns to hurt chicken.
                    }
                    if (Player.Bounds.IntersectsWith(x.Bounds)) // if player and egg image intersect
                    {
                        x.Top = randY.Next(80, 300) * -1; //egg resets at the top
                        x.Left = randX.Next(5, this.ClientSize.Width - x.Width);
                        score += 1; // score increased
                    }
                 }
            }
            if (score >10)
            {
                speed = 12;
            }
            if (missed > 5)
            {
                GameTimer.Stop();
                MessageBox.Show("Game Over" + Environment.NewLine + "We've Lost good Eggs!" + Environment.NewLine + "Click ok to retry");
                RestartGame();
            }
            //Brendan Hannon CPS-3330-01 Spring2023 Final Project
        }

        private void KeyIsDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left) // if left key clicked go left
            { 
                goLeft = true;
            }
            if (e.KeyCode == Keys.Right) // if right key clicked go right
            {
                goRight = true;
            }

        }

        private void KeyIsUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left) //if left key released set to false
            {
                goLeft = false;
            }
            if (e.KeyCode == Keys.Right) // if right key released set to false
            {
                goRight =false; 
            }
        }

        private void RestartGame()
        {
            foreach (Control x in this.Controls)
            {
                if (x is PictureBox && (string)x.Tag == "eggs") //check for the eggs tag in top 3 picture box
                {
                    x.Top = randY.Next(80, 300) * -1; // Gives random X coordinate. Set to negative so Timer brings down towards player
                    x.Left = randX.Next(5, this.ClientSize.Width - x.Width); //Random Y Coordinate 5 from left or right
                                                                             //Brendan Hannon CPS-3330-01 Spring2023 Final Project
                }
            }

            Player.Left = this.ClientSize.Width / 2; //centers the player image of the chicken
            Player.Image = Properties.Resources.chicken_normal; //loads chicken face to the right as default player

            score = 0; // resets score to 0
            missed = 0; // resets missed to 0
            speed = 8; //sets speed to 8

            goLeft = false; //resets direction player is going to false
            goRight = false; //resets direction player is going to false

            GameTimer.Start(); //starts game timer
        }
    }
}