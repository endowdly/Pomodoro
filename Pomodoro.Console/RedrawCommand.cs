namespace Endowdly.Pomodoro.Console
{
    class RedrawCommand : ICommand
    {
        const string CommandName = "redraw";
        const string CommandHelpText = @"Redraws the output of the command line progarm. Mapped to Ctrl+L.";

        public string Name => CommandName;

        public string HelpText => CommandHelpText;

        public void Execute(string[] args) => throw new System.NotImplementedException();
    }
}
