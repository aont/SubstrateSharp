using System;
using System.Drawing;

namespace SubstrateSharp
{
    class SandPainter
    {
        public static Substrate mediator;
        private Color c;
        private double g;

        public SandPainter()
        {
            //this.mediator = mediator;
            c = mediator.somecolor();
            g = Methods.random(0.01, 0.1);
        }
        public void render(double x, double y, double ox, double oy)
        {
            var dx = (int)(x - ox);
            var dy = (int)(y - oy);
            // modulate gain
            g += Methods.random(-0.050, 0.050);
            g = Math.Min(1.0, Math.Max(0, g));

            // calculate grains by distance
            //int grains = (int)(Math.Sqrt((ox-x)*(ox-x)+(oy-y)*(oy-y)));
            int grains = 64;

            // lay down grains of sand (transparent pixels)
            double w = g / (grains - 1);
            for (int i = 0; i < grains; i++)
            {
                var a = 0.1 - i / (grains * 10.0);
                var b = Math.Sin(Math.Sin(i * w));
                var px = (int)(ox + dx * b);
                var py = (int)(oy + dy * b);
                //mediator.setPixel(px, py, c, a);
                mediator.setPixel(px, py, c, a);
            }
        }
    }
}
