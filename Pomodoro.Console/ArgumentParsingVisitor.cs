namespace Endowdly.Pomodoro.Console
{
    internal class ArgumentParsingVisitor : IParsingVisitor
    {
        public bool Parse(Token x, out TokenValue y)
        { 
            if (x.Value is string)
            {
                y = new Argument(x, x.Value);
                return true; 
            }

            y = null;
            return false;
        } 
    }
}
