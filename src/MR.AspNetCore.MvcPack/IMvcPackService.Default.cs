using System;
using System.Collections.Generic;
using System.Linq;

namespace MR.AspNetCore.MvcPack
{
	public class MvcPackService : IMvcPackService
	{
		private Dictionary<Type, ControllerFilterModel> _hash;

		public MvcPackService(IEnumerable<ControllerFilterModel> models)
		{
			_hash = models.ToDictionary(m => m.ControllerType);
		}

		public ControllerFilterModel GetModelForControllerType(Type controllerType)
		{
			if (_hash.TryGetValue(controllerType, out var model))
			{
				return model;
			}

			return null;
		}
	}
}
