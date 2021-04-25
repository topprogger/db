using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
namespace parasha
{
    class Program
    {
        static MenuController MenuController = new MenuController();        
        
        static void Main(string[] args)
        {

            
            menu MainMenu = new menu();
            
            MenuItem tblList = new MenuItem("Список таблиц",new MenuItem.Action (GetTableList));
            MenuItem CrtTbl = new MenuItem("Создать таблицу", new MenuItem.Action(CreateTable));
            MenuItem Exit = new MenuItem("Выйти", new MenuItem.ActionObject (ActionObject=> Environment.Exit(0)));

            MainMenu.add_menu_item(tblList);
            MainMenu.add_menu_item(CrtTbl);
            MainMenu.add_menu_item(Exit);

            MenuController.Add(MainMenu);
            MainMenu.Draw();

           /* ConsoleKeyInfo keyInfo = new ConsoleKeyInfo();

            while (keyInfo.Key != ConsoleKey.Enter)
            {
                
                keyInfo = Console.ReadKey(intercept: true);
                MainMenu.Switch_menu_item(keyInfo);
            }
            MainMenu.ChooseAction(); */           
        }

        public static void CreateTable() 
        {
            string TableName = "", ColumnName = "";
            List<string> Columns = new List<string>();
            DataTable NewDT = new DataTable();
            menu CreateTable = new menu();

            menu SetIdentity = new menu();

            MenuItem Yes = new MenuItem("Да", new MenuItem.ActionObject(ActionObject=>DataWriter.NewTable(NewDT)));
            MenuItem No = new MenuItem("Нет",new MenuItem.ActionObject(ActionObject => Main(new string[] { })));
            
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


           
            foreach(string colName in Columns) 
            {
                
                NewDT.Columns.Add(colName);
                MenuItem item = new MenuItem(colName, new MenuItem.ActionObject(ActionObject =>NewDT=setIdentity(colName, NewDT)));
                SetIdentity.add_menu_item(item);
            }
            SetIdentity.Draw("Необходимо установить первичный ключ");
            CreateTable.Draw($"Создать таблицу {NewDT.TableName}?");
           
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

            int colCnt = dataTable.Columns[0].Caption.Length;
            menu TablePrimaryRow = new menu();
            DataColumn[] Primary = dataTable.PrimaryKey;
            if (dataTable.Rows.Count > 0 && Primary!=null) 
            {
                    foreach(DataRow dr in dataTable.Rows) 
                {
                     MenuItem item = new MenuItem(dr[0].ToString(), new MenuItem.ActionObject(ActionObject => Environment.Exit(0)));
                    TablePrimaryRow.add_menu_item(item);
                }
            }
            TablePrimaryRow.Draw("Выберите столбец на который будет установлен первичный ключ");
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
