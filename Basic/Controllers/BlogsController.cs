using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using MR.AspNetCore.MvcPack;

namespace Basic.Controllers
{
	[Route("api/blogs")]
	public class BlogsController : BaseController
	{
		private string _some;

		public new class Pack : MvcPackSupport<BlogsController>
		{
			public Pack()
			{
				BeforeAction(
					x => x.DoSomething,
					only: L(nameof(BlogsController.Action1)));
			}
		}

		[HttpGet("1")]
		public IActionResult Action1()
		{
			return Json(_some);
		}

		[HttpGet("2")]
		public IActionResult Action2()
		{
			return Json(_some);
		}

		private Task DoSomething(ActionExecutingContext context)
		{
			Logger.LogInformation("Executing...");
			_some = "foo";
			if (context.HttpContext.Request.Query.ContainsKey("404"))
			{
				context.Result = new NotFoundResult();
			}
			return Task.CompletedTask;
		}
	}
}
