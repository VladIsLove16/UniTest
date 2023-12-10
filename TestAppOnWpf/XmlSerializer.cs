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
                    Console.WriteLine("Неудачная попытка чтения списка студентов");
                    return null;
                }
                return students;
            }
        }
    }
}
