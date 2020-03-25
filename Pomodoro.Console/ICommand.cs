namespace Endowdly.Pomodoro.Console
{
    interface ICommand
    {
        string Name { get; }
        string HelpText { get; }
        void Execute(string[] args);
    }
}
