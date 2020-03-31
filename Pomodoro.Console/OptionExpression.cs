
namespace Endowdly.Pomodoro.Console
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    internal class OptionExpression
    {
        const string Space = " ";
        Switch switchOption;
        Argument[] arguments;
        bool isComplete = false;
        static readonly Dictionary<SwitchType, Func<Option, string, Option>> funMap =
            new Dictionary<SwitchType, Func<Option, string, Option>>()
            {
                { SwitchType.TaskName, (x, y) => x.With(TaskName: y) },
                { SwitchType.TaskDuration, (x, y) => x.With(TaskDuration: y) },
                { SwitchType.ShortBreak, (x, y) => x.With(ShortBreakDuration: y) },
                { SwitchType.LongBreak, (x, y) => x.With(LongBreakDuration: y) },
                { SwitchType.SetLength, (x, y) => x.With(SetLength: int.Parse(y)) },
                { SwitchType.Help, (x, y) => x.With(DisplayHelp: true) },
                { SwitchType.RecordStats, (x, y) => x.With(CollectStats: true) },
                { SwitchType.BeAnnoying, (x, y) => x.With(BeAnnoying: true) },
            };

        private OptionExpression(Switch x, Argument[] ys)
        {
            switchOption = x;
            arguments = ys;
        }

        public bool IsComplete
        {
            get
            {
                if (SwitchType.BinarySwitch.HasFlag(switchOption.Type))
                    return true;

                if (SwitchType.SingleArgumentSwitch.HasFlag(switchOption.Type) && arguments.Length == 1)
                    return true;

                return isComplete;
            }

            private set
            {
                isComplete = value;
            }
        }


        public void Complete()
        {
            isComplete = true;
        }

        public static OptionExpression Switch(Switch x)
        {
            return new OptionExpression(x, new Argument[0]);
        }

        public void ToOption(ref Option opt)
        {
            var argument = string.Join(Space, arguments.Select(x => x.Value));
            var fun = funMap[switchOption.Type]; 
            opt = fun(opt, argument);
        }

        public OptionExpression Argument(Argument y)
        {
            if (!IsComplete)
            {
                var a = new Argument[arguments.Length + 1];
                a[arguments.Length] = y;

                return new OptionExpression(switchOption, a);
            }

            Console.WriteLine("Ignoring bad argument: {0}", y);
            return this;
        }

        public OptionExpression Argument(Argument[] ls)
        {
            if (!IsComplete)
            {
                return new OptionExpression(switchOption, ls);
            }

            Console.WriteLine("Ignoring bad {0} arguments", ls.Length);
            return this;
        }

    }
}
