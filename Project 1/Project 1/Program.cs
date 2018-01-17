using System;
using System.Data;
using System.IO;
using System.Text;

namespace Project_1
{
    public class Matrix
    {
        public DataTable data = new DataTable();
        public int numRows;
        public int numCols;
        public int[] size = new int[2];

        public int get_cols()
        {
            return numCols;
        }

        public int get_rows()
        {
            return numRows;
        }

        public DataTable get_data()
        {
            return data;
        }

        public int[] get_size()
        {
            return size;
        }

        public void set_size()
        {
            size[0] = numRows;
            size[1] = numCols;
        }


        public void set_numRows()
        {
            numRows = data.Rows.Count;
        }

        public void set_numCols()
        {
            numCols = data.Columns.Count;
        }
        public void set_data(DataTable dt)
        {
            data = dt;
        }

        public void print_data()
        // REDO ME

        {
            int i;
            int j;

            for (i = 0; i < data.Rows.Count; i++)
            {
                for (j = 0; j < data.Columns.Count; j++)
                {
                    Console.Write("[" + data.Rows[i][j].ToString() + "]");
                }
                Console.Write("\n");
            }
        }

        public void createBlankMatrix(Matrix one, int rows, int columns)
        {
            for (int i= 0; i< columns; i++)
            {
                data.Columns.Add(new DataColumn());
            }
            for (int j=0; j < rows; j++)
            {
                data.NewRow();
            }
        }

        public void importDataMatrix(string filepath, int startColumns)
        {
                
            for (int col = 0; col < 2; col++)
                {
                    data.Columns.Add(new DataColumn());
                }
                string[] lines = File.ReadAllLines(filepath);

            foreach (string line in lines)
            {
                var cols = line.Split('\t');
                int f = 0;
                DataRow dr = data.NewRow();
                if (startColumns != 0)
                {
                    for (int cIndex = startColumns - 1; cIndex < startColumns + 1; cIndex++)
                    {
                     dr[f] = cols[cIndex];
                        f++;
                    }
                    data.Rows.Add(dr);
                }
                else
                {
                    data.Rows.Add();
                }
            }
            numCols = data.Columns.Count;
            numRows = data.Rows.Count;
            set_size();
        }

        public void print(Matrix one)
        {
            int i;
            int j;

            for (i = 0; i < one.numRows; i++)
            {
                for (j = 0; j < one.numCols; j++)
                {
                    Console.Write("[" + one.data.Rows[i][j].ToString() + "]");
                }
                Console.Write("\n");
            }
        }

        public Matrix add(Matrix one, Matrix two, Matrix results)
        {
            int i;
            int j;

            if (one.numRows == two.numRows)
            {
                for (i = 0; i < one.numRows; i++)
                {
                    for (j = 0; j < one.numCols; j++)
                    {
                        results.data.Rows[i][j] = Double.Parse(one.data.Rows[i][j].ToString()) + Double.Parse(two.data.Rows[i][j].ToString());
                    }
                }
                return results;
            }
            else
            {
                return zero();
            }
        }

        public Matrix subtract(Matrix one, Matrix two, Matrix results)
        {
            int i;
            int j;

            if (one.numRows == two.numRows)
            {
                for (i = 0; i < one.numRows; i++)
                {
                    for (j = 0; j < one.data.Columns.Count; j++)
                    {
                        results.data.Rows[i][j] = Double.Parse(one.data.Rows[i][j].ToString()) - Double.Parse(two.data.Rows[i][j].ToString());
                    }
                }
                return results;
            }
            else
            {
                return zero();
            }
        }

        public Matrix zero()
        {
            DataTable zero = new DataTable();
            for (int col = 0; col < numCols; col++)
            {
                zero.Columns.Add(new DataColumn());
            }
            for(int rows=0; rows<numRows; rows++)
            {
                zero.Rows.Add();
            }
            for(int i=0; i<numRows; i++)
            {
                for(int j=0; j<numCols; j++)
                {
                    zero.Rows[i][j] = 0;
                }
            }
            Matrix res = new Matrix();
            res.set_data(zero);
            return res;
        }

        public Matrix identity()
        {
            Matrix res = new Matrix();
            res = res.zero();
            int i;
            
            for (i = 0; i < numRows; i++)
            {
                    res.data.Rows[i][i] = 1;
            }
            return res;
        }

        public void scalar(int scale)
        {
            foreach(DataRow dr in data.Rows)
            {
                foreach(DataColumn dc in data.Columns)
                {
                    dr.SetField(dc, Convert.ToDouble(dr[dc]) * scale);
                }
            } 
        }

        public Matrix transpose(Matrix one)
        {
            Matrix res = new Matrix();
            for(int i=0; i< numRows; i++)
            {
                res.data.Columns.Add(new DataColumn());
            }
            for(int j=0; j<numCols; j++)
            {
                res.data.NewRow();
            }
            for(int i=0; i < numRows; i++)
            {
                for(int j=0; j<numCols; j++)
                {
                    res.data.Rows[i][j] = one.data.Rows[j][i]; 
                }
            }
            res.set_numCols();
            res.set_numRows();
            return res;
        }

        public double trace(Matrix one)
        {
            double res = 0;
            for(int i = 0; i < numRows; i++)
            {
                res += Double.Parse(one.data.Rows[i][i].ToString());
            }
            return res;
        }

        public double find_max(Matrix one)
        {
            double res = Math.Abs(Double.Parse(one.data.Rows[0][0].ToString()));
            for(int i =0; i < numRows; i++)
            {
                for(int j=0; j < numCols; j++)
                {
                    if(Math.Abs(Double.Parse(one.data.Rows[i][j].ToString())) > res)
                    {
                        res = Math.Abs(Double.Parse(one.data.Rows[i][j].ToString()));
                    }
                }
            }
            return res;
        }

        public Matrix multiply(Matrix one, Matrix two, Matrix results)
        {
            if (one.numCols == two.numRows)
            {
                results.createBlankMatrix(results, one.numCols * two.numRows, one.numCols * two.numRows);

                for(int i=0; i < one.numRows; i++)
                {
                    for(int j=0; j<two.numCols; j++)
                    {
                        for(int k=0; k < two.numRows; k++)
                        {
                            results.data.Rows[i][j] = (Double.Parse(one.data.Rows[i][k].ToString()) * (Double.Parse(two.data.Rows[k][j].ToString())));
                        }
                    }

                }
                return results;
            }
            else
            {
                return results.zero();
            }
        }


        public static void Main()
        {
            string filepath = Directory.GetCurrentDirectory().ToString() + "\\2017 Fall Project 1 data.txt";
            Matrix one = new Matrix();
            Matrix two = new Matrix();
            Matrix results = new Matrix();
            one.importDataMatrix(filepath, 1);
            two.importDataMatrix(filepath, 3);
            results.importDataMatrix(filepath, 0);

            // results.add(one, two, results);
            one.scalar(2);
            results.print(results);
            Console.ReadKey();
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
