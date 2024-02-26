using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Shapes;

namespace TestAppOnWpf
{
    internal class WordandTxtTestLoader : BaseTestLoader
    {
        private string testsPath;
        private string answersPath;
        public override string TestsPath { get { return testsPath; } set => testsPath = value; }
        public override string AnswersPath { get { return answersPath; } set => answersPath = value; }
        private WordandTxtTestLoader() { }
        public WordandTxtTestLoader(string testsPath,string answersPath)
        {
            TestsPath = testsPath;
            AnswersPath = answersPath;
        }
        public override List<Test> LoadDefaultTests()
        {
            List<Test> tests= new List<Test>();
            string folderPath = testsPath;
            string[] files = Directory.GetFiles(folderPath, "*.txt");
            foreach (string file in files)
            {
                tests.Add(LoadTestFromDirectory(file));
            }
            return tests;
        }
        public override Test LoadTestFromDirectory(string filepath)
        {
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
                int Quectioncount = 0;
                string line;
                while (!src.EndOfStream)
                {
                    while (String.IsNullOrEmpty(line = src.ReadLine()) && !src.EndOfStream) { }
                    Question question = new Question { QuestionString = line };
                    //Loger.Log("Question: " + question.QuestionString);
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
                Loger.Log(Quectioncount.ToString());
            }
            return test;
        }
        public void LoadAnswers(Test test)
        {
            string AnswerFile="";
            string[] answersFiles = Directory.GetFiles(answersPath, "*.txt");
            foreach (string answersFile in answersFiles)
            {
                string[] folders= answersFile.Split('\\');
                foreach (string folder in folders)
                {
                    Loger.PropertyLog(folder, "WordandTxtTestLoader");
                }
                Loger.PropertyLog("Cодержит ли" + folders[folders.Length - 1] + " СТРОКУ " + test.Title.Substring(0, 10), "WordandTxtTestLoader");
                if (folders[folders.Length-1].Contains(test.Title.Substring(0, 10))) { AnswerFile = answersFile; Loger.PropertyLog("ДА", "WordandTxtTestLoader"); break; }
                
                //if (answersFile.Contains(test.Title.Substring(0, 20))) { AnswerFile = answersFile; Loger.PropertyLog("ДА", "WordandTxtTestLoader"); break; }
                Loger.PropertyLog("Нет", "WordandTxtTestLoader");
            }
            //AnswerFile = "D:\\Projects\\VS\\UniTest\\TestAppOnWpf\\Answers\\Тест № 1 по теме «Объекты патентного права»_Ответы.txt";
            if (string.IsNullOrEmpty(AnswerFile)) { MessageBox.Show("Файл ответов на тест " + test.Title + " не найден", "Ошибка при загрузке", MessageBoxButton.OK); return; }
            var srcEncoding = Encoding.GetEncoding(1251);
            try
            {
                using (var src = new StreamReader(AnswerFile, encoding: srcEncoding))
                {
                    string line;
                    for (int j = 0; j < test.QuestionCollection.QuestionCount; j++)
                    {
                        line = src.ReadLine();
                        Loger.PropertyLog("Ответ"+j+line, "WordandTxtTestLoader");
                        test.QuestionCollection[j].SetRightAnswer((Answer)int.Parse(line) - 1);
                    }
                }
            }
            catch(Exception e)
            {
                Loger.PropertyLog("Ошибка"+e.ToString(), "WordandTxtTestLoader");
                MessageBox.Show("Ответы на " + test.Title + " не загружены", "Ошибка при загрузке", MessageBoxButton.OK); 
                return;
            }
        }
    }
}
