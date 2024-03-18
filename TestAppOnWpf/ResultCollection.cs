//using MySqlX.XDevAPI.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestAppOnWpf
{
    [Serializable]
    public class ResultCollection
    {
        private List<Result> results;
        public List<Result> Results
            {
            get { return results; }
            set { results = value; }
        }
        public void Add(Result result)
        {
            results.Add(result);
        }
    }
}
