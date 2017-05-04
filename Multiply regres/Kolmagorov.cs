using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multiply_regres
{
    class Kolmagorov
    {
        Dictionary<double, int> varList;
        Dictionary<double, double> empList;
        static int N;

        public void VarSeries(double[,] mas)
        {
            N = mas.GetLength(1);
            double[,] vr = mas;
            // сделать цикл по всем переменным, пока по 1-ой
            double[] sign = new double[mas.GetLength(1)];
            for (int i = 0; i < sign.Length; i++)
            {
                sign[i] = vr[0, i];
            }
            Array.Sort(sign);

            int freqc = 1;
            varList = new Dictionary<double, int>();
            for (int i = 0; i < sign.Length - 1; i++)
            {
                if (sign[i] == sign[i + 1])
                {
                    freqc = 1;
                    while (i < sign.Length - 1 && sign[i] == sign[i + 1])
                    {
                        freqc++;
                        i++;
                    }
                    varList.Add(sign[i - 1], freqc);
                }
                else
                {
                    {
                        freqc = 1;
                        varList.Add(sign[i], freqc);
                    }
                }

                if (sign.Length - 2 == i)
                {
                    freqc = 1;
                    varList.Add(sign[i + 1], freqc);
                }

            }            
        }        

        public void DefineEmpFunc(double[,] mas)
        {
            this.VarSeries(mas);
            empList = new Dictionary<double, double>();

            double sum = 0.0d;
            foreach (var sign in varList )
            {
                sum = sum + (double)sign.Value / N;
                empList.Add(sign.Key, sum);
            }            
        }

        public double EmpFunc(double value)
        {

            double res = empList[value];
            return res;
        }
    }
}
