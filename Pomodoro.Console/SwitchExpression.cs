namespace Endowdly.Pomodoro.Console
{
    internal class SwitchExpression : IOptionExpression
    {
        public TokenValue Option { get; }

        public SwitchExpression(TokenValue option)
        {
            Option = option;
        }
    }
}
