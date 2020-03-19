namespace Endowdly.Pomodoro.Console
{
    using System;
    using System.Threading;

    using Endowdly.Pomodoro.Core;

    class Program
    {
        const string Title = "PomTimer";
        const string Version = "1.0.0";

        static void Main(string[] args)
        {
            Console.CursorVisible = false;

            Color c;
            c.fg = ConsoleColor.Black; c.bg = ConsoleColor.Blue;

            Console.WindowHeight = 20;
            Console.WindowWidth = 80; 
            Console.Clear();

            Pos from;
            Pos to;

            // Let's get the window size
            var h = Console.WindowHeight;
            var w = Console.WindowWidth;

            from.r = 0; from.c = 0;
            to.r = w; to.c = h;

            var s = Title + " v" + Version;

            // Find center

            DrawColorBlock(c.bg, from, to);
            WriteCenterText(s, h / 2, c);

            Thread.Sleep(500);
            Console.Clear();

            Pos barPos; Pos barSize;
            barPos.r = h / 2; barPos.c = 10;
            barSize.r = 1; barSize.c = 60;

            var pt = PomodoroTimer.New().With(TaskDuration: TimeSpan.FromSeconds(60));
            var bar = new ProgressBar(0, barPos, barSize);
            bar.BarColor = ConsoleColor.Green;

            Color ptColor;
            ptColor.bg = ConsoleColor.Black; ptColor.fg = ConsoleColor.Green; 

            DrawColorBlock(ptColor.bg, from, to);
            WriteCenterText(pt.Task.Value, 2, ptColor);
            var cText = string.Format("[ {0} / {1} ]", pt.PomodoroCounter, pt.SetLength);
            WriteCenterText(cText, (h / 2) - 1, ptColor);
            bar.Draw(); 
            int nSeconds = 0;
            var second = new System.Timers.Timer(1000);
            second.Elapsed += (o, e) => nSeconds++;
            second.Start();
            pt.Start();

            Pos pPer;
            pPer.c = 72; pPer.r = h / 2;

            while (pt.IsActive && !pt.IsBreak && nSeconds <= pt.TaskDuration.TotalSeconds)
            {
                WriteCenterText(nSeconds.ToString(), (h / 2) + 2, ptColor);

                var totalSeconds = pt.TaskDuration.TotalSeconds;
                var percentage = (nSeconds / totalSeconds) * 100;
                WriteText((percentage/100).ToString("p"), pPer, ptColor);
                bar.BarColor = ConsoleColor.Green;
                bar.BackgroundColor = ConsoleColor.Black;
                bar.SetPercentage(percentage);
            }

            Console.ReadLine(); 
        }


        private static void WriteText(string s, Pos p, Color c)
        {
            Console.CursorLeft = p.c;
            Console.CursorTop = p.r;
            Console.BackgroundColor = c.bg;
            Console.ForegroundColor = c.fg;
            Console.Write(s); 
        }

        static void WriteCenterText(string s, int r, Color c)
        {
            Pos p;
            var w = Console.WindowWidth;
            var col = (w - s.Length) / 2; 
            p.r = r; p.c = col;

            WriteText(s, p, c); 
        }

        static void DrawColorBlock(ConsoleColor c, Pos start, Pos end)
        {
            Console.BackgroundColor = c;

            for (int y = start.c; y < end.c - 1; y++) 
            {
                Console.CursorLeft = start.r;
                Console.CursorTop = y;
                Console.WriteLine(string.Empty.PadLeft(end.r - start.r)); 
            }

            Console.CursorTop = end.c - 1;
            Console.Write(string.Empty.PadLeft(end.r - start.r));
        }
    }

    internal struct Color
    {
        internal ConsoleColor fg;
        internal ConsoleColor bg;
    }

    internal struct Pos
    {
        internal int r; 
        internal int c;
    }
}
