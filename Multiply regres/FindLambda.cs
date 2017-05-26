using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multiply_regres
{
    class FindLambda
    {
        static public double[] t(double[] x, double lambda)
        {
            int N = x.Length;
            double[] t = new double[N];
            lambda = Math.Round(lambda, 2);
            for (int i = 0; i < N; i++)
            {
                if (lambda != 0)
                {
                    double tmp;
                    if (x[i] < 0)
                        tmp = -Math.Pow(-x[i], lambda);
                    else
                        tmp = Math.Pow(x[i], lambda);
                   
                    tmp = tmp - 1.0d;
                    tmp = tmp / lambda;
                    t[i] = tmp;
                }
                    //t[i] = ( - 1.0d) / lambda;
                else
                    t[i] = Math.Log(x[i]);
            }
            return t;
        }

        public List<float> GenerateLambdaList(float a, float b, float step)
        {
            
            List<float> lambdaLst = new List<float>();
            double h = (Math.Abs(a) + Math.Abs(b)) / step;
            lambdaLst.Add(a);
            while (Math.Round(a,1) != b) //Math.Round(a,0
            {
                a = (a + step);
                lambdaLst.Add((float)Math.Round(a,4));
            }
            
            //while ()
            //a = a + h;
            return lambdaLst;
        }
    }
}
