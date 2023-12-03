using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestAppOnWpf
{
    [Serializable]
    class Result
    {
        public TimeSpan Time= TimeSpan.Zero;
        private int _RightAnswers=0;
        public int RightAnswers {
            get { return _RightAnswers; } 
            set
            {
                if (value < 0)
                {
                    Console.WriteLine($"Колво _RightAnswers было {value}");
                    _RightAnswers = Math.Max(value, 0);
                }
                else _RightAnswers = value;
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
                    Console.WriteLine($"Колво _WrongAnswers было {value}");
                    _WrongAnswers = Math.Max(value, 0);
                }
                else _WrongAnswers = value;
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
                    Console.WriteLine($"Колво _Skipped было {value}");
                    _Skipped = Math.Max(value, 0);
                }
                else _Skipped = value;
            }
        }
    }
}
