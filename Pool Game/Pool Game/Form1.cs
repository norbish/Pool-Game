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
        private float botWall = 400;//y
        private float leftWall = 0;
        private float rightWall = 300;//x

        float ballspeed = 1;

        Ball[] Ballz = new Ball[5];

        Panel Canvas = new Panel();
        Label label = new Label();

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
        public void FixedUpdate(object sender, EventArgs e)
        {
            for(int b = 0; b<Ballz.Length; b++)
                Ballz[b].UpdateVars(topWall, botWall, leftWall, rightWall);

            drawBalls();//can be put inside for loop for 1 ball move at a time.
            checkCollision();

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
                    case 2: drawing.FillEllipse(Brushes.Yellow, Ballz[b].getX() - Ballz[b].getRadius(), Ballz[b].getY() - Ballz[b].getRadius(), Ballz[b].getRadius() * 2, Ballz[b].getRadius() * 2); break;
                    case 3: drawing.FillEllipse(Brushes.Red, Ballz[b].getX() - Ballz[b].getRadius(), Ballz[b].getY() - Ballz[b].getRadius(), Ballz[b].getRadius() * 2, Ballz[b].getRadius() * 2); break;
                    case 4: drawing.FillEllipse(Brushes.Green, Ballz[b].getX() - Ballz[b].getRadius(), Ballz[b].getY() - Ballz[b].getRadius(), Ballz[b].getRadius() * 2, Ballz[b].getRadius() * 2); break;
                }
                //drawing.FillEllipse(Brushes.Black, Ballz[b].getX() - Ballz[b].getRadius(), Ballz[b].getY() - Ballz[b].getRadius(), Ballz[b].getRadius()*2, Ballz[b].getRadius()*2);
            //(color, xPos, yPos, Width, Height)
        }

        public void checkCollision()
        {
            for (int i = 0; i < Ballz.Length; i++)
            {
                for (int j = i+1; j < Ballz.Length; j++)
                {
                    if (Ballz[i].checkCollision(Ballz[j]))
                    {
                        label.Text = "Collision!!!!";//det funker, men må lage en label for hver ball for å sjekke.
                    }
                    else
                        label.Text = "no collision";
                }
            }
        }



        public void InitializeBallz()
        {
            Ballz[0] = new Ball(1, 1, ballspeed, ballspeed, 10);//xPos, yPos, xSpeed,  ySpeed, radius,
            Ballz[1] = new Ball(150, 350, ballspeed, ballspeed, 10);
            Ballz[2] = new Ball(80, 100, ballspeed, ballspeed, 10);
            Ballz[3] = new Ball(200, 50, ballspeed, ballspeed, 10);
            Ballz[4] = new Ball(200, 150, ballspeed, ballspeed, 10);

        }
        public void InitializeGUI()
        {
            Canvas.Top = (int)topWall;
            Canvas.Left = (int)leftWall;
            Canvas.Height = (int)botWall;
            Canvas.Width = (int)rightWall;
            Canvas.BorderStyle = BorderStyle.FixedSingle;

            label.Top = 0;
            label.Left = 500;
            label.Height = 20;
            label.Width = 100;
            label.Text = "no collision";

            this.Controls.Add(Canvas);
            this.Controls.Add(label);
        }
    }
}
