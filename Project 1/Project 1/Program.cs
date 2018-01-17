using System;
using System.Data;
using System.IO;
using System.Text;

namespace Project_1
{
      public class Matrix
    {
        public double[,] matrix;

        public Matrix(double[,] values)
        {
            matrix = values;
        }

        public int getRows()
        {
            return matrix.Length;
        }

        public int getCols()
        {
            return matrix.GetLength(0);
        }

        public double getVal(int i, int j)
        {
            return matrix[i,j];
        }

        public Matrix scalar(double scalar)
        {
            Matrix temp = new Matrix(matrix);
            for (int i = 0; i < matrix.Length; i++)
            {
                for (int j = 0; j < matrix.GetLength(0); j++)
                {
                    temp.matrix[i,j] *= scalar;
                }
            }
            return temp;
        }

        public Matrix multiply(Matrix other)
        {
            if (other.getCols() != getRows())
                return null;

            double[,] c = new double[getRows(),other.getCols()];
            for (int i = 0; i < getRows(); i++)
            {
                for (int j = 0; j < other.getCols(); j++)
                {
                    for (int k = 0; k < getCols(); k++)
                    {
                        c[i,j] = c[i,j] + (matrix[i,k] * other.getVal(k, j));
                    }
                }
            }
            Matrix temp = new Matrix(c);
            return temp;
        }

        public Matrix add(Matrix other)
        {
            if (getRows() != other.getRows() || getCols() != other.getCols())
            {
                return null;
            }
            double[,] temp = new double[getRows(),getCols()];
            for (int i = 0; i < getRows(); i++)
            {
                for (int j = 0; j < getCols(); j++)
                {
                    temp[i,j] = matrix[i,j] + other.matrix[i,j];
                }
            }
            return new Matrix(temp);
        }

        public Matrix subtract(Matrix other)
        {
            if (getRows() != other.getRows() || getCols() != other.getCols())
            {
                return null;
            }
            double[,] temp = new double[getRows(),getCols()];
            for (int i = 0; i < getRows(); i++)
            {
                for (int j = 0; j < getCols(); j++)
                {
                    temp[i,j] = matrix[i,j] - other.matrix[i,j];
                }
            }
            return new Matrix(temp);
        }

        public void printMatrix()
        {
            for (int i = 0; i < getRows(); i++)
            {
                Console.Write("[");
                for (int j = 0; j < getCols(); j++)
                {
                    Console.Write(matrix[i,j] + ", ");
                }
                Console.Write("]");
            }
        }

        public Matrix transpose()
        {
            double[,] temp = new double[getCols(),getRows()];
            for (int i = 0; i < getRows(); i++)
            {
                for (int j = 0; j < getCols(); j++)
                {
                    temp[j,i] = matrix[i,j];
                }
            }
            return new Matrix(temp);
        }

        public void interchange(int rowA, int rowB)
        {
            double[] temp = new double[getCols()];
            for (int i = 0; i < getCols(); i++)
            {
                temp[i] = getVal(rowA, i);
                matrix[rowA,i] = matrix[rowB,i];
                matrix[rowB,i] = temp[i];
            }
        }

        public double findDeterminant()
        {
            int p;
            int r = 0;
            double determinant = 0;

            // augment current matrix with the values
            Matrix C = new Matrix(matrix);

            for (int j = 0; j < C.getRows(); j++)
            {
                p = j;

                // loop through each row and find the pivot
                for (int i = 0; i < C.getRows(); i++)
                {

                    // look at the absolute value of the pivot and see if it is larger than current pivot
                    // if so, make that the new pivot
                    if (Math.Abs(C.matrix[p,j]) < Math.Abs(C.matrix[i,j]))
                    {
                        p = i;
                    }
                }

                // interchange if the pivot is below row j
                if (p > j)
                {
                    C.interchange(j, p);
                    r++;
                }

                for (int i = 0; i < C.getRows(); i++)
                {
                    if (i > j)
                    {
                        double Cij = C.matrix[i,j];
                        double Cjj = C.matrix[j,j];
                        for (int k = 0; k < C.getCols(); k++)
                        {
                            C.matrix[i,k] = C.matrix[i,k] - (C.matrix[j,k] * (Cij / Cjj));
                        }
                    }
                }

            }

            // initialize determinant to the value in C00
            determinant = C.matrix[0,0];
            // then multiply down the diagoanl
            for (int i = 1; i < C.getRows(); i++)
            {
                determinant = determinant * C.matrix[i,i];
            }

            // put the negative/positive sign if needed
            determinant = determinant * Math.Pow((-1), r);

            return determinant;
        }

        public double trace()
        {
            if (getRows() != getCols())
            {
                Console.Write("Not a square matrix, returning 0");
                return 0;
            }
            double sum = 0;
            for (int i = 0; i < getRows(); i++)
            {
                sum += matrix[i,i];
            }

            return sum;

        }
    }    
 public class Operations
    {
        public Matrix mean = new Matrix();
        public Matrix covariance = new Matrix();
        

        public Matrix get_mean()
        {
            return mean;
        }

        public Matrix get_covariance()
        {
            return covariance;
        }

        public void setup()
        {
            covariance.createBlankMatrix(covariance, 2, 2);
            mean.createBlankMatrix(mean, 1, 2);
        }

        public void loadEigendata()
        {

        }

        public void set_mean(Matrix one)
        {
            double a;
            double b;

            for (int i = 0; i < one.numRows; i++)
            {
                a += double.Parse(one.data.Rows[i][0].ToString());
                b += double.Parse(one.data.Rows[i][1].ToString());
            }
            one.data.Rows[0][0] = a;
            one.data.Rows[0][1] = b;
            one.scalar(1 / one.numRows);
            

        }
    }
    

}
