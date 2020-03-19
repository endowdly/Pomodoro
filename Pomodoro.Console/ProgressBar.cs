namespace Endowdly.Pomodoro.Console
{
    using System;

    // Todo: Functionalize
    public class ProgressBar
    {
        const char FullBlock = (char)0x2588;
        const char MediumShade = (char)0x2592; 

        public ConsoleColor BackgroundColor = ConsoleColor.Gray;
        public ConsoleColor BarColor = ConsoleColor.Black;

        Color c;

        double percentComplete;
        Pos pos;
        int w;
        int h;

        internal ProgressBar(double pc, Pos p, Pos size)
        {
            percentComplete = pc;
            pos = p;
            w = size.c;
            h = size.r;
        }

        public void Draw()
        {
            c.bg = BackgroundColor ; c.fg = BarColor; 
            double widthDone = Math.Round(w * (percentComplete / 100));
            double widthRem = w - widthDone;

            Pos remPos;
            remPos.c = pos.c + (int)widthDone; remPos.r = pos.r; 

            WriteText(string.Empty.PadRight((int)widthDone, FullBlock), pos, c);
            WriteText(string.Empty.PadRight((int)widthRem, MediumShade), remPos, c); 
        }

        public void Inc()
        {
            percentComplete++;
            Draw();
        }

        public void Dec()
        {
            percentComplete--;
            Draw();
        }

        public void SetPercentage(double n)
        {
            percentComplete = n;
            Draw();
        }

        // Todo: move this into a class
        private static void WriteText(string s, Pos p, Color c)
        {
            Console.CursorLeft = p.c;
            Console.CursorTop = p.r;
            Console.BackgroundColor = c.bg;
            Console.ForegroundColor = c.fg;
            Console.Write(s);
        }

        // Todo: move this into a class
        static void WriteCenterText(string s, int r, Color c)
        {
            Pos p;
            var w = Console.WindowWidth;
            var col = (w - s.Length) / 2;
            p.r = r; p.c = col;

            WriteText(s, p, c);
        }
    }
}
