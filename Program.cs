using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
namespace parasha
{
    class Program
    {
        static void Main(string[] args)
        {
            
            
            menu menu = new menu();         

            MenuItem tblList = new MenuItem("Список таблиц",new MenuItem.Action (TableList));
            MenuItem CrtTbl = new MenuItem("Создать таблицу", new MenuItem.Action(CreateTable));
           // MenuItem menu3 = new MenuItem("Punkt 3");



            menu.add_menu_item(tblList);
            menu.add_menu_item(CrtTbl);
           // menu.add_menu_item(menu3);

      
            menu.Draw();

            ConsoleKeyInfo keyInfo = new ConsoleKeyInfo();

            while (keyInfo.Key != ConsoleKey.Enter)
            {
                
                keyInfo = Console.ReadKey(intercept: true);
                menu.Switch_menu_item(keyInfo);
            }
            menu.ChooseAction();            
        }


        private static void RetToMainMenu() 
        {
            string[] _args = new string[1];
            Main(_args);
        }
        public static void CreateTable() 
        {
            string TableName, ColumnName="",CSV;
            List<string> Columns = new List<string>();

            Console.Write("Введите название таблицы:");
            TableName = Console.ReadLine();
            Console.WriteLine("Введите название столбцов(- для завершения):");

            

            while (ColumnName != "-")
            {
                ColumnName = Console.ReadLine();
                Columns.Add(ColumnName);
            }
            Columns.Remove("-");
            //foreach(string col in Columns) { Console.WriteLine(col); }

            CSV = TableName + ";";
            foreach (string col in Columns)
            {
                CSV =CSV +col+";";
            }
            CSV = CSV.Substring(0,CSV.Length-1);
            FileHandler.WriteCSV(CSV,"Tables.csv");

            //Console.WriteLine(CSV);
            Console.ReadKey();
            RetToMainMenu();
        }

        private static void ShowDT(DataTable dataTable) 
        {
            Console.WriteLine("this is " + dataTable.TableName);

        }
        public static void TableList()
        {
            int i = 0;
            menu tablesMenu = new menu();
            List<List<String>> Raw_tabledata = FileHandler.ReadCsv("Tables.csv");
           // List<DataTable> tables = new List<DataTable>();
            foreach (List<string> table in Raw_tabledata) 
            {
                DataTable dt = new DataTable(table[0]);
                var x = table.Skip(0).ToList();
               
                foreach (string column in x.ToList())
                {
                    dt.Columns.Add();
                }
                
                MenuItem menuItem = new MenuItem(table[0], new MenuItem.ActionObject(ActionObject => ShowDT(dt)));
                tablesMenu.add_menu_item(menuItem);
                i++;
            }
            tablesMenu.Draw();

            ConsoleKeyInfo keyInfo = new ConsoleKeyInfo();

            while (keyInfo.Key != ConsoleKey.Enter)
            {

                keyInfo = Console.ReadKey(intercept: true);
                tablesMenu.Switch_menu_item(keyInfo);
            }
            tablesMenu.ChooseAction();
           Console.ReadKey();
            tablesMenu.Draw();
        }

    }
}
