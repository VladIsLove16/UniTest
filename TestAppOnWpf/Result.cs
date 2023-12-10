using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestAppOnWpf
{
    [Serializable]
    public class Result
    {
        public TimeSpan Time= TimeSpan.Zero;
        private int _RightAnswers=0;
        public int RightAnswers {
            get { return _RightAnswers; } 
            set
            {
                if (value < 0)
                {
                    throw new Exception($"Колво _RightAnswers было {value}");
                }
                _RightAnswers = Math.Max(value, 0);
            }   
        }
        private int _WrongAnswers =0;
        public int WrongAnswers
        {
            get { return _WrongAnswers; }
            set
            {
                if (value < 0)
                {
                    throw new Exception($"Колво _RightAnswers было {value}");
                }
                _WrongAnswers = Math.Max(value, 0);
            }
        }
        private int _Skipped =0;
        public int Skipped
        {
            get { return _Skipped; }
            set
            {
                if (value < 0)
                {
                    throw new Exception($"Колво _RightAnswers было {value}");
                }
                _Skipped = Math.Max(value, 0);
            }
        }
        public Result() { }
    }
}
