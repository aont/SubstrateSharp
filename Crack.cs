using System;
using System.Drawing;

namespace SubstrateSharp
{
    public class Crack
    {
        public static Substrate mediator;
        double x, y;
        double t;    // direction of travel in degrees

        // sand painter
        SandPainter sp;

        public Crack()
        {
            //this.mediator = mediator;
            // find placement along existing crack
            findStart();
            sp = new SandPainter();
        }
        void findStart()
        {
            // pick random point
            int px = 0;
            int py = 0;

            // shift until crack is found
            bool found = false;
            int timeout = 0;
            while ((!found) || (timeout++ > 1000))
            {
                px = mediator.randomx();
                py = mediator.randomy();
                if (mediator.getCGrid(px, py) < 10000)
                {
                    found = true;
                }
            }

            if (found)
            {
                // start crack
                int a = mediator.getCGrid(px, py);
                if (Methods.random(100) < 50)
                {
                    a -= 90 + (int)Methods.random(-2.0, 2.1);
                }
                else
                {
                    a += 90 + (int)Methods.random(-2.0, 2.1);
                }
                startCrack(px, py, a);
            }
            else
            {
                //println("timeout: "+timeout);
            }
        }
        void startCrack(int X, int Y, int T)
        {
            x = X;
            y = Y;
            t = T;//%360;
            x += 0.61 * Math.Cos(t * Math.PI / 180);
            y += 0.61 * Math.Sin(t * Math.PI / 180);
        }
        public void move()
        {
            // continue cracking
            x += 0.42 * Math.Cos(t * Math.PI / 180);
            y += 0.42 * Math.Sin(t * Math.PI / 180);

            // bound check
            double z = 0.33;
            int cx = (int)(x + Methods.random(-z, z));  // add fuzz
            int cy = (int)(y + Methods.random(-z, z));

            // draw sand painter
            regionColor();


            var px = (int)(x + Methods.random(-z, z));
            var py = (int)(y + Methods.random(-z, z));

            // draw black crack
            mediator.setPixel(px, py, Color.Black, 85 / 256.0);


            if (mediator.containsxy(cx, cy))
            {
                var cg = mediator.getCGrid(cx, cy);
                var acgt = (int)Math.Abs(cg - t);
                // safe to check
                if ((cg > 10000) || (acgt < 5))
                {
                    // continue cracking
                    mediator.setCGrid(cx, cy, (int)t);
                }
                else if (acgt > 2)
                {
                    // crack encountered (not self), stop cracking
                    findStart();
                    mediator.makeCrack();
                }
            }
            else
            {
                // out of bounds, stop cracking
                findStart();
                mediator.makeCrack();
            }
        }

        void regionColor()
        {
            // start checking one step away
            double rx = x;
            double ry = y;

            // find extents of open space
            while (true)
            {
                // move perpendicular to crack
                rx += 0.81 * Math.Sin(t * Math.PI / 180);
                ry -= 0.81 * Math.Cos(t * Math.PI / 180);
                int cx = (int)rx;
                int cy = (int)ry;
                if (mediator.containsxy(cx, cy))
                {
                    // safe to check
                    if (mediator.getCGrid(cx, cy) > 10000)
                    {
                        // space is open
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    break;
                }
            }
            // draw sand painter
            sp.render(rx, ry, x, y);
        }
    }

}
