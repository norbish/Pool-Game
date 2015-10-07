using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Pool_Game
{
    public partial class Form1 : Form
    {
        private float topWall = 0;
        private float botWall = 600;//y
        private static float leftWall = 0;
        private static float rightWall = 1000;//x
        private static float padMidPos = rightWall / 2;
        private static float padPosy = 550;
        public float ballspeed = 3F;
        float radius =10F;
        
        Ball[] Ballz = new Ball[2];

        Paddle pad = new Paddle(padMidPos, padPosy, leftWall, rightWall);

        static Panel Canvas = new Panel();

        
        

        public Form1()
        {
            InitializeComponent();
            InitializeGUI();//add the GUI's
            InitializeBallz();//add the balls

            Timer Timer1 = new Timer();//create timer
            Timer1.Interval = 20;
            Timer1.Enabled = true;
            Timer1.Start();
            Timer1.Tick += new EventHandler(FixedUpdate);//make timer time.

        }

        public void update()
        {

        }
        //move paddle
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if(keyData == Keys.Left && keyData == Keys.Right)
            {
                return true;
            }
            else if (keyData == Keys.Left)
            {
                pad.updateVars(false);
                return true;
            }
            else if(keyData == Keys.Right)
            {
                pad.updateVars(true);
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        public void FixedUpdate(object sender, EventArgs e)
        {
            for (int b = 0; b < Ballz.Length; b++)
            {
                Ballz[b].UpdateVars(topWall, botWall, leftWall, rightWall);

                
                
            }
            drawBalls();
           checkCollision();//if this is inside loop, code doesnt work with less than 5 balls. idk why not
        }
        public void checkCollision()
        {
            for (int i = 0; i < Ballz.Length; i++)
            {
                Ballz[i].checkPadCollision(pad, ballspeed);
                //collide with bot pad
                for (int j = i+1; j < Ballz.Length; j++)
                {
                    if (Ballz[i].checkCollision(Ballz[j]))
                    {
                        Ballz[i].calculateCollision(Ballz[j]);
                        
                    }
                    //else
                         //label.Text = "no collision";
                         
                }
            }
        }
        public void drawBalls()
        {
            Graphics drawing = Canvas.CreateGraphics();
            drawing.Clear(Canvas.BackColor);
            for(int b = 0; b < Ballz.Length; b++)//can be removed if drawBalls() is put in for loop
                switch(b)
                {
                    case 0: drawing.FillEllipse(Brushes.Black, Ballz[b].getX() - Ballz[b].getRadius(), Ballz[b].getY() - Ballz[b].getRadius(), Ballz[b].getRadius() * 2, Ballz[b].getRadius() * 2); break;
                    case 1: drawing.FillEllipse(Brushes.Blue, Ballz[b].getX() - Ballz[b].getRadius(), Ballz[b].getY() - Ballz[b].getRadius(), Ballz[b].getRadius() * 2, Ballz[b].getRadius() * 2); break;
                    //case 2: drawing.FillEllipse(Brushes.Yellow, Ballz[b].getX() - Ballz[b].getRadius(), Ballz[b].getY() - Ballz[b].getRadius(), Ballz[b].getRadius() * 2, Ballz[b].getRadius() * 2); break;
                    //case 3: drawing.FillEllipse(Brushes.Red, Ballz[b].getX() - Ballz[b].getRadius(), Ballz[b].getY() - Ballz[b].getRadius(), Ballz[b].getRadius() * 2, Ballz[b].getRadius() * 2); break;
                    //case 4: drawing.FillEllipse(Brushes.Green, Ballz[b].getX() - Ballz[b].getRadius(), Ballz[b].getY() - Ballz[b].getRadius(), Ballz[b].getRadius() * 2, Ballz[b].getRadius() * 2); break;
                }
            //draw pad
            //drawing.FillRectangle(Brushes.Gray, pad.getX() - pad.getWidth() / 2, padPosy, pad.getWidth(), 15);//height = 5 


            drawing.FillRectangle(Brushes.Red, pad.getLL(), padPosy, pad.getWidth()/4, 15);//height = 5 
            drawing.FillRectangle(Brushes.Yellow, pad.getML(), padPosy, pad.getWidth()/2, 15);//height = 5 REAL POSITION
            drawing.FillRectangle(Brushes.Blue, pad.getMR()     , padPosy, pad.getWidth()/4, 15);//height = 5 

        }

        



        public void InitializeBallz()
        {
            Ballz[0] = new Ball(pad.getLL()-40, padPosy -60, ballspeed, ballspeed, radius);//xPos, yPos, xSpeed,  ySpeed, radius,
            Ballz[1] = new Ball(pad.getML(), padPosy -60, ballspeed, ballspeed, radius);
            //Ballz[2] = new Ball(140, 100, ballspeed, ballspeed, radius);
            //Ballz[3] = new Ball(200, 50, ballspeed, ballspeed, radius);
            //Ballz[4] = new Ball(200, 150, ballspeed, ballspeed, radius);
        }
        public void InitializeGUI()
        {
            Canvas.Top = (int)topWall;
            Canvas.Left = (int)leftWall;
            Canvas.Height = (int)botWall;
            Canvas.Width = (int)rightWall;
            Canvas.BorderStyle = BorderStyle.FixedSingle;






            this.Controls.Add(Canvas);
        }
    }
}
