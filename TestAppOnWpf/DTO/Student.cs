using System;
using System.Collections.Generic;
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

        [XmlIgnore]
        public List<TestResult> allResults = new List<TestResult>();
        public List<TestResult> AllResults
        {
            get { return allResults; }
            set { allResults = value; }
        }
        [XmlIgnore]
        public List<Result> lastResults = new List<Result>();
        [XmlIgnore]
        public List<Result> LastResults
        {
            get { return lastResults; }
            set { lastResults = value; }
        }

        [XmlIgnore]
        private Dictionary<string, List<Result>> TestResults = new Dictionary<string, List<Result>>();
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
        public void AddResult(Test test, Result result)
        {
            Loger.Log("Попытка добавить студенту " + stringName + " результат:" + result.Print() + " к тесту: " + test.Title);
            if (!TestResults.ContainsKey(test.Title))
            {
                TestResults[test.Title] = new List<Result>();
            }
            TestResults[test.Title].Add(result);
            UpdateAllResults();
        }

        private void UpdateAllResults()
        {
            AllResults.Clear();
            LastResults.Clear();
            foreach (KeyValuePair<string, List<Result>> item in TestResults)
            {
                TestResult testResult = new TestResult(item.Key, item.Value);
                AllResults.Add(testResult);
                LastResults.Add(item.Value[item.Value.Count - 1]);
            }
        }

        public bool ContainsTestResult(Test test)
        {
            foreach (TestResult testResult in AllResults)
            {
                if (testResult.TestTitle == test.Title)
                {
                    return true;
                }
            }
            return false;
        }
        public void Print()
        {
            Loger.Log("У студента " + stringName + " есть следующие результаты:");
            foreach (TestResult testResult in AllResults)
            {
                Loger.Log(testResult.TestTitle);
                foreach (Result result in testResult.Results)
                {
                    Loger.Log(result.TimeString + " " + result.Print());
                }
            }
        }

        internal void LoadTestResultsCollection()
        {
            TestResults.Clear();
            foreach (TestResult item in AllResults)
            {
                TestResults[item.TestTitle] = item.Results;
                Loger.PropertyLog(item.TestTitle + "из ВСЕ РЕЗ", "TestTitle");
                LastResults.Add(item.Results[item.Results.Count - 1]);
            }
            Print();
            foreach (Result result in LastResults)
            {
                Loger.PropertyLog(result.TestTitle + "выполнен на " + result.RightAnswers, "TestTitle");
            }
        }
    }
}
