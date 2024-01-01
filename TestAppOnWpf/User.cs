using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
       
        private AnswerCollection answerCollection = new AnswerCollection();
         public AnswerCollection Answers
        {
            get { return answerCollection; }
        }

        private Question currentQuestion;
        public Question CurrentQuestion
        {
            get
            {
                return currentQuestion;
            }
            set
            {
                currentQuestion = value;
                OnQuestionChanged?.Invoke();
            }
        }
        private Test currentTest;
        public Test CurrentTest
        {
            get
            {
                return currentTest;
            }
            set
            {
                currentTest = value;
                if (value == null) return;
                OnCurrentTestChanged();
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
       
        private void OnCurrentTestChanged()
        {
            CurrentQuestion = CurrentTest.QuestionCollection[0];
            Console.WriteLine("Текущий ворос:"+CurrentQuestion.QuestionString );
            SetAllAnswersToNull();
        }
        public void SetCurrentTest(Test Test)
        {
            CurrentTest = Test;
            CurrentQuestion = CurrentTest.QuestionCollection[0];
            SetAllAnswersToNull();
        }
        public void SetAllAnswersToNull()
        {
            ClearAnswers();
            for (int i = 0; i < CurrentTest.QuestionCount; i++)
            {
                Answers[CurrentTest.QuestionCollection[i]] = (Answer)(-1);
            }
        }
        public void ClearAnswers()
        {
            Answers.Clear();
        }
        public void NewUserEnter()
        {
            Result = new Result();
            SetAllAnswersToNull();
        }
        internal void SetTestResult()
        {
            foreach (Question question in Answers.GetQuestions())
            {
                if (Answers[question] == question.RightAnswer) Result.RightAnswers++;
                else if (Answers[question] == (Answer)(-1)) Result.Skipped++;
                else Result.WrongAnswers++;
            }
        }

        internal void SaveAnswer(Question question,Answer answer)
        {
             answerCollection[question] = answer;
        }
    }
}
