using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestAppOnWpf
{
    [Serializable]
    public class Student
    {
        public static int ID;
        public List<Result> TestResults;
        public string stringName;
        public Student()
        {

        }
        public  Student(string name)
        {
            this.stringName = name;
        }
    }
}
