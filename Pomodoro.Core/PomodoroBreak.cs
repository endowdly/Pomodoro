namespace Endowdly.Pomodoro.Core
{

    public partial class PomodoroBreak 
    {
        internal PomodoroBreak()
        {
        }

        public static PomodoroBreak New()
        {
            return new PomodoroBreak();
        }

        public IPomodoroState Stop()
        {
            return new InactivePomodoro();
        }

        public IPomodoroState Continue()
        {
            return new ActivePomodoro();
        }
    }
}
