namespace Endowdly.Pomodoro.Core
{
    using System;
    using System.Timers;

    public sealed class PomodoroTimer
    {
        public Timer Timer { get; private set; }
        public Timer Metronome { get; private set; }
        public int SecondsElapsed { get; private set; }
        public int CurrentDuration { get; private set; } // The current state's timer interval in seconds

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
        public State CurrentState
        {
            get
            {
                return state.Func(
                    pomStateInactive => State.Inactive,
                    pomStateActive => State.Active,
                    pomStateBreak => PomodoroCounter == SetLength
                        ? State.LongBreak
                        : State.ShortBreak
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

            Metronome = new Timer(1000);
            Metronome.Elapsed += (o, e) => SecondsElapsed++;
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
            CurrentDuration = GetCurrentDuration();
            PomodoroCounter = 1;
            SecondsElapsed = 0;
            Timer = new Timer(TaskDuration.TotalMilliseconds);
            Timer.Elapsed += TimerElapsed;

            Timer.Start();
            Metronome.Start(); 
            OnStateChanged();
        }

        private int GetCurrentDuration() =>
            (int)state.Func(
                pomStateInactive => 0,
                pomStateActive => TaskDuration.TotalSeconds,
                pomStateBreak => PomodoroCounter == SetLength
                    ? LongBreakDuration.TotalSeconds
                    : ShortBreakDuration.TotalSeconds);
                    
        private void Update()
        {
            Timer.Stop();
            SecondsElapsed = 0;
            
            var nextTimerLength = state.Func(
                pomStateInactive => TaskDuration,
                pomStateActive => PomodoroCounter == SetLength
                    ? LongBreakDuration
                    : ShortBreakDuration,
                pomStateBreak => TaskDuration
                ); 
            PomodoroCounter = state.Func(
                pomStateInactive => PomodoroCounter, 
                pomStateActive => PomodoroCounter,
                pomStateBreak => PomodoroCounter == SetLength
                    ? 1
                    : PomodoroCounter + 1
                ); 
            state = state.Transition(
                pomStateInactive => pomStateInactive,
                pomStateActive => pomStateActive.Continue(),
                pomStateBreak => pomStateBreak.Continue()
                );
            CurrentDuration = GetCurrentDuration();
            Timer = new Timer(nextTimerLength.TotalMilliseconds);
            Timer.Elapsed += TimerElapsed;

            Timer.Start();
        }

        public void Stop()
        {
            Timer?.Stop();
            Metronome?.Stop();
            

            state = state.Transition(
                pomStateInactive => pomStateInactive,
                pomStateActive => pomStateActive.Stop(),
                pomStateBreak => pomStateBreak.Stop()
                ); 
        }

        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            Update();
            OnStateChanged();
        }

        private void OnStateChanged() => StateChange?.Invoke(this, EventArgs.Empty);
    }
}

