using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TestAppOnWpf
{
    internal class TestCollection
    {
        private Dictionary<string, Test> TestDictionary=new Dictionary<string, Test>();
        public Test GetTest(string Title)
        {
            return TestDictionary[Title];
        }
        public void SetTests(List<Test> tests)
        {
            TestDictionary.Clear();
            foreach (Test test in tests)
            {
                TestDictionary[test.Title] = test;
            }

        }
        public void AddTest(Test test)
        {
            if (test.FilePath == null)
            {
                throw new Exception("Test filePath==null");
            }
            if(TestDictionary.ContainsKey(test.Title))
            {
                Console.WriteLine("Тест {test.Title} уже существует", test.Title);
                test.Title = test.Title + "2";
            }
            TestDictionary[test.Title]=test;
        }
        public List<string> GetTestPathes()
        {
            if (TestDictionaryNullDetection()) return null;
            List<string> testsPathes=new List<string>();
            foreach (Test test in TestDictionary.Values)
            {
                testsPathes.Add(test.FilePath);
            }
            return testsPathes;
        }
        public List<string>? GetTestTitles()
        {
            if (TestDictionaryNullDetection()) return null;
            List<string> TestTitles = new List<string>();
            foreach (Test test in TestDictionary.Values)
            {
                TestTitles.Add(test.Title);
            }
            return TestTitles;
        }
        private bool TestDictionaryNullDetection()
        {
            if (TestDictionary.Count == 0) { return true; }
            return false;
        }

    }
}
