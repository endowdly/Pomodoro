namespace Endowdly.Pomodoro.Console
{
    interface IParsingVisitor
    {
        bool Parse(Token token, out TokenValue value);
    }
}
