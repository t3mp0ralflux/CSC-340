using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Project_3
{
    class signal_generator
    {
        List<double> lp_list = new List<double>();
        List<double> hp_list = new List<double>();
        List<double> bp_list = new List<double>();
        List<double> nf_list = new List<double>();

        public double low_pass(int x, int S)
        {
            double temp_sum = 0.0;
            List<double> temp_list = new List<double>();
            double numerator;
            double ans;

            for (int k = 1; k < S + 1; k++)
            {
                numerator = Math.Sin(2 * Math.PI * ((2 * k) - 1) * x);
                ans = numerator / ((2 * k) - 1);
                temp_list.Add(ans);
            }
            temp_list.Sort();


            return temp_sum;
        }

        public double high_pass(int x, int S)
        {
            double temp_sum = 0.0;
            List<double> temp_list = new List<double>();
            List<double> max_list = new List<double>();
            double numerator;
            double ans;

            for (int k = 1; k < S + 1; k++)
            {
                numerator = Math.Sin(2 * Math.PI * ((2 * k) - 1) * x);
                ans = numerator / ((2 * k) - 1);
                temp_list.Add(ans);

            }
            for (int i = 5; i<temp_list.Count-1; i++)
            {
                max_list.Add(temp_list[i]);
            }
            foreach (double item in max_list)
            {
                temp_sum += item;
            }

            return temp_sum;
        }

        public double band_pass(int x, int S)
        {
            double temp_sum = 0.0;
            List<double> temp_list = new List<double>();
            List<double> band_list = new List<double>();
            double numerator;
            double ans;

            for (int k = 1; k < S + 1; k++)
            {
                numerator = Math.Sin(2 * Math.PI * ((2 * k) - 1) * x);
                ans = numerator / ((2 * k) - 1);
                temp_list.Add(ans);

            }
            for (int i = 3; i < 7; i++)
            {
                band_list.Add(temp_list[i]);
            }
            foreach (double item in band_list)
            {
                temp_sum += item;
            }

            return temp_sum;
        }

        public double notch_filter(int x, int S)
        {
            double temp_sum = 0.0;
            List<double> temp_list = new List<double>();
            List<double> notch_list = new List<double>();
            double numerator;
            double ans;

            for (int k = 1; k < S + 1; k++)
            {
                numerator = Math.Sin(2 * Math.PI * ((2 * k) - 1) * x);
                ans = numerator / ((2 * k) - 1);
                temp_list.Add(ans);
            }

            for (int i=0; i<3; i++)
            {
                notch_list.Add(temp_list[i]);
            }
            for (int i=7; i<temp_list.Count-1; i++)
            {
                notch_list.Add(temp_list[i]);
            }
            foreach (double item in notch_list)
            {
                temp_sum += item;
            }

            return temp_sum;
        }

        public double commonsignal1(int x, int S)
        {
            double temp_sum = 0.0;
            double numerator;
            double ans;

            for (int k = 1; k < S + 1; k++)
            {
                numerator = Math.Sin(2 * Math.PI * ((2 * k) - 1) * x);
                ans = numerator / ((2 * k) - 1);
                temp_sum +=ans;
            }

            return temp_sum;
        }

        public double commonsignal2(int x, int S)
        {
            double temp_sum = 0.0;
            double numerator;
            double ans;

            for (int k = 1; k < S+1; k++)
            {
                numerator = Math.Sin(2 * Math.PI * (2 * k) * x);
                ans = numerator / (2 * k);
                temp_sum += ans;    
            }

            return temp_sum;
        }

        public List<double> org_fft(List<double> z, double n)
        {
            int i = 0;
            int r, m, k;
            double t;

            while (i < n)
            {
                r = i;
                k = 0;
                m = 1;
                while (m < n)
                {
                    k = 2 * k + (r % 2);
                    //k = Convert.ToInt64(k);
                    r /= 2;
                    m *= 2;
                }
                if (k > i)
                {
                    t = z[i];
                    z[i] = z[k];
                    z[k] = t;
                }
                i += 1;
            }

            return z;
        }

        public List<double> fft(List<double> z, int d, int n)
        {
            int r, k, u, m, i;
            double t, theta;
            Complex w;
            
            if (d == 1)
            {
                r = n / 2;
                i = 1;
                theta = (-2 * Math.PI * d) / n;
                while (i < n)
                {
                    w = Math.Cos(i*theta) + Math.Sin(i*theta);
                }
            }

        }
    }
}
