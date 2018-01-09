using System;
using System.Data;
using System.Text;

namespace Project_1
{
    public class Matrix
    {
        public DataTable data;
        public int numRows;
        public int cols;
        public int[] size;

        public int get_cols()
        {
            return cols;
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
     

        public int set_rows {get;set;}

        public int set_cols { get; set; }

        public string print_data()
        {
            string dataPrint = string.Empty;
            StringBuilder sb = new StringBuilder();
            foreach(DataRow dataRow in data.Rows) { 
                foreach(var item in dataRow.ItemArray)
                {
                    sb.Append(item);
                    sb.Append(',');
                }
                sb.AppendLine();
            }
            dataPrint = sb.ToString();
            return dataPrint;
        }

        public Matrix(DataTable dt)
        {
            int i;
            data = dt;
            numRows = data.Rows.Count;
            cols = data.Columns.Count;
            size[0]=numRows;
            size.SetValue(cols, 1);
            for (i = 0; i < cols; i++)
            {
                data.Columns[i].DataType = typeof(Int32);
            }
        }
        

    }

    public class Operations
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

            for(i=0; i<one.Rows.Count; i++)
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

        public static void importData(string filepath)
        {
            for(int col = 0; col < 2; col++)
            {
                m_dataTableOne.Columns.Add(new DataColumn());
                m_dataTableTwo.Columns.Add(new DataColumn());
                m_results.Columns.Add(new DataColumn());
            }
            string[] lines = System.IO.File.ReadAllLines(filepath);

            foreach(string line in lines)
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
            for (int rIndex = 0; rIndex<m_dataTableOne.Rows.Count; rIndex++)
            {
                m_results.Rows.Add();
            }
        }

        public static void Main()
            {
            string filepath = "E:\\Drive\\CSC340\\2017 Fall Project 1 data.txt";
            importData(filepath);

            m_results = Operations.add(m_dataTableOne, m_dataTableTwo);

            print(m_results);
            Console.ReadKey();
            }



       
    }
    
}
