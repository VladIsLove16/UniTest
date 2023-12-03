using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestAppOnWpf
{
    [Serializable]
    class Task
    {
        public string Question;
        public List<string> PossibleAnswers;
        public int RightAnswerNumber;
        public int Selected = -1;
    }
    
}
