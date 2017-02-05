using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MR.AspNetCore.MvcPack.Internal
{
	internal static class ReflectionHelper
	{
		public static List<TypeInfo> IncludeBaseTypes(TypeInfo type)
		{
			var list = new List<TypeInfo>();

			do
			{
				list.Add(type);
			} while ((type = type.BaseType.GetTypeInfo()).AsType() != typeof(object));

			list.Reverse();
			return list;
		}

		public static MethodInfo ExtractMethodInfo(LambdaExpression expression)
		{
			var unaryExpression = (UnaryExpression)expression.Body;
			var methodCallExpression = (MethodCallExpression)unaryExpression.Operand;
			var methodCallObject = (ConstantExpression)methodCallExpression.Object;
			var methodInfo = (MethodInfo)methodCallObject.Value;
			return methodInfo;
		}

		public static Func<T, ActionExecutingContext, Task> MakeFastMethodCall<T>(MethodInfo mi)
			where T : class
		{
			var delegateType = typeof(Func<T, ActionExecutingContext, Task>);
			var methodDelegate = (Func<T, ActionExecutingContext, Task>)mi.CreateDelegate(delegateType);
			return methodDelegate;
		}

		public static Func<object, ActionExecutingContext, Task> MakeObjectFastMethodCall<T>(MethodInfo mi)
			where T : class
		{
			var d = MakeFastMethodCall<T>(mi);
			return MakeObjectFastMethodCall(d);
		}

		public static Func<object, ActionExecutingContext, Task> MakeObjectFastMethodCall<T>(
			Func<T, ActionExecutingContext, Task> d)
			where T : class
		{
			return (obj, context) =>
			{
				return d((T)obj, context);
			};
		}
	}
}
