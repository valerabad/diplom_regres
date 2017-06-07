using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multiply_regres.StepRegres
{
    class SelectingSigns
    {
        public List<Sign> X_A = new List<Sign>();
        public List<Sign> X_M = new List<Sign>();

        //double[,] x;
        //double[] Y;
        //public SelectingSigns(double [,] mas)
        //{
        //    x = new double[mas.GetLength(0)-1, mas.GetLength(1)];
        //    Y = new double[mas.GetLength(1)];
        //    for (int i = 0; i < mas.GetLength(0)-1; i++)
        //    {
        //        for (int j = 0; j < mas.GetLength(1); j++)
        //        {
        //            x[i, j] = mas[i, j];
        //        }
        //    }

        //    for (int j = 0; j < mas.GetLength(1); j++)
        //    {
        //        Y[j] = mas[mas.GetLength(0)-1, j];
        //    }        
        //}

        public void FindMaxKorelation(List<Sign> list_x, Sign Y)        
        {
            int n = list_x.Count;              
            foreach (Sign x in list_x)
            {
                //Sign x = list_x.ElementAt(i);
                x.r = Korelation(x.x, x.x_srednee, Y.x, Y.x_srednee); // выбираем наиболее корелированный признак
            }
            X_A = list_x;
            Sign max_cor = list_x.Max(); // максимальное по моддулю r
            X_M.Add(max_cor);
            X_A.Remove(max_cor);

        }

        public List<Sign> StepRegresAlgoritm()
        {

            return X_M;
        }

        public bool FindMaxStatistic(double alfa1)
        {
            Sign maxStat =  X_A.OrderByDescending(x => x.t).First(); // выбираем признак с максимальой статистикой
            QuantileFisher quantileFisher = new QuantileFisher();
            int N = FirstAnalis.N_;
            int r = X_M.Count;
            double v1 = 1.0d;
            double v2 = N-r-1.0d;
            double qFisher = quantileFisher.z(alfa1, v1, v2);

            // step 2: сравниваем статистику с квантилем Фишера
            if (maxStat.t > qFisher)
            {
                X_M.Add(maxStat);
                X_A.Remove(maxStat);
                return true;                
            }
            else
                return false;

        }

        public bool FindMinStatistic(double alfa1)
        {
            Sign maxStat = X_M.OrderBy(x => x.t).First(); // выбираем признак с максимальой статистикой
            QuantileFisher quantileFisher = new QuantileFisher();
            int N = FirstAnalis.N_;
            int r = X_M.Count;
            double v1 = 1.0d;
            double v2 = N - r - 1.0d;
            double qFisher = quantileFisher.z(alfa1, v1, v2);

            // step 2: сравниваем статистику с квантилем Фишера
            if (maxStat.t < qFisher)
            {
                X_M.Remove(maxStat);
                //X_A.Add(maxStat); нужно ли добавлять снова в X_A
                return true;
            }
            else
                return false;

        }

        public double Korelation(double[] x, double sr_x, double[] y, double sr_y)
        {
            double m = x.Length;
            double r = 0;
            double tmp=0, tmp2=0, tmp3 = 0;
            for (int i = 0; i < m; i++)
            {
                tmp += (x[i] - sr_x) * (y[i] - sr_y);
                tmp2 += Math.Pow((x[i] - sr_x), 2);
                tmp3 += Math.Pow((y[i] - sr_y), 2);
            }
            r = tmp / Math.Sqrt(tmp2*tmp3);

            return r;
        }
    }
}
