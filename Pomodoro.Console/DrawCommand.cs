namespace Endowdly.Pomodoro.Console
{
    class DrawCommand : ICommand
    {
        const string CommandName = "draw";
        const string CommandHelpText = @"Draws the initial output of the command line progarm. Unmapped.";

        public string Name => CommandName;

        public string HelpText => CommandHelpText;

        public void Execute(string[] args) => throw new System.NotImplementedException();
    }
}
