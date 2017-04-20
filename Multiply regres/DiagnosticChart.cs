using System;
using System.Windows.Forms.DataVisualization.Charting;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multiply_regres
{
    class DiagnosticChart
    {
        public void BuildChart(double[] a, double[,] x, double[] y, Series chart)
        {
            chart.Points.Clear();
            chart.Name = "Диагностическая диаграмма";
            double[] Y_ = new double[x.GetLength(1)]; // массив на 100
            double[] e = new double[x.GetLength(1)]; // массив на 100
            for (int l = 0; l < x.GetLength(1); l++)
            {
                double tmp = 0;
                for (int k = 1; k < x.GetLength(0); k++)
                {
                    tmp = tmp + a[k] * x[k, l];
                }
                Y_[l] = a[0] + tmp;
                e[l] = y[l] - Y_[l];                
                chart.Points.AddXY(Math.Round(Y_[l],4), Math.Round(e[l],4));
            }
            
        }
    }
}
