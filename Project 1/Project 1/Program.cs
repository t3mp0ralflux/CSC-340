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

        public string print_data()
        // REDO ME

        {
            string dataPrint = string.Empty;
            StringBuilder sb = new StringBuilder();
            foreach (DataRow dataRow in data.Rows)
            {
                foreach (var item in dataRow.ItemArray)
                {
                    sb.Append(item);
                    sb.Append(',');
                }
                sb.AppendLine();
            }
            dataPrint = sb.ToString();
            return dataPrint;
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
                    for (j = 0; j < one.data.Columns.Count; j++)
                    {
                        results.data.Rows[i][j] = Double.Parse(one.data.Rows[i][j].ToString()) + Double.Parse(two.data.Rows[i][j].ToString());
                    }
                }
                return results;
            }
            else
            {
                return /*zero*/(results);
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

            results.add(one, two, results);

            results.print(results);
            Console.ReadKey();
        }

    }

/*   public class Operations
    {
        public static DataTable m_dataTableOne = new DataTable();
        public static DataTable m_dataTableTwo = new DataTable();
        public static DataTable m_results = new DataTable();


        public static DataTable add(DataTable one, DataTable two)
        {
            int i;
            int j;

            if (one.Rows.Count == two.Rows.Count)
            {
                for (i = 0; i < one.Rows.Count; i++)
                {
                    for (j = 0; j < one.Columns.Count; j++)
                    {
                        m_results.Rows[i][j] = Double.Parse(one.Rows[i][j].ToString()) + Double.Parse(two.Rows[i][j].ToString());
                    }
                }
                return m_results;
            }
            else
            {
                return zero(m_results);
            }
        }

        public static DataTable subtract(DataTable one, DataTable two)
        {
            int i;
            int j;

            if (one.Rows.Count == two.Rows.Count)
            {
                for (i = 0; i < one.Rows.Count; i++)
                {
                    for (j = 0; j < one.Columns.Count; j++)
                    {
                        m_results.Rows[i][j] = Double.Parse(one.Rows[i][j].ToString()) - Double.Parse(two.Rows[i][j].ToString());
                    }
                }
                return m_results;
            }
            else
            {
                return zero(m_results);
            }
        }

        public static DataTable identity(DataTable one)
        {
            int i;
            int j;
            one = zero(one);

            for (i = 0; i < one.Rows.Count; i++)
            {
                for (j = 0; j < one.Columns.Count; j++)
                    one.Rows[i][j] = 1;
            }
            return one;
        }

        public static DataTable zero(DataTable one)
        {
            int i;
            int j;
            for (i = 0; i < one.Rows.Count; i++)
            {
                for (j = 0; j < one.Columns.Count; j++)
                    one.Rows[i][j] = 0;
            }
            return one;
        }

        public static void print(DataTable results)
        {
            int i;
            int j;

            for (i = 0; i < results.Rows.Count; i++)
            {
                for (j = 0; j < results.Columns.Count; j++)
                {
                    Console.Write("[" + results.Rows[i][j].ToString() + "]");
                }
                Console.Write("\n");
            }
        }

        public static void importDataMatrix(string filepath, int startColumns)
        {
            for (int col = 0; col < 2; col++)
            {
                m_dataTableOne.Columns.Add(new DataColumn());
                m_dataTableTwo.Columns.Add(new DataColumn());
                m_results.Columns.Add(new DataColumn());
            }
            string[] lines = System.IO.File.ReadAllLines(filepath);

            foreach (string line in lines)
            {
                var cols = line.Split('\t');

                DataRow dr = m_dataTableOne.NewRow();
                DataRow dr2 = m_dataTableTwo.NewRow();

                for (int cIndex = 0; cIndex < 2; cIndex++)
                {
                    dr[cIndex] = cols[cIndex];
                    dr2[cIndex] = cols[cIndex + 2];
                }
                //for (int cIndex = 2; cIndex <4; cIndex++)
                //{
                //    dr2[cIndex] = cols[cIndex];
                //}
                m_dataTableOne.Rows.Add(dr);
                m_dataTableTwo.Rows.Add(dr2);
            }
            DataRow dr3 = m_results.NewRow();
            for (int rIndex = 0; rIndex < m_dataTableOne.Rows.Count; rIndex++)
            {
                m_results.Rows.Add();
            }
        }

        public static void Main()
        {
            string filepath = Directory.GetCurrentDirectory().ToString() + "\\2017 Fall Project 1 data.txt";
            importData(filepath);

            m_results = Operations.add(m_dataTableOne, m_dataTableTwo);

            print(m_results);
            Console.ReadKey();
        }




    }
    */
}
