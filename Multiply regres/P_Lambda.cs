using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multiply_regres
{
    class P_Lambda : IComparable
    {
        public double P;
        public float lambda;

        public P_Lambda(double p_, float l_)
        {
            this.P = p_;
            this.lambda = l_;
        }

        public int CompareTo(object obj)
        {
            P_Lambda obj1 = obj as P_Lambda;
            if (this.P <= ((P_Lambda)obj).P)
                return -1;
            else
            {
                if (this.P >= ((P_Lambda)obj).P)
                    return 1; //Данный экземпляр следует за параметром obj в порядке сортировки.            
                else
                    return 0;
            }
        }
    }
}
