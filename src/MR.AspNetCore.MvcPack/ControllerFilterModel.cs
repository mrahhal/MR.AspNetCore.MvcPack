using System;
using System.Collections.Generic;

namespace MR.AspNetCore.MvcPack
{
	public class ControllerFilterModel
	{
		public ControllerFilterModel(Type controllerType, IList<MethodCallWrapper> filters)
		{
			ControllerType = controllerType;
			Filters = filters;
		}

		public Type ControllerType { get; }

		public IList<MethodCallWrapper> Filters { get; }
	}
}
