namespace Endowdly.Pomodoro.Core
{

    public partial class ActivePomodoro
    {
        internal ActivePomodoro()
        {
        }
 
        public static ActivePomodoro New()
        {
            return new ActivePomodoro();
        }
                
        public IPomodoroState Stop()
        {
            return new InactivePomodoro();
        }

        public IPomodoroState Continue()
        {
            return new PomodoroBreak();
        }
    }
}
