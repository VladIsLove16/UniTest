using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TestAppOnWpf
{
    public class User
    {
        private static User instance;
        private User(){ }
        private Result result=new Result("defaul",0,0,0,TimeSpan.Zero);
        public Result Result
        { 
            get {return result;}
            private set { result = value; }
        }
        public TimeSpan ElapsedTime;
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
            Loger.Log("Текущий вопрос:"+CurrentQuestion.QuestionString );
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
            if (Answers == null) return;
            if(CurrentTest == null) return;
            Answers.Clear();
            for (int i = 0; i < CurrentTest.QuestionCount; i++)
            {
                Answers[CurrentTest.QuestionCollection[i]] = (Answer)(-1);
            }
        }
        public void NewUserEnter()
        {
            ClearResult();
            SetAllAnswersToNull();
        }

        private void ClearResult()
        {
            if(result== null) { return; }
            Result = new Result();
        }

        internal void SetResult()
        {
            string title = CurrentTest.Title;
            TimeSpan time = ElapsedTime;
            int r=0, w=0, s=0;
            foreach (Question question in Answers.GetQuestions())
            {
                if (Answers[question] == question.RightAnswer) r++;
                else if (Answers[question] == (Answer)(-1)) s++;
                else w++;
            }
            Result=new Result(title,r,w,s,time);
        }

        internal void SaveAnswer(Question question,Answer answer)
        {
             answerCollection[question] = answer;
        }
    }
}
