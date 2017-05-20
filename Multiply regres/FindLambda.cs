using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multiply_regres
{
    class FindLambda
    {
        public double[] t(double[] x, double lambda)
        {
            int N = x.Length;
            double[] t = new double[N];
            for (int i = 0; i < N; i++)
            {
                if (lambda != 0)
                    t[i] = (Math.Pow(x[i], lambda) - 1) / lambda;
                else
                    t[i] = Math.Log(x[i]);
            }
            return t;
        }
    }
}
