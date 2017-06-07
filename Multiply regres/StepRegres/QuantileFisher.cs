using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multiply_regres.StepRegres
{
    class QuantileFisher
    {       
        const double c0 = 2.515517d;
        const double c1 = 0.802853d;
        const double c2 = 0.010328d;
        const double d1 = 1.432788d;
        const double d2 = 0.1892659d;
        const double d3 = 0.001308d;

        public double Fi(double alfa)
        {
            double t = Math.Sqrt(-2.0d*Math.Log(alfa));            
            double fi_p = t -(c0 + c1 * t + c2 * t * t) / (1.0d + d1 * t + d2 * t * t + d3 * t * t * t);
            return fi_p;
        }

        public double u(double p)
        {
            if (p <= 0.5d)
                return -Fi(p);
            else
                return Fi(1.0d - p);
        }

        public double t_(double p)
        {
            return Math.Sqrt(Math.Log( 1/(p*p) ));
        }

        public double z(double p, double v1, double v2)
        {
            //p = 0.975 = 1 - 0,05 / 2
            double up = u(1 - p/2);
            double sigma = 1.0d / v1 + 1.0 / v2;
            double delta = 1.0d / v1 - 1.0 / v2;
            double a1 = up * Math.Sqrt(sigma / 2.0d) - 1.0d / 6.0d * delta * (up * up + 2.0d); //+
            double a2 = Math.Sqrt(sigma / 2.0d) * (sigma / 24 * (up * up + 3 * up) + (1.0d / 72.0d) * 
                        (delta * delta / sigma) * (Math.Pow(up, 3) + 11.0d * up)); // -
            double a3 = (delta * sigma / 120.0d) * (Math.Pow(up, 4) + 9.0d * Math.Pow(up, 2) + 8); //+
            double a4 = Math.Pow(delta, 3) / (3240.0d * sigma) * (3.0d * Math.Pow(up, 4) + 7.0d * Math.Pow(up, 2) - 16.0d); //+
            double aa = Math.Sqrt(sigma / 2.0d);// * 
            double a5 = (sigma * sigma / 1920.0d * (Math.Pow(up, 5) + 20.0d * Math.Pow(up, 3) + 15 * up));
            double a6 = Math.Pow(delta, 4) / 2880.0d * (Math.Pow(up, 5) + 44.0d * Math.Pow(up, 3) + 183.0d * up);
            double a7 = Math.Pow(delta, 4) / (155520.0d *sigma*sigma) * (9*Math.Pow(up, 5) - 284.0d * Math.Pow(up, 3) - 1513.0d * up);

            double z_ = a1 + a2 - a3 + a4 + aa * (a5+a6+a7);
            return Math.Pow(Math.E,(2.0d*z_));
        }           
    }
}
