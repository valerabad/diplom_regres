    using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multiply_regres
{
    public class Matrix<T>
    {
        public T[,] matrix;
        public int sizeX;
        public int sizeY;
        public Matrix(int x, int y)
        {
            matrix = new T[x, y];
            sizeX = x;
            sizeY = y;
        }

        public Matrix()
        {
            
        }

        public Matrix(Matrix<T> mtx)
        {
            matrix = new T[mtx.sizeX, mtx.sizeY];           
        }        

        public static double[,] Exclude(int row, int col, double[,] old)
        {
            double[,] new_ = new double[old.GetLength(0), old.GetLength(1) - 1]; //old.GetLength(0) - 1   
            // columns         
            for (int i = 0; i < old.GetLength(0); i++)
            {
                for (int j = 0; j < old.GetLength(1) - 1; j++)
                {
                    if (col > j)
                    {
                        new_[i, j] = old[i, j];
                    }
                    else
                    {
                        new_[i, j] = old[i, j + 1];
                    }
                }
            }
            // rows
            double[,] new2 = new double[new_.GetLength(0) - 1, new_.GetLongLength(1)];
            for (int i = 0; i < new_.GetLength(0) - 1; i++)
            {
                for (int j = 0; j < new_.GetLength(1); j++)
                {
                    if (row > i)
                    {
                        new2[i, j] = new_[i, j];
                    }
                    else
                    {
                        new2[i, j] = new_[i + 1, j];
                    }
                }
            }
            return new2;
        }

        public static double[,] Multiplication(double[,] m1, double[,] m2)
        {
            double[,] m_matrix = new double[m1.GetLength(0), m1.GetLength(0)];
            for (int i = 0; i < m2.GetLength(1); i++) // по всем столюцам матрицы 2
            {
                for (int j = 0; j < m1.GetLength(0); j++) // по всем строкам каждый раз матрицы 1
                {
                    double tmp_sum = 0;
                    for (int k = 0; k < m1.GetLength(1); k++)
                        tmp_sum = tmp_sum + m1[j, k] * m2[k, i];
                    m_matrix[j, i] = tmp_sum;
                }
            }
            return m_matrix;
        }

        public static double[] Multiplication(double[,] m1, double[] m2)
        {
            double[] m_matrix = new double[m1.GetLength(0)];
            {
                for (int j = 0; j < m1.GetLength(0); j++) // по всем строкам каждый раз матрицы 1
                {
                    double tmp_sum = 0;
                    for (int k = 0; k < m1.GetLength(1); k++)
                        tmp_sum = tmp_sum + m1[j, k] * m2[k];
                    m_matrix[j] = tmp_sum;
                }
            }
            return m_matrix;
        }

        public static double Multiplication(double[] m1, double[] m2)
        {
            //double[] m_matrix = new double[m1.Length];
            double res = 0;
            {
                double tmp_sum = 0;
                for (int j = 0; j < m1.Length; j++) // по всем строкам каждый раз матрицы 1
                {

                    tmp_sum = tmp_sum + m1[j] * m2[j];                   
                }
                res = tmp_sum;
            }
            return res;
        }

        public static double[,] T_(double[,] matrix)
        {
            double[,] T_matrix = new double[matrix.GetLength(1), matrix.GetLength(0)];
            for (int j = 0; j < matrix.GetLength(0); j++)
            {
                for (int i = 0; i < matrix.GetLength(1); i++)
                    T_matrix[i, j] = matrix[j, i];
            }
            return T_matrix;
        }

        public static double Determinant(double[,] m)
        {
            double det = 1;
            //определяем переменную EPS
            const double EPS = 1E-9;
            //размерность матрицы

            int n = m.GetLength(0); ;
            //int n = 3;

            double[][] a = new double[n][];
            double[][] b = new double[1][];
            b[0] = new double[n];

            for (int i = 0; i < n; i++)
            {
                a[i] = new double[n];
                for (int j = 0; j < n; j++)
                {
                    a[i][j] = m[i, j];
                }
            }
            //проходим по строкам
            for (int i = 0; i < n; ++i)
            {
                //присваиваем k номер строки
                int k = i;
                //идем по строке от i+1 до конца
                for (int j = i + 1; j < n; ++j)
                    //проверяем
                    if (Math.Abs(a[j][i]) > Math.Abs(a[k][i]))
                        //если равенство выполняется то k присваиваем j
                        k = j;
                //если равенство выполняется то определитель приравниваем 0 и выходим из программы
                if (Math.Abs(a[k][i]) < EPS)
                {
                    det = 0;
                    break;
                }
                //меняем местами a[i] и a[k]
                b[0] = a[i];
                a[i] = a[k];
                a[k] = b[0];
                //если i не равно k
                if (i != k)
                    //то меняем знак определителя
                    det = -det;
                //умножаем det на элемент a[i][i]
                det *= a[i][i];
                //идем по строке от i+1 до конца
                for (int j = i + 1; j < n; ++j)
                    //каждый элемент делим на a[i][i]
                    a[i][j] /= a[i][i];
                //идем по столбцам
                for (int j = 0; j < n; ++j)
                    //проверяем
                    if ((j != i) && (Math.Abs(a[j][i]) > EPS))
                        //если да, то идем по k от i+1
                        for (k = i + 1; k < n; ++k)
                            a[j][k] -= a[i][k] * a[j][i];
            }
            return det;
        }

        public static Matrix<double> Inverse(Matrix<double> mA, uint round = 0)
        {
            if (mA.sizeX != mA.sizeY) throw new ArgumentException("Обратная матрица существует только для квадратных, невырожденных, матриц.");
            Matrix<double> matrix = new Matrix<double>(mA); //Делаем копию исходной матрицы (копируем разверность, а не значения)
            double determinant = Determinant(mA.matrix); //Находим детерминант

            if (determinant == 0) return matrix; //Если определитель == 0 - матрица вырожденная

            for (int i = 0; i < mA.sizeX; i++)
            {
                for (int t = 0; t < mA.sizeY; t++)
                {
                    double[,] tmp = Matrix<double>.Exclude(i, t, mA.matrix);  //получаем матрицу без строки i и столбца t
                                                                              //(1 / determinant) * Determinant(tmp) - формула поределения элемента обратной матрицы
                    matrix.matrix[t, i] = round == 0 ? (1 / determinant) * Math.Pow(-1, i + t) * Determinant(tmp) : Math.Round(((1 / determinant) * Math.Pow(-1, i + t) * Determinant(tmp)), (int)round, MidpointRounding.ToEven);
                    //matrix.matrix[t, i] = (1 / determinant) * Math.Pow(-1, i + t) * Determinant(tmp);
                }
            }
            return matrix;
            //return new Matrix<double>(2,2);
        }

        public static double[] Subtraction(double[] Y, double[] multXA)
        {
            double[] res = new double[Y.Length]; // здесь везде одинаковые рамерности, так как вычитать можно м с одинаковыми размерностями
            for (int i = 0; i < Y.Length; i++)
            {
                res[i] = Y[i] - multXA[i];
            }                   
            return res;
        }


    }
}
