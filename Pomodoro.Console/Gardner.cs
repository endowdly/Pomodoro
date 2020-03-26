using System;
using System.Collections.Generic;
using System.Linq;

namespace Endowdly.Pomodoro.Console
{
    internal static class Gardener
    { 
        public static Option Garden(TokenValue[] tokens)
        { 
            Stack<IOptionExpression> r = new Stack<IOptionExpression>();
            Stack<TokenValue> options = new Stack<TokenValue>();
            Stack<Argument> arguments = new Stack<Argument>();


            foreach (var token in tokens)
            {
                if ((token is AnnoySwitch) ||
                    (token is HelpSwitch) ||
                    (token is RecordSwitch))
                {
                    r.Push(new SwitchExpression(token));
                }

                if ((token is TaskName) ||
                    (token is TaskDuration) ||
                    (token is ShortBreak) || 
                    (token is LongBreak) || 
                    (token is SetLength))
                {
                    if (options.Count > 0)
                        throw new Exception();

                    options.Push(token); 
                } 

                if (token is Argument)
                {
                    if (options.Count < 1) 
                        throw new Exception();

                    if ((arguments.Count < 1) || (options.Peek() is TaskName))
                    {
                        arguments.Push((Argument)token); 
                    } 
                }

                if ((options.Count == 1) && (arguments.Count == 1))
                    r.Push(new OptionExpression(options.Pop(), arguments.Pop()));
                
                if ((options.Count == 1) && (arguments.Count > 1))
                { 
                    var ls = arguments.Reverse().ToList();

                    var s = ls.Select(x => x.Value).Join();

                    r.Push(new OptionExpression(options.Pop(), s));

                    arguments.Clear();
                }
            }
        }
    }
}
