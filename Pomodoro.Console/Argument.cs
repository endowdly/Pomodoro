namespace Endowdly.Pomodoro.Console
{
    internal class Argument : TokenValue
    {
        public readonly string Value; 
        public Argument(Token token, string value) : base(token)
        {
            Value = value;
        }
    }
}