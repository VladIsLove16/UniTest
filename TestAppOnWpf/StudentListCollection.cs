using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestAppOnWpf
{
    internal class StudentListCollection //: IStudentCollection 
    {
        List<Student> students=new List<Student>();

        Student this[string Name] { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public List<Student> Get()
        {
            throw new NotImplementedException();
        }
        public void Set(object a)
        {
            throw new NotImplementedException();
        }

        void Set(List<Student> a)
        {
            students = a;
        }
        public void Add(Student student)
        {
            students.Add(student);
        }

        public void Clear()
        {
            students=new List<Student>();
        }


        public List<string> GetNames()
        {
           List<string> names=new List<string> ();
            foreach(Student s in students) { names.Add(s.stringName); }
            return names;
        }

    }
}
