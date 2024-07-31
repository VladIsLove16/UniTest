using System;
using System.Xml.Serialization;

namespace TestAppOnWpf
{
    [Serializable]
    public class TestResult
    {
        private TimeSpan time = TimeSpan.Zero;
        private string testTitle = "Default";
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
                timeString = value.ToString(@"hh\:mm\:ss");
            }
        }
        private string timeString;
        public string TimeString
        {
            get
            {
                return timeString;
            }
            set
            {
                timeString = value;
            }
        }
        private int _RightAnswers = 0;
        public int RightAnswers
        {
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
        private int _WrongAnswers = 0;
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
        private int _Skipped = 0;
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


        public TestResult()
        {
            TestTitle = "default";
            Time = TimeSpan.Zero;
            RightAnswers = 0;
            WrongAnswers = 0;
            Skipped = 0;
        }
        public TestResult(string testTitle, int rightAnswers, int wrongAnswers, int skippedAnswers, TimeSpan time)
        {
            this.TestTitle = testTitle;
            this.RightAnswers = rightAnswers;
            this.WrongAnswers = wrongAnswers;
            this.Skipped = skippedAnswers;
            this.Time = time;
        }

        internal void Clear()
        {
            TestTitle = "default";
            Time = TimeSpan.Zero;
            RightAnswers = 0;
            WrongAnswers = 0;
            Skipped = 0;
        }

        public override string ToString()
        {
            return timeString + "," + RightAnswers + "," + WrongAnswers + "," + Skipped + " ";
        }
        public string Print()
        {
            return timeString + "," + RightAnswers + "," + WrongAnswers + "," + Skipped + " ";
        }
        [XmlIgnore]
        public string ResultString
        {
            get { return timeString + "," + RightAnswers + "," + WrongAnswers + "," + Skipped + " "; }
        }
        //public static string CSVFormatString=
    }
}
