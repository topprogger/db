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
           
        }

      public static void BaseMenu(string BD )
        {
            CurrentDB = BD;
            menu MainMenu = new menu();

            MenuItem tblList = new MenuItem("Список таблиц", new MenuItem.ActionObject(ActionObject => GetTableList(CurrentDB)));
            MenuItem CrtTbl = new MenuItem("Создать таблицу", new MenuItem.ActionObject(ActionObject => CreateTable(CurrentDB)));
            MenuItem Exit = new MenuItem("Назад", new MenuItem.Action(MenuController.ReturnToPrevMenu));

            MainMenu.add_menu_item(CrtTbl);
            MainMenu.add_menu_item(tblList);
            MainMenu.add_menu_item(Exit);

            MenuController.Add(MainMenu);
            MainMenu.Draw($"Текущая база -> {BD}");

        }

        public static void  GetDBlist()
        {
            menu DbMenu = new menu();
            bool Dbexist = false;
            var DbList = DataWriter.GetDBlist();

            if (DbList != null)
            {
                if (DbList.Count>0)
                {
                    Dbexist = true;
                    foreach (string _base in DataWriter.GetDBlist())
                    {
                        DbMenu.add_menu_item(new MenuItem(_base,new MenuItem.ActionObject(ActionObject => BaseMenu(_base))));
                    }
                    DbMenu.add_menu_item(new MenuItem("Создать новую базу", new MenuItem.ActionObject(ActionObject => BaseMenu(CurrentDB=DataWriter.CreateDB(Console.ReadLine())) )));
                    DbMenu.add_menu_item(new MenuItem("Выход", new MenuItem.Action(MenuController.ReturnToPrevMenu)));
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
             
                DataWriter.CreateDB(CurrentDB = Console.ReadLine()) ;
                BaseMenu(CurrentDB);
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
            MenuItem No = new MenuItem("Нет", new MenuItem.ActionObject(ActionObject => BaseMenu(CurrentDB)));

            CreateTable.add_menu_item(Yes);
            CreateTable.add_menu_item(No);


            Console.Write("Введите название таблицы:");
            while (TableName is null || TableName.Length<1)
            { TableName = Console.ReadLine();
                TableName= TableName.Replace(" ","_");
            }

            NewDT.TableName = TableName;
            Console.WriteLine("Введите название столбцов( Введите '-' для завершения):");

           
                while (ColumnName is null || ColumnName.Length < 1 || ColumnName!="-")
                { ColumnName = Console.ReadLine();
                

                if (!string.IsNullOrWhiteSpace (ColumnName)&& !string.IsNullOrEmpty(ColumnName) &&  !Columns.Contains(ColumnName)) 
             
                {
                    ColumnName = ColumnName.Replace(" ", "_");
                    Columns.Add(ColumnName);
                }
 
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


           if (DataWriter.GetTableList(CurrentDB).Exists(x => x.TableName == NewDT.TableName))
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
        public static void GetTableList(string dbName)
        {
            int i = 0;
            menu tablesMenu = new menu();

            List<DataTable> tables = DataWriter.GetTableList(CurrentDB);
            if(tables.Count<1 || tables is null) 
            {
                Console.WriteLine($"No tables in {CurrentDB} \n Create new one?(y/n)");
                string ans = Console.ReadLine();
                if (ans=="y") 
                {
                    CreateTable(CurrentDB);
                }
            else { GetDBlist(); };
            }
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
