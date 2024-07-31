namespace TestAppOnWpf
{
    public class ResultData
    {
        public ResultData(string name, TestResult testResult) {
            this.StudentName = name;
            this.Result = testResult;
        }
        public string StudentName { get; set; }
        public TestResult Result { get; set; }
    }
}
