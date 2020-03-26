// This is the parser that will turn our tokens into something useful.

namespace Endowdly.Pomodoro.Console
{
    using System;
    using System.Collections.Generic;

    internal class Parser
    {
        private IParsingVisitor[] visitors = new IParsingVisitor[]
        {
            new ArgumentParsingVisitor(),
            new OptionParsingVisitor()
        };

        public TokenValue[] Parse(Token[] tokens)
        {
            var tokenValueList = new List<TokenValue>();
            var invalidTokenList = new List<Token>();

            foreach (var token in tokens)
            {

                bool isParsed = false;
                TokenValue tokenValue;

                foreach (var visitor in visitors) 
                {
                    isParsed = visitor.Parse(token, out tokenValue); 

                    if (isParsed)
                    {
                        tokenValueList.Add(tokenValue);
                    }
                }

                if (!isParsed)
                    invalidTokenList.Add(token);
            } 

            // todo: logger? 

            if (invalidTokenList.Count > 0)
            {
                var fg = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.DarkYellow;

                foreach (var invalidToken in invalidTokenList)
                    Console.WriteLine("Ignoring unrecognized option: {0}", invalidToken);

                Console.ForegroundColor = fg;
            }

            return tokenValueList.ToArray(); 
        }
    }
}
