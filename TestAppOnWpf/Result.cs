using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TestAppOnWpf
{
    [Serializable]
    public class Result
    {
        private TimeSpan time= TimeSpan.Zero;
        private string testTitle="Default";

        [XmlIgnore]
        public string TestTitle
        {
            get
            {
                return testTitle;
            }
            set
            {
                testTitle = value;
            }
        }
        [XmlIgnore]
        public TimeSpan Time
        {
            get
            {
                return time;
            }
            set
            {
                time = value;
                timeString=value.ToString(@"hh\:mm\:ss\.ff");
            }
        }
        private string timeString;
        public string TimeString { 
            get
            {
                return timeString;
            }
            set
            {
                timeString=value;
            }
        }

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
        public Result() {
            TestTitle = "default";
            Time = TimeSpan.Zero;
            RightAnswers = 0;
            WrongAnswers = 0;
            Skipped = 0;
        }
        public Result(string testTitle,int rightAnswers, int wrongAnswers, int skippedAnswers, TimeSpan time)   
        {
            this.TestTitle = testTitle;
            this.RightAnswers = rightAnswers;
            this.WrongAnswers = wrongAnswers;  
            this.Skipped = skippedAnswers;
            this.Time=time;
        }

        internal void Clear()
        {
            TestTitle = "default";
            Time = TimeSpan.Zero;
            RightAnswers=0;
            WrongAnswers=0;
            Skipped=0;
        }

        internal string Print()
        {
            return RightAnswers + "/" + WrongAnswers + "/" + Skipped + " за " + timeString;
        }
    }
}
