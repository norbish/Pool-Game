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
        //Ball ball1 = new Ball(1, 1, 10, 10, 10);//create a ball.(xPos, yPos, xSpeed, ySpeed, radius)
        //Ball ball2 = new Ball(200, 200, 10, 10, 10);
        Ball[] Ballz = new Ball[5];

        Panel Canvas = new Panel();

        public Form1()
        {
            InitializeComponent();
            InitializeGUI();//add the GUI's
            InitializeBallz();//add the balls

            Timer Timer1 = new Timer();//create timer
            Timer1.Interval = 50;
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

            //ball1.UpdateVars(topWall, botWall, leftWall, rightWall);
            drawBalls();//can be put inside for loop for 1 ball move at a time.

        }
        public void drawBalls()
        {
            Graphics drawing = Canvas.CreateGraphics();
            drawing.Clear(Canvas.BackColor);
            for(int b = 0; b < Ballz.Length; b++)//can be removed if drawBalls() is put in for loop
                drawing.FillEllipse(Brushes.Black, Ballz[b].getX() - Ballz[b].getRadius(), Ballz[b].getY() - Ballz[b].getRadius(), Ballz[b].getRadius()*2, Ballz[b].getRadius()*2);//(color, xPos, yPos, Width, Height)
        }


        public void InitializeBallz()
        {
            Ballz[0] = new Ball(1, 1, 10, 10, 10);
            Ballz[1] = new Ball(150, 350, 10, 10, 10);
            Ballz[2] = new Ball(80, 100, 10, 10, 10);
            Ballz[3] = new Ball(200, 50, 10, 10, 10);
            Ballz[4] = new Ball(200, 150, 10, 10, 10);

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
