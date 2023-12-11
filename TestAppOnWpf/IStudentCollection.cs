using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestAppOnWpf
{
    internal interface IStudentCollection 
    {
        public Student this[string Name] { get; set; }
        public void Add (Student student);
        public void Clear();
        public void Set(List<Student> a);
        public List<string> GetNames();
        public List<Student> Get();
        public bool Contains(string Name);  
    }
}
