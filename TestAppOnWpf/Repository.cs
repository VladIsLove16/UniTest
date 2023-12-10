using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestAppOnWpf
{
    internal class Repository
    {
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
            Test test = new Test();
            LoadTestInfo(test,filepath);
            LoadAnswers(test);
            return test;
        }
        public void LoadTestInfo(Test test,string filepath)
        {
            test.FilePath = filepath;
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
                    Question question = new Question { NumberInTest = Quectioncount, Id = Quectioncount, QuestionString = line };
                    //Console.WriteLine("Question: " + question.QuestionString);
                    //while (string.IsNullOrEmpty(src.ReadLine())) { }
                    for (int j = 0; j < 4; j++)
                    {
                        //SetPossibleAnswers
                        while ((line = src.ReadLine()) == null && !src.EndOfStream) { }
                        if (question.PossibleAnswers.Count <= j)
                            question.PossibleAnswers.Add(line);
                        else
                            question.PossibleAnswers[j] = line;
                        //Console("Answer: " + line);
                    }
                    test.Questions.Add(question);
                    Quectioncount++;
                }
                Console.WriteLine(Quectioncount);
            }
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
                //for (int i = 0; i < CurrentTest.QuestionCount; i++)
                int i = 0;
                while (src.ReadLine() != null)
                {
                    Console.WriteLine(src.ReadLine());
                    test.Questions[i].RightAnswer = (Answer)int.Parse(src.ReadLine()) - 1;
                    Console.WriteLine("LoadAnswers" + i);
                    i++;
                }

            }
        }
    }
}
