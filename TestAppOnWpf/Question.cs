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
        public int NumberInTest;
        public int Id { get; set; }
        public string QuestionString;
        public List<string> PossibleAnswers=new List<string>();
        public Answer RightAnswer;
       // public int Selected = -1;
    }
    
}
