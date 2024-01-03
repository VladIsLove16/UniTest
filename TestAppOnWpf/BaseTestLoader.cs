using System.Collections.Generic;

namespace TestAppOnWpf
{
    internal abstract class BaseTestLoader
    {
        public abstract string TestsPath { get; set; }
        public abstract string AnswersPath { get; set; }
        public abstract List<Test> LoadDefaultTests();
        public abstract Test LoadTestFromDirectory(string path);
    }
}