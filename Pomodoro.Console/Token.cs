// This is part of a visitor pattern parser.

namespace Endowdly.Pomodoro.Console
{
    internal class Token
    {
        public readonly string Value;
        public readonly int Position;

        public Token(string value, int pos)
        {
            Value = value;
            Position = pos;
        }

        private bool Equals(Token other)
        {
            return Value.equals(other.Value);
        }

        public override bool Equals(object o)
        {
            return (o is Token)
                ? Equals((Token)o)
                : false;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode() + Position.GetHashCode();
        }
    }
}
