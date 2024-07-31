//using MySqlX.XDevAPI.Common;
using System;
using System.Collections.Generic;

namespace TestAppOnWpf
{
    [Serializable]
    public class ResultCollection
    {
        private List<TestResult> results;
        public List<TestResult> Results
        {
            get { return results; }
            set { results = value; }
        }
        public void Add(TestResult result)
        {
            results.Add(result);
        }
    }
}
