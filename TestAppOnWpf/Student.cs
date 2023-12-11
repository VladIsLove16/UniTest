using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
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
        private Dictionary<Test,Result> TestResults=new Dictionary<Test, Result>();
        public static int StudentCount;
        public int ID;
        public string stringName;
        public Student()
        {
            StudentCount++;//не уверен что это работает как я хочу(что будет при создании временных студентов?)
            ID = StudentCount;
        }
        public  Student(string name):this()
        {
            
             this.stringName = name;
        }
        public void AddResult(Test test,Result result)
        {
            TestResults[test] = result;
        }
    }
}
