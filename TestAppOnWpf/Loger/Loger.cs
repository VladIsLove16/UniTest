using System.Collections.Generic;
using System.Diagnostics;

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
        public static void PropertyLog(string a, string property)
        {
            CanLog["TestTitle"] = true;
            CanLog["Student"] = true;
            CanLog["student"] = true;
            CanLog["WordandTxtTestLoader"] = true;
            if (CanLog.TryGetValue(property, out bool value))
                if (value != true) return;
            Debug.WriteLine(a);
        }
    }
}
