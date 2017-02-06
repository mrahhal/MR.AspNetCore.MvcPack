using System.Threading.Tasks;
using Basic.Models;
using Basic.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using MR.AspNetCore.MvcPack;

namespace Basic.Controllers.Users
{
	[Route("api/users")]
	public class UsersController : UsersBaseController
	{
		private IUserRepository _repo;
		private AppUser _user;

		public new class Pack : MvcPackSupport<UsersController>
		{
			public Pack()
			{
				SkipBeforeAction(x => x.AuthorizeSome);

				BeforeAction(x => x.Some, only: L(
					nameof(UsersController.Show)));

				BeforeAction(x => x.SetUser, only: L(
					nameof(UsersController.Get),
					nameof(UsersController.Update)));
			}
		}

		public UsersController(IUserRepository repo)
		{
			_repo = repo;
		}

		[HttpGet("{id:int}")]
		public IActionResult Get(int id)
		{
			return Json(_user);
		}

		[HttpPut("{id:int}")]
		public IActionResult Update(int id)
		{
			return Json(_user);
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

		private Task SetUser(ActionExecutingContext context)
		{
			Logger.LogInformation($"Executing {nameof(SetUser)}...");
			var id = (int)context.ActionArguments["id"];

			// Here, you can get user from id and set _user to it for example.
			_user = _repo.FindById(id);
			if (_user == null)
			{
				// This will shortcircuit the pipeline, the controller action won't be called.
				context.Result = new NotFoundResult();
			}

			return Task.CompletedTask;
		}
	}
}
