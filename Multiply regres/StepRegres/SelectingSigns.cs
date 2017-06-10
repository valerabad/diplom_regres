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
        public Sign Y;
        int N;

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
            this.Y = Y;
            N = Y.x.Length;
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

        public double[,] PrepareArray(List<Sign> newList_X)
        {                  
            int countSign = newList_X.Count + 1; // возврашаемся к масиву с Y

            double[,] mas2 = new double[countSign, N];

            int j = 0;
            foreach (Sign item in newList_X)
            {
                for (int i = 0; i < N; i++)
                {
                    mas2[j, i] = item.x[i];
                }
                j++;
            }

            for (int i = 0; i < N; i++)
            {
                mas2[j, i] = Y.x[i]; // y не изменяется
            }

            return mas2;
        }

        public bool FindMaxStatistic(double alfa1)
        {
            //2 вариант
            foreach (Sign x in X_A)
            {                 
                Controls.RecoveryRegres rr = new Controls.RecoveryRegres();
                List<Sign> x_tmp = new List<Sign>();
                x_tmp.Add(x);
                foreach (Sign xm in X_M)
                {
                    x_tmp.Add(xm);
                }               
                
                double[,] preArr = PrepareArray(x_tmp);
                double[] t = rr.Computing_t(preArr);
                x.t = t[1] * t[1];
                //int i = 1; // так как первый столбец свободный член
                //foreach (Sign s in X_A)
                //{
                //    s.t = t[i];
                //    i++;
                //}
                
                
            }           
        
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

        public void TrueTest(List<Sign> list_)
        {
            X_M.Add(list_.ElementAt(2));
            X_M.Add(list_.ElementAt(0));
        }

        public bool FindMinStatistic(double alfa1)
        {
            // пересчитываем t            
                Controls.RecoveryRegres rr = new Controls.RecoveryRegres();
                //List<Sign> x_tmp = new List<Sign>();
                //x_tmp.Add(x);
                //x_tmp.Add(X_M.First());

                double[,] preArr = PrepareArray(X_M);
                double[] t = rr.Computing_t(preArr);
            int h = 1;
            foreach (Sign xm in X_M)
            {
                xm.t = t[h] * t[h];
            }
            //X_M.ElementAt(0).t = t[1] * t[1];
            //X_M.ElementAt(1).t = t[2] * t[2];

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
