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
        public string Name;//как быстро понять в коде функция получает или сетает имя, кроме создания для этого отдельных функций.
        public Dictionary<Question, Answer> Answers=new Dictionary<Question, Answer>();
        public Question CurrentQuestion { get; set; }
        public Test CurrentTest { get; set; }
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
            CurrentQuestion = CurrentTest.Questions[num];
        }
        public void ClearAnswers()
        {
            Answers.Clear();
        }
        public void SetAllAnswersToNull()
        {
            for(int i = 0; i < CurrentTest.QuestionCount; i++)
            {
                Answers[CurrentTest.Questions[i]] = (Answer)(-1);
            }
        }
        public void OnCurrentTestChanged()
        {

        }
        public void NewUserEnter()
        {
            Result = new Result();
            Name = null;
            Answers = new Dictionary<Question, Answer>();
            SetAllAnswersToNull();
            SetCurrentQuestion(0);

        }
    }
}
