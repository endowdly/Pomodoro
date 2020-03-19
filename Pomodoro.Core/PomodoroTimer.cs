namespace Endowdly.Pomodoro.Core
{
    using System;
    using System.Timers;

    public sealed class PomodoroTimer
    {
        private Timer timer;
        private IPomodoroState state;

        public event EventHandler StateChange;

        public Task Task { get; }
        public bool IsActive
        {
            get
            {
                return state.Func(
                    pomStateInactive => false,
                    pomStateActive => true,
                    pomStateBreak => true
                    );
            }
        }
        public bool IsBreak
        {
            get
            {
                return state.Func(
                    pomStateInactive => false,
                    pomStateActive => false,
                    pomStateBreak => true
                    );
            }
        }
        public TimeSpan TaskDuration { get; }
        public TimeSpan ShortBreakDuration { get; }
        public TimeSpan LongBreakDuration { get; }
        public int SetLength { get;  }
        public int PomodoroCounter { get; private set; }

        public static PomodoroTimer Default = new PomodoroTimer(
            Task: Task.Default,
            State: InactivePomodoro.New(),
            TaskDuration: TimeSpan.FromMinutes(20.0),
            ShortBreakDuration: TimeSpan.FromMinutes(3.0),
            LongBreakDuration: TimeSpan.FromMinutes(15.0)
            );

        private PomodoroTimer(Task Task,
            IPomodoroState State,
            TimeSpan TaskDuration,
            TimeSpan ShortBreakDuration,
            TimeSpan LongBreakDuration,
            int SetLength = 4)
        {
            this.Task = Task;
            state = State;
            this.TaskDuration = TaskDuration;
            this.ShortBreakDuration = ShortBreakDuration;
            this.LongBreakDuration = LongBreakDuration;
            this.SetLength = SetLength;
            PomodoroCounter = 0;
        }

        public static PomodoroTimer New()
        {
            return Default;
        }

        public static PomodoroTimer New(Task t)
        {
            return Default.With(
                Task: t
                );
        }

        public PomodoroTimer With (Task Task = null,
            TimeSpan? TaskDuration = null,
            TimeSpan? ShortBreakDuration = null,
            TimeSpan? LongBreakDuration = null,
            int? SetLength = null) 
        {
            return new PomodoroTimer(
                Task: Task ?? this.Task,
                State: state,
                TaskDuration: TaskDuration ?? this.TaskDuration,
                ShortBreakDuration: ShortBreakDuration ?? this.ShortBreakDuration,
                LongBreakDuration: LongBreakDuration ?? this.LongBreakDuration,
                SetLength: SetLength ?? this.SetLength); 
        } 

        public void Start()
        {
            state = state.Transition(
                pomStateInactive => pomStateInactive.Start(),
                pomStateActive => pomStateActive,
                pomStateBreak => pomStateBreak
                );

            timer = new Timer(TaskDuration.TotalMilliseconds);
            timer.Elapsed += TimerElapsed;
            timer.Start();
        }

        private void Update()
        {
            
            var nextTimerLength = state.Func(
                pomStateInactive => TaskDuration,
                pomStateActive => PomodoroCounter < SetLength
                    ? ShortBreakDuration
                    : LongBreakDuration,
                pomStateBreak => TaskDuration
                );

            state.Action(
                pomStateInactive => { },
                pomStateActive => PomodoroCounter = PomodoroCounter != SetLength
                    ? PomodoroCounter + 1
                    : 0,
                pomStateBreak => { }
                );

            state = state.Transition(
                pomStateInactive => pomStateInactive,
                pomStateActive => pomStateActive.Continue(),
                pomStateBreak => pomStateBreak.Continue()
                );

            timer = new Timer(nextTimerLength.TotalMilliseconds);
            timer.Elapsed += TimerElapsed;
            timer.Start();
        }

        public void Stop()
        {
            timer?.Stop();

            state = state.Transition(
                pomStateInactive => pomStateInactive,
                pomStateActive => pomStateActive.Stop(),
                pomStateBreak => pomStateBreak.Stop()
                ); 
        }

        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            Update();
        }
    }
}
