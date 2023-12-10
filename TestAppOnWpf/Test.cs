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
        public string Title;
        public string FilePath;
        //public Answers Answers=new Answers();
        public string AnswerPath;
        public int Number;
        //private int questionCount;
        public int QuestionCount
        {
            get { return Questions.Count; }
        }
        public List<Question> Questions=new List<Question>();
       
       
    }
}
