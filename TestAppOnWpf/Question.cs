using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestAppOnWpf
{
    [Serializable]
    public class Question
    {
        static Random r = new Random();
        public List<string> PossibleAnswers = new List<string>();
        public int PossibleAnswersCount { get { return PossibleAnswers.Count; } }
        public int NumberInTest;
        public int Id;
        public string QuestionString;
        private Answer rightAnswer= (Answer)(-1);
        public Answer RightAnswer
        { get { return rightAnswer; }
            set {
                if((int)value < -1) { Loger.Log("Правильный ответ не может быть меньше -1 (правильного ответа нет)"); return; }
                
                if ((int)value > PossibleAnswers.Count) { Loger.Log("Вариантов ответа меньше, чем указано"); return; }
                rightAnswer = value;
                if ((int)value == -1)
                {
                    Loger.Log($"Теперь у вопроса {QuestionString} нет правильного ответа");
                }
                else {
                    Loger.Log($"Теперь у вопроса {QuestionString} правильный ответ: {rightAnswer.ToString()}");
                }
            } 
        }
       public void SetRightAnswer(Answer a)
        {
            RightAnswer = a;
        }
        public void AddPossibleAnswer(string Question)
        {
            PossibleAnswers.Add(Question);
        }
        public List<string> GetPossibleAnswers()
        {
           return PossibleAnswers;
        }
        public void ShuffleAnswers()
        {
            Loger.Log("PossibleAnswers.Count"+PossibleAnswers.Count);
            Loger.Log(RightAnswer.ToString());
            for (int i = PossibleAnswers.Count-1; i > 0; i--)
            {
                int j =r.Next(i);
                Swap(i, j);
            }
        }

        private void Swap(int i, int j)
        {
            Loger.Log("Swap"+i+j);
            if (RightAnswer==(Answer) i)rightAnswer = (Answer) j;
            else if(RightAnswer == (Answer)j) rightAnswer = (Answer)i;
            Loger.Log(RightAnswer.ToString());
            string temp = PossibleAnswers[i];
            PossibleAnswers[i] = PossibleAnswers[j];
            PossibleAnswers[j] = temp;
        }
        // public int Selected = -1;
    }
    
}
