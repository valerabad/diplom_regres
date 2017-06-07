using System;
using System.Threading;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms.DataVisualization.Charting;
using System.Threading.Tasks;

namespace Multiply_regres
{
    

    public partial class Form1 : Form
    {
        //class Worker
        //{
        //    public static string SomeLongOperation(IProgress<string> progress)
        //    {
        //        // Perform a long running work...
        //        //for (var i = 0; i < 10; i++)
        //        //{
        //        //    Task.Delay(500).Wait();
        //        //    progress.Report(i.ToString());
        //        //}
        //         FindingLambda();
        //        return "подождите...";
        //    }
        //}

        public string SomeLongOperation(IProgress<string> progress)
        {
            // Perform a long running work...
            //for (var i = 0; i < 10; i++)
            //{
            //    Task.Delay(500).Wait();
            //    progress.Report(i.ToString());
            //}
            FindingLambda();
            return "подождите...";
        }
        List<ResultPLP> resultListPLP = new List<ResultPLP>();

        public void FindingLambda()
        {            
            int count_sign = mas.GetLength(0);
            int N = mas.GetLength(1);

            // таблица для трансформации Бокса-Кокса
            this.dataGridView4.RowCount = 3;
            this.dataGridView4.ColumnCount = count_sign;
            this.dataGridView4.Rows[0].HeaderCell.Value = string.Format("P");
            this.dataGridView4.Rows[1].HeaderCell.Value = string.Format("λ оптимальное");
            this.dataGridView4.Rows[2].HeaderCell.Value = string.Format("P оптимальное");
            dataGridView4.AutoSize = true;
            for (int l = 0; l < count_sign; l++)
            {
                dataGridView4.Columns[l].HeaderCell.Value = string.Format("Признак " + (l + 1).ToString());
            }

            FindLambda fl = new FindLambda();
            List<float> lstLambda = fl.GenerateLambdaList(-3, 3, 0.05f);


            List<double[]> list_x = new List<double[]>();
            for (int i = 0; i < count_sign; i++)
            {

                double[] x = new double[mas.GetLength(1)];
                for (int j = 0; j < N; j++)
                {
                    x[j] = mas[i, j];
                }
                list_x.Add(x);

            }
            List<float> lstLambdaWithoutNegative = null;
            //List<float> lstLambdaCopy = null;
            // проверка на 0
            for (int i = 0; i < count_sign; i++)
                if (list_x.ElementAt(i).Contains(0) || isExistNegativeNumbers(list_x.ElementAt(i)))
                {
                    //Array.Copy(lstLambda, lstLambdaWithoutNegative, lstLambda.Count); //
                    lstLambdaWithoutNegative = new List<float>(lstLambda);

                    lstLambdaWithoutNegative.RemoveAll(NegativeNumbers);
                }

            resultListPLP = new List<ResultPLP>(); // для каждого файла - новый
            SortedSet<P_Lambda> lst_pLambda;// = new List<P_Lambda>();

            FirstAnalis fs = new FirstAnalis(mas);
            Chart chart;
            ChartArea chartArea;
            tabControl2.TabPages.Clear();
            for (int i = 0; i < count_sign; i++) // по всем n признакам
            {

                lst_pLambda = new SortedSet<P_Lambda>(); // нужно обнулять

                Kolmagorov1 kolm_x = new Kolmagorov1(list_x.ElementAt(i), sredn_kvadr_otkl[i], srednee[i]);
                double kz_begin = kolm_x.K();
                double P_begin = 1 - kz_begin;


                tabControl2.TabPages.Add(i.ToString(), "Признак " + (i + 1).ToString());
                chart = new Chart();
                chartArea = new ChartArea();
                //this.tabPage3.Controls.Add(chart);
                chart.ChartAreas.Add(chartArea);

                chart.Parent = tabControl2.TabPages[i]; //i-1 // for test

                //chart.Location = new System.Drawing.Point(26, 415);
                chart.Name = "chart" + i.ToString();
                chart.Size = new System.Drawing.Size(chart.Parent.Size.Width,
                                                     chart.Parent.Size.Height);


                chart.Series.Add("Признак " + i.ToString());
                chart.Series[0].ChartType = SeriesChartType.Point;

                List<float> lstLambdaToWork;
                if (list_x.ElementAt(i).Contains(0) || isExistNegativeNumbers(list_x.ElementAt(i)))
                {
                    lstLambdaToWork = lstLambdaWithoutNegative;
                }
                else
                {
                    lstLambdaToWork = lstLambda;
                }


                foreach (var lambda in lstLambdaToWork)
                {
                    double[] t_ = FindLambda.t(list_x.ElementAt(i), lambda); // почему здусь отсортированній массив???
                    double sred = fs.srednee(t_); //i=0
                    double sred_kvadr = fs.S(t_); // i=0
                    Kolmagorov1 kolm1 = new Kolmagorov1(t_, sred_kvadr, sred);

                    double kz = kolm1.K();

                    lst_pLambda.Add(new P_Lambda(1 - kz, lambda));



                    chart.Series[0].Points.AddXY(Math.Round(lambda, 2), 1 - kz);
                    //chart2.Series[0].AxisLabel = "test";                               

                }

                //lst_pLambda.Sort();
                double max_P = lst_pLambda.ElementAt(lst_pLambda.Count - 1).P; // переделать, поиск максимального P и соответстующего лямбда
                double opt_lambda = lst_pLambda.ElementAt(lst_pLambda.Count - 1).lambda;
                resultListPLP.Add(new ResultPLP(P_begin, opt_lambda, max_P)); // добавили инфо по первому признаку 1 файла
                dataGridView4.Rows[0].Cells[i].Value = P_begin;
                dataGridView4.Rows[1].Cells[i].Value = opt_lambda;
                dataGridView4.Rows[2].Cells[i].Value = max_P;
            }
        }

        //private void button2_Click(object sender, EventArgs e)
        //{
        //    //FindingLambda();
        //    //var progress = new Progress<string>(s => label2.Text = s);
        //    //(new Thread((s) =>
        //    //{
        //    //    var result = SomeLongOperation(progress);

        //    //    this.label2.BeginInvoke((MethodInvoker)(() => this.label2.Text = result));

        //    //})).Start();

        //    //WaitMethod();
        //    //Thread thread = new Thread(new ThreadStart(WaitMethod));
        //    //thread.Start();

        //    //label2.Text = "Подождите, идёт поиск ...";

        //}
    }
}