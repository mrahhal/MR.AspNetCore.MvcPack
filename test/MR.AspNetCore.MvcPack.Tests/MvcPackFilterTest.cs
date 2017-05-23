using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;

namespace MR.AspNetCore.MvcPack
{
	public class MvcPackFilterTest
	{
		private Mock<IMvcPackService> _mockService;

		public MvcPackFilterTest()
		{
			_mockService = new Mock<IMvcPackService>();
		}

		private class Controller
		{
			private void Some1()
			{
			}

			private void Some2()
			{
			}
		}

		[Fact]
		public async Task NoModel_CallsNext()
		{
			_mockService
				.Setup(m => m.GetModelForControllerType(typeof(Controller)))
				.Returns((ControllerFilterModel)null);
			var controller = new Controller();

			var nextCalled = await Execute(controller);

			nextCalled.Should().BeTrue();
		}

		[Fact]
		public async Task CallsMatchingFilters()
		{
			var dCalled = false;
			_mockService
				.Setup(m => m.GetModelForControllerType(typeof(Controller)))
				.Returns(new ControllerFilterModel(typeof(Controller), new[] { CreateFilter(() => dCalled = true, false) }));

			var controller = new Controller();

			var nextCalled = await Execute(controller);

			nextCalled.Should().BeTrue();
			dCalled.Should().BeTrue();
		}

		[Fact]
		public async Task DoesntCallNextIfFilterSetsResult()
		{
			var dCalled = false;
			_mockService
				.Setup(m => m.GetModelForControllerType(typeof(Controller)))
				.Returns(new ControllerFilterModel(typeof(Controller), new[] { CreateFilter(() => dCalled = true, true) }));

			var controller = new Controller();

			var nextCalled = await Execute(controller);

			nextCalled.Should().BeFalse();
			dCalled.Should().BeTrue();
		}

		private MethodCallWrapper CreateFilter(Action onDelegateCall, bool setsResult, bool isSkip = false)
		{
			var hook = new HookInfo(
				typeof(Controller).GetTypeInfo().GetMethod("Some1"), (_, context) =>
				{
					if (setsResult)
					{
						context.Result = new NotFoundResult();
					}
					onDelegateCall?.Invoke();
					return Task.FromResult(0);
				}, null, null);
			return new MethodCallWrapper(hook, isSkip);
		}

		private async Task<bool> Execute(object controller)
		{
			var typeFilter = new MvcPackFilter();
			var services = new ServiceCollection();
			services.AddSingleton(_mockService.Object);
			var provider = services.BuildServiceProvider();
			var filter = typeFilter.CreateInstance(provider) as IAsyncActionFilter;

			var context = CreateActionExecutingContext(filter, controller);
			var nextCalled = false;
			var next = new ActionExecutionDelegate(() =>
			{
				nextCalled = true;
				return Task.FromResult(CreateActionExecutedContext(context));
			});

			await filter.OnActionExecutionAsync(context, next);
			return nextCalled;
		}

		private static ActionExecutingContext CreateActionExecutingContext(
			IFilterMetadata filter,
			object controller)
		{
			return new ActionExecutingContext(
				CreateActionContext(),
				new IFilterMetadata[] { filter, },
				new Dictionary<string, object>(),
				controller);
		}

		private static ActionExecutedContext CreateActionExecutedContext(
			ActionExecutingContext context)
		{
			return new ActionExecutedContext(context, context.Filters, context.Controller);
		}

		private static ActionContext CreateActionContext()
		{
			return new ActionContext(new DefaultHttpContext(), new RouteData(), new ControllerActionDescriptor()
			{
				ActionName = "Action1"
			});
		}
	}
}
