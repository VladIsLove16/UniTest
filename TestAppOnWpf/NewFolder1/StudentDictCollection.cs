using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestAppOnWpf
{
    [Serializable]
    internal class StudentDictCollection : BaseStudentCollection
    {
        private Dictionary<string, Student> StudentList = new Dictionary<string, Student>();

        public Student this[string Name]
        {
            get
            {
                return StudentList[Name];
            }
            set
            {
                StudentList[Name]= value;
            }
        }
        public List<Student> GetStudentList()
        {
            return StudentList.Values.ToList();
        }

        public void Set(Dictionary<string, Student> a)
        {
            StudentList = a;
        }
        public void Set(List<Student> list)
        {
            foreach (Student student in list) {
                student.LoadTestResultsCollection();
                StudentList[student.stringName] = student; }
        }

        public int StudentCount
        {
            get { return StudentList.Count; }
        }
        public void Add(Student student)
        {
            StudentList[student.stringName] = student;
        }
        public void AddResult(string studentName,Test test,Result result)
        {
            if(!StudentList.ContainsKey(studentName))
                StudentList[studentName]=new Student(studentName);
            StudentList[studentName].AddResult(test,result);

        }
        public bool Contains(string Name)
        {
            return StudentList.ContainsKey(Name);
        }
        public void Clear()
        {
            StudentList = new Dictionary<string, Student>();
        }
        public List<string> GetNames()
        {
            if (StudentList.Count == 0) return new List<string>();
            List<string> names = new List<string>();
            foreach (var student in StudentList)
            {
                names.Add(student.Value.stringName);
            }
            return names;
        }
    }
}
