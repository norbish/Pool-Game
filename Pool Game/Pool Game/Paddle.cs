using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pool_Game
{
    class Paddle
    {
        // 3 "blocks" decide outcome of ball.
        // block far left: if movement is positive, force ball upwards. if negative, increase angle. 
        private float leftEndL, leftEndR;//left end of pad
        private float middleL, middleR;
        private float rightEndL, rightEndR;//right end of pad
        private float width;
        private float xPos, yPos;
        private float movespeed = 10;
        private float leftWall, rightWall;

        public Paddle(float x, float y, float leftWall, float rightWall)
        {
            xPos = x; yPos = y;
            updatePoints(x);
            width = x+50;//width is the length of the x value. RR is the point to the furthest right

        }
       
        public void updateVars(bool moveRight)
        {   //oh lord this is so much better than the long if sentences!!
            xPos += moveRight ? movespeed : -movespeed;

            updatePoints(xPos);
        }
        public void updatePoints(float x)//so i dont have to change them in updateVars and Paddle.
        {
            leftEndL = x - 50;
            leftEndR = x - 30;
            middleL = x - 10;
            middleR = x + 10;
            rightEndL = x + 30;
            rightEndR = x + 50;
        }


        public float getX()
        {
            return xPos;
        }
        public float getY()
        {
            return yPos;
        }
        public float getWidth()
        {
            return width;
        }
        public float getLL()
        {
            return leftEndL;
        }
        public float getLR()
        {
            return leftEndR;
        }
        public float getML()
        {
            return middleL;
        }
        public float getMR()
        {
            return middleR;
        }
        public float getRL()
        {
            return rightEndL;
        }
        public float getRR()
        {
            return rightEndR;
        }
    }
}
