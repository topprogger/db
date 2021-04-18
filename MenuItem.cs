using System;
using System.Collections.Generic;
using System.Text;

namespace parasha
{
    class MenuItem
    {
        public delegate void Action();
        public string ItemName;
        Action _action;
        public MenuItem(string name, Action action)
        {
            ItemName = name;
            _action = action;
        }



        public override string ToString()
        {          
            return $"[{ItemName}]";
        }

        public void setMenuAction(Action action) 
        {
            //menuAction = action;

        }

        public void ExeMenuAction() 
        {
            _action();
        }

        
    }
}
