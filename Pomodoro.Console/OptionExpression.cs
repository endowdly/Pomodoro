namespace Endowdly.Pomodoro.Console
{
    using System.Collections.Generic; 

    internal class OptionExpression : IOptionExpression
    {
        public TokenValue Option { get; }
        public readonly TokenValue Argument;

        public OptionExpression(TokenValue option, TokenValue argument)
        {
            Option = option; 
            Argument = argument;
        }

        public Option ToOption(out Option option)
        {
        }
    } 
}
