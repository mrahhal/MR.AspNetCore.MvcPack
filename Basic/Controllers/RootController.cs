using Microsoft.AspNetCore.Mvc;

namespace Basic.Controllers
{
	[Route("")]
	public class RootController : BaseController
	{
		[HttpGet]
		public IActionResult Get()
		{
			return Json("...");
		}
	}
}
