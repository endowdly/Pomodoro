namespace Endowdly.Pomodoro.Console
{
    using System.Collections.Generic;

    internal interface IOptionExpression
    {
        TokenValue Option { get; }

        Option ToOption();
    }
}
