using System.Reflection;
using FluentAssertions;
using Xunit;

namespace MR.AspNetCore.MvcPack.Internal
{
	public class ReflectionHelperTest
	{
		private class Base
		{
		}

		private class Sub : Base
		{
		}

		[Fact]
		public void IncludeBaseTypes_InOrder()
		{
			var types = ReflectionHelper.IncludeBaseTypes(typeof(Sub).GetTypeInfo());

			types.Should().ContainInOrder(typeof(Base).GetTypeInfo(), typeof(Sub).GetTypeInfo());
		}
	}
}
