using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace clock
{
    class MyTime
    {
        private int hour;
        private int minute;
        private int second;

        public MyTime(int hour = 0, int minute = 0, int second = 0)
        {
            Hour = hour;
            Minute = minute;
            Second = second;
        }

        public int Hour
        {
            get { return hour; }
            set
            {
                if (value >= 0 && value < 24)
                {
                    hour = value;
                }
                else
                {
                    throw new ArgumentOutOfRangeException("hour invalid!");
                }
            }
        }

        public int Minute
        {
            get { return minute; }
            set
            {
                if (value >= 0 && value < 60)
                {
                    minute = value;
                }
                else
                {
                    throw new ArgumentOutOfRangeException("minute invalid!");
                }
            }
        }

        public int Second
        {
            get { return second; }
            set
            {
                if (value >= 0 && value < 60)
                {
                    second = value;
                }
                else
                {
                    throw new ArgumentOutOfRangeException("second invalid!");
                }
            }
        }

        public override bool Equals(object obj)
        {
            var time = obj as MyTime;
            return time != null &&
                   Hour == time.Hour &&
                   Minute == time.Minute &&
                   Second == time.Second;
        }

        public override int GetHashCode()
        {
            var hashCode = 1505761165;
            hashCode = hashCode * -1521134295 + Hour.GetHashCode();
            hashCode = hashCode * -1521134295 + Minute.GetHashCode();
            hashCode = hashCode * -1521134295 + Second.GetHashCode();
            return hashCode;
        }
    }

    class MyClock
    {
        public delegate void AlarmEventHandler(MyClock sender);

        public delegate void TickEventHandler(MyClock sender);

        public event AlarmEventHandler AlarmEvent;

        public event TickEventHandler TickEvent;

        public MyClock()
        {
            CurrentTime = new MyTime();
        }

        public MyTime CurrentTime { get; set; }

        public MyTime AlarmTime { get; set; }

        public void Run()
        {
            while (true)
            {
                DateTime now = DateTime.Now;
                CurrentTime = new MyTime(now.Hour, now.Minute, now.Second);
                TickEvent(this);
                if (AlarmTime.Equals(CurrentTime))
                    AlarmEvent(this);
                Thread.Sleep(1000);
            }
        }
    }

    class MainClass
    {
        static void Main(string[] args)
        {
            try
            {
                MyClock clock = new MyClock(); //时钟

                clock.AlarmTime = new MyTime(DateTime.Now.Hour,
                              DateTime.Now.Minute,
                              DateTime.Now.Second + 5);
                clock.TickEvent += ShowTime;
                clock.AlarmEvent += Alarming;
                clock.Run();
                //ClockInNewthread(clock);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private static void ShowTime(MyClock sender)
        {
            MyTime time = sender.CurrentTime;
            Console.WriteLine($"Tick Event: " +
              $"{time.Hour}:{time.Minute}:{time.Second}");
        }

        public static void Alarming(MyClock sender)
        {
            MyTime time = sender.CurrentTime;
            Console.WriteLine($"Alarm Event: {time.Hour}:{time.Minute}:{time.Second}");
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine("dingling dingling dingling");
                Thread.Sleep(1000);
            }
        }

        private static void ClockInNewthread(MyClock clock)
        {
            Thread thread = new Thread(clock.Run);
            thread.Start();
            while (Console.ReadKey() == null)
            {
                Thread.Sleep(500);
            }
            thread.Abort();
        }
    }
}