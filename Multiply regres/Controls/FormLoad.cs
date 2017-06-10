using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms.DataVisualization.Charting;

namespace Multiply_regres
{
    public partial class Form1 : Form
    {
        private void Form1_Load(object sender, EventArgs e)
        {
            //double tmp  = Math.Pow(-19.3705d, 2.5d);
            //double tmp2 = Math.Pow(19.3705d, 2.5d);
            //double tmp3 = Math.Pow(19.3705d, -2.5d);
            //double tmp4 = Math.Pow(-19.3705d, -2.5d);
            //double tmp5 = Math.Log(-19.3705d);
            //double tmp6 = Math.Log(19.3705d);
            // таблица первичного статистического анализа
            this.dataGridView2.RowCount = 8;
            this.dataGridView2.ColumnCount = 3;
            this.dataGridView2.Rows[0].HeaderCell.Value = string.Format("Среднее");
            this.dataGridView2.Rows[2].HeaderCell.Value = string.Format("Среднеквадратическое");
            this.dataGridView2.Rows[4].HeaderCell.Value = string.Format("Асимметрия");
            this.dataGridView2.Rows[6].HeaderCell.Value = string.Format("Эксцесс");
            for (int i = 0; i < 7; i = i + 2)
            {
                this.dataGridView2.Rows[i].Cells[0].Value = "Оценка";
                this.dataGridView2.Rows[i + 1].Cells[0].Value = "Дов. интервал";
            }

            // 2.1. таблица на вкладке 2 - оценка параметров регрессии
            this.dataGridView3.RowCount = 2; // пока 2, неизвестен массив A
            this.dataGridView3.ColumnCount = 8;            
            this.dataGridView3.Columns[0].HeaderCell.Value = string.Format("α");
            this.dataGridView3.Columns[1].HeaderCell.Value = string.Format("Оценка");
            this.dataGridView3.Columns[2].HeaderCell.Value = string.Format("Стандартизированная оценка");
            this.dataGridView3.Columns[3].HeaderCell.Value = string.Format("Среднеквадратическое отклонение");
            this.dataGridView3.Columns[4].HeaderCell.Value = string.Format("Статистика");
            this.dataGridView3.Columns[5].HeaderCell.Value = string.Format("Квантиль");
            this.dataGridView3.Columns[6].HeaderCell.Value = string.Format("Значимость");
            this.dataGridView3.Columns[7].HeaderCell.Value = string.Format("Доверительный интервал");
            this.dataGridView3.Columns[0].Width = 25;

            // 2.2. таблица на вкладке 2 - оценка параметров регрессии
            this.dataGridView5.RowCount = 2; // пока 2, неизвестен массив A
            this.dataGridView5.ColumnCount = 8;
            this.dataGridView5.Columns[0].HeaderCell.Value = string.Format("α");
            this.dataGridView5.Columns[1].HeaderCell.Value = string.Format("Оценка");
            this.dataGridView5.Columns[2].HeaderCell.Value = string.Format("Стандартизированная оценка");
            this.dataGridView5.Columns[3].HeaderCell.Value = string.Format("Среднеквадратическое отклонение");
            this.dataGridView5.Columns[4].HeaderCell.Value = string.Format("Статистика");
            this.dataGridView5.Columns[5].HeaderCell.Value = string.Format("Квантиль");
            this.dataGridView5.Columns[6].HeaderCell.Value = string.Format("Значимость");
            this.dataGridView5.Columns[7].HeaderCell.Value = string.Format("Доверительный интервал");
            this.dataGridView5.Columns[0].Width = 25;

            this.dataGridView6.RowCount = 2; // пока 2, неизвестен массив A
            this.dataGridView6.ColumnCount = 8;
            this.dataGridView6.Columns[0].HeaderCell.Value = string.Format("α");
            this.dataGridView6.Columns[1].HeaderCell.Value = string.Format("Оценка");
            this.dataGridView6.Columns[2].HeaderCell.Value = string.Format("Стандартизированная оценка");
            this.dataGridView6.Columns[3].HeaderCell.Value = string.Format("Среднеквадратическое отклонение");
            this.dataGridView6.Columns[4].HeaderCell.Value = string.Format("Статистика");
            this.dataGridView6.Columns[5].HeaderCell.Value = string.Format("Квантиль");
            this.dataGridView6.Columns[6].HeaderCell.Value = string.Format("Значимость");
            this.dataGridView6.Columns[7].HeaderCell.Value = string.Format("Доверительный интервал");
            this.dataGridView6.Columns[0].Width = 25;
        }
    }
}
