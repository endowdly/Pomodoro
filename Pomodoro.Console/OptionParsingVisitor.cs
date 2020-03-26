namespace Endowdly.Pomodoro.Console
{ 
    internal class OptionParsingVisitor : IParsingVisitor
    {
        const string Dash = "-";
        const string DoubleDash = "--";

        // No support for slash names yet, e.g., /target:library (csc.exe)
        // This would require a more complicated Tokenizer.cs

        public bool Parse(Token x, out TokenValue y)
        { 
            // Could use compiled regex here, but KISS for now 
            var s = x.Value.Replace(Dash, string.Empty);

            if (x.Value.StartsWith(Dash))
            { 
                switch (s)
                {
                    case "t":
                        y = new TaskName(x);
                        break;

                    case "d":
                        y = new TaskDuration(x);
                        break;

                    case "s":
                        y = new ShortBreak(x);
                        break;

                    case "l":
                        y = new LongBreak(x);
                        break;

                    case "p":
                        y = new SetLength(x); 
                        break;

                    case "h":
                        y = new HelpSwitch(x); 
                        break;

                    case "r":
                        y = new RecordSwitch(x); 
                        break;

                    case "!":
                        y = new AnnoySwitch(x); 
                        break;

                    default:
                        y = null;
                        return false; 
                }

                return true;
            }
            if (x.Value.StartsWith(DoubleDash))
            { 
                switch (s)
                {
                    case "taskname":
                        y = new TaskName(x);
                        break;

                    case "taskduration":
                        y = new TaskDuration(x);
                        break;

                    case "shortbreak":
                        y = new ShortBreak(x);
                        break;

                    case "longbreak":
                        y = new LongBreak(x);
                        break;

                    case "setlength":
                        y = new SetLength(x); 
                        break;

                    case "help":
                        y = new HelpSwitch(x); 
                        break;

                    case "record":
                        y = new RecordSwitch(x); 
                        break; 

                    default:
                        y = null;
                        return false; 
                }

                return true;
            } 
            else
            {
                y = null;
                return false;
            } 
        } 
    }
}
