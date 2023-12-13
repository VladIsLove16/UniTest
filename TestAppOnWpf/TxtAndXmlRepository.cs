using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestAppOnWpf
{
    internal class TxtAndXmlRepository
    {
        public List<Student> GetStudentsFromFile(string studentsFolder)
        {
           return MyXmlSerializer.GetStudentResults(studentsFolder);

        }
        public void SaveStudents(string where, List<Student> what)
        {
            MyXmlSerializer.SaveStudentResults(where, what);
        }
        public List<Test> LoadTestsFromDirectory()
        {
            List<Test> tests= new List<Test>();
            string folderPath = "D:\\Projects\\VS\\UniTest\\TestAppOnWpf\\Tests";
            string[] files = Directory.GetFiles(folderPath, "*.txt");
            foreach (string file in files)
            {
                Console.WriteLine("Loading test:" + file);
                tests.Add(GetTestFromFile(file));
            }
            return tests;
        }
        public Test GetTestFromFile(string filepath)
        {
            Console.WriteLine("Loading Test from: " + filepath);
            Test test=LoadTestInfo(filepath);
            LoadAnswers(test);
            return test;
        }
        public Test LoadTestInfo(string filepath)
        {
            Test test=new Test();
            var srcEncoding = Encoding.GetEncoding(1251);
            using (var src = new StreamReader(filepath, encoding: srcEncoding))
            {
                test.Title = src.ReadLine();
                Console.WriteLine("Title:" + test.Title);// а что с ними делать
                int Quectioncount = 0;
                string line;
                while (!src.EndOfStream)
                {
                    while (String.IsNullOrEmpty(line = src.ReadLine()) && !src.EndOfStream) { }
                    Question question = new Question { QuestionString = line };
                    //Console.WriteLine("Question: " + question.QuestionString);
                    //while (string.IsNullOrEmpty(src.ReadLine())) { }
                    for (int j = 0; j < 4; j++)
                    {
                        //SetPossibleAnswers
                        while ((line = src.ReadLine()) == null && !src.EndOfStream) { }
                        question.AddPossibleAnswer(line);
                        //Console("Answer: " + line);
                    }
                    test.AddQuestion(question);
                    Quectioncount++;
                }
                Console.WriteLine(Quectioncount);
            }
            return test;
        }
        public void LoadAnswers(Test test)
        {
            string AnswerFile;
            if (!string.IsNullOrEmpty(test.AnswerPath))
                AnswerFile = test.AnswerPath;
            // else if(FilePath!=null)
            // {
            //    
            //    AnswerFile = "D:\\Projects\\VS\\UniTest\\TestAppOnWpf\\Answers_"+;
            //     Answers.AnswersFilePath = AnswerFile;
            //  }
            else
            {
                AnswerFile = "D:\\Projects\\VS\\UniTest\\TestAppOnWpf\\Answers\\Answers_Test1.txt";
                test.AnswerPath = AnswerFile;
            }
            var srcEncoding = Encoding.GetEncoding(1251);
            using (var src = new StreamReader(AnswerFile, encoding: srcEncoding))
            {
                int i = 0; string line;
                while ((line = src.ReadLine()) != null)
                {
                    test.QuestionCollection[i].SetRightAnswer((Answer) int.Parse(line) - 1);
                    Console.WriteLine("LoadAnswers" + i);
                    i++;
                }

            }
        }
    }
}
