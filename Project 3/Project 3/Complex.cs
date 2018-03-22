using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_3
{
    class Complex
    {
        private double real;
        private double imag;
 
        public Complex(double re, double im)
            {
            real = re;
            imag = im;
            }
        public Complex(double re)
        {
            real = re;
            imag = 0;
        }
        public double getReal()
        {
            return real;
        }
        public double getImag()
        {
            return imag;
        }
        public double absolute()
        {
            return Math.Sqrt(absoluteSq());
        }
        public Complex add(Complex x)
        {
            return new Complex(real + x.getReal(), imag + x.getImag());
        }
        public Complex minus(Complex x)
        {
            return new Complex(real - x.getReal(), imag - x.getImag());
        }
        public Complex multiply(Complex x)
        {
            return new Complex(real * x.getReal() - imag * x.getImag(), real * x.getImag() + imag * x.getReal());
        }
        public Complex division(double d)
        {
            return new Complex(real / d, imag / d);
        }
        public Complex conjugate()
        {
            return new Complex(real, -imag);
        }
        public double absoluteSq()
        {
            return real * real + imag * imag;
        }
        public String toString()
        {
            return real + " - " + imag + "i";
        }
    }
}
