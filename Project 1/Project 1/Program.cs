using System;
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

        public double findDeterminant()
        {
            double[,] meg = new double[getRows(), getCols()];
            //establish the meg
            for (int i = 0; i < getRows(); i++)
            {
                for (int j = 0; j < getCols(); j++)
                {
                    meg[i, j] = getVal(i, j);
                }
            }
            //meg established
                double det = 0;
                for (int i = 0; i < getRows(); i++)  //iterate through each row
                {
                    for (int j = i + 1; j < getRows(); j++)  //utilize through every other row
                    {
                        det = meg[j, i] / meg[i, i];  //divides row below by current row at
                    for (int k = i; k < getRows(); k++)  //moves as i increases, creating the zero's in triangle
                        meg[j, k] = meg[j, k] - det * meg[i, k];  //multiplies row beneath by the division above.
                    }
                }
                det = 1;
                for (int i = 0; i < getRows(); i++)
                    det = det * meg[i, i];  //finishes out multiplication
                return det;
            
        }
    }    
 public class ProjectOne
    {
        private static String dataFile = Directory.GetCurrentDirectory().ToString() + "\\p1data.txt";

        private static List<Matrix> class1 = new List<Matrix>();
        private static List<Matrix> class2 = new List<Matrix>();

        public static void Import(List<Matrix> class1, List<Matrix> class2)
        {
            String currentLine = "";

            var sr = new StreamReader(dataFile, Encoding.UTF8, true, 128);

            //skips N number of lines
            for (int n = 0; n < 2; n++)
            {
                sr.ReadLine();
            }
            while ((currentLine = sr.ReadLine()) != null)
            {
                // currentLine = sr.ReadLine();
                string[] line = currentLine.Split('\t');
                double[,] matrix1 = new double[1, 2];
                double[,] matrix2 = new double[1, 2];
                matrix1[0, 0] = Double.Parse(line[0]);
                matrix1[0, 1] = Double.Parse(line[1]);
                class1.Add(new Matrix(matrix1));
                matrix2[0, 0] = Double.Parse(line[2]);
                matrix2[0, 1] = Double.Parse(line[3]);
                class2.Add(new Matrix(matrix2));
            }

            sr.Close();
        }

        public static Matrix Mean(List<Matrix> class1)
        {
            Matrix m1 = class1[0];

            for (int i = 1; i < class1.Count; i++)
            {
                m1 = m1.add(class1[i]);
            }

            m1 = m1.multiply(1.0 / class1.Count);

            return m1;
        }

        public static Matrix Covariance(List<Matrix> class1, Matrix m1)
        {
            Matrix cov1;
            Matrix cov1transpose;


            cov1 = class1[0].subtract(m1);
            cov1transpose = cov1.transpose();
            cov1 = cov1transpose.multiply(cov1);

            for (int i = 1; i < class1.Count; i++)
            {
                Matrix temp1 = class1[i];
                Matrix temp1transpose;
                temp1 = temp1.subtract(m1);
                temp1transpose = temp1.transpose();
                temp1 = temp1transpose.multiply(temp1);
                cov1 = cov1.add(temp1);
            }

            cov1 = cov1.multiply(1.0 / class1.Count);

            return cov1;
        }

        public static double MeanEval(Matrix cov1Inverse, Matrix cov2Inverse, Matrix mThis, Matrix mOther)
        {
            {
                Matrix g1, g2;

                g1 = mThis.subtract(mThis);
                g1 = g1.transpose();
                g1 = cov1Inverse.multiply(g1);
                g1 = g1.multiply(mThis.subtract(mThis));
                g1 = g1.multiply(-0.5);

                g2 = mThis.subtract(mOther);
                g2 = g2.transpose();
                g2 = cov2Inverse.multiply(g2);
                g2 = g2.multiply(mThis.subtract(mOther));
                g2 = g2.multiply(-0.5);

                if (g1.getVal(0, 0) > g2.getVal(0, 0))
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
        }
        public static void Main(String[] args)
        {
            Matrix m1, m2, cov1, cov2, g1, g2;
            double mean1Spot, mean2Spot, answer1, answer2, last_half;
            List<Matrix> outcast1, outcast2, boundary1, boundary2;
            outcast1 = outcast2 = boundary1 = boundary2 = new List<Matrix>();
            Double epsilon = 0.01;
            Double scale = 0.005;
            Double mag = 0;

            Import(class1, class2);
            m1 = Mean(class1);
            Console.WriteLine("Mean Vector 1:");
            m1.printMatrix();

            m2 = Mean(class2);
            Console.WriteLine("Mean Vector 2:");
            m2.printMatrix();

            cov1 = Covariance(class1, m1);
            cov2 = Covariance(class2, m2);

            Console.WriteLine("\n");
            Console.WriteLine("The covariance matrix for class 1:");
            cov1.printMatrix();

            Console.WriteLine("\n");
            Console.WriteLine("The covariance matrix for class 2:");
            cov2.printMatrix();

            double cov1Determinant = cov1.findDeterminant();
            double cov2Determinant = cov2.findDeterminant();

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
            mean1Spot = MeanEval(cov1Inverse, cov2Inverse, m1, m2);
            mean2Spot = MeanEval(cov1Inverse, cov2Inverse, m2, m1);
            if (mean1Spot > 0)
            {
                Console.WriteLine("M1 was placed in Class 1");
            }
            else
            {
                Console.WriteLine("M1 was placed in Class 2");
            }
            if (mean2Spot > 0)
            {
                Console.WriteLine("M2 was placed in Class 2");
            }
            else
            {
                Console.WriteLine("M2 was placed in Class 1");
            }

            Console.ReadKey();


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
                g2 = g2.multiply(-0.5);
                g2 = g2.multiply(cov2Inverse);
                g2 = g2.multiply(class1[i].subtract(m2).transpose());
                
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
                g2 = g2.multiply(-0.5);
                g2 = g2.multiply(cov2Inverse);
                g2= g2.multiply(class2[i].subtract(m2).transpose());
                
                answer1 = g2.getVal(0, 0);
                last_half = (Math.Log(cov2Determinant));
                last_half += Math.Log(0.5);
                answer1 -= last_half;

                g1 = class2[i].subtract(m1);
                g1 = g1.multiply(-0.5);
                g1 = g1.multiply(cov1Inverse);
                g1 = g1.multiply(class2[i].subtract(m1).transpose());
                
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
            s1det = s1.findDeterminant();
            Console.WriteLine("Determinant of System of Equations is: " + s1det);
            s2 = s1.findInverse();
            Console.WriteLine("\n");
            Console.WriteLine("Inverse of System is:");
            s2.printMatrix();
            Console.ReadKey();
            s2det = s2.findDeterminant();
            Console.WriteLine("The determinant inverse is: " + s2det);
            s2 = s2.multiply(s2det);
            Console.WriteLine("The product of the determinant of the system and it's inverse is: " + (s1det * s2det));
            Console.ReadKey();


            Matrix condmat1 = new Matrix(sys1);
            Matrix condmat2 = condmat1.findInverse(); 
            condmat2 = s1.findInverse();
            Double cond1 = 0;
            Double cond2 = 0;

            for (int i=0; i<condmat1.getRows(); i++)
            {
                for(int j=0; j<condmat1.getCols(); j++)
                {
                    sys1cond[0, i] += (condmat1.getVal(i, j));

                }
                sys1cond[0, i] = Math.Abs(sys1cond[0, i]);
            }

            for (int i = 0; i < condmat2.getRows(); i++)
            {
                for (int j = 0; j < condmat2.getCols(); j++)
                {
                    sys2cond[0, i] += (condmat2.getVal(i, j));
                }
                sys2cond[0, i] = Math.Abs(sys2cond[0, i]);
            }

            for(int i=0; i<sys1cond.Length; i++)
            {
                if (sys1cond[0,i] > cond1)
                {
                    cond1 = sys1cond[0, i];
                }
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
