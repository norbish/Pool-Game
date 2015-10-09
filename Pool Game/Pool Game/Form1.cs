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
        private static float padHeight = 5;

        private float brickGroupPosX = 150;
        private float brickGroupPosY = 100;
        private float brickWidth = 50;
        private float brickHeight = 25;
        int normal = 0;
        int doubleBall = 1;
        int slowBalls = 2;
        int fastBalls = 3;
        
        public float ballspeed = 2F;
        float radius =6F;
        
        Ball[] Ballz = new Ball[5];
        Brick[] Brickz = new Brick[45];

        Paddle pad = new Paddle(padMidPos, padPosy, leftWall, rightWall, padHeight);

        static Panel Canvas = new Panel();
        static Panel sideCanvas = new Panel();
        static Label score = new Label();
        Button restartButton = new Button();
        Button startButton = new Button();

        int tempBrickNumber;
        int tempBallNumber;
        private bool hasStarted = false;

        private int numBricksDestroyed = 0;
        private int numBallsActive = 1;

        public Form1()
        {
            InitializeComponent();
            InitializeGUI();//add the GUI's
            InitializeBallz();//add the balls
            InitializeBrickz();
            InitializeSidepanelObjects();
            
            Timer Timer1 = new Timer();//create timer
            Timer1.Interval = 10;
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
            Graphics drawing = Canvas.CreateGraphics();
            for (int b = 0; b < Ballz.Length; b++)
            {
                Ballz[b].UpdateVars(topWall, botWall, leftWall, rightWall, padPosy);
 
            }
            if (!hasStarted)//sets ball position to paddle position before game has started
                Ballz[0].xPos = pad.getX();

            drawBalls(drawing);
            drawPad(drawing);
            drawBricks(drawing);

           CheckCollisions();//if this is inside loop, code doesnt work with less than 5 balls. 
        }

        public void CheckCollisions()
        {
            for (int f = 0; f < Ballz.Length; f++)//can change length with numballsactive
            {//check collision with paddle
                Ballz[f].checkPadCollision(pad, ballspeed);
                
                for (int s = f+1; s < Ballz.Length; s++)
                {       //check collision with balls
                    if (Ballz[f].checkBallCollision(Ballz[s]))
                    {//COLLISION
                        Ballz[f].calculateBallCollision(Ballz[s]);
                    } 
                }
                for (int b = 0; b < Brickz.Length; b++)
                {//check collisions with bricks
                    if(Ballz[f].checkBrickCollision(Brickz[b], ballspeed, Brickz[b].isAlive))
                    {//COLLISION
                        Brickz[b].isAlive = false;
                        numBricksDestroyed += 1;
                        score.Text = numBricksDestroyed.ToString();
                        //check bricktype and react accordingly
                        if(Brickz[b].brickType == 1)
                        {
                            tempBrickNumber = b;
                            tempBallNumber = f;
                            DropNewBall(b, f);
                        }
                        switch(Brickz[b].brickType)
                        {
                            case 1: tempBrickNumber = b; tempBallNumber = f; DropNewBall(b, f); break;// drop ball on destroyed brick
                            case 2: Ballz[f].setXspeed(Ballz[f].getXspeed() * (float)1.3); Ballz[f].setYspeed(Ballz[f].getYspeed() * (float)1.3); break;// set slow speed on ball
                            case 3: Ballz[f].setXspeed(Ballz[f].getXspeed() * (float)1.3); Ballz[f].setYspeed(Ballz[f].getYspeed() * (float)1.3); break;// set fast speed on ball
                            default: break;
                        }
                        

                    }
                }
            }
        }
        public void drawBalls(Graphics drawing)
        {
            
            drawing.Clear(Canvas.BackColor);
            for(int b = 0; b < numBallsActive; b++)//can be removed if drawBalls() is put in for loop
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
            drawing.FillRectangle(Brushes.Red, pad.getLL(), padPosy, pad.getWidth()/4, padHeight);//height = 5 
            drawing.FillRectangle(Brushes.Yellow, pad.getML(), padPosy, pad.getWidth()/2, padHeight);//height = 5 REAL POSITION
            drawing.FillRectangle(Brushes.Blue, pad.getMR()     , padPosy, pad.getWidth()/4, padHeight);//height = 5
        }
        //draw bricks if not deactivated.
        public void drawBricks(Graphics drawing)
        {
            Brush normal = Brushes.Gray;
            Brush newBall = Brushes.Green;
            Brush slowDown = Brushes.Blue;
            Brush speedup = Brushes.Orange;
            
            for (int i = 0; i < Brickz.Length; i++)
            {
                if (Brickz[i].isAlive && Brickz[i].brickType == 0)//normal balls/gray
                {
                    drawing.FillRectangle(normal, Brickz[i].getX() - Brickz[i].getWidth() / 2, Brickz[i].getY() - Brickz[i].getHeight() / 2, Brickz[i].getWidth(), Brickz[i].getHeight());
                }
                if (Brickz[i].isAlive && Brickz[i].brickType == 1)//newball / green
                {
                    drawing.FillRectangle(newBall, Brickz[i].getX() - Brickz[i].getWidth() / 2, Brickz[i].getY() - Brickz[i].getHeight() / 2, Brickz[i].getWidth(), Brickz[i].getHeight());
                }
                if (Brickz[i].isAlive && Brickz[i].brickType == 2)//slow ball / blue
                {
                    drawing.FillRectangle(slowDown, Brickz[i].getX() - Brickz[i].getWidth() / 2, Brickz[i].getY() - Brickz[i].getHeight() / 2, Brickz[i].getWidth(), Brickz[i].getHeight());
                }
                if (Brickz[i].isAlive && Brickz[i].brickType == 3)//fast ball / orange
                {
                    drawing.FillRectangle(speedup, Brickz[i].getX() - Brickz[i].getWidth() / 2, Brickz[i].getY() - Brickz[i].getHeight() / 2, Brickz[i].getWidth(), Brickz[i].getHeight());
                }
            }
        }
        public void InitializeBrickz()
        {
            //
            for(int i = 0; i<Brickz.Length; i++)//SETS LOCATION AND TYPES OF BRICKS. initializing.
            {
                if(i < 5)//if I want a special brick, make a small if statement in between.
                Brickz[i] = new Brick(brickGroupPosX + i * brickWidth + i, brickGroupPosY, brickHeight, brickWidth, slowBalls, true);//slow balls / blue

                if (i >= 5 && i < 10)
                Brickz[i] = new Brick(brickGroupPosX + i * brickWidth + i, brickGroupPosY, brickHeight, brickWidth, normal, true);//normal / gray

                if (i >= 10 && i < 15)
                Brickz[i] = new Brick(brickGroupPosX + i * brickWidth + i, brickGroupPosY, brickHeight, brickWidth, fastBalls, true);//fast balls / orange

                else if(i >= 15 && i < 30)
                Brickz[i] = new Brick(brickGroupPosX + (i - 15) * brickWidth + (i - 15), brickGroupPosY + 1 * brickHeight + 1, brickHeight, brickWidth, doubleBall, true);//double ball / green
                else if(i >= 30 && i < 45)
                Brickz[i] = new Brick(brickGroupPosX + (i - 30) * brickWidth + (i - 30), brickGroupPosY + 2 * brickHeight + 2, brickHeight, brickWidth, normal, true);//normal / gray
                else if (i >= 45 && i < 60)
                Brickz[i] = new Brick(brickGroupPosX + (i - 45) * brickWidth + (i - 45), brickGroupPosY + 3 * brickHeight + 3, brickHeight, brickWidth, normal, true);//normal / gray
                //                                        (xPos,                                    yPos,                           height,    width, bricktype, is active?)
            }
        }

        public void InitializeBallz()
        {
            Ballz[0] = new Ball(pad.getX(), padPosy-10, 0, 0, radius, true);//xPos, yPos, xSpeed,  ySpeed, radius, inPlay
            Ballz[1] = new Ball(1000, botWall - radius, 0, 0, radius, false);
            
            Ballz[2] = new Ball(1000, botWall - radius, 0, 0, radius, false);
            Ballz[3] = new Ball(1000, botWall - radius, 0, 0, radius, false);
            Ballz[4] = new Ball(1000, botWall - radius, 0, 0, radius, false);
        }
        public void DropNewBall(int b, int f)
        {
            int count = 0;
           while(count < Ballz.Length)
            {
                if(!Ballz[count].inPlay)
                {
                    Ballz[count] = new Ball(Brickz[b].getX(), Brickz[b].getY(), Ballz[f].getXspeed() , Ballz[f].getYspeed(), radius, true);
                    numBallsActive += 1;
                    count = Ballz.Length;
                }
                count++;
            }
        }
        

        public void restart(object sender, EventArgs e)
        {
           
            
            InitializeBallz();//add the balls
            InitializeBrickz();
            hasStarted = false;
        }
        public void StartBall(object sender, EventArgs e)
        {
            Ballz[0].setXspeed(ballspeed);
            Ballz[0].setYspeed(-ballspeed);
            hasStarted = true;
        }
        
        private void InitializeSidepanelObjects()
        {
            restartButton.Top = 570;
            restartButton.Left = 1;
            restartButton.Text = "Restart";
            restartButton.Click += new EventHandler(restart);

            startButton.Top = 540;
            startButton.Left = 1;
            startButton.Text = "start";
            startButton.Click += new EventHandler(StartBall);

            score.Top = 1;
            score.Left = 1;
            score.Text = numBricksDestroyed.ToString();

            sideCanvas.Controls.Add(restartButton);
            sideCanvas.Controls.Add(startButton);
            sideCanvas.Controls.Add(score);
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
            sideCanvas.Height = 600;
            sideCanvas.Width = 80;
            sideCanvas.BorderStyle = BorderStyle.FixedSingle;





            this.Controls.Add(Canvas);
            this.Controls.Add(sideCanvas);
        }
    }
}
