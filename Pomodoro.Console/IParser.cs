namespace Endowdly.Pomodoro.Console
{
    internal interface IParser
    {
        bool IsParsed { get; }
        string Value { get; }
    }
}