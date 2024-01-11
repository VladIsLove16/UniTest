using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Input;

namespace TestAppOnWpf
{
    public static class Loger
    {
        private static Dictionary<string, bool> CanLog = new Dictionary<string, bool>();
        public static void Log(string a)
        {
            bool log = false;
            if (log)
                Debug.WriteLine(a);
        }
        public static void PropertyLog(string a,string property)
        {
            CanLog["TestTitle"] = true;
            CanLog["Student"]=true;
            CanLog["student"]=true;
            bool log = true;
            if (!log) return;
            if (CanLog.TryGetValue(property, out bool value))
                if(value==true)
                    Debug.WriteLine(a);
        }
    }
}
