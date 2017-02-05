using System;

namespace MR.AspNetCore.MvcPack
{
	public interface IMvcPackService
	{
		ControllerFilterModel GetModelForControllerType(Type controllerType);
	}
}
