
namespace Endowdly.Pomodoro.Console
{
    using System;

    [Flags]
    internal enum SwitchType
    { 
        None = 0,

        Help         = 1 << 0,
        TaskName     = 2 << 1,
        TaskDuration = 3 << 2,
        ShortBreak   = 4 << 3,
        LongBreak    = 5 << 4,
        SetLength    = 6 << 5,
        RecordStats  = 7 << 6,
        BeAnnoying   = 8 << 7, 
        
        All = ~(~0 << 8),

        BinarySwitch = Help
            | RecordStats
            | BeAnnoying,

        SingleArgumentSwitch = TaskDuration
            | ShortBreak
            | LongBreak
            | SetLength,
    }
}
