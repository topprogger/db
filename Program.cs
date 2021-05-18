using System;
using System.Collections.Generic;
using System.Data;
namespace parasha
{
    class Program
    {
        static MenuController MenuController = new MenuController();
        static string CurrentDB ="";
        static void Main(string[] args)
        {

            GetDBlist();

            
            menu MainMenu = new menu();

            MenuItem tblList = new MenuItem("Список таблиц", new MenuItem.Action(GetTableList));
            MenuItem CrtTbl = new MenuItem("Создать таблицу", new MenuItem.ActionObject(ActionObject=>CreateTable(CurrentDB)));
            MenuItem Exit = new MenuItem("Выйти", new MenuItem.ActionObject(ActionObject => Environment.Exit(0)));

            MainMenu.add_menu_item(CrtTbl);
            MainMenu.add_menu_item(tblList);
            MainMenu.add_menu_item(Exit);

            MenuController.Add(MainMenu);
            MainMenu.Draw();

        }

        public static void  GetDBlist()
        {
            menu DbMenu = new menu();
            bool Dbexist = false;
            var DbList = DataWriter.GetDBlist(CurrentDB);

            if (DbList != null)
            {
                if (DbList.Count>0)
                {
                    Dbexist = true;
                    foreach (string _base in DataWriter.GetDBlist(CurrentDB))
                    {
                        DbMenu.add_menu_item(new MenuItem(_base, Action => CurrentDB = _base));
                    }
                    MenuController.Add(DbMenu);
                    DbMenu.Draw();
                }
               
            }
            if(!Dbexist) 
            {
                Console.WriteLine("No databases found \n To create base press any key...");
                Console.ReadKey();
                Console.Clear();
                Console.Write("Enter DB name:");
             
                DataWriter.CreateDB(Console.ReadLine());
            }
            
        }

       

        public static void CreateTable(string DbName)
        {
            Console.Clear();
            string TableName = "", ColumnName = "";
            List<string> Columns = new List<string>();
            DataTable NewDT = new DataTable();
            
            menu CreateTable = new menu();
            menu SetIdentity = new menu();

            MenuItem Yes = new MenuItem("Да", new MenuItem.ActionObject(ActionObject => DataWriter.NewTable(NewDT, DbName)));
            MenuItem No = new MenuItem("Нет", new MenuItem.ActionObject(ActionObject => Main(new string[] { })));

            CreateTable.add_menu_item(Yes);
            CreateTable.add_menu_item(No);


            Console.Write("Введите название таблицы:");
            TableName = Console.ReadLine();

            NewDT.TableName = TableName;
            Console.WriteLine("Введите название столбцов( Введите '-' для завершения):");

            while (ColumnName != "-")
            {
                ColumnName = Console.ReadLine();
                if (Columns.Contains(ColumnName)) { Console.Write("Наименования столбцов должны быть уникальны"); }
                else { Columns.Add(ColumnName); }
            }
            Columns.Remove("-");

            if (Columns.Count == 0)
            {
                MenuController.ReturnToPrevMenu();
            }
            foreach (string colName in Columns)
            {

                NewDT.Columns.Add(colName);
                MenuItem item = new MenuItem(colName, new MenuItem.ActionObject(ActionObject => NewDT = setIdentity(colName, NewDT)));
                SetIdentity.add_menu_item(item);
            }


           if (DataWriter.GetTableList().Exists(x => x.TableName == NewDT.TableName))
            {
                menu tblExist = new menu()  ;

                MenuItem Y = new MenuItem("Да", new MenuItem.ActionObject(ActionObject=>Program.CreateTable(CurrentDB)));
                MenuItem N = new MenuItem("Нет", new MenuItem.Action(MenuController.ReturnToPrevMenu));
                tblExist.add_menu_item(Y);
                tblExist.add_menu_item(N);

                tblExist.Draw("Таблица с таким названием уже существует. Повторить ввод?");
            }

            SetIdentity.Draw("Необходимо установить первичный ключ");
            CreateTable.Draw($"Создать таблицу {NewDT.TableName}?");
            MenuController.ReturnToPrevMenu();
        }
       
        private static DataTable setIdentity(string ColName,DataTable table) 
        { 
            table.PrimaryKey = new DataColumn[] { table.Columns[ColName]};;
            return table;
        }

        private static DataTable FillTestData(DataTable dt)
        {
            DataTable ret = dt;

            for (int i= 0; i < 10; i++) 
            {
                DataRow row = ret.NewRow();
                foreach (DataColumn col in ret.Columns) 
                {                   
                    row[col.ColumnName] = i;
                } ;

                ret.Rows.Add(row);
            }
            DataColumn[] PrimaryKeyColumns = new DataColumn[1];
            PrimaryKeyColumns[0] = ret.Columns[0];
            ret.PrimaryKey = PrimaryKeyColumns; 
            return ret;
        }

        private static void ShowDT(DataTable dataTable) 
        {
            int Width = 0;
            foreach (DataColumn column in dataTable.Columns) 
            {
                Console.SetCursorPosition(Width,0);
                Console.Write("|"+column+"|");
                Width = Width + column.ColumnName.Length+1;
            }
            Console.ReadKey();
            MenuController.ReturnToPrevMenu();
        }
        public static void GetTableList()
        {
            int i = 0;
            menu tablesMenu = new menu();

            List<DataTable> tables = DataWriter.GetTableList();
              
            foreach (DataTable table in tables) 
            {               
                MenuItem menuItem = new MenuItem(table.TableName, new MenuItem.ActionObject(ActionObject => ShowDT(table)));
                tablesMenu.add_menu_item(menuItem);
                i++;
            }

            MenuItem back = new MenuItem("Back", new MenuItem.Action(MenuController.ReturnToPrevMenu));
            tablesMenu.add_menu_item(back);
            MenuController.Add(tablesMenu);
            tablesMenu.Draw();               
        }

    }
}
