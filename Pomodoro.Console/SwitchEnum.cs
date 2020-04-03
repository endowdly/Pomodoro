
namespace Endowdly.Pomodoro.Console
{
    using System;

    [Flags]
    internal enum SwitchType
    { 
        None = 0,

        Help         = 1 << 0,
        TaskName     = 1 << 1,
        TaskDuration = 1 << 2,
        ShortBreak   = 1 << 3,
        LongBreak    = 1 << 4,
        SetLength    = 1 << 5,
        RecordStats  = 1 << 6,
        BeAnnoying   = 1 << 7, 
        
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
