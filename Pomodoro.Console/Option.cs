namespace Endowdly.Pomodoro.Console
{
    using System;
    using System.Collections.Generic;
    using Endowdly.Pomodoro.Core;

    internal class Option
    {
        private TimeSpan longBreakTs;
        private TimeSpan shortBreakTs;
        private TimeSpan taskTs;

        private Option(
            string TaskName = null,
            string TaskDuration = null,
            string ShortBreakDuration = null,
            string LongBreakDuration = null,
            int SetLength = 0,
            bool DisplayHelp = false,
            bool DisplayVersion = false,
            bool CollectStats = false,
            bool BeAnnoying = false,
            string LogFilePath = null)
        {
            this.TaskName = TaskName;
            this.TaskDuration = TaskDuration;
            this.ShortBreakDuration = ShortBreakDuration;
            this.LongBreakDuration = LongBreakDuration;
            this.SetLength = SetLength;
            this.DisplayHelp = DisplayHelp;
            this.DisplayVersion = DisplayVersion;
            this.CollectStats = CollectStats;
            this.BeAnnoying = BeAnnoying;
            this.LogFilePath = LogFilePath;

            SetTimeSpans();
        }

        public bool BeAnnoying { get; set; }
        public bool CollectStats { get; set; }
        public bool DisplayHelp { get; set; }
        public bool DisplayVersion { get; set; }
        public string LogFilePath { get; set; }
        public string LongBreakDuration { get; set; }
        public TimeSpan LongBreakTs => longBreakTs;
        public int SetLength { get; set; }
        public string ShortBreakDuration { get; set; }
        public TimeSpan ShortBreakTs => shortBreakTs;
        public string TaskDuration { get; set; }
        public string TaskName { get; set; }
        public TimeSpan TaskTs => taskTs;
        internal static Option Empty => new Option();

        internal static Option Parse(string[] input)
        {
            // Do this iteratively -- it will just be easier
            // Set up some token stacks for our input
            Stack<Switch> switchStack = new Stack<Switch>();
            Queue<Argument> argumentQ = new Queue<Argument>();
            Stack<OptionExpression> options = new Stack<OptionExpression>();

            // First, we need to ensure the first argument is a switch of some kind
            // Side note: I really dislike the OG `TryParse` pattern you see in .NET 2.0
            // I much prefer the psuedo-functional `None/Some` pattern of validated types


            for (int i = 0; i < input.Length; i++)
            {
                var sw = Switch.Parse(input[i]); 
                var argument = Argument.Parse(input[i]);

                // Always check to make sure a switch is on the stack first.
                if (switchStack.Count < 1)
                {

                    if (sw.IsParsed)
                    {
                        switchStack.Push(sw);
                    }
                    else
                    {
                        Console.WriteLine("Unrecognized switch: {0}", input[i]);
                        continue; 
                    } 
                } 

                // Now, start making the command and see if it is complete 
                if (OptionExpression.Switch(switchStack.Peek()).IsComplete)
                {
                    options.Push(OptionExpression.Switch(switchStack.Pop()));
                } 
                
              

                // It did not, so get some args 
                if (argument.IsParsed) 
                {
                    argumentQ.Enqueue(argument); 
                    continue;
                }

                // If there is a switch on the stack and arguments in the q
                // that means we have a completed command that cannot be parsed internally;
                // Force it
                if (switchStack.Count > 0 && argumentQ.Count > 0 && sw.IsParsed)
                {
                    // Maybe the command needed an argument waiting in the q
                    if (argumentQ.Count > 0 && OptionExpression.Switch(switchStack.Peek()).Argument(argumentQ.Peek()).IsComplete)
                    {
                        options.Push(OptionExpression
                            .Switch(switchStack.Pop())
                            .Argument(argumentQ.Dequeue())
                            );
                    }
                    else
                    {
                        // We may have to force it
                        var option = OptionExpression
                            .Switch(switchStack.Pop())
                            .Argument(argumentQ.ToArray());

                        option.Complete();
                        options.Push(option);
                        argumentQ.Clear();
                    }

                    switchStack.Push(sw);
                } 
            
                // All tokens should be parsed in their proper place
                // The OptionExpression stack should have executable expressions now 
            }

            // May end up with no completed commands, so complete what is in the stacks
            if (switchStack.Count == 1 && argumentQ.Count > 0)
            {
                var option = OptionExpression
                    .Switch(switchStack.Pop())
                    .Argument(argumentQ.ToArray());

                option.Complete();
                options.Push(option);
            }

            var result = Option.Empty;
            foreach (var option in options)
            {
                option.ToOption(ref result);
            }

            return result;
        }

        public Option With(
            string TaskName = null,
            string TaskDuration = null,
            string ShortBreakDuration = null,
            string LongBreakDuration = null,
            int? SetLength = null,
            bool? DisplayHelp = null,
            bool? DisplayVersion = null,
            bool? CollectStats = null,
            bool? BeAnnoying = null,
            string LogFilePath = null
        ) => new Option(
            TaskName: TaskName ?? this.TaskName,
            TaskDuration: TaskDuration ?? this.TaskDuration,
            ShortBreakDuration: ShortBreakDuration ?? this.ShortBreakDuration,
            LongBreakDuration: LongBreakDuration ?? this.LongBreakDuration,
            SetLength: SetLength ?? this.SetLength,
            DisplayHelp: DisplayHelp ?? this.DisplayHelp,
            DisplayVersion: DisplayVersion ?? this.DisplayVersion,
            CollectStats: CollectStats ?? this.CollectStats,
            BeAnnoying: BeAnnoying ?? this.BeAnnoying,
            LogFilePath: LogFilePath ?? this.LogFilePath
            );

        private void SetTimeSpans()
        {
            var defaultTimer = PomodoroTimer.Default;
            if (!TimeSpan.TryParse(TaskDuration, out taskTs))
                taskTs = defaultTimer.TaskDuration;

            if (!TimeSpan.TryParse(ShortBreakDuration, out shortBreakTs))
                shortBreakTs = defaultTimer.ShortBreakDuration;

            if (!TimeSpan.TryParse(LongBreakDuration, out longBreakTs))
                longBreakTs = defaultTimer.LongBreakDuration;
        }
    }
}