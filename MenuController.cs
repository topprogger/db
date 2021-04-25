using System;
using System.Collections.Generic;
using System.Text;

namespace parasha
{

    class MenuController
    {
        private Dictionary<int,menu> menus = new Dictionary<int, menu>();
        private int count = 0;
        private int current_menu = -1;
     
        public void Add(menu menu)
        {
            menus.Add(count,menu);
            count++;
            current_menu++;
        }

        public void ReturnToPrevMenu()
        {
            //int _currentmenu = menus.IndexOf(currentMenu);
            //menu _menuToDraw = currentMenu;
           menu _out = new menu();
            current_menu--;
            if (current_menu < 0)
            {
                System.Environment.Exit(1);
            }
            menus.TryGetValue(current_menu,out _out);           
            _out.Draw();
           
        }
    }
}
