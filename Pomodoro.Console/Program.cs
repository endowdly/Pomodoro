namespace Endowdly.Pomodoro.Console
{
    using System;

    using Endowdly.Pomodoro.Core;

    class Program
    {
        const string Title = "Endowdly Pomodoro Timer";
        const string Version = "0.0.1";
        const string Space = " ";
        static string TaskLabel = "Task".CenterString(15);

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

            ConsoleUtility.WriteColor(
                Text: TaskLabel,
                ForegroundColor: ConsoleColor.Black,
                BackgroundColor: ConsoleColor.Blue);
            ConsoleUtility.WriteColorLine(
                Text: pt.Task.Value.CenterString(35),
                ForegroundColor: ConsoleColor.Blue,
                BackgroundColor: ConsoleColor.Black);
            Console.WriteLine();

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

    internal static class ConsoleUtility
    {
        internal static void WriteColor(string Text, ConsoleColor? ForegroundColor = null, ConsoleColor? BackgroundColor = null)
        {
            var r0 = Console.ForegroundColor;
            var r1 = Console.BackgroundColor;

            Console.ForegroundColor = ForegroundColor ?? r0;
            Console.BackgroundColor = BackgroundColor ?? r1; 
            Console.Write(Text); 
            Console.ForegroundColor = r0; Console.BackgroundColor = r1;
        }

        internal static void WriteColor(char Char, ConsoleColor? ForegroundColor = null, ConsoleColor? BackgroundColor = null)
        {
            var r0 = Console.ForegroundColor;
            var r1 = Console.BackgroundColor;

            Console.ForegroundColor = ForegroundColor ?? r0;
            Console.BackgroundColor = BackgroundColor ?? r1; 
            Console.Write(Char); 
            Console.ForegroundColor = r0; Console.BackgroundColor = r1; 
        }

        internal static void WriteColorLine(string Text, ConsoleColor? ForegroundColor = null, ConsoleColor? BackgroundColor = null)
        {
            var r0 = Console.ForegroundColor;
            var r1 = Console.BackgroundColor;

            Console.ForegroundColor = ForegroundColor ?? r0;
            Console.BackgroundColor = BackgroundColor ?? r1; 
            Console.WriteLine(Text); 
            Console.ForegroundColor = r0; Console.BackgroundColor = r1;
        }

        internal static void ClearLine(int row)
        {
            Console.SetCursorPosition(0, row);
            Console.Write(string.Empty.PadRight(Console.WindowWidth));
            Console.SetCursorPosition(0, row);
        }
    }

    public class ProgressBar
    {
        const string AltTwirl = @"└┌┐┘";
        const string Space = " ";
        const char Radical = (char)0x221a;

        double percentComplete;
        readonly int width;
        string label;
        int progress;
        string postfix;
        readonly int y0;

        internal ProgressBar(double x, int n, string s)
        {
            percentComplete = x;
            width = n;
            label = s;
            progress = 0;
            postfix = string.Empty;
            y0 = Console.CursorTop;
        }

        public bool IsComplete
        {
            get { return percentComplete == 100; }
        }

        public bool Mark { get; set; }

        public void Draw()
        {
            var s = label.CenterString(width);
            double widthDone = Math.Round(width * (percentComplete / 100));
            double widthRem = width - widthDone;
            var s0 = s.Substring(0, (int)widthDone);  // 0-length substrings are allowed
            var s1 = s.Substring(s.Length - (int)widthRem);

            ConsoleUtility.ClearLine(y0);
            ConsoleUtility.WriteColor(
                Text: s0,
                ForegroundColor: ConsoleColor.Black,
                BackgroundColor: ConsoleColor.Green);
            Console.SetCursorPosition((int)widthDone, y0);
            Console.Write(s1); 
            Console.Write(Space);
            ConsoleUtility.WriteColor(
                Char: AltTwirl[progress % AltTwirl.Length],
                ForegroundColor: ConsoleColor.Magenta);
            Console.SetCursorPosition(Console.CursorLeft - 1, y0);  // Console.CursorLeft-- sometimes causes an overflow

            if (percentComplete >= 100) Console.Write(postfix);
          
            Console.ForegroundColor = ConsoleColor.Green;

            if (percentComplete >= 100 && Mark)
                ConsoleUtility.WriteColor(
                    Text: Space + Radical,
                    ForegroundColor: ConsoleColor.Green); 
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