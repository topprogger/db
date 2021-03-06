using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace parasha
{
    class menu
    {

        private List<MenuItem> Items;
        private int _CurrentMenuPos = 0;
        public MenuItem currentItemName;
        public menu() 
        {
            Items = new List<MenuItem>();
        }
 
        public void Draw(string AdditionalMessage = null) 
        {
            var _Items = Items.Count;
            int i = 0;

           
            Console.Clear();

            if (AdditionalMessage != null)
            {
                Console.SetCursorPosition(Console.WindowWidth / 2-(AdditionalMessage.Length/2), Items.Count);
                Console.WriteLine(AdditionalMessage);
            }

            if (_Items>0)
            {
                foreach (MenuItem itm in Items)
                {
                    Console.SetCursorPosition(Console.WindowWidth/2 , i);
                    Console.Write(itm.ToString());
                    i++;
                }
        
            } else 
            {
              
                Console.SetCursorPosition(Console.WindowWidth / 2, 0);
                Console.Write ("|В меню нет пунктов|");
            }
            
            Console.SetCursorPosition(Console.WindowWidth / 2 - 2, _CurrentMenuPos);
            Console.Write(">");

            ConsoleKeyInfo keyInfo = new ConsoleKeyInfo();

            while (keyInfo.Key != ConsoleKey.Enter)
            {

                keyInfo = Console.ReadKey(intercept: true);
                Switch_menu_item(keyInfo);
            }
            ChooseAction();
        }

        public void Switch_menu_item(ConsoleKeyInfo keyInfo)
        {
           
                if (keyInfo.Key == ConsoleKey.UpArrow)
                {
                DelPointer(_CurrentMenuPos);
                _CurrentMenuPos--;          
                RedrawPointer(_CurrentMenuPos);               
                }

                if (keyInfo.Key == ConsoleKey.DownArrow)
                {

                DelPointer(_CurrentMenuPos);
                _CurrentMenuPos++;
                RedrawPointer(_CurrentMenuPos);                               
                }
            
        }

        private void RedrawPointer(int CurrentMenuPos) 
        {
            if (CurrentMenuPos > Items.Count-1 ) 
            { _CurrentMenuPos = 0; }
            if (CurrentMenuPos < 0) { _CurrentMenuPos = Items.Count-1; }

            Console.SetCursorPosition(Console.WindowWidth / 2 - 2, _CurrentMenuPos);
            Console.Write(">"); 
        }

        private void DelPointer(int CurrentMenuPos) 
        {
            if (CurrentMenuPos < 0) { _CurrentMenuPos = 0;}
            
            Console.SetCursorPosition(Console.WindowWidth / 2 - 2, _CurrentMenuPos);
            Console.Write(" ");

        }

       public void add_menu_item(MenuItem Item) 
       {
            Items.Add(Item);
       }

        public int getItemsCount()
        {
            return Items.Count;
        }

        public void ChooseAction() 
        {

            var menuItem = Items[_CurrentMenuPos];
            currentItemName = menuItem;
            Console.Clear();
            menuItem.ExeMenuAction();
        }

     
    }
}
