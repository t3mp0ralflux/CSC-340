using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Project_1
{

    class Project2
    {
        
        private static String dataFile = Directory.GetCurrentDirectory().ToString() + "\\eigendata.txt";
        

        private static List<Matrix> class1 = new List<Matrix>();

        public static void Import(List<Matrix> class1)
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
            }

            sr.Close();
        }

        public static double trace(Matrix one)
        {
            if (one.getRows() != one.getCols())
            {
                Console.Write("Not a square matrix, returning 0");
                return 0;
            }
            double sum = 0;
            for (int i = 0; i < one.getRows(); i++)
            {
                sum += one.getVal(i, i);
            }

            return sum;

        }

        public static List<double> leverriers(Matrix cov)
        {


            Matrix Bk = cov.deepcopy();
            Matrix Bk2 = cov.deepcopy();
            double Ak = -(trace(Bk));
            int n = Bk.getRows();
            Matrix ID = cov.identity();
            Matrix scale;
            Matrix Ak_temp;

            List<double> cof = new List<double>();
            cof.Add(Ak);

            for(int k=n-1; k>0; k--)
            {
                scale = ID.multiply(Ak);
                Ak_temp = scale.add(Bk);
                Bk = Ak_temp.multiply(Bk2);
                Ak = -((trace(Bk)) / (n-k+1));
                cof.Add(Ak);
            }
            return cof;


        }

        public static void eigenvalues(Matrix cov)
        {
            List<double> cof_m = leverriers(cov);
            double a = 1;
            double b = cof_m[0];
            double c = cof_m[1];
            double d = (Math.Pow(b,2))-4*(a * c);
            double e1 = (-b - (Math.Sqrt(d))) / (2 * a);
            double e2 = (-b + (Math.Sqrt(d))) / (2 * a);

            Console.WriteLine("Eigenvalue 1: " + e1);
            Console.WriteLine("Eigenvalue 2: " + e2);

           // double[,] sys1 = new double[cov.getRows(), 1];
           // Matrix lamdaI;
           // Matrix A = cov.deepcopy();
           //// Matrix B;
           // Matrix augment = new Matrix(sys1);
           // lamdaI = cov.identity();
           // lamdaI.multiply(e1);
           // A.subtract(lamdaI);
           // A.gaussJordan(augment);
            
           // A.printMatrix();
            Matrix B = eigenvector(cov, e1);
            Console.WriteLine("Eigenvector for e1: ");
            B.printMatrix();
            Console.ReadKey();

        }

        public static Matrix eigenvector(Matrix cov, double e1)
        {
            //double[,] sys1 = new Double[cov.getRows(), 1];
            //Matrix lamdaI;
            //Matrix A = cov.deepcopy();
            //Matrix augment = new Matrix(sys1);
            //lamdaI = cov.identity();
            //lamdaI.multiply(e1);
            //A.subtract(lamdaI);
            //A.gaussianElim(augment);
            {
                Matrix B = cov.deepcopy();
                double a = B.getVal(0, 0);
                double b = B.getVal(0, 1);
                double d = B.getVal(1, 1);
                double[,] songs = new double[2, 2];
                int i = 0;

                if (b == 0)
                {
                    songs[0, 0] = songs[1, 1] = 1;
                    songs[0, 1] = songs[1, 0] = 0;
                    return new Matrix(songs);
                }

                double[,] bass = { { a - e1, b }, { b, d - e1 } };

                //double[] rone = {-b,bass[0,0]};
                songs[0, 0] = -b;
                songs[1, 0] = bass[0, 0];
                double mag = Math.Sqrt((songs[1, 0] * songs[1, 0]) + (songs[0, 0] * songs[0, 0]));
                songs[0, 0] /= mag;
                songs[1, 0] /= mag;

                songs[0, 1] = -songs[1, 0];
                songs[1, 1] = songs[0, 0];

                return new Matrix(songs);
            }

            //return A;
        }

        public static double normalize(Matrix one)
        {
            Matrix m1 = one.deepcopy();
            double col = m1.getCols();
            double row = m1.getRows();
            double m_norm = 0;

            for(int i=0; i<col; i++)
            {
                for(int j=0;j<row; j++)
                {
                    if (i == j)
                    {
                        m_norm = m_norm + m1.getVal(i, j) * m1.getVal(i, j);
                    }
                }
            }
            m_norm = Math.Sqrt(m_norm);
            return m_norm;
        }

       // public static void Main()
       // {
       //     Matrix m1;
       //     Matrix cov1;
       //     double trace1;
       //     double det1;

       //     Import(class1);
       //     m1 = ProjectOne.Mean(class1);
       //     cov1 = ProjectOne.Covariance(class1, m1);
       //     trace1 = trace(cov1);
       //     det1 = cov1.findDeterminant();

       //     Console.WriteLine("The mean of the eigendata: ");
       //     m1.printMatrix();
       //     Console.WriteLine("The Covariance Matrix for the eigendata: ");
       //     cov1.printMatrix();
       //     Console.WriteLine("The trace of the Covariance Matrix for the eigendata: " + trace1);
       //     Console.WriteLine("The determinant of the Covariance Matrix: " + det1);
       //     Console.ReadKey();
       //     eigenvalues(cov1);
       ////     double[,] cof_m;
       //     Console.ReadKey();
            
       // }
    }
}
