using System.Collections.Generic;
using System.Diagnostics;
using TestAppOnWpf.SaveLoaderSystem;

namespace TestAppOnWpf
{
    public static class Loger
    {
        enum LogType
        {
            studentsResultAdded
        }

        private static Dictionary<string, bool> CanLog = new Dictionary<string, bool>();
        public static void Log(string a)
        {
            bool log = true;
            if (log)
                Debug.WriteLine(a);
        }
        public static void PropertyLog(string a, string property)
        {
            CanLog["TestTitle"] = false;
            CanLog["Student"] = false;
            CanLog["student"] = false;
            CanLog["SaveSystem"] = false;
            CanLog["Question"] = false;
            CanLog["WordandTxtTestLoader"] = false;
            CanLog["Closing"] = true;
            CanLog["SaveLoadersManager"] = true;
            CanLog["JsonSaveService"] = true;
            CanLog["Repository"] = true; 
            CanLog["StudentResultAdded"] = true;
            CanLog["ResultsWindow"] = true;
            CanLog["SaveLoader"] = true;
            CanLog["StudentResultSettingUp"] = true;
            if (CanLog.TryGetValue(property, out bool value))
                if (value != true)
                    return;
            Debug.WriteLine(a);
        }
        public static void PropertyLog<T>(List<T> values, string property)
        {
            foreach (var t in values)
            {
                PropertyLog(t.ToString(), property);
            }
        }
    }
}
