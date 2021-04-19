using System;
using System.Collections.Generic;
using System.Text;

namespace parasha
{
    class MenuItem
    {
        public delegate void Action();
        public delegate void ActionObject(object obj);
        public string ItemName;
        Action _action;
        ActionObject _actionObject;
        
        public MenuItem(string name, Action action)
        {
            ItemName = name;
            _action = action;
            _actionObject = null;
        }

        public MenuItem(string name, ActionObject action)
        {
            ItemName = name;
            _actionObject = action;
            _action = null; 
        }

        public override string ToString()
        {          
            return $"[{ItemName}]";
        }

        public void setMenuAction(Action action) 
        {
            //menuAction = action;

        }

        public void ExeMenuAction(object o = null) 
        {
            if (_actionObject != null) 
            { 
                _actionObject(o) ; 
                return; 
            }
             _action();
        }

        
    }
}
