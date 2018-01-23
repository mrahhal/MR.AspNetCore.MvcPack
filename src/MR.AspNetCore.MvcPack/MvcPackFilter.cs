using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MR.AspNetCore.MvcPack
{
	public class MvcPackFilter : TypeFilterAttribute
	{
		public MvcPackFilter()
			: base(typeof(Impl))
		{
		}

		internal class Impl : IAsyncActionFilter
		{
			private IMvcPackService _service;

			public Impl(IMvcPackService service)
			{
				_service = service;
			}

			public Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
			{
				var controllerType = context.Controller.GetType();
				var filterModel = _service.GetModelForControllerType(controllerType);

				if (filterModel == null)
				{
					return next();
				}

				return OnActionExecutionCoreAsync(context, next, filterModel);
			}

			private async Task OnActionExecutionCoreAsync(
				ActionExecutingContext context, ActionExecutionDelegate next, ControllerFilterModel filterModel)
			{
				var actionName = (context.ActionDescriptor as ControllerActionDescriptor).ActionName;

				var filters = filterModel.Filters;
				for (var i = 0; i < filters.Count; i++)
				{
					var filter = filters[i];
					var match = FilterHelper.Matches(actionName, filter);
					if ((match && filter.IsSkip) || (!match && !filter.IsSkip))
					{
						continue;
					}

					await filter.Delegate(context.Controller, context);
					if (context.Result != null)
					{
						return;
					}
				}

				await next();
			}
		}
	}
}
