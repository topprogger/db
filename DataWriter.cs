
using System;
using System.IO;
using System.Data;
using System.Collections.Generic;
using System.Linq;

namespace parasha
{
    static class DataWriter
    {
        private static string ExePath = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location));
     
        public static void NewTable(DataTable table,string DBname) 
        {
            string fname = ExePath+@"\Databases\"+DBname+@"\"+table.TableName + ".csv";

            if (!Directory.Exists(ExePath)) 
            {
                Directory.CreateDirectory(ExePath);
            }
            try
            {
                using (StreamWriter stream = new StreamWriter(fname, true))
                {
                    string columns = "";
                    foreach (DataColumn column in table.Columns)
                    {
                        if (table.PrimaryKey[0] == column)
                        {
                            column.ColumnName = column + "(PK)";
                        }
                        columns = columns + column + ";";
                    }

                    stream.WriteLine(columns.Substring(0, columns.Length - 1));
                }
            }
            catch (IOException e) { LogError(e.Message); }
        }

        public static List<DataTable> GetTableList(string DbName) 
        {
            List<DataTable> OutList = new List<DataTable>();


            if (Directory.Exists(Path.Combine(ExePath,"Databases",DbName))) 
            {
                string[] files = Directory.GetFiles(Path.Combine(ExePath, "Databases",DbName));
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

        public static string CreateDB(string DBname) 
        {
            DBname = DBname.Replace(" ","_");
            string res =  $"База {DBname} успешно создана";
            string DbPath = ExePath + @$"\DataBases\{DBname}";
            try 
            {
                if (!Directory.Exists(DbPath)) 
                {
                    Directory.CreateDirectory(DbPath);
                }
            }catch(IOException e) { LogError(e.Message); res = "Ошибка создания базы!"; }

            return res;
        }
       
        public static List<string> GetDBlist() 
        {
            
            if (Directory.Exists(Path.Combine(ExePath, "DataBases")))
            {
                try
                {
                    List<string> directories = Directory.GetDirectories(Path.Combine(ExePath, "DataBases")).ToList();
                for(int i=0;i<directories.Count();i++) 
                    {
                        DirectoryInfo info = new DirectoryInfo(directories[i]);
                     directories[i] =  info.Name;
                    }
                    return directories;
                
                }
                catch (IOException e) { LogError(e.Message); }
            }
            return null; 
        }

        public static void LogError(string Message) 
        {

            string Fname = "error_" + System.DateTime.Now.ToString("hh_mm_ss") + ".txt";
            string date = System.DateTime.Now.ToString("dd_MM_yyyy");

            if (!Directory.Exists(Path.Combine(ExePath, "Logs")))
            {
                Directory.CreateDirectory(Path.Combine(ExePath, "Logs"));
            }

            if (!Directory.Exists(Path.Combine(ExePath, "Logs", date)))
            {
                Directory.CreateDirectory(Path.Combine(ExePath, "Logs", date));
            }

            using (StreamWriter stream = new StreamWriter(Path.Combine(ExePath,"Logs",date, Fname))) 
            {
                stream.WriteLine(Message);
            }
            Console.WriteLine("Something goes wrong((( \n press any key to continue...");
            Console.ReadKey();
        }
    }
}
