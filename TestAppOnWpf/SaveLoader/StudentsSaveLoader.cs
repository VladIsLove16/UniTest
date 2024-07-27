using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestAppOnWpf.SaveLoaderSystem
{
    internal class StudentsSaveLoader : SaveLoader<List<Student>, IStudentCollection>
    {
        public override List<Student> GetData(IStudentCollection service)
        {
            Debug.Write("Student get data");
           return service.GetStudentList();
        }
        public override void SetupData(List<Student> data, IStudentCollection service)
        {
            Debug.Write("Student set data");
            service.Set(data);
        }
    }
}
