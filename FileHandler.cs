using System.IO;
using System.Data;
using System.Collections.Generic;
using System.Linq;
namespace parasha
{
    static class FileHandler
    {
        private static string ExePath = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
        
        public static void WriteCSV(string Str,string Fname) 
        {
            
            using (StreamWriter outputFile = new StreamWriter(Path.Combine(ExePath, Fname)))
            {
             outputFile.WriteLine(Str);
            }

        }

        public static DataTable ReadCsv(string Fname,string DtName) 
        {
          DataTable _out = new DataTable();
            List<string> data = new List<string>();
            using (StreamReader stringReader = new StreamReader(Path.Combine(ExePath, Fname)))
            {
                string line;

                while ((line = stringReader.ReadLine()) != null || data[0]!=DtName)
                {
                    data=line.Split(";").ToList();                    
                }

                
            }
            data.Remove(DtName);
            foreach (string s in data) 
            {
                _out.Columns.Add(s);
            }

            return _out;
        }

    }
}
