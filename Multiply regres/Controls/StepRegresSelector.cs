using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms.DataVisualization.Charting;
using System.Threading;

namespace Multiply_regres
{
    public partial class Form1 : Form
    {
        private void button3_Click(object sender, EventArgs e)
        {

        }
        // пошаговая регрессия
        //
        public void StepRegresInvoke()
        {
            StepRegres.SelectingSigns selectSign = new StepRegres.SelectingSigns();
            List<Sign> list_x = new List<Sign>();
            for (int i = 0; i < mas.GetLength(0); i++) //  не для всех признаков так как есть в конце Y
            {
                double[] x_ = new double[mas.GetLength(1)];
                for (int k = 0; k < mas.GetLength(1); k++)
                {
                    x_[k] = mas[i, k];
                }
                list_x.Add(new Sign(x_, srednee[i], sredn_kvadr_otkl[i], t[i] * t[i]));
            }
            Sign y_ = list_x.Last<Sign>();
            Sign Y = new Sign(y_.x, y_.x_srednee, y_.s, Math.Pow(t[t.Length - 1], 2)); // вычисляем t^2
            list_x.RemoveAt(list_x.Count - 1);

            // начало алгоритма пошаговой регрессии
            bool flag = true;
            selectSign.FindMaxKorelation(list_x, Y);
            while (flag && selectSign.X_A.Count != 0)
            {
                bool fl1 = selectSign.FindMaxStatistic(0.05);
                if (fl1 == false) // статисика оказалась меньше квантиля
                {
                    flag = false;
                    break;
                }
                else // произошло добавление
                {
                    bool fl2 = selectSign.FindMinStatistic(0.1);
                    if (fl2 == false)
                        flag = true;  // куда идти????
                    else // если флаг = true, то => шаг 2
                        flag = true;

                }

            }

            List<Sign> res = selectSign.X_M;
            List<Sign> del = selectSign.X_A;
            int N = mas.GetLength(1);
            int countSign = selectSign.X_M.Count + 1; // возврашаемся к масиву с Y

            double[,] mas2 = new double[countSign, N];

            int j = 0;
            foreach (Sign item in selectSign.X_M)
            {
                for (int i = 0; i < N; i++)
                {
                    mas2[j, i] = item.x[i];
                }
                j++;
            }

            for (int i = 0; i < N; i++)
            {
                mas2[j, i] = Y.x[i];
            }

            double[] A;// = new double[];
            Matrix<double> inv_m;
            double[,] X;
            double[] Y_;
            RegressionParams regresParams = new RegressionParams();
            A = regresParams.FindingA(mas2, out inv_m, out Y_, out X);
            this.dataGridView5.RowCount = A.Length;
            for (int i = 0; i < A.Length; i++)
            {
                dataGridView5.Rows[i].HeaderCell.Value = "x_"+i;
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
            //double N = mas.GetLength(1); // кол-во строк  
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

        }
    }
}
