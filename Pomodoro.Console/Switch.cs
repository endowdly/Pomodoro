// This is essentially a visitor

namespace Endowdly.Pomodoro.Console
{
    using System.Collections.Generic;

    internal class Switch : IParser
    {
        private const string Dash = "-";
        private const string DoubleDash = "--";

        private static Dictionary<string, SwitchType> dashMap = new Dictionary<string, SwitchType>
        {
            { "t", SwitchType.TaskName },
            { "d", SwitchType.TaskDuration },
            { "s", SwitchType.ShortBreak },
            { "l", SwitchType.LongBreak },
            { "p", SwitchType.SetLength },
            { "h", SwitchType.Help },
            { "r", SwitchType.RecordStats },
            { "!", SwitchType.BeAnnoying },
        };

        private static Dictionary<string, SwitchType> doubleDashMap = new Dictionary<string, SwitchType>
        {
            { "taskname", SwitchType.TaskName },
            { "taskduration", SwitchType.TaskDuration },
            { "shortbreak", SwitchType.ShortBreak },
            { "longbreak", SwitchType.LongBreak },
            { "setlength", SwitchType.SetLength },
            { "help", SwitchType.Help },
            { "recordstats", SwitchType.RecordStats },
            { "beannoying", SwitchType.BeAnnoying },
        };

        private Switch(string s)
        {
            var s1 = Dashless(s);
            Value = IsValid(s)
               ? s1.Trim()
               : string.Empty;

            Type = Map(s);
        }

        public bool IsParsed
        {
            get { return IsValid(Value); }
        }

        public SwitchType Type { get; }
        public string Value { get; }

        public static Switch Parse(string s)
        {
            return new Switch(s);
        }

        public override bool Equals(object obj)
        {
            return (obj is Switch)
                ? Equals((Switch)obj)
                : false;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        private static string Dashless(string s)
        {
            return s.Replace(Dash, string.Empty);
        }

        private static bool IsValid(string s)
        {
            var s1 = Dashless(s);

            return (dashMap.ContainsKey(s1) || doubleDashMap.ContainsKey(s1));
        }

        private static SwitchType Map(string s)
        {
            var s1 = Dashless(s);

            if (s.StartsWith(Dash) && IsValid(s))
                return dashMap[s1];

            if (s.StartsWith(DoubleDash) && IsValid(s))
                return doubleDashMap[s1];

            return SwitchType.None;
        }

        private bool Equals(Switch other)
        {
            return Value.Equals(other.Value);
        }
    }
}