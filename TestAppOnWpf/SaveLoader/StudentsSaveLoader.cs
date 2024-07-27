using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestAppOnWpf.SaveLoaderSystem
{
    internal class StudentsSaveLoader : SaveLoader<List<Student>, StudentDictCollection>
    {
        public override List<Student> GetData(StudentDictCollection service)
        {
           return service.GetStudentList();
        }
        public override void SetupData(List<Student> data, StudentDictCollection service)
        {
            service.Set(data);
        }
    }
}
