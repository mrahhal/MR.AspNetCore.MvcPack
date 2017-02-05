using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MR.AspNetCore.MvcPack;

namespace Basic.Controllers
{
	public abstract class BaseController : Controller
	{
		public class Pack : MvcPackSupport<BaseController>
		{
			public Pack()
			{
				BeforeAction(x => x.AuthorizeSome);
			}
		}

		public ILogger Logger => HttpContext.RequestServices.GetService<ILoggerFactory>().CreateLogger(GetType());

		protected Task AuthorizeSome(ActionExecutingContext context)
		{
			Logger.LogInformation($"Executing {nameof(AuthorizeSome)}...");
			return Task.CompletedTask;
		}
	}
}
