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

        float ballspeed = 2F;
        float radius =10F;

        Ball[] Ballz = new Ball[5];

        Panel Canvas = new Panel();
        Label label = new Label();
        Label label2 = new Label();
        Label label3 = new Label();
        Label label4 = new Label();
        Label label5 = new Label();

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
            for (int b = 0; b < Ballz.Length; b++)
            {
                Ballz[b].UpdateVars(topWall, botWall, leftWall, rightWall);
             drawBalls();//can be put inside for loop for 1 ball move at a time.
           checkCollision();
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
                //collide with bot pad
                for (int j = i+1; j < Ballz.Length; j++)
                {
                    if (Ballz[i].checkCollision(Ballz[j]))
                    {
                        Ballz[i].calculateCollision(Ballz[j]);
                        if(i==0)label.Text = "Collision!!!!";//det funker, men må lage en label for hver ball for å sjekke.
                        if(i == 1)label2.Text = "Collision!!!!";
                        if(i == 2)label3.Text = "Collision!!!!";
                        if(i == 3)label4.Text = "Collision!!!!";
                        if(i == 4)label5.Text = "Collision!!!!";
                    }
                    //else
                         //label.Text = "no collision";
                         
                }
            }
        }



        public void InitializeBallz()
        {
            Ballz[0] = new Ball(1, 1, ballspeed, ballspeed, radius);//xPos, yPos, xSpeed,  ySpeed, radius,
            Ballz[1] = new Ball(100, 10, ballspeed, ballspeed, radius);
            Ballz[2] = new Ball(140, 100, ballspeed, ballspeed, radius);
            Ballz[3] = new Ball(200, 50, ballspeed, ballspeed, radius);
            Ballz[4] = new Ball(200, 150, ballspeed, ballspeed, radius);
        }
        public void InitializeGUI()
        {
            Canvas.Top = (int)topWall;
            Canvas.Left = (int)leftWall;
            Canvas.Height = (int)botWall;
            Canvas.Width = (int)rightWall;
            Canvas.BorderStyle = BorderStyle.FixedSingle;

            //labels to check collision
            label.Top = 0;
            label.Left = 500;
            label.Height = 20;
            label.Width = 100;
            label.Text = "no collision";

            label2.Top = 20;
            label2.Left = 500;
            label2.Height = 20;
            label2.Width = 100;
            label2.Text = "no collision";

            label3.Top = 40;
            label3.Left = 500;
            label3.Height = 20;
            label3.Width = 100;
            label3.Text = "no collision";

            label4.Top = 60;
            label4.Left = 500;
            label4.Height = 20;
            label4.Width = 100;
            label4.Text = "no collision";

            label5.Top = 80;
            label5.Left = 500;
            label5.Height = 20;
            label5.Width = 100;
            label5.Text = "no collision";




            this.Controls.Add(Canvas);

            this.Controls.Add(label);
            this.Controls.Add(label2);
            this.Controls.Add(label3);
            this.Controls.Add(label4);
            this.Controls.Add(label5);
        }
    }
}
