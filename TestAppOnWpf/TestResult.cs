using System;
using System.Collections.Generic;

namespace TestAppOnWpf
{
    [Serializable]
    public class TestResult
    {
        public string TestTitle;
        public List<Result> Results=new List<Result>();
        public TestResult() { }
        public TestResult( string testTitle, List<Result> results)
        {
            TestTitle= testTitle;
            Results = results;
        }
    }
}