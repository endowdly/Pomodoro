// This is part of a visitor pattern parser.
// The Tokenizer, aka parser, will turn the main `args` into tokens
// Each token will either be an option or an argument
// One argument, the Task Name, can be a string by itself
// So, this tokenizer should just trim up errant whitespace

namespace Endowdly.Pomodoro.Console
{
    using System.Linq;

    internal class Tokenizer
    {
        public Token[] Tokenize(string[] a)
        {
            int n = 0;   // I guess I could start at -1 ... nah

            return a
                .Select(x => new Token(x.Trim(), ++n - 1))
                .ToArray();
        } 
    }
}
