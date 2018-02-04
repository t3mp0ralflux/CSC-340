﻿using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;

// THE ENTIRE PROJECT RUNS UNDER Matrix[ROWS, COLUMNS]
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
            return matrix.GetLength(0);
        }

        public int getCols()
        {
            return matrix.GetLength(1);
        }

        public double getVal(int i, int j)
        {
            return matrix[i,j];
        }

        public Matrix multiply(double scalar)
        {
            Matrix temp = new Matrix(matrix);
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    temp.matrix[i,j] *= scalar;
                }
            }
            return temp;
        }

        public Matrix multiply(Matrix other)
        {
            if (other.getRows() != getCols())
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
                Console.WriteLine("]");
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

        // multiplies a row by a constant
        public void rowMultiply(int row, double scalar)
        {
            for (int i = 0; i < getCols(); i++)
            {
                matrix[row,i] *= scalar;
            }
        }

        public void rowAdd(int rowA, int rowB)
        {
            for (int i = 0; i < getCols(); i++)
            {
                matrix[rowA,i] += matrix[rowB,i];
            }
        }

        public Matrix augment(double[,] values)
        {
            // makes a 2d array that is the size of augmented matrix
            double[,] newMatrix = new double[getRows(),getCols() + 1];
            for (int i = 0; i < getRows(); i++)
            {
                for (int j = 0; j < getCols(); j++)
                {
                    newMatrix[i,j] = matrix[i,j];
                }
                newMatrix[i,getCols()] = values[i,0];
            }
            return new Matrix(newMatrix);
        }

        public Matrix augment(Matrix mat)
        {
            // makes a 2d array that is the size of augmented matrix
            double[,] newMatrix = new double[getRows(),getCols() + mat.getCols()];
            for (int i = 0; i < newMatrix.GetLength(0); i++)
            {
                for (int j = 0; j < newMatrix.GetLength(1); j++)
                {
                    if (j < getCols())
                    {
                        newMatrix[i,j] = matrix[i,j];
                    }
                    else
                    {
                        newMatrix[i,j] = mat.matrix[i,j - getCols()];
                    }
                }
                newMatrix[i,getCols()] = mat.getVal(i, 0);
            }
            return new Matrix(newMatrix);
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

        public Matrix gaussJordan(Matrix b)
        {
            int e = 1;
            int p;

            // augment current matrix with the values
            Matrix C = augment(b);

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
                // if an entire column is 0s, there is no sln
                if (C.matrix[p,j] == 0)
                {
                    e = 0;
                }
                // move the pivot row to row j
                if (p > j)
                {
                    C.interchange(j, p);
                }

                // divide row j by the leading coefficient
                C.rowMultiply(j, (1.0 / C.getVal(j, j)));
                // back substitution
                for (int i = 0; i < C.getRows(); i++)
                {
                    if (i != j)
                    {
                        double Cij = C.matrix[i,j];
                        for (int k = 0; k < C.getCols(); k++)
                        {
                            C.matrix[i,k] = C.matrix[i,k] - C.matrix[j,k] * Cij;
                        }
                    }
                }

            }

            // if there is no unique soln, return null
            if (e == 0)
                return null;
            else
                return C;
        }


        // returns a matrix with the solns
        public double[] gaussianElim(Matrix b)
        {
            int _E = 1;
            int p;

            // augment current matrix with the values
            Matrix C = augment(b);

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

                // if an entire column is zeroes, return 0. no unique soln
                if (C.matrix[p,j] == 0)
                {
                    _E = 0;
                }

                // interchange if the pivot is below row j
                if (p > j)
                {
                    C.interchange(j, p);
                }

                // divide the row by the leading coefficient
                C.rowMultiply(j, (1.0 / C.getVal(j, j)));

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

            // return null if no unique soln
            if (_E == 0)
                return null;

            // create partitions D and e
            double[,] D = new double[C.getRows(),C.getCols() - 1];
            double[,] e = new double[C.getRows(),1];

            // populate the partitions
            for (int i = 0; i < C.getRows(); i++)
            {
                for (int j = 0; j < C.getCols() - 1; j++)
                {
                    D[i,j] = C.getVal(i, j);
                }
            }

            // populate the partitions
            for (int i = 0; i < e.Length; i++)
                e[i,0] = C.getVal(i, C.getCols() - 1);

            double[] x = new double[C.getRows()];
            double sum;

            // back substitution
            for (int j = D.Length - 1; j >= 0; j--)
            {
                sum = 0;
                for (int i = j + 1; i < D.Length; i++)
                {
                    sum = sum + (D[j,i] * x[i]);
                }
                x[j] = (e[j,0] - sum) / D[j,j];
            }

            return x;
        }

        // find the inverse matrix
        public Matrix findInverse()
        {
            double[,] temp = new double[getRows(),getCols()];

            // error check
            if (getRows() != getCols())
                return null;

            // augments the matrix with its corresponding indentity matrix to find inverse
            for (int i = 0; i < getRows(); i++)
            {
                for (int j = 0; j < getCols(); j++)
                {
                    if (i == j)
                    {
                        temp[i,j] = 1;
                    }
                }
            }

            // turns temp into a matrix object and then returns the inverse
            Matrix inverse = new Matrix(temp);
            inverse = gaussJordan(inverse);

            // create a new array to store the inverse matrix
            temp = new double[inverse.getRows(),inverse.getCols() / 2];

            // we have to start the column index j, at the half way mark, since our matrix is still augmented
            for (int i = 0; i < inverse.getRows(); i++)
            {
                for (int j = (inverse.getCols() / 2); j < inverse.getCols(); j++)
                {
                    temp[i,j - inverse.getCols() / 2] = inverse.matrix[i,j];
                }
            }

            return new Matrix(temp);

        }

    }    
 public class ProjectOne
    {
        private static String dataFile = Directory.GetCurrentDirectory().ToString() + "\\p1data.txt";
        private static String findingsFile = Directory.GetCurrentDirectory().ToString() + "\\p1findings.txt";

        private static List<Matrix> class1 = new List<Matrix>();
        private static List<Matrix> class2 = new List<Matrix>();

        public static double findDeterminant(Matrix mat)
        {
            int p;
            int r = 0;
            double determinant = 0;

            // augment current matrix with the values
            Matrix det = mat;
            //had to add meg as this thing wouldn't stop passing by ref instead of value
            double[,] meg = new double[mat.getRows(),mat.getCols()];
            for(int i=0; i<mat.getRows(); i++)
            {
                for(int j=0; j<mat.getCols(); j++)
                {
                    meg[i, j] = mat.getVal(i, j);
                }
            }

            for (int j = 0; j < det.getRows(); j++)
            {
                p = j;

                // loop through each row and find the pivot
                for (int i = 0; i < det.getRows(); i++)
                {

                    // look at the absolute value of the pivot and see if it is larger than current pivot
                    // if so, make that the new pivot
                    if (Math.Abs(det.matrix[p, j]) < Math.Abs(det.matrix[i, j]))
                    {
                        p = i;
                    }
                }

                // interchange if the pivot is below row j
                if (p > j)
                {
                    det.interchange(j, p);
                    r++;
                }

                for (int i = 0; i < det.getRows(); i++)
                {
                    if (i > j)
                    {
                        double Cij = det.matrix[i, j];
                        double Cjj = det.matrix[j, j];
                        for (int k = 0; k < det.getCols(); k++)
                        {
                            meg[i,k] = det.matrix[i, k] - (det.matrix[j, k] * (Cij / Cjj));
                        }
                    }
                }

            }

            // initialize determinant to the value in C00
            determinant = meg[0, 0];
            // then multiply down the diagonal
            for (int i = 1; i < meg.GetLength(0); i++)
            {
                determinant = determinant * meg[i, i];
            }

            // put the negative/positive sign if needed
            determinant = determinant * Math.Pow((-1), r);

            return determinant;
        }

        public static void Main(String[] args)
        {
            String currentLine = "";

            var sr = new StreamReader(dataFile, Encoding.UTF8, true, 128);
            
            //skips N number of lines
            for(int n = 0; n < 2; n++)
            {
                sr.ReadLine();
            }
            while((currentLine = sr.ReadLine()) != null)
            {
               // currentLine = sr.ReadLine();
                string[] line = currentLine.Split('\t');
                double[,] matrix1 = new double[1,2];
                double[,] matrix2 = new double[1,2];
                matrix1[0,0] = Double.Parse(line[0]);
                matrix1[0,1] = Double.Parse(line[1]);
                class1.Add(new Matrix(matrix1));
                matrix2[0,0] = Double.Parse(line[2]);
                matrix2[0,1] = Double.Parse(line[3]);
                class2.Add(new Matrix(matrix2));
            }

            sr.Close();

            Matrix m1 = class1[0];
            Matrix m2 = class2[0];
            
            for(int i=1; i < class1.Count; i++)
            {
                m1 = m1.add(class1[i]);
            }

            m1 = m1.multiply(1.0 / class1.Count);

            Console.WriteLine("Mean Vector 1:");
            m1.printMatrix();

            for(int i=1;i<class2.Count; i++)
            {
                m2 = m2.add(class2[i]);
            }

            m2 = m2.multiply(1.0 / class2.Count);

            Console.WriteLine("\n");
            Console.WriteLine("Mean Vector 2:");
            m2.printMatrix();

            Matrix cov1;
            Matrix cov1transpose;
            Matrix cov2;
            Matrix cov2transpose;

            cov1 = class1[0].subtract(m1);
            cov1transpose = cov1.transpose();
            cov1 = cov1transpose.multiply(cov1);

            cov2 = class2[0].subtract(m2);
            cov2transpose = cov2.transpose();
            cov2 = cov2transpose.multiply(cov2);

            for (int i = 1; i < class1.Count; i++)
            {
                Matrix temp1 = class1[i];
                Matrix temp1transpose;
                temp1 = temp1.subtract(m1);
                temp1transpose = temp1.transpose();
                temp1 = temp1transpose.multiply(temp1);
                cov1 = cov1.add(temp1);
            }

            for (int i = 1; i < class2.Count; i++)
            {
                Matrix temp2 = class2[i];
                Matrix temp2transpose;
                temp2 = temp2.subtract(m2);
                temp2transpose = temp2.transpose();
                temp2 = temp2transpose.multiply(temp2);
                cov2 = cov2.add(temp2);
            }

            cov1 = cov1.multiply(1.0 / class1.Count);
            cov2 = cov2.multiply(1.0 / class2.Count);

            Console.WriteLine("\n");
            Console.WriteLine("The covariance matrix for class 1:");
            cov1.printMatrix();

            Console.WriteLine("\n");
            Console.WriteLine("The covariance matrix for class 2:");
            cov2.printMatrix();

            double cov1Determinant = findDeterminant(cov1);
            double cov2Determinant = findDeterminant(cov2);

            Console.WriteLine("\n");
            Console.WriteLine("Determinant of Covariance Matrix 1:" + cov1Determinant);
            Console.WriteLine("Determinant of Covariance Matrix 2:" + cov2Determinant);
            

            Matrix cov1Inverse = cov1.findInverse();
            Matrix cov2Inverse = cov2.findInverse();

            Console.WriteLine("Inverse Covariance of Matrix 1:");
            cov1Inverse.printMatrix();
            Console.WriteLine("Inverse Covariance of Matrix 2:");
            cov2Inverse.printMatrix();
            Console.ReadKey();
            //next is to do the analysis of the two and categorize the points

            Matrix g1;
            Matrix g2;
            double answer1;
            double answer2;
            double last_half;
            List<Matrix> outcast1 = new List<Matrix>();
            List<Matrix> outcast2 = new List<Matrix>();
            List<Matrix> boundary1 = new List<Matrix>();
            List<Matrix> boundary2 = new List<Matrix>();
            Double epsilon = 0.01;
            Double scale = 0.005;
            Double mag = 0;

            {
                g1 = m1.subtract(m1);
                g1 = g1.transpose();
                g1 = cov1Inverse.multiply(g1);
                g1 = g1.multiply(m1.subtract(m1));
                g1 = g1.multiply(-0.5);

                g2 = m1.subtract(m2);
                g2 = g2.transpose();
                g2 = cov2Inverse.multiply(g2);
                g2 = g2.multiply(m1.subtract(m2));
                g2 = g2.multiply(-0.5);

                if (g1.getVal(0, 0) > g2.getVal(0, 0))
                {

                    Console.WriteLine("m1 was classified into class 1");
                }
                else
                {
                    Console.WriteLine("m1 was classified into class 2");
                }
            }

            {
                g1 = m2.subtract(m2);
                g1 = g1.transpose();
                g1 = cov2Inverse.multiply(g1);
                g1 = g1.multiply(m2.subtract(m2));
                g1 = g1.multiply(-0.5);

                g2 = m2.subtract(m1);
                g2 = g2.transpose();
                g2 = cov1Inverse.multiply(g2);
                g2 = g2.multiply(m2.subtract(m1));
                g2 = g2.multiply(-0.5);

                if (g1.getVal(0, 0) > g2.getVal(0, 0))
                {

                    Console.WriteLine("m2 was classified into class 2");
                }
                else
                {
                    Console.WriteLine("m2 was classified into class 1");
                }
            }
            Console.ReadKey();
            g1 = null;
            g2 = null;
            for (int i = 0; i < class1.Count; i++)
            {
                g1 = class1[i].subtract(m1);
                g1 = g1.multiply(-0.5);
                g1 = g1.multiply(cov1Inverse);
                g1 = g1.multiply(class1[i].subtract(m1).transpose());
                
                answer1 = g1.getVal(0, 0);
                last_half = (0.5*Math.Log(cov1Determinant));
                last_half += Math.Log(0.5);
                answer1 -= last_half;

                g2 = class1[i].subtract(m2);
                g2 = g2.transpose();
                g2 = cov2Inverse.multiply(g2);
                g2 = g2.multiply(class1[i].subtract(m2));
                g2 = g2.multiply(-0.5);
                answer2 = g2.getVal(0, 0);
                last_half = (0.5*Math.Log(cov2Determinant));
                last_half += Math.Log(0.5);
                answer2 -= last_half;

                if (answer1 < answer2){

                    outcast1.Add(class1[i]);
                    Console.WriteLine("Point " + i + "G1: " + answer1);
                    Console.WriteLine("Point " + i + "G2: " + answer2);
                }
                answer1 *= scale;
                answer2 *= scale;
                mag = Math.Abs(answer1 - answer2);
                if (mag < epsilon)
                {
                    boundary1.Add(class1[i]);
                }
            }

            for (int i = 0; i < class2.Count; i++)
            {
                g2 = class2[i].subtract(m2);
                g2 = g2.transpose();
                g2 = cov2Inverse.multiply(g2);
                g2= g2.multiply(class2[i].subtract(m2));
                g2 = g2.multiply(-0.5);
                answer1 = g2.getVal(0, 0);
                last_half = (Math.Log(cov2Determinant));
                last_half += Math.Log(0.5);
                answer1 -= last_half;

                g1 = class2[i].subtract(m1);
                g1 = g1.transpose();
                g1 = cov1Inverse.multiply(g1);
                g1 = g1.multiply(class2[i].subtract(m1));
                g1 = g1.multiply(-0.5);
                answer2 = g1.getVal(0, 0);
                last_half =(Math.Log(cov1Determinant));
                last_half += Math.Log(0.5);
                answer2 -= last_half;

                if (answer1 < answer2)
                {

                    outcast2.Add(class2[i]);
                    Console.WriteLine("Point " + i + "G2:" + answer1);
                    Console.WriteLine("Point " + i + "G1:" + answer2);
                }
                answer1 *= scale;
                answer2 *= scale;
                mag = Math.Abs(answer1 - answer2);
                if (mag < epsilon)
                {
                    boundary2.Add(class2[i]);
                }
            }

            Console.WriteLine("The error points in class 1 are:");
            for (int i = 0; i < outcast1.Count; i++)
            {
                outcast1[i].printMatrix();
            }

            Console.WriteLine("The error points in class 2 are:");
            for (int i=0; i< outcast2.Count; i++)
            {
                outcast2[i].printMatrix();
            }
            Console.ReadKey();
            //above was the analysis.

            Console.WriteLine("The boundary points of Class 1 are: ");
            for(int i=0; i<boundary1.Count; i++)
            {
                boundary1[i].printMatrix();
            }
            Console.WriteLine("The boundary points of Class 2 are: ");
            for (int i = 0; i < boundary2.Count; i++)
            {
                boundary2[i].printMatrix();
            }
            Console.ReadKey();


            //below is linear system stuff
            double[,] sys1 = { { 2, 1, -1, -1, 1, 0, -1, -1 }, { 1, 0, 2, 0, -1, -2, 2, 2 }, { 0, -2, 5, 4, -1, 0, 3, 1 }, { 1, 1, -7, 3, 2, 1, -1, 0 }, { 1, 1, 2, 3, -2, 2, 2, 9 }, { 0, -3, -2, 2, 0, 2, 4, -5 }, { -2, 5, -1, 1, 1, 3, 0, -2 }, { 1, 0, 1, 1, 0, 2, 1, 1 } };
            double[,] sys2 = { { 1 }, { -1 }, { 2 }, { -2 }, { 3 }, { -3 }, { 4 }, { -4 } };

            Matrix s1 = new Matrix(sys1);
            Matrix s2 = new Matrix(sys2);

            double[,] sys1cond = new Double[1, s1.getCols()];
            double[,] sys2cond = new Double[1,s1.getCols()];

            Double s1det;
            Double s2det;

            s2 = s1.gaussJordan(s2);
            s2.printMatrix();
            Console.WriteLine("\n");
            s1det = findDeterminant(s1);
            Console.WriteLine("Determinant of System of Equations is:" + s1det);
            s2 = s1.findInverse();
            Console.WriteLine("\n");
            Console.WriteLine("Inverse of System is:");
            s2.printMatrix();
            Console.ReadKey();
            s2det = findDeterminant(s2);
            Console.WriteLine("The determinant inverse is: " + s2det);
            s2 = s2.multiply(s2det);
            Console.WriteLine("The product of the determinant of the system and it's inverse is: " + (s1det * s2det));
            Console.ReadKey();


            //just put the fucking numbers back in here for god's sake.  Mother fucker won't stop changing values.
            double[,] sys3 = { { 2, 1, -1, -1, 1, 0, -1, -1 }, { 1, 0, 2, 0, -1, -2, 2, 2 }, { 0, -2, 5, 4, -1, 0, 3, 1 }, { 1, 1, -7, 3, 2, 1, -1, 0 }, { 1, 1, 2, 3, -2, 2, 2, 9 }, { 0, -3, -2, 2, 0, 2, 4, -5 }, { -2, 5, -1, 1, 1, 3, 0, -2 }, { 1, 0, 1, 1, 0, 2, 1, 1 } };

            Matrix condmat1 = new Matrix(sys3);
            Matrix condmat2 = condmat1.findInverse(); 
            condmat2 = s1.findInverse();
            Double cond1 = 0;
            Double cond2 = 0;

            for (int i=0; i<condmat1.getRows(); i++)
            {
                for(int j=0; j<condmat1.getCols(); j++)
                {
                    sys1cond[0, j] += Math.Abs(condmat1.getVal(i, j));

                }
            }

            for (int i = 0; i < condmat2.getRows(); i++)
            {
                for (int j = 0; j < condmat2.getCols(); j++)
                {
                    sys2cond[0, j] += Math.Abs(condmat2.getVal(i, j));

                }
            }
            for(int i=0; i<sys1cond.Length; i++)
            {
                if (sys1cond[0,i] > cond1)
                {
                    cond1 = sys1cond[0, i];                }
            }
            for (int i = 0; i < sys2cond.Length; i++)
            {
                if (sys2cond[0, i] > cond2)
                {
                    cond2 = sys2cond[0, i];
                }
            }

            Console.WriteLine("The condition number for the system and the inverse is: " + (cond1 * cond2));
            Console.ReadKey();
        } 
    }
    

}
