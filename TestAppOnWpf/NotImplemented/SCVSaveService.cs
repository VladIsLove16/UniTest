using System;
namespace TestAppOnWpf.FileSaveSystem
{
    class SCVSaveService : ISaveService
    {
        public void Save<T>(T data)
        {
            throw new NotImplementedException();
        }
        public bool Load<T>(out T data)
        {
            throw new NotImplementedException();
        }
    }


    //public static void SaveToCsv(List<Student> students, string filePath)
    //{

    //    StringBuilder csvContent = new StringBuilder();

    //    // Заголовок CSV
    //    csvContent.AppendLine(TestResult.CSVFormatString);

    //    // Данные
    //    foreach (Student student in students)
    //    {
    //        foreach (TestResult TestResult in student.AllResults)
    //        {
    //            csvContent.AppendLine(TestResult.ToString());
    //        }
    //    }
    //    // Сохраняем в файл
    //    //File.WriteAllText(filePath, csvContent.ToString());
    //}
}
