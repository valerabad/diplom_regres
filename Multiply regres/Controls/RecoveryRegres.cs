using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms.DataVisualization.Charting;

using System.Text;
using System.Threading.Tasks;

namespace Multiply_regres.Controls
{
    class RecoveryRegres
    {
        public const double t_kvantil = 1.960d;

        public double[] RecoveryAndShow(double[,] mas2, DataGridView dataGridView5, Chart chart2,
            Label label3, Label label2)
        {
            double[] t;
            double[] A;// = new double[];
            Matrix<double> inv_m;
            double[,] X;
            double[] Y_;
            RegressionParams regresParams = new RegressionParams();
            A = regresParams.FindingA(mas2, out inv_m, out Y_, out X);
            dataGridView5.RowCount = A.Length;
            //dataGridView5.ColumnCount = 10;
            for (int i = 0; i < A.Length; i++)  
            {
                dataGridView5.Rows[i].HeaderCell.Value = "x_" + i;
                dataGridView5.Rows[i].Cells[0].Value = i;
                dataGridView5.Rows[i].Cells[1].Value = A[i];
            }

            //статистика
            t = new double[A.Length]; //  (выносим в класс для доступа из вне) 
            double s;
            double[] a__ = new double[A.Length];
            double[] D = new double[A.Length];
            FirstAnalis fs = new FirstAnalis(mas2);

            s = regresParams.S2_Zal(Y_, X, A);
            for (int i = 0; i < A.Length; i++)
            {
                double y = fs.S2(Matrix<double>.T_(mas2), mas2.GetLength(0) - 1); //fs.S(mas, mas.GetLength(0) - 1);
                a__[i] = (Math.Sqrt(fs.S2(X, i)) * A[i]) / Math.Sqrt(y);
                dataGridView5.Rows[i].Cells[2].Value = (a__[i]); // стандартизированная оценка

                D[i] = s * inv_m.matrix[i, i];
                dataGridView5.Rows[i].Cells[3].Value = Math.Sqrt(D[i]); // дисперсия
                t[i] = A[i] / (Math.Sqrt(D[i])); // 
                dataGridView5.Rows[i].Cells[4].Value = t[i]; // статистика
                dataGridView5.Rows[i].Cells[5].Value = t_kvantil; //квантиль
                if (Math.Abs(t[i]) < t_kvantil)
                    dataGridView5.Rows[i].Cells[6].Value = "не значимая";
                else
                    dataGridView5.Rows[i].Cells[6].Value = "значимая";
                //доверительній интревал
                double a = A[i] - t_kvantil * Math.Sqrt(D[i]);
                double b = A[i] + t_kvantil * Math.Sqrt(D[i]);
                dataGridView5.Rows[i].Cells[7].Value = "[" + Math.Round(a, 4) + "; " + Math.Round(b, 4) + "]";


            }
            #region R2, F-тест, диагностическая диаграмма      

            double n = mas2.GetLength(0); // кол-во столбцов
            double N = mas2.GetLength(1); // кол-во строк  
            double s_ = fs.S2(Matrix<double>.T_(mas2), mas2.GetLength(0) - 1); //fs.S_non(Y); 
            double r1 = ((s) / (s_));
            double r2 = (N - n) / (N - 1);  // deleted -1 
            double R_kvadrat = (1.0d - r1 * r2);

            label3.Text = Math.Round(R_kvadrat, 4).ToString();
            double F = ((N - n) / (n - 1)) * ((1.0d / (1.0d - R_kvadrat)) - 1); //0.48510988
            label2.Text = Math.Round(F, 7).ToString();
            DiagnosticChart diagnosticChart = new DiagnosticChart();
            //double[,] mas3 = new double[mas.GetLength(0) - 1, mas.GetLength(1)];

            diagnosticChart.BuildChart(A, X, Y_, chart2.Series[0]);
            #endregion

            return t;
        }

        public double[] Computing_t(double[,] mas2)
        {
            double[] t;
            double[] A;// = new double[];
            Matrix<double> inv_m;
            double[,] X;
            double[] Y_;
            RegressionParams regresParams = new RegressionParams();
            A = regresParams.FindingA(mas2, out inv_m, out Y_, out X);
            //this.dataGridView5.RowCount = A.Length;
            //for (int i = 0; i < A.Length; i++)
            //{
            //    dataGridView5.Rows[i].HeaderCell.Value = "x_" + i;
            //    dataGridView5.Rows[i].Cells[0].Value = i;
            //    dataGridView5.Rows[i].Cells[1].Value = A[i];
            //}

            //статистика
            t = new double[A.Length]; //  (выносим в класс для доступа из вне) 
            double s;
            double[] a__ = new double[A.Length];
            double[] D = new double[A.Length];
            FirstAnalis fs = new FirstAnalis(mas2);

            s = regresParams.S2_Zal(Y_, X, A);
            for (int i = 0; i < A.Length; i++)
            {
                double y = fs.S2(Matrix<double>.T_(mas2), mas2.GetLength(0) - 1); //fs.S(mas, mas.GetLength(0) - 1);
                a__[i] = (Math.Sqrt(fs.S2(X, i)) * A[i]) / Math.Sqrt(y);
                //dataGridView5.Rows[i].Cells[2].Value = (a__[i]); // стандартизированная оценка

                D[i] = s * inv_m.matrix[i, i];
                //dataGridView5.Rows[i].Cells[3].Value = Math.Sqrt(D[i]); // дисперсия
                t[i] = A[i] / (Math.Sqrt(D[i])); // 
                //dataGridView5.Rows[i].Cells[4].Value = t[i]; // статистика
                //dataGridView5.Rows[i].Cells[5].Value = t_kvantil; //квантиль
                //if (Math.Abs(t[i]) < t_kvantil)
                //    dataGridView5.Rows[i].Cells[6].Value = "не значимая";
                //else
                //    dataGridView5.Rows[i].Cells[6].Value = "значимая";
                // доверительній интревал
                //double a = A[i] - t_kvantil * Math.Sqrt(D[i]);
                //double b = A[i] + t_kvantil * Math.Sqrt(D[i]);
                //dataGridView5.Rows[i].Cells[7].Value = "[" + Math.Round(a, 4) + "; " + Math.Round(b, 4) + "]";

            
            }
            return t;
        }

        public double[] Recovery(double[,] mas2, List<Sign> X_M, DataGridView dataGridView5, Chart chart2, 
            Label label3, Label label2)
        {
            double[] t;
            double[] A;// = new double[];
            Matrix<double> inv_m;
            double[,] X;
            double[] Y_;
            RegressionParams regresParams = new RegressionParams();
            A = regresParams.FindingA(mas2, out inv_m, out Y_, out X);
            dataGridView5.RowCount = A.Length;
            //dataGridView5.ColumnCount = X_M.Count + 1;
            for (int i = 1; i < A.Length; i++)
            {
                dataGridView5.Rows[i].HeaderCell.Value = X_M.ElementAt(i - 1).name;
            }

            for (int i = 0; i < A.Length; i++)
            {                
                dataGridView5.Rows[i].Cells[0].Value = i;
                dataGridView5.Rows[i].Cells[1].Value = A[i];
            }

            //статистика
            t = new double[A.Length]; //  (выносим в класс для доступа из вне) 
            double s;
            double[] a__ = new double[A.Length];
            double[] D = new double[A.Length];
            FirstAnalis fs = new FirstAnalis(mas2);

            s = regresParams.S2_Zal(Y_, X, A);
            for (int i = 0; i < A.Length; i++)
            {
                double y = fs.S2(Matrix<double>.T_(mas2), mas2.GetLength(0) - 1); //fs.S(mas, mas.GetLength(0) - 1);
                a__[i] = (Math.Sqrt(fs.S2(X, i)) * A[i]) / Math.Sqrt(y);
                dataGridView5.Rows[i].Cells[2].Value = (a__[i]); // стандартизированная оценка

                D[i] = s * inv_m.matrix[i, i];
                dataGridView5.Rows[i].Cells[3].Value = Math.Sqrt(D[i]); // дисперсия
                t[i] = A[i] / (Math.Sqrt(D[i])); // 
                dataGridView5.Rows[i].Cells[4].Value = t[i]; // статистика
                dataGridView5.Rows[i].Cells[5].Value = t_kvantil; //квантиль
                if (Math.Abs(t[i]) < t_kvantil)
                    dataGridView5.Rows[i].Cells[6].Value = "не значимая";
                else
                    dataGridView5.Rows[i].Cells[6].Value = "значимая";
                // доверительній интревал
                double a = A[i] - t_kvantil * Math.Sqrt(D[i]);
                double b = A[i] + t_kvantil * Math.Sqrt(D[i]);
                dataGridView5.Rows[i].Cells[7].Value = "[" + Math.Round(a, 4) + "; " + Math.Round(b, 4) + "]";

            }

            #region R2, F-тест, диагностическая диаграмма      
            
            double n = mas2.GetLength(0); // кол-во столбцов
            double N = mas2.GetLength(1); // кол-во строк  
            double s_ = fs.S2(Matrix<double>.T_(mas2), mas2.GetLength(0) - 1); //fs.S_non(Y); 
            double r1 = ((s) / (s_));
            double r2 = (N - n) / (N - 1);  // deleted -1 
            double R_kvadrat = (1.0d - r1 * r2);

            label3.Text = Math.Round(R_kvadrat, 4).ToString();
            double F = ((N - n) / (n - 1)) * ((1.0d / (1.0d - R_kvadrat)) - 1); //0.48510988
            label2.Text = Math.Round(F, 7).ToString();
            DiagnosticChart diagnosticChart = new DiagnosticChart();
            //double[,] mas3 = new double[mas.GetLength(0) - 1, mas.GetLength(1)];

            diagnosticChart.BuildChart(A, X, Y_, chart2.Series[0]);
            #endregion

            return t;
        }
    }
}
