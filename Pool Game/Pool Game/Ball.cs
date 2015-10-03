using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pool_Game
{
    class Ball
    {
        public float xPos, yPos;
        private float xSpeed, ySpeed;
        private float radius;
        private float mass = 0.15F;
        private float xDist, yDist;

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
            xDist = xPos - otherBall.getX();//dx
            yDist = yPos - otherBall.getY();//dy

            float radDist = radius + otherBall.getRadius();

            

            if (radDist * radDist >= (xDist * xDist + yDist * yDist)) //check if distance is bigger than the balls touching range
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void calculateCollision(Ball otherBall)//this should not run on next frame, or balls will return. 
        {
            double collisionTangent = Math.Atan2((double)yDist, (double)xDist); //returns the angle of the tangent of the vector which is the collision x and y distance.
            double sin = Math.Sin(collisionTangent);
            double cos = Math.Cos(collisionTangent);

            //old velocity
            double V0Ball1x = xSpeed * cos;
            double V0Ball1y = ySpeed * sin;
            double V0Ball2x = otherBall.getXspeed() * cos;
            double V0Ball2y = otherBall.getYspeed() * sin;

            //new velocity
            double V1Ball1x = ((mass - otherBall.getMass()) / (mass + otherBall.getMass())) * V0Ball1x + (2 * otherBall.getMass() / (mass + otherBall.getMass())) * V0Ball2x;
            double V1Ball1y = (2 * mass / (mass + otherBall.getMass())) * V0Ball1x + ((otherBall.getMass() - mass) / (otherBall.getMass() + mass)) * V0Ball2x;
            double V1Ball2x = V0Ball1y;
            double V1Ball2y = V0Ball2y;

            //set new velocity
            xSpeed = (float)V1Ball1x;
            ySpeed = (float)V1Ball1y;
            xPos += xSpeed *3;
            yPos += ySpeed *3;
            otherBall.setXspeed((float)V1Ball2x);
            otherBall.setYspeed((float)V1Ball2y);
            otherBall.xPos += otherBall.xSpeed *3;
            otherBall.yPos += otherBall.ySpeed *3;
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
        public float getMass()
        {
            return mass;
        }

        public void setXspeed(float xSp)
        {
            xSpeed = xSp;
        }
        public void setYspeed(float ySp)
        {
            ySpeed = ySp;
        }
    }
}
