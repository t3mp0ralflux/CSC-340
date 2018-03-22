using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_3
{
    class Project3
    {
        public static double PI = Math.PI;
        public static double TWOPI = Math.PI * 2;

        static void Main(string[] args)
        {
            double x = 0;
            Complex[] z = new Complex[512]; //change this number for number of iterations
            for(int i=0; i<z.Length; i++)
            {
                z[i] = new Complex(oddHarmonic(5, i / 512f));

            }

            z = cooleyTukey(1, z);
            double[] r = psd(z);
            for(int i=0; i<z.Length; i++)
            {
                Console.WriteLine(r[i]);
            }
            Console.ReadKey();
        }

        public static double oddHarmonic(int s, double value)
        {
            double sum = 0;
            for (int i=1; i<=s; i++)
            {
                sum += (Math.Sin(TWOPI * ((2 * i-1) * value)) / (2 * i -1));
            }
            return sum;
        }
        public static double evenHarmonic(int s, double value)
        {
            double sum = 0;
            for(int i=1; i<=s; i++)
            {
                sum += (Math.Sin(TWOPI * ((2 * i) * value)) / (2 * i));
            }
            return sum;
        }

        public static Complex[] cooleyTukey(int d, Complex[] z)
        {
            double th = ((-TWOPI) * d) / z.Length;
            int r = z.Length / 2;
            for (int i = 1; i < z.Length; i = 2 * i, r = r / 2)
            {
                Complex w = new Complex(Math.Cos(i * th), Math.Sin(i * th));
                for (int q = 0; q < z.Length; q = q + (2 * r))
                {
                    Complex u = new Complex(1);
                    for(int m=0; m <= (r-1); m++)
                    {
                        Complex t = z[q + m].minus(z[q + m + r]);
                        z[q + m] = z[q + m].add(z[q + m + r]);
                        z[q + m + r] = t.multiply(u);
                        u = w.multiply(u);
                    }
                }
            }
            int k = 0;
            r = 0;
            for (int i=0; i < z.Length; i++)
            {
                r = i;
                k = 0;
                for (int m=1; m<z.Length-1; m = 2 * m)
                {
                    k = 2 * k + (r % 2);
                    r = r / 2;
                }
                if (k > i)
                {
                    Complex t = z[i];
                    z[i] = z[k];
                    z[k] = t;

                }
            }
            if (d > 0)
            {
                for(int i=0; i <z.Length; i++)
                {
                    z[i] = z[i].division(z.Length);
                }
            }

            return z;
        }
        public static double[] psd (Complex[] z)
        {
            double[] powerSpectral = new double[z.Length];
            for (int i=0; i< z.Length; i++)
            {
                powerSpectral[i] = z[i].absolute() * z[i].absolute();
            }
            return powerSpectral;
        }
    }
}
