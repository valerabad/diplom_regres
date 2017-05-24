using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multiply_regres
{
    class FirstAnalis
    {
        double[,] mas;
        int N = 0;
        public FirstAnalis(double[,] mas)
        {
            this.mas = mas;
            this.N = mas.GetLength(1);
        }
        public double srednee(double[,] x, int sign)
        {
            int N = x.GetLength(1);
            double summX = 0;
            for (int j = 0; j < N; j++)
            {
                summX = x[sign, j] + summX;
            }
            return ((1.0d / N) * summX);           
        }

        public double srednee(double[] x)
        {
            int N = x.Length;
            double summX = 0;
            for (int j = 0; j < N; j++)
            {
                summX = x[j] + summX;
            }
            return ((1.0d / N) * summX);
        }

        // Средне квадратическое
        public double S(double[,] x, int sign)
        {
            int N = x.GetLength(1);
            double tmp = 0;
            for (int j = 0; j < N; j++)
            {
                tmp = tmp + x[sign, j];
            }

            double x_ = (1.0d / N) * tmp;
            tmp = 0;
            for (int j = 0; j < N; j++)
            {
                tmp = tmp + (Math.Pow((x[sign, j] - x_), 2));
                //tmp = tmp +((x[sign, j] * x[sign, j] - x_ * x_));
            }
            return ((1.0d / (N-1)) * tmp); // для Колмагорова надо sqrt
        }

        // Средне квадратическое
        public double S(double[] x)
        {
            int N = x.Length;
            double tmp = 0;
            for (int j = 0; j < N; j++)
            {
                tmp = tmp + x[j];
            }

            double x_ = (1.0d / N) * tmp;
            tmp = 0;
            for (int j = 0; j < N; j++)
            {
                tmp = tmp + (Math.Pow((x[j] - x_), 2));
                //tmp = tmp +((x[sign, j] * x[sign, j] - x_ * x_));
            }
            return ((1.0d / (N - 1)) * tmp); // для Колмагорова надо sqrt
        }

        public double S2(double[,] x, int sign)
        {
            int N = x.GetLength(0);
            double tmp = 0;
            for (int j = 0; j < N; j++)
            {
                tmp = tmp + x[j, sign];
            }

            double x_ = (1.0d / N) * tmp;
            tmp = 0;
            for (int j = 0; j < N; j++)
            {
                tmp = tmp + (Math.Pow((x[j, sign] - x_), 2));                
            }
            return (1.0d / (N - 1)) * tmp;
        }

        public double otsenka_sred_kvadr(double sr_otkl)
        {
            return sr_otkl / Math.Sqrt(mas.GetLength(1));
        }

        public double asimetria(int sign)
        {
            double res;
            double tmp = 0;
            int N = mas.GetLength(1);
            double x_sred = this.srednee(mas, sign);
            int k = 0;
            for (int j = 0; j < N; j++)
            {               
                tmp += Math.Pow(mas[sign, j] - x_sred,3);
            }
            double sr_otkl = S(mas, sign);
            return (1.0d/(N* Math.Pow(sr_otkl, 3))) * tmp;
        }

        public double Eksces(int sign)
        {
            double tmp = 0;
            int N = mas.GetLength(1);
            double x_sred = this.srednee(mas, sign);
            int k = 0;
            for (int j = 0; j < N; j++)
            {
                tmp += Math.Pow(mas[sign, j] - x_sred, 4);
            }
            double sr_otkl = S(mas, sign);
            return (1.0d / (N * Math.Pow(sr_otkl, 4))) * tmp;
        }       

        const double u = 1.96d;
        public double IntervalForSrednee_A(double sr)
        {
            double q = sr / (Math.Sqrt(N));
            return sr - u * q;
        }

        public double IntervalForSrednee_B(double sr)
        {
            double q = sr / (Math.Sqrt(N));
            return sr + u * q;
        }

        public double IntervalForDispersia_A(double sr)
        {
            double q = sr / (Math.Sqrt(2 * N));
            return sr - u * q;
        }

        public double IntervalForDispersia_B(double sr)
        {
            double q = sr / (Math.Sqrt(2*N));
            return sr + u * q;
        }

        public double IntervalForAsimetria_A(double sr)
        {
            double q = Math.Sqrt( (6.0d / N) * (1.0d - (12.0d / (2.0d * N + 7.0d) )));
            return sr - u * q;
        }

        public double IntervalForAsimetria_B(double sr)
        {
            double q = Math.Sqrt((6.0d / N) * (1.0d - (12.0d / (2.0d * N + 7.0d))));
            return sr + u * q;
        }

        public double IntervalForEksces_A(double sr)
        {
            double q = Math.Sqrt(24.0d / N * (1.0d - 225.0d / (15.0d * N + 124.0d)));
            return sr - u * q;
        }

        public double IntervalForEksces_B(double sr)
        {
            double q = Math.Sqrt(24.0d / N * (1.0d - 225.0d / (15.0d * N + 124.0d)));
            return sr + u * q;
        }
    }
}
