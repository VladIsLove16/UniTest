using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TestAppOnWpf
{
    class MyXmlSerializer
    {
        public static void SaveStudentResults(string filename, List<Student> Students)
        {
            XmlSerializer  serializer = new XmlSerializer(typeof(List<Student>));
            using (FileStream fileStream = new FileStream(filename, FileMode.Create))
            {
                serializer.Serialize(fileStream, Students);
            }
        }
        public static List<Student> GetStudentResults(string filename)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<Student>));
            using (FileStream fileStream = new FileStream(filename, FileMode.Open))
            {
                List<Student> students = (List<Student>)serializer.Deserialize(fileStream);
                if (students == null)
                {
                    return null;
                }
                return students;
            }
        }
        //public static void SaveToCsv(List<Student> students, string filePath)
        //{
           
        //    StringBuilder csvContent = new StringBuilder();

        //    // Заголовок CSV
        //    csvContent.AppendLine(TestResult.CSVFormatString);

        //    // Данные
        //    foreach (Student student in students)
        //    {
        //        foreach (TestResult TestResult in student.AllResults)
        //        {
        //            csvContent.AppendLine(TestResult.ToString());
        //        }
        //    }
        //    // Сохраняем в файл
        //    //File.WriteAllText(filePath, csvContent.ToString());
        //}
    }
}
