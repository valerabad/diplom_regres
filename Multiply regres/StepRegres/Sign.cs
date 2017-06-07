using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multiply_regres
{
    class Sign : IComparable
    {
        public double[] x;
        public double x_srednee;
        public double s;
        public double r;
        public double t;

        public Sign(double[] x, double x_srednee, double s, double t)
        {
            this.x = x;
            this.x_srednee = x_srednee;
            this.s = s;
            this.t = t;
            //this.r = r;
        }

        public int CompareTo(object obj)
        {
            Sign obj1 = obj as Sign;
            if (Math.Abs(this.r) <= Math.Abs(((Sign)obj).r))
                return -1;
            else
            {
                if (Math.Abs(this.r) >= Math.Abs(((Sign)obj).r))
                    return 1; //Данный экземпляр следует за параметром obj в порядке сортировки.            
                else
                    return 0;
            }
        }
    }
}

