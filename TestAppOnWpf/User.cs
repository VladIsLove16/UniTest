using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace TestAppOnWpf
{
    public class User
    {
        private static User instance;
        private User(){ }
        public Result Result=new Result();
        public static User getInstance()
        {
            if (instance == null)
                instance = new User();
            return instance;
        }
        private string Name;//как быстро понять в коде функция получает или сетает имя, исключая создание для этого отдельных функций.
        public Dictionary<Question, Answer> Answers=new Dictionary<Question, Answer>();
        public Question CurrentQuestion { get; set; }
        public Test CurrentTest
        {
            get
            {
                return CurrentTest;
            }
            set
            {
                CurrentTest = value;
                //OnCurrentTestChanged();
                OnTestChanged?.Invoke();
            }
        }
        public delegate void Notify();
        public event Notify OnTestChanged,OnQuestionChanged;
        public static Student ToStudent()
        {
           Student student= new Student();
            student.AddResult(User.getInstance().CurrentTest, User.getInstance().Result);
            student.stringName = User.getInstance().Name;
            return student;

        }
        public void SetName(string Name )
        {
            this.Name = Name;
        }
        public string GetName()
        {
            return Name;
        }
        public void SetCurrentQuestion(int num)
        {
            CurrentQuestion = CurrentTest.QuestionCollection[num];
        }
        public void ClearAnswers()
        {
            Answers.Clear();
        }
        public void SetAllAnswersToNull()
        {
            Answers = new Dictionary<Question, Answer>(); 
            for (int i = 0; i < CurrentTest.QuestionCount; i++)
            {
                Answers[CurrentTest.QuestionCollection[i]] = (Answer)(-1);
            }
        }
        public void SetCurrentTest(Test Test)
        {
            CurrentTest = Test;
            CurrentQuestion = CurrentTest.QuestionCollection[0];
            Result = new Result();
            SetAllAnswersToNull();
        }
        public void NewUserEnter()
        {
            Result = new Result();
            Name = null;
            CurrentTest = null;
            CurrentQuestion = null;
        }

        internal void SetTestResult()
        {
            foreach (Question question in Answers.Keys)
            {
                if (Answers[question] == question.RightAnswer) Result.RightAnswers++;
                else if (Answers[question] == (Answer)(-1)) Result.Skipped++;
                else Result.WrongAnswers++;
            }
        }
    }
}
