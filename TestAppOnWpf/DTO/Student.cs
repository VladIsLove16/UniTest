using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace TestAppOnWpf
{
    [Serializable]
    public class Student
    {
        [XmlIgnore]
        private string stringName = "default";
        public string StringName
        {
            get
            {
                return stringName;
            }
            set
            {
                stringName = value;
            }
        }
        public int ID { get; set; }
        private Dictionary<Test, TestResults> TestResultsDict = new Dictionary<Test, TestResults>();
        [XmlIgnore]
        public static int StudentCount;
        private Student()
        {
        }
        public Student(string name)
        {
            StudentCount++;//не уверен что это работает как я хочу(что будет при создании временных студентов?)
            ID = StudentCount;
            this.stringName = name;
        }
        public void AddResult(Test test, TestResult result)
        {
            Loger.Log("Попытка добавить студенту " + stringName + " результат:" + result.Print() + " к тесту: " + test.Title);
            if (!TestResultsDict.ContainsKey(test))
            {
                TestResultsDict[test] = new TestResults();
            }
            TestResultsDict[test].Add(result);
        }
        public List<TestResults> GetTestResults()
        {
            return TestResultsDict.Values.ToList();
        }
        public List<TestResult> GetLastResults()
        {
            List <TestResult> results = new List <TestResult>();
            foreach (TestResults testResults in TestResultsDict.Values)
            {
                TestResult testResult = testResults.GetLast();
                 results.Add(testResult);
                Loger.PropertyLog("last test res:" + testResult.ToString(), "Student");
            }
            return results;
        }
        public bool ContainsTestResult(Test test)
        {
            return TestResultsDict.ContainsKey(test);
        }
        public void PrintResults()
        {
            Loger.Log("У студента " + stringName + " есть следующие результаты:");
            foreach (TestResults testResults in TestResultsDict.Values)
            {
                foreach (TestResult testResult in testResults.GetTestResults())
                {
                    Loger.PropertyLog(testResult.ResultString, "Student");
                }
            }
        }
        public override string ToString()
        {
            return stringName;
        }
    }
}
