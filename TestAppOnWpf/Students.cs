using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestAppOnWpf
{
    [Serializable]
    internal class Students
    {
        public List<Student> StudentList=new List<Student>();
          
        public int StudentCount
        {
            get { return StudentList.Count; }
        }
        public void AddStudent(Student student)
        {
            StudentList.Add(student);
        }
        public List<string> GetStudentNames()
        {
            List<string> names = new List<string>();
            foreach(var student in StudentList)
            {
                names.Add(student.stringName);
            }
            return names;
        }
        public List<string> GetStudentStringNames()
        {
            if(StudentList.Count==0)return new List<string>();
            List<string> names = new List<string>(); 
            foreach (var student in StudentList)
            {
                names.Add(student.stringName);
            }
            return names;
        }
    }
}
