using System;

namespace Endowdly.Pomodoro.Core
{
    public class Task
    {
        private const string Smiley = ":)";
        public static Task Default = new Task();
        public static Task Empty = new Task(string.Empty);

        private Task(string s)
        {
            Value = IsValid(s)
                ? s.Trim()
                : string.Empty;

            Value = s.Trim();
        }

        private Task() : this(Smiley)
        {
        }

        public string Value { get; }

        public static Task New(string s)
        {
            return new Task(s);
        }

        public static Task New()
        {
            return Empty;
        }

        private static bool IsValid(string s)
        {
            var lineBreakChars = new char[]
            {
                (char)0x0a,   // lf
                (char)0x0b,   // ff
                (char)0x0c,   // f
                (char)0x0d,   // cr
                (char)0x85,   // cr
                (char)0x1e,   // rs
                (char)0x2028, // nel (next line)
                (char)0x2029, // ps (paragraph seperator)
            };

            return s.IndexOfAny(lineBreakChars).Equals(-1);
        }

        private bool Equals(Task other)
        {
            return Value.Equals(other.Value); 
        }

        public override bool Equals(object obj)
        {
            return (obj is Task)
                ? Equals((Task)obj)
                : false; 
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }
    }
}