using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using MR.AspNetCore.MvcPack.Internal;

namespace MR.AspNetCore.MvcPack
{
	public abstract class MvcPackSupport
	{
		protected List<HookInfo> _hooks = new List<HookInfo>();
		protected List<HookInfo> _skipHooks = new List<HookInfo>();

		public MvcPackSupport(Type type)
		{
			ControllerType = type.GetTypeInfo();
		}

		public TypeInfo ControllerType { get; }

		public List<HookInfo> Hooks => _hooks;

		public List<HookInfo> SkipHooks => _skipHooks;
	}

	public abstract class MvcPackSupport<T> : MvcPackSupport
		where T : class
	{
		public MvcPackSupport()
			: base(typeof(T))
		{
		}

		protected void BeforeAction(
			Expression<Func<T, Func<ActionExecutingContext, Task>>> expression,
			IList<string> only = null,
			IList<string> except = null)
		{
			ExtractHookAndAddTo(_hooks, expression, only, except);
		}

		protected void SkipBeforeAction(
			Expression<Func<T, Func<ActionExecutingContext, Task>>> expression,
			IList<string> only = null,
			IList<string> except = null)
		{
			ExtractHookAndAddTo(_skipHooks, expression, only, except);
		}

		protected IList<string> L(params string[] list)
		{
			return list;
		}

		private void ExtractHookAndAddTo(
			List<HookInfo> hooks,
			Expression<Func<T, Func<ActionExecutingContext, Task>>> expression,
			IList<string> only = null,
			IList<string> except = null)
		{
			if (expression == null) throw new ArgumentNullException(nameof(expression));
			Debug.Assert(hooks != null);

			var methodInfo = ExtractMethodInfo(expression);
			var d = ReflectionHelper.MakeObjectFastMethodCall<T>(methodInfo);
			var hook = new HookInfo(methodInfo, d, only, except);
			hooks.Add(hook);
		}

		private MethodInfo ExtractMethodInfo(LambdaExpression expression)
		{
			return ReflectionHelper.ExtractMethodInfo(expression);
		}
	}
}
