using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestAppOnWpf
{
    public class XMLRepository : BaseRepository
    {
        private string studentResultsPath;
        public override string StudentResultsPath { get => studentResultsPath; set => studentResultsPath=value; }
        public override List<Student> GetStudentsFromFile()
        {

            Loger.Log("Результаты из " + StudentResultsPath + " загружаются...");
            return MyXmlSerializer.GetStudentResults(studentResultsPath);
        }
        public override void SaveStudentsToFile(List<Student> students)
        {
            Loger.Log("Результаты");
            foreach(Student student in students)
            {
                student.Print();
            }
            Loger.Log("cохраняются в" + StudentResultsPath);
            MyXmlSerializer.SaveStudentResults(StudentResultsPath, students);
            
        }
        private XMLRepository() { }
        public XMLRepository(string studentResultsPath)
        {
           this.studentResultsPath = studentResultsPath;
        }
    }
}
