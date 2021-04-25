
using System.IO;
using System.Data;
using System.Collections.Generic;

namespace parasha
{
    static class DataWriter
    {
        private static string ExePath = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location),"Data");
     
        public static void NewTable(DataTable table) 
        {
            string fname = ExePath+@"\"+table.TableName + ".csv";

            if (!Directory.Exists(ExePath)) 
            {
                Directory.CreateDirectory(ExePath);
            }
            using (StreamWriter stream = new StreamWriter(fname,true)) 
            {
                string columns = "";
                foreach(DataColumn column in table.Columns) 
                {
                    if(table.PrimaryKey.Equals(column)) 
                    { 
                        column.ColumnName = column + "(PK)"; 
                    }

                    columns = columns + column + ";";
                }
                
                stream.WriteLine(columns.Substring(0,columns.Length-1));
            }
        }

        public static List<DataTable> GetTableList() 
        {
            List<DataTable> OutList = new List<DataTable>();


            if (Directory.Exists(ExePath)) 
            {
                string[] files = Directory.GetFiles(ExePath);
                foreach (string file in files) 
                {
                    DataTable table = new DataTable(Path.GetFileNameWithoutExtension(file));
                    using (StreamReader stream = new StreamReader(file)) 
                    {
                       string columns = stream.ReadLine();
                        string[] c =  columns.Split(";");
                        foreach(string col in c) 
                        {
                            table.Columns.Add(col);
                        }
                    };
                    OutList.Add(table);
                };
            }

            return OutList;
        }

    }
}
