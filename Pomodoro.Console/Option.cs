namespace Endowdly.Pomodoro.Console
{
    using System;

    internal class Option
    {
        TimeSpan longBreakTs;
        TimeSpan shortBreakTs;
        TimeSpan taskTs;

        private Option()
        {
            TaskName = null;
            TaskDuration = null;
            ShortBreakDuration = null;
            LongBreakDuration = null;
            SetLength = 0;
            DisplayHelp = false;
            CollectStats = false;
            BeAnnoying = false;

            SetTimeSpans();
        }

        public bool BeAnnoying { get; set; }
        public bool CollectStats { get; set; }
        public bool DisplayHelp { get; set; }
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

        // todo: better name than 'newWith'? 
        internal static void NewWith(
            string TaskName = null,
            string TaskDuration = null,
            string ShortBreakDuration = null,
            string LongBreakDuration = null,
            int? SetLength = null,
            bool? DisplayHelp = null,
            bool? CollectStats = null,
            bool? BeAnnoying = null,
            string LogFilePath = null
        )
        {
            Empty.With(
                TaskName,
                TaskDuration,
                ShortBreakDuration,
                LongBreakDuration,
                SetLength,
                DisplayHelp,
                CollectStats,
                BeAnnoying,
                LogFilePath);
        }

        internal void With(
            string TaskName = null,
            string TaskDuration = null,
            string ShortBreakDuration = null,
            string LongBreakDuration = null,
            int? SetLength = null,
            bool? DisplayHelp = null,
            bool? CollectStats = null,
            bool? BeAnnoying = null,
            string LogFilePath = null
        )
        {
            this.TaskName = TaskName ?? this.TaskName;
            this.TaskDuration = TaskDuration ?? this.TaskDuration;
            this.ShortBreakDuration = ShortBreakDuration ?? this.ShortBreakDuration;
            this.LongBreakDuration = LongBreakDuration ?? this.LongBreakDuration;
            this.SetLength = SetLength ?? this.SetLength;
            this.DisplayHelp = DisplayHelp ?? this.DisplayHelp;
            this.CollectStats = CollectStats ?? this.CollectStats;
            this.BeAnnoying = BeAnnoying ?? this.BeAnnoying;
            this.LogFilePath = LogFilePath ?? this.LogFilePath;

            SetTimeSpans();
        }

        private void SetTimeSpans()
        {
            TimeSpan.TryParse(TaskDuration, out taskTs);
            TimeSpan.TryParse(ShortBreakDuration, out shortBreakTs);
            TimeSpan.TryParse(LongBreakDuration, out longBreakTs);
        }
    }
}