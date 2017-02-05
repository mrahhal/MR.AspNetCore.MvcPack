using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MR.AspNetCore.MvcPack.Internal;

namespace MR.AspNetCore.MvcPack
{
	public static class MvcPackServiceBuilder
	{
		public static IMvcPackService Build(IList<TypeInfo> controllerTypes, IEnumerable<TypeInfo> types)
		{
			var packTypes = types
				.Where(t =>
					typeof(MvcPackSupport).GetTypeInfo().IsAssignableFrom(t) &&
					!t.IsAbstract)
				.ToList();

			var packs = InstantiateTypes(packTypes);
			var models = controllerTypes.Select(t => BuildModel(t, packs)).ToList();
			return new MvcPackService(models);
		}

		private static List<MvcPackSupport> InstantiateTypes(List<TypeInfo> packTypes)
		{
			return packTypes.Select(t => (MvcPackSupport)Activator.CreateInstance(t.AsType())).ToList();
		}

		private static ControllerFilterModel BuildModel(TypeInfo controllerType, List<MvcPackSupport> packs)
		{
			var hierarchy = ReflectionHelper.IncludeBaseTypes(controllerType);
			var filters = new List<MethodCallWrapper>();

			foreach (var type in hierarchy)
			{
				var pack = packs.FirstOrDefault(p => p.ControllerType == type);
				if (pack == null) continue;

				foreach (var skipHook in pack.SkipHooks)
				{
					var filter = filters.FirstOrDefault(f => f.MethodInfo.Name == skipHook.MethodInfo.Name);
					if (!skipHook.Only.Any() && !skipHook.Except.Any())
					{
						// unconditional skip
						filters.Remove(filter);
					}
					else
					{
						filters.Add(new MethodCallWrapper(skipHook, true));
					}
				}

				filters.AddRange(pack.Hooks.Select(h => new MethodCallWrapper(h, false)));
			}

			return new ControllerFilterModel(controllerType.AsType(), filters);
		}
	}
}
