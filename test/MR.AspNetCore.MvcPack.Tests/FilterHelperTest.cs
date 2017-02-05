using System.Collections.Generic;
using System.Reflection;
using FluentAssertions;
using Xunit;

namespace MR.AspNetCore.MvcPack
{
	public class FilterHelperTest
	{
		private class SomeClass
		{
			public void Some()
			{
			}
		}

		public class Matches
		{
			[Fact]
			public void Unconditional_True()
			{
				var filter = CreateFilter(null, null);

				var result = FilterHelper.Matches("Foo", filter);

				result.Should().BeTrue();
			}

			[Fact]
			public void HasOnly_NotIncluded_False()
			{
				var filter = CreateFilter(new[] { "Bar" }, null);

				var result = FilterHelper.Matches("Foo", filter);

				result.Should().BeFalse();
			}

			[Fact]
			public void HasOnly_Included_True()
			{
				var filter = CreateFilter(new[] { "Bar", "Foo" }, null);

				var result = FilterHelper.Matches("Foo", filter);

				result.Should().BeTrue();
			}

			[Fact]
			public void HasExcept_NotIncluded_True()
			{
				var filter = CreateFilter(null, new[] { "Bar" });

				var result = FilterHelper.Matches("Foo", filter);

				result.Should().BeTrue();
			}

			[Fact]
			public void HasExcept_Included_False()
			{
				var filter = CreateFilter(null, new[] { "Bar", "Foo" });

				var result = FilterHelper.Matches("Foo", filter);

				result.Should().BeFalse();
			}

			[Fact]
			public void HasOnlyAndExcept_TakesOnlyIntoAccount()
			{
				var filter = CreateFilter(new[] { "Bar", "Foo" }, new[] { "Foo" });

				var result = FilterHelper.Matches("Foo", filter);

				result.Should().BeTrue();
			}

			private MethodCallWrapper CreateFilter(IList<string> only, IList<string> except)
			{
				return new MethodCallWrapper(
					new HookInfo(
						typeof(SomeClass).GetTypeInfo().GetMethod("Some"),
						null,
						only,
						except),
					false);
			}
		}
	}
}
