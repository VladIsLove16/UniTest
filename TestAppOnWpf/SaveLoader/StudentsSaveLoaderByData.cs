using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestAppOnWpf.SaveLoaderSystem
{
    internal class StudentsSaveLoaderByData : SaveLoader<List<ResultData>, IStudentCollection>
    {
        public override void SetupData(List<ResultData> resultDatas, IStudentCollection service)
        {
            foreach (ResultData resultData in resultDatas)
            {
                service.AddResult(resultData.StudentName, new Test(resultData.Result.TestTitle), resultData.Result);
            }
        }
        public override List<ResultData> GetData(IStudentCollection service)
        {
            List<Student> students = service.GetStudentList();
            List<ResultData> resultDataList = new List<ResultData>();
            foreach (Student student in students)
            {
                foreach (TestResults testResults in student.GetTestResults())
                {
                    foreach (TestResult testResult in testResults.GetTestResults())
                    {
                        ResultData data = new ResultData(student.StringName, testResult);
                        resultDataList.Add(data);
                    }
                }
            }
            return resultDataList;
        }
    }
}
