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

           menu _out = new menu();
            

             menus.TryGetValue(current_menu, out _out);
            current_menu--;
             if(_out is null) { System.Environment.Exit(1); }
           _out.Draw();          
        }    
    }
}
