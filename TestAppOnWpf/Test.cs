using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;
using static System.Net.Mime.MediaTypeNames;

namespace TestAppOnWpf
{
    [Serializable]
    public class Test
    {
        public Test() { }
        public Test(string title) {Title = title; }
        private QuestionCollection questionCollection = new QuestionCollection();
        public QuestionCollection QuestionCollection
        {
            get
            {
                return questionCollection;
            }
            set
            {
                questionCollection = value; 
            }
        }
        public int QuestionCount { get { return QuestionCollection.QuestionCount; } }
        public string Title;
        //public Answers Answers=new Answers();
        public string AnswerPath;
        public int Number;
        public static bool operator ==(Test obj1, Test obj2)
        {
            if (ReferenceEquals(obj1, obj2))
                return true;

            if (ReferenceEquals(obj1, null) || ReferenceEquals(obj2, null))
                return false;

            return obj1.Title == obj2.Title;
        }
        public static bool operator !=(Test test, Test test1)
        {
            return !(test == test1);
        }
        //private int questionCount;
        public void AddQuestion(Question question)
        {
            QuestionCollection.AddQuestion(question);
        }
        public void CreateQuestion(string questionString,List<string>possibleAnswers,Answer rightAnswer )
        {
            Question question = new Question() { QuestionString = questionString,NumberInTest=QuestionCount,RightAnswer= rightAnswer,PossibleAnswers= possibleAnswers };
            AddQuestion(question);
        }
        public QuestionCollection GetQuestionCollection()
        {
            return QuestionCollection;
        }
        public void SetQuestions(QuestionCollection questions)
        {
            QuestionCollection = questions;
        }
        public void ShuffleQuestions()
        {
            QuestionCollection.ShuffleQuestions();
        }

        public override bool Equals(object obj)
        {
            return obj is Test test &&
                   Title == test.Title;
        }

        public override int GetHashCode()
        {
            return 434131217 + EqualityComparer<string>.Default.GetHashCode(Title);
        }
    }
}
