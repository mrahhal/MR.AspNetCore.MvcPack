using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using MR.AspNetCore.MvcPack;

namespace Basic.Controllers.Users
{
	[Route("api/users")]
	public class UsersController : UsersBaseController
	{
		public new class Pack : MvcPackSupport<UsersController>
		{
			public Pack()
			{
				SkipBeforeAction(x => x.AuthorizeSome);

				BeforeAction(
					x => x.Some,
					only: L(nameof(UsersController.Show)));
			}
		}

		[HttpGet]
		public IActionResult Get()
		{
			return Ok();
		}

		[HttpGet("show")]
		public IActionResult Show()
		{
			return Ok();
		}

		[HttpGet("hide")]
		public IActionResult Hide()
		{
			return Ok();
		}

		public Task Some(ActionExecutingContext context)
		{
			Logger.LogInformation($"Executing {nameof(Some)}...");
			return Task.CompletedTask;
		}
	}
}
