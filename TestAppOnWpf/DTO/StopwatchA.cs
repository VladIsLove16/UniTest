using System;
using System.Diagnostics;
using System.Windows.Threading;

namespace TestAppOnWpf
{
    internal class StopwatchA
    {
        public DispatcherTimer timer;//надо сделать private
        Stopwatch stopwatch;
        TimeSpan elapsedTime;
        public string GetElapsedTimeStr()
        {
            return elapsedTime.ToString(@"hh\:mm\:ss\.ff");
        }
        public TimeSpan GetElapsedTime()
        {
            return elapsedTime;
        }
        public TimeSpan ElapsedTime => elapsedTime;
        public StopwatchA()
        {
            timer = new DispatcherTimer();
            timer.Tick += Timer_Tick;
            timer.Interval = new TimeSpan(0, 0, 0, 0, 10); // Интервал - 10 миллисекунд

            stopwatch = new Stopwatch();
            elapsedTime = new TimeSpan();
        }
        public void AddSubscriber()
        {

        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            if (stopwatch.IsRunning)
            {
                elapsedTime = stopwatch.Elapsed;
            }
        }

        internal void Start()
        {
            if (!stopwatch.IsRunning)
            {
                stopwatch.Start();
                timer.Start();
            }
        }
        public void Stop()
        {
            if (stopwatch.IsRunning)
            {
                stopwatch.Stop();
                timer.Stop();
            }
        }
        public void Reset()
        {
            stopwatch.Reset();
            elapsedTime = new TimeSpan();
        }

    }
}