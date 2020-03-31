 namespace Endowdly.Pomodoro.Test
{
    using System;
    using System.Threading;

    using Xunit;

    using Endowdly.Pomodoro.Core;

    public class TaskTests
    {
        const string Smiley = ":)";
        const string Whitespace = "    ";
        const string Value = "test";

        [Fact]
        public void EmptyTaskIsEmpty()
        {
            var expected = string.Empty;
            var actual = Task.Empty;
            Assert.Equal(expected, actual.Value);
        }

        [Fact]
        public void DefaultTaskIsSmiley()
        {
            var expected = Smiley;
            var actual = Task.Default;
            Assert.Equal(expected, actual.Value);
        }

        [Fact]
        public void GivenNothingReturnEmptyTask()
        {
            var expected = Task.Empty;
            var actual = Task.New();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GivenValueReturnTaskofValue()
        {
            var given = Value;
            var actual = Task.New(Value);
            Assert.Equal(given, actual.Value);
        }

        [Fact]
        public void GivenValueWithWhitespaceReturnTaskofValueWithoutWhiteSpace()
        {
            var given = Value + Whitespace;
            var expected = Value;
            var actual = Task.New(given);
            Assert.Equal(expected, actual.Value); 
        }
    } 

    public class PomodoroStateTests
    {
        [Fact]
        public void WhenNoPomodoroGivenStartReturnsPomodoro()
        {
            var state = InactivePomodoro.New();
            var newState = state.Start();
            var isActive = newState.Func(
                pomStateInactive => false,
                pomStateActive => true,
                pomStateBreak => true
                );
            Assert.True(isActive);
        }

        [Fact]
        public void WhenActivePomodoroGivenContinueReturnsPomodoroBreak() 
        {
            var state = ActivePomodoro.New();
            var newState = state.Continue();
            var isPause = newState.Func(
                pomStateInactive => false,
                pomStateActive => false,
                pomStateBreak => true
                );
            Assert.True(isPause); 
        }

        [Fact]
        public void WhenPomodoroBreakGivenContinueReturnsActivePomodoro() 
        {
            var state = PomodoroBreak.New();
            var newState = state.Continue();
            var isActive = newState.Func(
                pomStateInactive => false,
                pomStateActive => true,
                pomStateBreak => false
                );
            Assert.True(isActive); 
        } 

        [Fact]
        public void WhenPomodoroBreakGivenStopReturnsInactivePomodoro() 
        {
            var state = PomodoroBreak.New();
            var newState = state.Stop();
            var isInactive = newState.Func(
                pomStateInactive => true,
                pomStateActive => false,
                pomStateBreak => false
                );
            Assert.True(isInactive); 
        } 

        [Fact]
        public void WhenActivePomodoroGivenStopReturnsInactivePomodoro() 
        {
            var state = ActivePomodoro.New();
            var newState = state.Stop();
            var isInactive = newState.Func(
                pomStateInactive => true,
                pomStateActive => false,
                pomStateBreak => false
                );
            Assert.True(isInactive); 
        } 
    }

    public class TimerTests
    {
        static PomodoroTimer timer = PomodoroTimer.Default;

        [Fact]
        public void WhenNewTimerIsNotActive()
        {
            Assert.False(timer.IsActive);
        }

        [Fact]
        public void WhenNewTimerIsNotBreak()
        {
            Assert.False(timer.IsBreak);
        }


        [Fact]
        public void WhenNewTimerTaskIsDefault()
        {
            Assert.Equal(Task.Default, timer.Task);
        }

        [Fact]
        public void WhenNewTimerTaskDurationIs20Minutes()
        {
            Assert.Equal(TimeSpan.FromMinutes(20.0), timer.TaskDuration);
        }

        [Fact]
        public void WhenNewTimerShortBreakDurationIs3Minutes()
        {
            Assert.Equal(TimeSpan.FromMinutes(3.0), timer.ShortBreakDuration);
        }

        [Fact]
        public void WhenNewTimerLongBreakDurationIs15Minutes()
        {
            Assert.Equal(TimeSpan.FromMinutes(15.0), timer.LongBreakDuration);
        }

        [Fact]
        public void WhenNewTimerSetLengthIs4()
        {
            Assert.Equal(4, timer.SetLength);
        }

        [Fact]
        public void WhenStartGivenNewTimerStateIsActive()
        {
            var innerTimer = PomodoroTimer.Default;
            innerTimer.Start();
            Assert.True(innerTimer.IsActive);
        }

        [Fact]
        public void WhenNewGivenFastTimeTaskDurationIsFast()
        {
            var fast = TimeSpan.FromMilliseconds(1);
            var innerTimer = PomodoroTimer.Default.With(TaskDuration: fast);
            Assert.Equal(TimeSpan.FromMilliseconds(1), innerTimer.TaskDuration);
        }

        [Fact]
        public void WhenStartGivenFastTimerStateIsActiveAndBreak()
        {
            var fast = TimeSpan.FromMilliseconds(1);
            var innerTimer = PomodoroTimer.Default.With(TaskDuration: fast);
            innerTimer.Start();   // after 10 ms we should be moving to break
            Thread.Sleep(10);
            Assert.NotEqual(innerTimer, timer);
            Assert.True(innerTimer.IsActive);
            Assert.True(innerTimer.IsBreak);
        }

        [Fact]
        public void WhenStopGiveNewTimerStateIsInActive()
        {
            var innerTimer = PomodoroTimer.Default;
            innerTimer.Stop();
            Assert.False(innerTimer.IsActive);
            Assert.False(innerTimer.IsBreak);
        }
    }
}
