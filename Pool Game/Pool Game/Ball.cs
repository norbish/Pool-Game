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

            if (xPos > RightWall)//too far right
            {
                xPos = RightWall;
                xSpeed = -xSpeed;
            }
            if (yPos > BotWall)//too far down
            {
                yPos = BotWall;
                ySpeed = -ySpeed;
            }
            if (xPos < TopWall)//too far up
            {
                xPos = TopWall;
                xSpeed = -xSpeed;
            }
            if (yPos < LeftWall)//too far left
            {
                yPos = LeftWall;
                ySpeed = -ySpeed;
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
