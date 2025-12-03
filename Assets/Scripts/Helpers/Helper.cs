using Cadpeople.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Helper
{
	public static void FireEventInMainThread(GameEvent evt, object par)
	{
		Dispatcher.RunOnMainThread(() => { evt?.Raise(par); });
	}

	public static void RunInMainThread(Action func)
	{
		Dispatcher.RunOnMainThread(() => func());
	}
}
