﻿using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multiply_regres
{
    class Kolmagorov1
    {        
        Dictionary<double, int> varList;
        Dictionary<double, double> empList;
        static int N;
        int count_x;
        double[,] mas;
        double[] x;
        double s; // массив среднеквадртических отклонений
        double m; // массив средних   
        List<double[]> list_x;

        const double b1 = 0.31938153;
        const double b2 = -0.356563782;
        const double b3 = 1.781477937;
        const double b4 = -1.821255978;
        const double b5 = 1.330274429;

        public Kolmagorov1(double[] x, double S, double m)
        {           
            N = x.Length; // 1
            list_x = new List<double[]>();          
               
            count_x = x.GetLength(0); // не понадобится            
            this.x = x;            
            this.m = m;
           
            this.s = Math.Sqrt(S);                                                        

        }

        public void VarSeries(double[] sign)
        {
            Form1.counter++;
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

        public void DefineEmpFunc(double[] mas)
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

        public double Fi(double x)
        {
            double u = this.u(x);
            if (u >= 0)
                return Fi_(x);
            else
            {
                u = Math.Abs(this.u(x));
                double t = 1 / (1 + 0.2316419 * u);
                return 1 -
                (1 - 1 / (Math.Sqrt(2 * Math.PI)) * Math.Pow(Math.E, -(u * u) / 2) *
            (b1 * t + b2 * t * t + b3 * Math.Pow(t, 3) + b4 * Math.Pow(t, 4) + b5 * Math.Pow(t, 5)));
            }
        }

        public double Fi_(double x)
        {
            double u = this.u(x);
            double t = 1 / (1 + 0.2316419d * u);                       
            return 1.0d - (1.0d / (Math.Sqrt(2.0d * Math.PI)) * Math.Pow(Math.E, -(u * u) / 2.0d) *
            (b1 * t + b2 * t * t + b3 * Math.Pow(t, 3) + b4 * Math.Pow(t, 4) + b5 * Math.Pow(t, 5)));            
        }

        public double u(double x)
        {
            //FirstAnalis fs = new FirstAnalis(mas);
            //double s = Math.Sqrt(fs.S(mas, 0));
            double s = this.s; // здесь уже Sqrt, см. конструктор
            //double s = (964.8965d);  //274.5605 964.8965 // тесты для файла 2.txt
            //double m = fs.srednee(mas, 0);
            double m = this.m;
            return (x - m) / s;
        }

        public class xt_list
        {
            public List<xt> list_xt;
            public xt_list()
            {
                list_xt = new List<xt>();
            }
            public void Add(xt xt_) //double[] x, double k
            {                
                list_xt.Add(xt_);
            }
        }

        public class xt
        {
            double[] x;
            //double[] t;
            double k;

            public xt(double[] x_, double k_)
            {
                this.x = x_;
                this.k = k_;
            }
            
        }

        public double K()
        {
            StreamWriter sw = new StreamWriter(@"D:\tmp_lists.txt", true, System.Text.Encoding.Default);
            xt_list listXT = new xt_list();
                                      
                DefineEmpFunc(x); 
                double z = Z(x); // вычисляем Z по каждому столбцу x
                double tmp = 0;
                for (int k = 1; k < 6; k++)
                {
                    double f1 = k * k - 0.5 * (1 - Math.Pow(-1, k));
                    double f2 = 5 * k * k + 22 - 7.5 * (1 - Math.Pow(-1, k));
                    double a0 = (double)Math.Pow(Math.E, -2 * k * k * z * z);

                    double a = Math.Pow((-1), k) * a0;
                    double b = (2 * k * k * z) / (3 * Math.Sqrt(varList.Count));//N
                    // если не поставить .0d , то с=0
                    double c = 1.0d / (18.0d * varList.Count) / ((f1 - 4.0d * (f1 + 3.0d)) * k * k * z * z + 8.0d * Math.Pow(k, 4) * Math.Pow(z, 4));
                    double d = (k * k * z) / (27 * Math.Sqrt(varList.Count * varList.Count * varList.Count)) * (f2 * f2 / 5 - 4 * (f2 + 45) * k * k * z * z / 15 + 8 * Math.Pow(k, 4) * Math.Pow(z, 4)); // N*N*N
                    tmp += a * (1 - b - c + d);
                }
                double temp = 2.0d * tmp;
                double k_ = 1.0d + temp;                              
                sw.Write(k_);
                sw.WriteLine();
            
            sw.Close();
            return k_;
            
        }

        public double Z(double[] x)
        {
            double max = 0;
            double d1 = D1(x);
            double d2 = D2(x);
            if (d1 > d2) max = d1;
                else max = d2;
            return Math.Sqrt(varList.Count) * max; //N
        }

        private double[] SortMas(double[,] mas)
        {
            double[] sort_x = new double[mas.GetLength(1)];
            for (int i = 0; i < mas.GetLength(1); i++)
            {
                sort_x[i] = mas[0, i];
            }
            Array.Sort(sort_x);
            return sort_x;
        }

        string writePath = @"D:\tmp.txt";
        
        public double D1(double[] x)
        {
            Array.Sort(x);
            List<double> lst = new List<double>();
            //StreamWriter sw = new StreamWriter(writePath, true, System.Text.Encoding.Default);
            for (int i = 0; i < N; i++) // от 1 или от 0 ?
            {
                double empFunction = EmpFunc(x[i]);
                double normRaspr = Fi(x[i]);
                //sw.Write(x + "    " + empFunction + "     "+ normRaspr);
                //sw.WriteLine();
                double item = Math.Abs(empFunction - normRaspr);
                lst.Add(item);
            }
            //sw.Close();
            return lst.Max();
        }

        public double D2(double[] x)
        {
            Array.Sort(x);
            List<double> lst = new List<double>();
            for (int i = 1; i < N; i++)
            {
                double empFunction = EmpFunc(x[i]);
                double normRaspr = Fi(x[i-1]);
                double item = Math.Abs(empFunction - normRaspr);
                lst.Add(item);
            }
            return lst.Max();
        }
    }
}