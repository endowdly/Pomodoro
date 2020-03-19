
namespace Endowdly.Pomodoro.Core
{

    public partial class InactivePomodoro
    {
        internal InactivePomodoro() 
        {
        }
       
        public static InactivePomodoro New()
        {
            return new InactivePomodoro();
        }


        public IPomodoroState Start()
        {
            return new ActivePomodoro();
        }
    }
}
