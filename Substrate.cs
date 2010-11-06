using System;
using System.Drawing.Imaging;
using System.Drawing;
using System.Windows.Forms;

namespace SubstrateSharp
{

    public partial class Substrate : Form
    {


        private int dimx = 640;
        private int dimy = 480;
        private int num = 0;
        private int maxnum = 16;

        private int[] cgrid;
        private Crack[] cracks;

        private int maxpal = 512;
        private int numpal = 0;
        private Color[] goodcolor;
        private Bitmap bmd;



        public Substrate()
        {
            InitializeComponent();
            myInitializeComponent();
            goodcolor = new Color[maxpal];
            takecolor("pollockShimmering.gif");
            cgrid = new int[dimx * dimy];
            Crack.mediator = SandPainter.mediator = this;

            cracks = new Crack[maxnum];
            pictureBox1.Image = bmd = new Bitmap(dimx, dimy, PixelFormat.Format24bppRgb);
            background();
            begin();
            this.timer1.Interval = 1000/30 ;//options.Speed; 
            this.timer1.Enabled = true;

        }

        void takecolor(String fn)
        {
            Bitmap b = new Bitmap(fn);

            for (int x = 0; x < b.Width; x++)
            {
                for (int y = 0; y < b.Height; y++)
                {
                    Color c = b.GetPixel(x, y);
                    bool exists = false;
                    for (int n = 0; n < numpal; n++)
                    {
                        if (c == goodcolor[n])
                        {
                            exists = true;
                            break;
                        }
                    }
                    if (!exists)
                    {
                        // add color to pal
                        if (numpal < maxpal)
                        {
                            goodcolor[numpal] = c;
                            numpal++;
                        }
                    }
                }
            }
            b.Dispose();
        }
        void begin()
        {
            // erase crack grid
            for (int y = 0; y < dimy; y++)
            {
                for (int x = 0; x < dimx; x++)
                {
                    cgrid[y * dimx + x] = 10001;
                }
            }
            // make random crack seeds
            for (int k = 0; k < 16; k++)
            {
                int i = Methods.random(dimx * dimy - 1);
                cgrid[i] = Methods.random(360);
            }

            // make just three cracks
            num = 0;
            for (int k = 0; k < 3; k++)
            {
                makeCrack();
            }
            //background();
        }
        public void makeCrack()
        {
            if (num < maxnum)
            {
                // make a new crack instance
                cracks[num] = new Crack();
                num++;
            }
        }


        void draw()
        {
            // crack all cracks
            for (int n = 0; n < num; n++)
            {
                cracks[n].move();
            }
            pictureBox1.Invalidate();
        }

        public Color somecolor()
        {
            // pick some random good color
            return goodcolor[Methods.random(numpal)];
        }
        public int randomx() { return Methods.random(dimx); }
        public int randomy() { return Methods.random(dimy); }
        public bool containsxy(int x, int y)
        {
            return (x >= 0) && (x < dimx) && (y >= 0) && (y < dimy);
        }
        public int getCGrid(int x, int y)
        {
            return cgrid[y * dimx + x];
        }
        public void setCGrid(int x, int y, int value)
        {
            cgrid[y * dimx + x] = value;
        }
        public void setPixel(int x, int y, Color c, double a)
        {

            if (containsxy(x, y))
            {
                bmd.SetPixel(x, y, alphablend(bmd.GetPixel(x, y), c, a));
            }
        }
        private Color alphablend(Color dst, Color src, double a)
        {
            var rd = dst.R;
            var gd = dst.G;
            var bd = dst.B;
            var rs = src.R;
            var gs = src.G;
            var bs = src.B;
            var e = 1 - a;
            var r = rd * e + rs * a;
            var g = gd * e + gs * a;
            var b = bd * e + bs * a;
            return Color.FromArgb(0, (byte)r, (byte)g, (byte)b);
        }

        private void Substrate_Resize(object sender, EventArgs e)
        {
            this.ClientSize = new System.Drawing.Size(dimx, dimy);
            if (this.WindowState == FormWindowState.Maximized)
                this.WindowState = FormWindowState.Normal;

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            begin();
            background();
        }

    }

}
