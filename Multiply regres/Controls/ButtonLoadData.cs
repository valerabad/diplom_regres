﻿using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms.DataVisualization.Charting;
using System.Threading;

namespace Multiply_regres
{
    public partial class Form1 : Form
    {
        double t_kvantil = 1.96;
        public Form1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
        double[,] mas;
        double[] sredn_kvadr_otkl;
        double[] srednee;

        public static int counter;
        private void button1_Click(object sender, EventArgs e)
        {

            // 2 массива для среднеего и среднеквадартического откл. 
            // Нужно для передачи для вычисления функции нормального распределения           

            mas = ReadFileClass.read_and_results(openFileDialog1, dataGridView1);            
             try
            #region Первичный анализ
            {
                textBox1.Text = openFileDialog1.FileName;
                int count_sign = mas.GetLength(0);
                dataGridView2.ColumnCount = count_sign + 1;

                FirstAnalis fs = new FirstAnalis(mas);

                for (int i = 0; i < count_sign; i++)
                {
                    dataGridView2.Columns[i + 1].HeaderCell.Value = string.Format("Признак " + (i + 1).ToString());
                }

                sredn_kvadr_otkl = new double[count_sign];
                srednee = new double[count_sign];

                for (int i = 0; i < count_sign; i++)
                {
                    //среднее  
                    double x_ = fs.srednee(mas, i);
                    srednee[i] = x_;
                    dataGridView2.Rows[0].Cells[i + 1].Value = Convert.ToString(Math.Round(x_));
                    // доверительные интeрвалы
                    double a = Math.Round(fs.IntervalForSrednee_A(x_), 2);
                    double b = Math.Round(fs.IntervalForSrednee_B(x_), 2);
                    dataGridView2.Rows[1].Cells[i + 1].Value = "[" + Convert.ToString(a) + "; " + Convert.ToString(b) + "]";
                    // среднеквадрадическое
                    double sr_otkl_X = Math.Round(fs.S(mas, i), 4);
                    sredn_kvadr_otkl[i] = sr_otkl_X;
                    dataGridView2.Rows[2].Cells[i + 1].Value = Convert.ToString(sr_otkl_X);
                    a = Math.Round(fs.IntervalForSrednee_A(sr_otkl_X), 2);
                    b = Math.Round(fs.IntervalForSrednee_B(sr_otkl_X), 2);
                    dataGridView2.Rows[3].Cells[i + 1].Value = "[" + Convert.ToString(a) + "; " + Convert.ToString(b) + "]";
                    //Асиметрия
                    double asim = Math.Round(fs.asimetria(i), 6);
                    dataGridView2.Rows[4].Cells[i + 1].Value = Convert.ToString(asim);
                    a = Math.Round(fs.IntervalForAsimetria_A(asim), 4);
                    b = Math.Round(fs.IntervalForAsimetria_B(asim), 4);
                    dataGridView2.Rows[5].Cells[i + 1].Value = "[" + Convert.ToString(a) + "; " + Convert.ToString(b) + "]";
                    //Эксцес
                    double ecsces = Math.Round(fs.Eksces(i), 6);
                    dataGridView2.Rows[6].Cells[i + 1].Value = Convert.ToString(ecsces);
                    a = Math.Round(fs.IntervalForEksces_A(ecsces), 4);
                    b = Math.Round(fs.IntervalForEksces_B(ecsces), 4);
                    dataGridView2.Rows[7].Cells[i + 1].Value = "[" + Convert.ToString(a) + "; " + Convert.ToString(b) + "]";
                }
                #endregion

            #region расчёт оценок регрессии
                //checked, 0 - кол-во столбцов         
                double[] A; // = new double[mas.GetLength(0)];                      

                RegressionParams regresParams = new RegressionParams();
                Matrix<double> inv_m;

                double[,] X;
                double[] Y;

                A = regresParams.FindingA(mas, out inv_m, out Y, out X);
                this.dataGridView3.RowCount = A.Length;
                for (int i = 0; i < A.Length; i++)
                {
                    dataGridView3.Rows[i].Cells[0].Value = i;
                    dataGridView3.Rows[i].Cells[1].Value = A[i];
                }

                //статистика
                double s;
                double[] a__ = new double[A.Length];
                double[] D = new double[A.Length];
                double[] t = new double[A.Length];
                s = regresParams.S2_Zal(Y, X, A);
                for (int i = 0; i < A.Length; i++)
                {
                    double y_ = fs.S2(Matrix<double>.T_(mas), mas.GetLength(0) - 1); //fs.S(mas, mas.GetLength(0) - 1);
                    a__[i] = (Math.Sqrt(fs.S2(X, i)) * A[i]) /Math.Sqrt(y_ ); 
                    dataGridView3.Rows[i].Cells[2].Value = (a__[i]); // стандартизированная оценка

                    D[i] = s * inv_m.matrix[i, i];
                    dataGridView3.Rows[i].Cells[3].Value = Math.Sqrt(D[i]); // дисперсия
                    t[i] = A[i] / (Math.Sqrt(D[i])); // 
                    dataGridView3.Rows[i].Cells[4].Value = t[i]; // статистика
                    dataGridView3.Rows[i].Cells[5].Value = t_kvantil; //квантиль
                    if (Math.Abs(t[i]) < t_kvantil)
                        dataGridView3.Rows[i].Cells[6].Value = "не значимая";
                    else
                        dataGridView3.Rows[i].Cells[6].Value = "значимая";
                    // доверительній интревал
                    double a = A[i] - t_kvantil * Math.Sqrt(D[i]);
                    double b = A[i] + t_kvantil * Math.Sqrt(D[i]);
                    dataGridView3.Rows[i].Cells[7].Value = "[" + Math.Round(a, 4) + "; " + Math.Round(b, 4) + "]";
                }
                #endregion
            
            #region R2, F-тест, диагностическая диаграмма            
                double n = mas.GetLength(0); // кол-во столбцов
                double N = mas.GetLength(1); // кол-во строк  
                double s_ = fs.S2(Matrix<double>.T_(mas), mas.GetLength(0)-1); //fs.S_non(Y);         
                double R_kvadrat = (1 - ((s) / (s_)) * ((N - n) / (N - 1))); // deleted -1 
                label_R_value.Text = Math.Round(R_kvadrat, 7).ToString();
                double F = ((N - n) / (n-1)) * ((1.0d / (1.0d - R_kvadrat)) - 1); //0.48510988
                label_F_value.Text = Math.Round(F, 7).ToString();
                DiagnosticChart diagnosticChart = new DiagnosticChart();
                double[,] mas2 = new double[mas.GetLength(0) - 1, mas.GetLength(1)];
                
                diagnosticChart.BuildChart(A, X, Y, chart1.Series[0]);
                #endregion

                //#region Критерий Колмагорова
                //Kolmagorov kolm = new Kolmagorov(mas, sredn_kvadr_otkl, srednee);
                ////kolm.DefineEmpFunc(mas);
                //Kolmagorov.xt_list k = kolm.K();

                //#endregion

                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Предупреждение");
            }
        }                    

        void WaitMethod()
        {
            //lock (this.label2)

            {
                Label label2 = new Label();
                label2.AutoSize = true;
                label2.Location = new System.Drawing.Point(315, 510);
                label2.Name = "label2";
                label2.Size = new System.Drawing.Size(10, 13);
                label2.TabIndex = 9;
                label2.Text = " ";


                label2.Text = "Подождите, идёт поиск ...";
            }                      
        }

        private static bool NegativeNumbers(float lambda)
        {
            return lambda <= 0;            
        }

        private bool isExistNegativeNumbers(double[] x)
        {
            bool flag = true;
            foreach (var n in x)
            {
                if (n < 0)
                {
                    flag = true;
                    break;
                }
                else flag = false; 
            }
            return flag;
        }               

        private void chart2_Click(object sender, EventArgs e)
        {

        }
    }
}