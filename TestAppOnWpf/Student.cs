﻿using MySqlX.XDevAPI.Common;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TestAppOnWpf
{
    [Serializable]
    public class Student
    {
        [XmlElement(Order = 1)]
        public string stringName { get; set; }
        [XmlElement(Order = 2)]
        public int ID;
        [XmlElement(Order = 3)]
        public List<TestResult> AllResults = new List<TestResult>();
        [XmlIgnore]
        private Dictionary<string, List<Result>> TestResults=new Dictionary<string, List<Result>>();
        [XmlIgnore]
        public static int StudentCount;
        private Student()
        {
        }
        public  Student(string name)
        {
            StudentCount++;//не уверен что это работает как я хочу(что будет при создании временных студентов?)
            ID = StudentCount;
            this.stringName = name;
        }
        public void AddResult(Test test,Result result)
        {
            Loger.Log("Попытка добавить студенту "+ stringName +" результат:" + result.Print() + " к тесту: "+ test.Title);
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
            foreach (KeyValuePair<string, List<Result>> item in TestResults)
            {
                TestResult testResult = new TestResult(item.Key , item.Value);
                AllResults.Add(testResult);
            }
            Print();
        }

        public bool ContainsTestResult(Test test)
        {
           foreach(TestResult testResult in AllResults)
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
            Loger.Log("У студента "+stringName+" есть следующие результаты:");
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
            }
        }
    }
}
