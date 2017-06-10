using System;
using System.Windows.Forms.DataVisualization.Charting;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Multiply_regres.Controls;

namespace Multiply_regres.RecoveryRegres_BoxCox
{
    class RecoveryRegres
    {        
            
        public double[,] RecoveryByLambdaOpt(List<Sign> xList, List<ResultPLP> plp)
        {
            double[,] newMas = new double[xList.Count, xList.ElementAt(0).x.Length];
            int i = 0;
            foreach (Sign x in xList)
            {
                double[] new_x = FindLambda.t(x.x, plp.ElementAt(i).lambda_opt);                
                for (int j = 0; j < x.x.Length; j++)
                {
                    newMas[i, j] = new_x[j];
                }
                i++;
            }
            return newMas;
        }

        public void Recovery_Step_ByLambdaOpt(double [,] trans_mas)
        {

        }
    }
}
