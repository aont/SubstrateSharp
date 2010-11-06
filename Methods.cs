using System;

namespace SubstrateSharp
{
    class Methods
    {
        static public Random rnd = new Random();
        static public int random(int i)
        {
            return rnd.Next(i);
        }
        
        static public int random(int a, int b)
        {
            return rnd.Next(a,a);
        }
        
        static public double random(double a, double b)
        {
            return a + (b - a) * rnd.NextDouble(); 
        }

    }
}
