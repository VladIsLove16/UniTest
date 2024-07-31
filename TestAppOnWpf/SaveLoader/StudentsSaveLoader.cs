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
            List<Student> students = service.GetStudentList();
            Loger.PropertyLog("SaveLoader.GetData stud list size" + students.Count, "SaveLoader");
            return students;
        }
        public override void SetupData(List<Student> data, IStudentCollection service)
        {
            Debug.WriteLine("Student setiupping data");
            Loger.PropertyLog< Student>(data, "StudentResultSettingUp");
            service.Set(data);
        }
    }
}
