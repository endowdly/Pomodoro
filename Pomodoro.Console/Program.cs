namespace Endowdly.Pomodoro.Console
{
    using System;

    using Endowdly.Pomodoro.Core;

    internal class Program
    {
        private const string Title = "Endowdly Pomodoro Timer";
        private const string Version = "0.0.3";
        private const string Space = " ";
        private static readonly string TaskLabel = "Task".CenterString(15);

        private static void Main(string[] args)
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

            var taskRow = Console.CursorTop;
            var barRow = Console.CursorTop + 2;
            var conRow = Console.CursorTop + 4;

            // Hack: the buffer may not be big enough for some of the cursor commands
            Console.BufferHeight += conRow;
            Console.SetCursorPosition(0, taskRow);

            Option opts = Option.Parse(args);

            var pt = PomodoroTimer.Default.With(
                Task: Task.New(opts.TaskName),
                TaskDuration: opts.TaskTs,
                ShortBreakDuration: opts.ShortBreakTs,
                LongBreakDuration: opts.LongBreakTs
                );

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
            Console.Write("Ctrl+S to Start/Stop".CenterString(50));

            bar.SetPostFix(Space);

            pt.Metronome.Elapsed += (o, e) => bar.SetPercentage(100 * pt.SecondsElapsed / pt.CurrentDuration);
            pt.StateChange += (o, e) =>
            {
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

                // Hack:
                // Sometimes the Metronome and the StateChange events are not synced.
                // This is due to rounding errors and one timer has wiggled off one way or the other.
                // This is a pretty dumb hack to get around that visually.
                ConsoleUtility.ClearLine(barRow);

                bar.SetLabel(pt.CurrentState.ToString());
                bar.SetPercentage(0);
                bar.Draw();
            };

            ConsoleKeyInfo conk;
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
                                Console.WriteLine();
                                bar = new ProgressBar(0, 50, pt.CurrentState.ToString());
                            }
                            execState = !execState;
                        }
                        else
                        { 
                            var s1 = execState
                                ? "Start? [ y / n ]".CenterString(50)
                                : "Stop? [ y / n ]".CenterString(50);
                            Console.SetCursorPosition(0, conRow);
                            Console.Write(s1);

                            do
                            {
                                conk = Console.ReadKey(true);
                            } while (conk.Key != ConsoleKey.N && conk.Key != ConsoleKey.Y);


                            if (conk.Key.HasFlag(ConsoleKey.Y))
                            {
                                if (execState)
                                {
                                    pt.Start();
                                }
                                else
                                {
                                    pt.Stop();
                                }
                                execState = !execState;
                            }
                            ConsoleUtility.ClearLine(conRow);
                        }
                        break;

                    case ConsoleKey.X: 
                        var s2 = "Exit? [ y / n ]".CenterString(50);
                        Console.SetCursorPosition(0, conRow);
                        Console.Write(s2);

                        do
                        {
                            conk = Console.ReadKey(true);
                        } while (conk.Key != ConsoleKey.N && conk.Key != ConsoleKey.Y);


                        if (conk.Key.HasFlag(ConsoleKey.Y))
                        {
                            ConsoleUtility.ClearLine(conRow);
                            return;
                        }

                        ConsoleUtility.ClearLine(conRow);
                        break; 

                    case ConsoleKey.C:
                        if (cki.Modifiers.HasFlag(ConsoleModifiers.Control))
                        {
                            ConsoleUtility.ClearLine(conRow);
                            return;
                        }
                        break;

                    case ConsoleKey.L:
                        if (cki.Modifiers.HasFlag(ConsoleModifiers.Control))
                        {
                            Console.Clear();
                            Console.ForegroundColor = x;
                            Console.BackgroundColor = y;
                            // Reset the row trackers
                            taskRow = Console.CursorTop;
                            barRow = Console.CursorTop + 2;
                            conRow = Console.CursorTop + 4;

                            ConsoleUtility.WriteColor(
                                Text: TaskLabel,
                                ForegroundColor: ConsoleColor.Black,
                                BackgroundColor: ConsoleColor.Blue);
                            ConsoleUtility.WriteColorLine(
                                Text: pt.Task.Value.CenterString(35),
                                ForegroundColor: ConsoleColor.Blue,
                                BackgroundColor: ConsoleColor.Black);
                            Console.WriteLine();

                            bar = new ProgressBar(bar.Progress, 50, pt.CurrentState.ToString()); 
                        }
                        break;

                    default:
                        break;
                }
            } while (cki.Key != ConsoleKey.Escape);
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
        private const string AltTwirl = @"└┌┐┘";
        private const string Space = " ";
        private const char Radical = (char)0x221a;

        private double percentComplete;
        private readonly int width;
        private string label;
        private string postfix;
        private readonly int y0;

        public int Progress { get; private set; }

        internal ProgressBar(double x, int n, string s)
        {
            percentComplete = x;
            width = n;
            label = s;
            Progress = 0;
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
            // Hack: Sometimes widthDone is overflow if the timers are out of sync!? 
            int widthDone = (width * percentComplete / 100) > width
                ? width
                : (int)(width * percentComplete / 100);
            // Hack: Sometimes widthRem is negative if the timers are out of sync!? 
            int widthRem = (width - widthDone) < 0
                ? 0
                : width - widthDone; 
            var s0 = s.Substring(0, widthDone);  // 0-length substrings are allowed
            var s1 = s.Substring(s.Length - widthRem);

            ConsoleUtility.ClearLine(y0);
            ConsoleUtility.WriteColor(
                Text: s0,
                ForegroundColor: ConsoleColor.Black,
                BackgroundColor: ConsoleColor.Green);
            Console.SetCursorPosition(widthDone, y0);
            Console.Write(s1);
            Console.Write(Space);
            ConsoleUtility.WriteColor(
                Char: AltTwirl[Progress % AltTwirl.Length],
                ForegroundColor: ConsoleColor.Magenta);
            Console.Write(postfix);

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
            Progress++;
            Draw();
        }

        public void Dec()
        {
            percentComplete--;
            Progress++;
            Draw();
        }

        public void SetPercentage(double n)
        {
            percentComplete = n;
            Progress++;
            Draw();
        }
    }
}