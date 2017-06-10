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
        List<Sign> _listX;
        private void button3_Click(object sender, EventArgs e)
        {

        }

        //public List<Sign> MasToList()
        //{
        //    //list_x = null;
        //    for (int i = 0; i < mas.GetLength(0); i++) //  не для всех признаков так как есть в конце Y
        //    {
        //        double[] x_ = new double[mas.GetLength(1)];
        //        for (int k = 0; k < mas.GetLength(1); k++)
        //        {
        //            x_[k] = mas[i, k];
        //        }
        //        list_x.Add(new Sign("x" + (i + 1).ToString(), x_, srednee[i], sredn_kvadr_otkl[i], t[i] * t[i]));
        //    }
        //    return list_x;
        //}
        // пошаговая регрессия
        //

        public void StepRegresInvoke(double[,] sended_mas)
        {
            #region отбираем признаки методом пошаговой регрессии
            StepRegres.SelectingSigns selectSign = new StepRegres.SelectingSigns();
            List<Sign> list_x = new List<Sign>(); // измененно, может присести к ошибке
            //list_x = MasToList();

            for (int i = 0; i < sended_mas.GetLength(0); i++) //  не для всех признаков так как есть в конце Y
            {
                double[] x_ = new double[sended_mas.GetLength(1)];
                for (int k = 0; k < sended_mas.GetLength(1); k++)
                {
                    x_[k] = sended_mas[i, k];
                }
                list_x.Add(new Sign("x" + (i + 1).ToString(), x_, srednee[i], sredn_kvadr_otkl[i], t[i] * t[i]));
            }
            _listX = new List<Sign>(list_x);
            Sign y_ = list_x.Last<Sign>();
            Sign Y = new Sign("Y", y_.x, y_.x_srednee, y_.s, Math.Pow(t[t.Length - 1], 2)); // вычисляем t^2
            list_x.RemoveAt(list_x.Count - 1);
            //selectSign.TrueTest(list_x);


            //начало алгоритма пошаговой регрессии
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
            #endregion


            #region Восстанавливаем по отобранным признакам
            List<Sign> res = selectSign.X_M;
            List<Sign> del = selectSign.X_A;

            int N = sended_mas.GetLength(1);

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

            Controls.RecoveryRegres rr = new Controls.RecoveryRegres();
            double[] t_ = rr.Computing_t(mas2);

            //resultListPLP.Add(new ResultPLP(1,1,1));

            rr.Recovery(mas2, selectSign.X_M, dataGridView5, chart2,label3, label2);

            #region вычисление параметров
            //double[] A;// = new double[];
            //Matrix<double> inv_m;
            //double[,] X;
            //double[] Y_;
            //RegressionParams regresParams = new RegressionParams();
            //A = regresParams.FindingA(mas2, out inv_m, out Y_, out X);
            //this.dataGridView5.RowCount = A.Length;
            //for (int i = 1; i < A.Length; i++)
            //{
            //    dataGridView5.Rows[i].HeaderCell.Value = selectSign.X_M.ElementAt(i - 1).name;
            //}

            //for (int i = 0; i < A.Length; i++)
            //{
            //    //dataGridView5.Rows[i].HeaderCell.Value = selectSign.X_M.ElementAt(i).name;
            //    dataGridView5.Rows[i].Cells[0].Value = i;
            //    dataGridView5.Rows[i].Cells[1].Value = A[i];
            //}

            ////статистика
            //t = new double[A.Length]; //  (выносим в класс для доступа из вне) 
            //double s;
            //double[] a__ = new double[A.Length];
            //double[] D = new double[A.Length];
            //FirstAnalis fs = new FirstAnalis(mas2);

            //s = regresParams.S2_Zal(Y_, X, A);
            //for (int i = 0; i < A.Length; i++)
            //{
            //    double y = fs.S2(Matrix<double>.T_(mas2), mas2.GetLength(0) - 1); //fs.S(mas, mas.GetLength(0) - 1);
            //    a__[i] = (Math.Sqrt(fs.S2(X, i)) * A[i]) / Math.Sqrt(y);
            //    dataGridView5.Rows[i].Cells[2].Value = (a__[i]); // стандартизированная оценка

            //    D[i] = s * inv_m.matrix[i, i];
            //    dataGridView5.Rows[i].Cells[3].Value = Math.Sqrt(D[i]); // дисперсия
            //    t[i] = A[i] / (Math.Sqrt(D[i])); // 
            //    dataGridView5.Rows[i].Cells[4].Value = t[i]; // статистика
            //    dataGridView5.Rows[i].Cells[5].Value = t_kvantil; //квантиль
            //    if (Math.Abs(t[i]) < t_kvantil)
            //        dataGridView5.Rows[i].Cells[6].Value = "не значимая";
            //    else
            //        dataGridView5.Rows[i].Cells[6].Value = "значимая";
            //    // доверительній интревал
            //    double a = A[i] - t_kvantil * Math.Sqrt(D[i]);
            //    double b = A[i] + t_kvantil * Math.Sqrt(D[i]);
            //    dataGridView5.Rows[i].Cells[7].Value = "[" + Math.Round(a, 4) + "; " + Math.Round(b, 4) + "]";

            //}
            #endregion


            #endregion

        }
    }
}
