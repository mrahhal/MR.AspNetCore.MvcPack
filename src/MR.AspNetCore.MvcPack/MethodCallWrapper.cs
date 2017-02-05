using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MR.AspNetCore.MvcPack
{
	public class MethodCallWrapper
	{
		public MethodCallWrapper(HookInfo hook, bool isSkip)
		{
			Hook = hook;
			IsSkip = isSkip;
		}

		public HookInfo Hook { get; }

		public MethodInfo MethodInfo => Hook.MethodInfo;

		public Func<object, ActionExecutingContext, Task> Delegate => Hook.Delegate;

		public IReadOnlyList<string> Only => Hook.Only;

		public IReadOnlyList<string> Except => Hook.Except;

		public bool IsSkip { get; }
	}
}
