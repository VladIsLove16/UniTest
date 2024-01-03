using System.Diagnostics;

namespace TestAppOnWpf
{
    public static class Loger
    {
        public static void Log(string a)
        {
            bool log = true;
            if (log)
                Debug.WriteLine(a);
        }
    }
}
