﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestAppOnWpf
{
    [Serializable]
    internal class StudentDictCollection : BaseStudentCollection
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
                StudentDict[Name]= value;
            }
        }
        public List<Student> GetStudentList()
        {
            return StudentDict.Values.ToList();
        }

        public void Set(Dictionary<string, Student> a)
        {
            StudentDict = a;
        }
        public void Set(List<Student> list)
        {
            foreach (Student student in list)
            {
                student.LoadTestResultsCollection();
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
        public void AddResult(string studentName,Test test,Result result)
        {
            if(!StudentDict.ContainsKey(studentName))
                StudentDict[studentName]=new Student(studentName);
            StudentDict[studentName].AddResult(test,result);

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
    }
}
