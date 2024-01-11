using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace TestAppOnWpf
{
    [Serializable]
    public class TestResult
    {
        public string TestTitle { get; set; }
        public List<Result> Results=new List<Result>();
        public TestResult() { }
        public TestResult( string testTitle, List<Result> results)
        {
            TestTitle= testTitle;
            Results = results;
        }
        public override string ToString()
        {
            Result LastResult= Results[Results.Count-1];
            return LastResult.ToString();


        }
    }
}