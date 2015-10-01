using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pool_Game
{
    class Ball
    {
        private float xPos, yPos;
        private float xSpeed, ySpeed;
        private float radius;
        private float mass = 15;
        

        public Ball(float x,float y,float xS,float yS, float r)
        {
            xPos = x;
            yPos = y;
            xSpeed = xS;
            ySpeed = yS;
            radius = r;
        }
        public void UpdateVars(float TopWall, float BotWall,float LeftWall, float RightWall)//updates ball's position 
        {
            xPos = xPos + xSpeed;
            yPos = yPos + ySpeed;

            if (xPos > RightWall -radius)//too far right
            {
                xPos = RightWall - radius;
                xSpeed = -xSpeed;
            }
            if (yPos > BotWall - radius)//too far down
            {
                yPos = BotWall - radius;
                ySpeed = -ySpeed;
            }
            if (xPos < TopWall +radius)//too far up
            {
                xPos = TopWall + radius;
                xSpeed = -xSpeed;
            }
            if (yPos < LeftWall +radius)//too far left
            {
                yPos = LeftWall + radius;
                ySpeed = -ySpeed;
            }
        }
        public bool checkCollision(Ball otherBall)
        {
            float xDist = xPos - otherBall.getX();
            float yDist = yPos - otherBall.getY();

            float radDist = radius + otherBall.getRadius();

            double distSqr = Math.Sqrt((double)xDist * (double)xDist + (double)yDist * (double)yDist);

            if (distSqr <= (double)radDist)
            {
                return true;
            }
            else
            {
                return false;
            }
        }



        public float getX()
        {
            return xPos;
        }
        public float getY()
        {
            return yPos;
        }
        public float getXspeed()
        {
            return xSpeed;
        }
        public float getYspeed()
        {
            return ySpeed;
        }
        public float getRadius()
        {
            return radius;
        }
    }
}
