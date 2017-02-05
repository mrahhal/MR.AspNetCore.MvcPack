using System.Linq;

namespace MR.AspNetCore.MvcPack
{
	public static class FilterHelper
	{
		public static bool Matches(string actionName, MethodCallWrapper filter)
		{
			var matches = false;

			if (filter.Only.Any())
			{
				matches = filter.Only.Any(n => n == actionName);
			}
			else if (filter.Except.Any())
			{
				matches = !filter.Except.Any(n => n == actionName);
			}
			else
			{
				matches = true;
			}

			return matches;
		}
	}
}
