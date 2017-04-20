using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multiply_regres
{
    // расчёт оценок регрессии
    class RegressionParams
    {
        public double[] FindingA(double[,] startMas, out Matrix<double> inv_m, out double[] Y, out double[,] X)
        {
            Y = new double[startMas.GetLength(1)];
            double[] A = new double[startMas.GetLength(0)];
            X = new double[startMas.GetLength(0), startMas.GetLength(1)]; // здесь 0 - кол-во столбцов
            for (int i = 0; i < startMas.GetLength(1); i++)
            {
                X[0, i] = 1;
                for (int j = 0; j < startMas.GetLength(0)-1; j++)
                {
                    X[j + 1, i] = startMas[j, i];
                }
            }

            for (int i = 0; i < startMas.GetLength(1); i++)
            {
                Y[i] = startMas[startMas.GetLength(0) - 1, i]; //mas.GetLength(0)-1 - последний столбец
            }
            double[,] X_copy = X;
            X = new double[startMas.GetLength(1), startMas.GetLength(0) + 1];
            X = Matrix<double>.T_(X_copy);

            double[,] X_T = Matrix<double>.T_(X);
            double[,] mult_m = Matrix<double>.Multiplication(X_T, X);

            Matrix<double> m4 = new Matrix<Double>(mult_m.GetLength(0), mult_m.GetLength(1));
            m4.matrix = mult_m;
            inv_m = Matrix<Double>.Inverse(m4);

            double[] mult2 = Matrix<double>.Multiplication(X_T, Y);

            A = Matrix<double>.Multiplication(inv_m.matrix, mult2);
            return A;          
        }

        public double S2_Zal(double[] Y, double[,] X, double[] A)
        {
            double s;
            double[] Y_XA_1;// = new double[Y.Length];
            double[] Y_XA_2;

            double[] multXA = Matrix<double>.Multiplication(X,A);
            Y_XA_1 = Matrix<double>.Subtraction(Y, multXA);
            Y_XA_2 = Y_XA_1;
            //Matrix<double>.T_(Y_XA);
            //Matrix<double> m = new Matrix<double>();

            double tmp = Matrix<double>.Multiplication(Y_XA_1, Y_XA_2);
            //double det = Matrix<double>.Determinant(tmp);
            s = (1.0d / ((X.GetLength(0) - X.GetLength(1)+1)-1)) * tmp; // + 1 столбец
            return s;
        }

        public double StandartA()
        {
                      
            return 1;
        }
    }
}
