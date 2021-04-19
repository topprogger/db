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

        public static List<List<string>> ReadCsv(string Fname) 
        {
         // DataTable _out = new DataTable();
            List<List<string>> data = new List<List<string>>();
            using (StreamReader stringReader = new StreamReader(Path.Combine(ExePath, Fname)))
            {
                string line;

                while ((line = stringReader.ReadLine()) != null )
                {
                   data.Add(line.Split(';').ToList()); //line.Split(";").to ;                    
                }

                
            }
           
           

            return data;
        }

    }
}
