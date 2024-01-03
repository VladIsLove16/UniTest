using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestAppOnWpf
{
    public abstract class BaseRepository
    {
        public abstract string StudentResultsPath { get; set; }
        public abstract void SaveStudentsToFile(List<Student> students); 
        public abstract List<Student> GetStudentsFromFile();
    }
}
