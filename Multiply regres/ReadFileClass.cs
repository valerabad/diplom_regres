using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Collections;

namespace Multiply_regres
{
  public class ReadFileClass
    {
        static public double[,] read_and_results(OpenFileDialog dialog, DataGridView Grid_1) 
        {
            double[,] sign = null;
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK) // для диалога
            {
                DataGridViewCell cel;
                string fileName;
                StreamReader sr = new StreamReader(dialog.FileName);  // для диалога 
                //StreamReader sr = new StreamReader(@"C:\Users\HP\Desktop\4 курс\Случайные процессы(Полонская, Мацуга)\03-09-2015_14-49-41\data\data\1Sl.txt"); // без диалога
                fileName = dialog.FileName;
                string s;
                int count_sign = 0, count_obj = 0;
                string[] strs;
                
                s = sr.ReadLine();
                s = s.Trim();
                Regex reg = new Regex((@" *[^0-9+\.+\-+\S]"), RegexOptions.IgnoreCase); //-[^0-9]+.[^0-9] // @" *\r+\n+\s+\s"             
                strs = reg.Split(s);
                count_sign = strs.Length;

                while ((s = sr.ReadLine()) != null)
                {
                    count_obj++;
                }
                count_obj++;

                Grid_1.RowCount = count_obj;        
                Grid_1.ColumnCount = count_sign; // +1 № п.п                
                sign = new double[count_sign, count_obj];
                
                sr.Close();
                //sr = new StreamReader(@"C:\Users\HP\Desktop\4 курс\Мацуга, Сидорова 1\data\1.txt");//sl.txt
                sr = new StreamReader(fileName); // раскомментировать для диалога
                //sr = new StreamReader((@"C:\Users\HP\Desktop\4 курс\Случайные процессы(Полонская, Мацуга)\03-09-2015_14-49-41\data\data\1Sl.txt")); // убрать для диалога
                count_obj = 0;
                while ((s = sr.ReadLine()) != null)
                {                    
                    s = s.Trim();                    
                    strs = reg.Split(s);
                    count_sign = strs.Length;

                    for (int i = 0; i < count_sign; i++)
                    {
                        sign[i, count_obj] = Convert.ToDouble(strs[i].Replace('.', ','));
                        cel = Grid_1.Rows[count_obj].Cells[i];  // +1 № п.п
                        cel.Value = sign[i, count_obj];                       
                    }
                    count_obj++;                   
                }               
                sr.Close();                
                for (int i = 0; i < count_obj; i++)
                {
                    Grid_1.Rows[i].HeaderCell.Value = string.Format((i + 1).ToString(), "0");
                }
                foreach (DataGridViewColumn column in Grid_1.Columns)
                {
                     column.HeaderText = String.Concat("Sign ",
                        (column.Index+1).ToString());
                }             
            }
            return sign;
        }
    }
}
