
// ============================================================================
// Auto-generated. Do not edit!
//
// To add functionality, create and edit the partial class in a separate file. 
// ============================================================================

using System;

namespace Endowdly.Pomodoro.Core
{


	// ======================================
	// Partial Interface
	// ======================================
	[System.CodeDom.Compiler.GeneratedCodeAttribute("T4Template:FoldStates.ttinclude","1.0.0")] 
	partial interface IPomodoroState 
	{
		IPomodoroState Transition(Func<InactivePomodoro, IPomodoroState> inactivePomodoro, Func<ActivePomodoro, IPomodoroState> activePomodoro, Func<PomodoroBreak, IPomodoroState> pomodoroBreak);
		void Action(Action<InactivePomodoro> inactivePomodoro, Action<ActivePomodoro> activePomodoro, Action<PomodoroBreak> pomodoroBreak);
		T Func<T>(Func<InactivePomodoro, T> inactivePomodoro, Func<ActivePomodoro, T> activePomodoro, Func<PomodoroBreak, T> pomodoroBreak);
	}


	
	// ======================================
	// Partial Class for InactivePomodoro
	// ======================================
	[System.CodeDom.Compiler.GeneratedCodeAttribute("T4Template:FoldStates.ttinclude","1.0.0")] 
	partial class InactivePomodoro : IPomodoroState 
	{
		IPomodoroState IPomodoroState.Transition(Func<InactivePomodoro, IPomodoroState> inactivePomodoro, Func<ActivePomodoro, IPomodoroState> activePomodoro, Func<PomodoroBreak, IPomodoroState> pomodoroBreak)
		{
			return inactivePomodoro(this);
		}

		void IPomodoroState.Action(Action<InactivePomodoro> inactivePomodoro, Action<ActivePomodoro> activePomodoro, Action<PomodoroBreak> pomodoroBreak)
		{
			inactivePomodoro(this);
		}

		T IPomodoroState.Func<T>(Func<InactivePomodoro, T> inactivePomodoro, Func<ActivePomodoro, T> activePomodoro, Func<PomodoroBreak, T> pomodoroBreak)
		{
			return inactivePomodoro(this);
		}

	}

	
	// ======================================
	// Partial Class for ActivePomodoro
	// ======================================
	[System.CodeDom.Compiler.GeneratedCodeAttribute("T4Template:FoldStates.ttinclude","1.0.0")] 
	partial class ActivePomodoro : IPomodoroState 
	{
		IPomodoroState IPomodoroState.Transition(Func<InactivePomodoro, IPomodoroState> inactivePomodoro, Func<ActivePomodoro, IPomodoroState> activePomodoro, Func<PomodoroBreak, IPomodoroState> pomodoroBreak)
		{
			return activePomodoro(this);
		}

		void IPomodoroState.Action(Action<InactivePomodoro> inactivePomodoro, Action<ActivePomodoro> activePomodoro, Action<PomodoroBreak> pomodoroBreak)
		{
			activePomodoro(this);
		}

		T IPomodoroState.Func<T>(Func<InactivePomodoro, T> inactivePomodoro, Func<ActivePomodoro, T> activePomodoro, Func<PomodoroBreak, T> pomodoroBreak)
		{
			return activePomodoro(this);
		}

	}

	
	// ======================================
	// Partial Class for PomodoroBreak
	// ======================================
	[System.CodeDom.Compiler.GeneratedCodeAttribute("T4Template:FoldStates.ttinclude","1.0.0")] 
	partial class PomodoroBreak : IPomodoroState 
	{
		IPomodoroState IPomodoroState.Transition(Func<InactivePomodoro, IPomodoroState> inactivePomodoro, Func<ActivePomodoro, IPomodoroState> activePomodoro, Func<PomodoroBreak, IPomodoroState> pomodoroBreak)
		{
			return pomodoroBreak(this);
		}

		void IPomodoroState.Action(Action<InactivePomodoro> inactivePomodoro, Action<ActivePomodoro> activePomodoro, Action<PomodoroBreak> pomodoroBreak)
		{
			pomodoroBreak(this);
		}

		T IPomodoroState.Func<T>(Func<InactivePomodoro, T> inactivePomodoro, Func<ActivePomodoro, T> activePomodoro, Func<PomodoroBreak, T> pomodoroBreak)
		{
			return pomodoroBreak(this);
		}

	}

	
}