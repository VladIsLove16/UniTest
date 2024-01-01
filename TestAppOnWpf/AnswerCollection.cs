using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestAppOnWpf
{
    public class AnswerCollection
    {
        public string AnswersFilePath; 
        private Dictionary<Question, Answer> answers = new Dictionary<Question, Answer>(); 
        public Answer this[Question  question]
        {
            get
            {
                if (!answers.ContainsKey(question))
                    answers[question] = (Answer)(-1);
                return answers[question];
            }
            set
            {
                answers[question] = value;
            }
        }
        public Dictionary<Question, Answer> Answers
        {
            get { return answers;  }
            
        }
        internal void Clear()
        {
            answers.Clear();
        }

        internal IEnumerable<Question> GetQuestions()
        {
            return answers.Keys;
        }
        // public Dictionary<Question, Answer> RightAnswers= new Dictionary<Question, Answer>();
    }
}
