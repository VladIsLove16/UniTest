using System;
using System.Collections.Generic;

namespace TestAppOnWpf
{
    [Serializable]
    public class TestResults
    {
        public string TestTitle { get; set; }
        public static string CSVFormatString { get; internal set; }

        private List<TestResult> Results = new List<TestResult>();
        public TestResults() { }
        public TestResults(string testTitle, List<TestResult> results)
        {
            TestTitle = testTitle;
            Results = results;
        }
        public override string ToString()
        {
            TestResult LastResult = Results[Results.Count - 1];
            return LastResult.ToString();
        }
        public void Add(TestResult result) {
            Results.Add(result);
        }
        public List<TestResult> GetTestResults()
        {
            return Results;
        }
        public TestResult GetLast()
        {
            return Results[Results.Count - 1];
        }
    }
}