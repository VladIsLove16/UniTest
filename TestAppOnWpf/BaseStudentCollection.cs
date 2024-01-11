using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestAppOnWpf
{
    public interface BaseStudentCollection 
    {
        public Student this[string Name] { get; set; }
        public void AddStudent (Student student);
        public void Clear();
        public void Set(List<Student> a);
        public List<string> GetNames();
        public List<Student> GetStudentList();
        public bool Contains(string Name);  
        public void AddResult(string name,Test test,Result result);
        void Delete(string studentName);
    }
}
