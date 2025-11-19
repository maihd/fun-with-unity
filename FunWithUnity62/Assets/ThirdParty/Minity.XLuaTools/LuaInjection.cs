using System;
using UnityEngine;
using XLua.LuaDLL;

namespace Minity.XLuaTools
{
    [Serializable]
    public class LuaInjection
    {
        public string       Name;
        public GameObject   Object;
        public Component    Component;

        public double       Number;
    }
}
