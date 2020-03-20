namespace Endowdly.Pomodoro.Console
{
    using System;

    using Endowdly.Pomodoro.Core;

    class Program
    {
        const string Title = "Endowdly Pomodoro Timer";
        const string Version = "0.0.1";
        const string Space = " ";

        static void Main(string[] args)
        {
            ConsoleKeyInfo cki;

            var w = Console.WindowWidth;
            var h = Console.WindowHeight;
            var x = Console.ForegroundColor;
            var y = Console.BackgroundColor;
            var s = Title + " v" + Version;

            Console.WriteLine();
            Console.WriteLine(s);
            Console.WriteLine();
            Console.WriteLine();

            var pt = PomodoroTimer.Default.With(
                TaskDuration: TimeSpan.FromSeconds(5),
                ShortBreakDuration: TimeSpan.FromSeconds(5),
                LongBreakDuration: TimeSpan.FromSeconds(10));

            Console.BackgroundColor = ConsoleColor.Blue; Console.ForegroundColor = ConsoleColor.Black;
            Console.Write("Task".CenterString(15));
            Console.BackgroundColor = ConsoleColor.Black; Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine(pt.Task.Value.CenterString(35));
            Console.WriteLine();
            Console.BackgroundColor = y; Console.ForegroundColor = x;

            var bar = new ProgressBar(0, 50, pt.CurrentState.ToString());
            bar.SetPostFix(Space);

            pt.Metronome.Elapsed += (o, e) => bar.SetPercentage(100 * pt.SecondsElapsed / pt.CurrentDuration);
            pt.StateChange += (o, e) =>
            {
                // Hack: 
                // Sometimes the Metronome and the StateChange events are not synced.
                // This is due to rounding errors and one timer has wiggled off one way or the other.
                // This is a pretty dumb hack to get around that visually.  
                if (pt.IsActive && !bar.IsComplete) bar.SetPercentage(100);

                switch (pt.CurrentState)
                {
                    case State.Active:
                        bar.SetPostFix((Space + pt.PomodoroCounter.ToString() + " / " + pt.SetLength.ToString()).CenterString(15));
                        if (pt.PomodoroCounter == pt.SetLength) bar.Mark = true;
                        break;
                    default:
                        bar.SetPostFix(Space);
                        bar.Mark = false;
                        break;
                }

                bar.SetLabel(pt.CurrentState.ToString());
                bar.SetPercentage(0);
                bar.Draw(); 
            };

            
            bool execState = true;

            Console.CursorVisible = false;

            do
            {
                cki = Console.ReadKey(true);

                switch (cki.Key)
                {
                    case ConsoleKey.S:
                        if (cki.Modifiers.HasFlag(ConsoleModifiers.Control))
                        {
                            if (execState)
                            {
                                pt.Start();
                            }
                            else
                            {
                                pt.Stop();
                                bar = new ProgressBar(0, 50, pt.CurrentState.ToString());
                                Console.WriteLine(); 
                            }
                            execState = !execState;
                        }
                        break;
                    default:
                        break;
                }

            } while (cki.Key != ConsoleKey.Escape);


            Console.ReadKey(true);
        }

        private static void WriteLineCenter(string s)
        {
            Console.CursorLeft = (Console.WindowWidth - s.Length) / 2;
            Console.WriteLine(s);
        }
    }

    public static class StringExtensions
    {
        public static string CenterString(this string toCenter, int totalLength) =>
            toCenter
                .PadLeft(((totalLength - toCenter.Length) / 2) + toCenter.Length)
                .PadRight(totalLength);
    }

    public class ProgressBar
    {
        const string AltTwirl = @"└┌┐┘";
        const string Space = " ";
        const char Radical = (char)0x221a;

        double percentComplete;
        int width;
        string label;
        int progress;
        string postfix;
        bool mark;
        int x0;
        int y0;

        internal ProgressBar(double x, int n, string s)
        {
            percentComplete = x;
            width = n;
            label = s;
            progress = 0;
            postfix = string.Empty;
            x0 = Console.CursorLeft;
            y0 = Console.CursorTop;
        }

        public bool IsComplete
        {
            get { return percentComplete == 100; }
        }

        public bool Mark
        {
            get { return mark; }
            set
            {
                mark = value;
            }
        }

        public void Draw()
        {
            var s = label.CenterString(width);
            var x = Console.ForegroundColor;
            var y = Console.BackgroundColor;
            double widthDone = Math.Round(width * (percentComplete / 100));
            double widthRem = width - widthDone;
            var s0 = s.Substring(0, (int)widthDone);  // 0-length substrings are allowed
            var s1 = s.Substring(s.Length - (int)widthRem);

            Console.SetCursorPosition(0, y0);
            Console.Write(string.Empty.PadRight(Console.WindowWidth));  // flush the line
            Console.SetCursorPosition(0, y0);
            Console.ForegroundColor = ConsoleColor.Black;
            Console.BackgroundColor = ConsoleColor.Green;
            Console.Write(s0);
            Console.SetCursorPosition((int)widthDone, y0);
            Console.BackgroundColor = y;
            Console.ForegroundColor = x;
            Console.Write(s1);
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write(Space);
            Console.Write(AltTwirl[progress % AltTwirl.Length]); 
            Console.ForegroundColor = x;
            Console.CursorLeft--;

            if (percentComplete >= 100) Console.Write(postfix);
          
            Console.ForegroundColor = ConsoleColor.Green;

            if (percentComplete >= 100 && mark) Console.Write(Space + Radical);

            Console.ForegroundColor = x;
            Console.BackgroundColor = y;
        }

        public void SetLabel(string s) => label = s;
        public void SetPostFix(string s) => postfix = s;

        public void Inc()
        {
            percentComplete++;
            progress++;
            Draw();
        }

        public void Dec()
        {
            percentComplete--;
            progress++;
            Draw();
        }

        public void SetPercentage(double n)
        {
            percentComplete = n;
            progress++;
            Draw();
        }
    }
}