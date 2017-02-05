using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MR.AspNetCore.MvcPack
{
	public class HookInfo
	{
		public HookInfo(
			MethodInfo mi,
			Func<object, ActionExecutingContext, Task> d,
			IList<string> only,
			IList<string> except)
		{
			MethodInfo = mi;
			Delegate = d;
			Only = new ReadOnlyCollection<string>(only ?? new List<string>());
			Except = new ReadOnlyCollection<string>(except ?? new List<string>());
		}

		public MethodInfo MethodInfo { get; }

		public Func<object, ActionExecutingContext, Task> Delegate { get; }

		public IReadOnlyList<string> Only { get; }

		public IReadOnlyList<string> Except { get; }
	}
}
