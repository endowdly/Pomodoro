namespace Endowdly.Pomodoro.Console
{
    internal class Argument : IParser
    {
        private const string Dash = "-";
        private const string DoubleDash = "--";

        private Argument(string s)
        {
            Value = IsValid(s)
                ? s.Trim()
                : string.Empty;
        }

        public string Value { get; }

        public bool IsParsed
        {
            get { return IsValid(Value); }
        }

        public static Argument Parse(string s)
        {
            return new Argument(s);
        }

        public override bool Equals(object o)
        {
            return (o is Argument)
                ? Equals((Argument)o)
                : false;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        private static bool IsValid(string s)
        {
            return !(s.StartsWith(Dash) || s.StartsWith(DoubleDash) || string.IsNullOrEmpty(s));
        }

        private bool Equals(Switch other)
        {
            return Value.Equals(other.Value);
        }
    }
}