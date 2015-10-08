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
        private static float padPosy = 530;

        private float brickGroupPosX = 150;
        private float brickGroupPosY = 100;
        private float brickWidth = 50;
        private float brickHeight = 25;

        public float ballspeed = 3F;
        float radius =6F;
        
        Ball[] Ballz = new Ball[5];
        Brick[] Brickz = new Brick[45];

        Paddle pad = new Paddle(padMidPos, padPosy, leftWall, rightWall);

        static Panel Canvas = new Panel();
        static Panel sideCanvas = new Panel();
        static Label score = new Label();
        Button restartButton = new Button();


        public Form1()
        {
            InitializeComponent();
            InitializeGUI();//add the GUI's
            InitializeBallz();//add the balls
            InitializeBrickz();
            
            Timer Timer1 = new Timer();//create timer
            Timer1.Interval = 10;
            Timer1.Enabled = true;
            Timer1.Start();
            Timer1.Tick += new EventHandler(FixedUpdate);//make timer time.
            InitializeRestartButton();
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
            Graphics drawing = Canvas.CreateGraphics();
            for (int b = 0; b < Ballz.Length; b++)
            {
                Ballz[b].UpdateVars(topWall, botWall, leftWall, rightWall, padPosy);
 
            }
            
            drawBalls(drawing);
            drawPad(drawing);
            drawBricks(drawing);

           CheckCollisions();//if this is inside loop, code doesnt work with less than 5 balls. idk why not
        }

        public void CheckCollisions()
        {
            for (int f = 0; f < Ballz.Length; f++)
            {//check collision with paddle
                Ballz[f].checkPadCollision(pad, ballspeed);
                
                for (int s = f+1; s < Ballz.Length; s++)
                {       //check collision with balls
                    if (Ballz[f].checkBallCollision(Ballz[s]))
                    {
                        Ballz[f].calculateBallCollision(Ballz[s]);
                    } 
                }
                for (int b = 0; b < Brickz.Length; b++)
                {//check collisions with bricks
                    if(Ballz[f].checkBrickCollision(Brickz[b], ballspeed, Brickz[b].isAlive))
                    {
                        Brickz[b].isAlive = false;
                    }
                }
            }
        }
        public void drawBalls(Graphics drawing)
        {
            
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
        }
        public void drawPad(Graphics drawing)
        {
            drawing.FillRectangle(Brushes.Red, pad.getLL(), padPosy, pad.getWidth()/4, 5);//height = 5 
            drawing.FillRectangle(Brushes.Yellow, pad.getML(), padPosy, pad.getWidth()/2, 5);//height = 5 REAL POSITION
            drawing.FillRectangle(Brushes.Blue, pad.getMR()     , padPosy, pad.getWidth()/4, 5);//height = 5
        }
        //draw bricks if not deactivated.
        public void drawBricks(Graphics drawing)
        {
            Brush normal = Brushes.Gray;
            Brush newBall = Brushes.Green;
            Brush speedup = Brushes.Red;
            Brush slowDown = Brushes.Blue;
            for (int i = 0; i < Brickz.Length; i++)
            {
                if (Brickz[i].isAlive)
                {
                    drawing.FillRectangle(normal, Brickz[i].getX() - Brickz[i].getWidth() / 2, Brickz[i].getY() - Brickz[i].getHeight() / 2, Brickz[i].getWidth(), Brickz[i].getHeight());
                }
            }
        }
        public void InitializeBrickz()
        {
            //
            for(int i = 0; i<Brickz.Length; i++)
            {
                if(i < 15)//if I want a special brick, make a small if statement in between.
                Brickz[i] = new Brick(brickGroupPosX + i * brickWidth+i, brickGroupPosY, brickHeight, brickWidth, "normal", true);
                else if(i >= 15 && i < 30)
                Brickz[i] = new Brick(brickGroupPosX + (i - 15) * brickWidth + (i - 15), brickGroupPosY + 1 * brickHeight + 1, brickHeight, brickWidth, "normal", true);
                else if(i >= 30 && i < 45)
                Brickz[i] = new Brick(brickGroupPosX + (i - 30) * brickWidth + (i - 30), brickGroupPosY + 2 * brickHeight + 2, brickHeight, brickWidth, "normal", true);
                else if (i >= 45 && i < 60)
                Brickz[i] = new Brick(brickGroupPosX + (i - 45) * brickWidth + (i - 45), brickGroupPosY + 3 * brickHeight + 3, brickHeight, brickWidth, "normal", true);

            }
        }

        public void InitializeBallz()
        {
            Ballz[0] = new Ball(pad.getLL()-40, padPosy -60, ballspeed, ballspeed, radius);//xPos, yPos, xSpeed,  ySpeed, radius,
            Ballz[1] = new Ball(1000, botWall - radius, 0, 0, radius);
            
            Ballz[2] = new Ball(1000, botWall - radius, 0, 0, radius);
            Ballz[3] = new Ball(1000, botWall - radius, 0, 0, radius);
            Ballz[4] = new Ball(1000, botWall - radius, 0, 0, radius);
        }

        public void restart(object sender, EventArgs e)
        {
            InitializeComponent();
            InitializeGUI();//add the GUI's
            
            InitializeBallz();//add the balls
            InitializeBrickz();
        }
        private void InitializeRestartButton()
        {
            restartButton.Top = 1;
            restartButton.Left = 1;
            restartButton.Text = "Restart";
            restartButton.Click += new EventHandler(restart);
            
            sideCanvas.Controls.Add(restartButton);
        }
        public void InitializeGUI()
        {
            Canvas.Top = (int)topWall;
            Canvas.Left = (int)leftWall;
            Canvas.Height = (int)botWall;
            Canvas.Width = (int)rightWall;
            Canvas.BorderStyle = BorderStyle.FixedSingle;

            sideCanvas.Top = 1;
            sideCanvas.Left = 1010;
            sideCanvas.Height = 500;
            sideCanvas.Width = 100;
            sideCanvas.BorderStyle = BorderStyle.FixedSingle;





            this.Controls.Add(Canvas);
            this.Controls.Add(sideCanvas);
        }
    }
}
