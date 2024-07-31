using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestAppOnWpf
{
    [Serializable]
    internal class StudentDictCollection : IStudentCollection
    {
        private Dictionary<string, Student> StudentDict = new Dictionary<string, Student>();
        public Student this[string Name]
        {
            get
            {
                return StudentDict[Name];
            }
            set
            {
                StudentDict[Name] = value;
            }
        }
        public List<Student> GetStudentList()
        {
            Loger.PropertyLog("Getting studList size:"+ StudentDict.Values.ToList().Count + " when dict size: " + StudentDict.Count, "StudentDictCollection");
            return StudentDict.Values.ToList();
        }
        public void Load(List<Student> keyValuePairs)
        {
            HardCopy(keyValuePairs);
        }
        private void HardCopy(List<Student> students)
        {
            foreach (Student student in students)
            {
                StudentDict[student.StringName] = student;
            }
        }
        public void Set(List<Student> list)
        {
            foreach (Student student in list)
            {
                StudentDict[student.StringName] = student;
            }
        }
        public int StudentCount
        {
            get { return StudentDict.Count; }
        }
        public void AddStudent(Student student)
        {
            StudentDict[student.StringName] = student;
        }
        public void AddResult(string studentName, Test test, TestResult result)
        {
            if (!StudentDict.ContainsKey(studentName))
                StudentDict[studentName] = new Student(studentName);
            StudentDict[studentName].AddResult(test, result);

        }
        public bool Contains(string Name)
        {
            return StudentDict.ContainsKey(Name);
        }
        public void Clear()
        {
            StudentDict = new Dictionary<string, Student>();
        }
        public List<string> GetNames()
        {
            return StudentDict.Keys.ToList<string>();
        }

        public void Delete(string studentName)
        {
            StudentDict.Remove(studentName);
        }
        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach(Student student in StudentDict.Values)
            {
                stringBuilder.AppendLine(student.ToString() + "\n");
            }
            return stringBuilder.ToString();
        }
    }
}
