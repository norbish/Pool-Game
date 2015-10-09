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
        public bool inPlay = false;

        public Ball(float x,float y,float xS,float yS, float r, bool inPlay)
        {
            xPos = x;
            yPos = y;
            xSpeed = xS;
            ySpeed = yS;
            radius = r;
            this.inPlay = inPlay;
        }
        //collision with wall and speeds
        public void UpdateVars(float TopWall, float BotWall,float LeftWall, float RightWall, float padPosy)//updates ball's position 
        {
            xPos = xPos + xSpeed;
            yPos = yPos + ySpeed;
            if (xPos > RightWall -radius)//too far right
            {
                xPos = RightWall - radius;
                xSpeed = -xSpeed;
            }
            if (yPos >= BotWall - radius )//too far down
            {
                yPos = BotWall - radius;
                ySpeed = 0; xSpeed = 5;
                inPlay = false;
                if (xSpeed <= 0)
                    xSpeed = 0;
            }
            if (yPos <= TopWall +radius)//too far up
            {
                yPos = TopWall + radius;
                ySpeed = -ySpeed;
            }
            if (xPos < LeftWall +radius)//too far left
            {
                xPos = LeftWall + radius;
                xSpeed = -xSpeed;
            }
        }
        //check pad colliding with ball
        public void checkPadCollision(Paddle pad, float iSpeed)//maybe not bool. maybe just void.
        {                                 //ball in yAxis inside pad                                                              //ball in xAxis  between points
            if ((yPos + radius >= pad.getY() - pad.getHeight()/2 && yPos + radius <= pad.getY() + pad.getHeight()/2) && (xPos + radius >= pad.getLL() && xPos + radius < pad.getML()))//if left area
            {//bounce back or up(goes back if hit on the side because of xSpeed changed to 0, and next frame still in same "x" of area, so goes back.
                xSpeed = xSpeed > 0 ? 0 : -iSpeed;
                ySpeed = -iSpeed;
            }
            else if ((yPos + radius >= pad.getY() - pad.getHeight()/2 && yPos + radius <= pad.getY() + pad.getHeight()/2) && (xPos + radius >= pad.getML() && xPos - radius <= pad.getMR()))
            {//bounce
                
                ySpeed = -iSpeed;
            }
            else if((yPos + radius >= pad.getY() - pad.getHeight()/2 && yPos + radius <= pad.getY() + pad.getHeight()/2) && (xPos - radius > pad.getMR() && xPos - radius <= pad.getRR()))
            {//bounce back or up
                xSpeed = xSpeed < 0 ? 0 : iSpeed;
                ySpeed = -iSpeed;
            }   
        }
        //brick collision
        public bool checkBrickCollision(Brick b, float iSpeed, bool isAlive)//might need to make to string
        {
            if (isAlive == true)
            {//brick top
                
                if (yPos + radius >= b.top && yPos + radius <= b.top + 5 && xPos >= b.left && xPos <= b.right)//first brick fucked. why
                {
                    ySpeed = -iSpeed;
                    return true;
                }//brick bot
                if (yPos - radius <= b.bot  && yPos - radius >= b.bot -5 && xPos >= b.left && xPos <= b.right)
                {
                    ySpeed = iSpeed;
                    return true;
                }//brick left
                if (yPos >= b.top - 2 && yPos <= b.bot + 2 && xPos + radius >= b.left && xPos + radius <= b.left + 5)
                {
                    xSpeed = -iSpeed;
                    return true;
                }//brick right
                if (yPos >= b.top - 2 && yPos <= b.bot + 2 && xPos - radius >= b.right + 5 && xPos - radius <= b.left)
                {
                    xSpeed = iSpeed;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        
        //check if balls collide
        public bool checkBallCollision(Ball otherBall)
        {
            
            xDist = otherBall.getX()-xPos;//dx
            yDist = otherBall.getY()-yPos;//dy

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
        //calculate the ball angle and velocity, and execute.
        public void calculateBallCollision(Ball otherBall)//this should not run on next frame, or balls will return. 
        {
            double collisionTangent = Math.Atan2((double)yDist, (double)xDist); //returns the angle of the tangent of the vector which is the collision x and y distance.
            double sin = Math.Sin(collisionTangent);
            double cos = Math.Cos(collisionTangent);
            //rotate ball 0
            double B0x = 0;
            double B0y = 0;
            //rotate ball 1
            double B1x = xDist * cos + yDist * sin;
            double B1y = yDist * cos - xDist * sin;
            //rotate ball 0 velocity
            double V0x = xSpeed * cos + ySpeed * sin;
            double V0y = ySpeed * cos - xSpeed * sin;

            double V1x = otherBall.xSpeed * cos + otherBall.ySpeed * sin;
            double V1y = otherBall.ySpeed * cos - otherBall.xSpeed * sin;

            //collision reaction
            double vxtotal = V0x - V1x;
            V0x = ((mass - otherBall.getMass()) * V0x + 2 * otherBall.getMass() * V1x) / (mass + otherBall.getMass());
            V1x = vxtotal + V0x;

            B0x += V0x;
            B1x += V1x;
            //rot pos
            double B0newPosx = B0x * cos - B0y * sin;
            double B0newPosy = B0y * cos + B0x * sin;

            double B1newPosx = B1x * cos - B1y * sin;
            double B1newPosy = B1y * cos + B1x * sin;

            //update pos
            otherBall.xPos = xPos + (float)B1newPosx;
            otherBall.yPos = yPos + (float)B1newPosy;
            xPos = xPos + (float)B0newPosx;
            yPos = yPos + (float)B0newPosy;

            //rot vel
            double B0newVelx = V0x * cos - V0y * sin;
            double B0newVely = V0y * cos + V0x * sin;

            double B1newVelx = V1x * cos - V1y * sin;
            double B1newVely = V1y * cos + V1x * sin;

            xSpeed = (float)B0newVelx;
            ySpeed = (float)B0newVely;
            otherBall.setXspeed((float)B1newVelx);
            otherBall.setYspeed((float)B1newVely);




            //old velocity
            /*double V0Ball1x = xSpeed * cos;
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
            otherBall.xPos += otherBall.xSpeed * otherBall.xSpeed;
            otherBall.yPos += otherBall.ySpeed * otherBall.xSpeed;*/
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
