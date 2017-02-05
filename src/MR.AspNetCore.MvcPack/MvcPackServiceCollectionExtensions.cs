using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;
using MR.AspNetCore.MvcPack;

namespace Microsoft.Extensions.DependencyInjection
{
	public static class MvcPackServiceCollectionExtensions
	{
		public static void AddMvcPack(this IServiceCollection services)
		{
			services.Configure<MvcOptions>(options =>
			{
				options.Filters.Add(new MvcPackFilter());
			});
		}

		public static IMvcBuilder InitializeMvcPack(this IMvcBuilder builder)
		{
			var feature = new ControllerFeature();
			builder.PartManager.PopulateFeature(feature);
			var controllerTypes = feature.Controllers;
			var types = builder.PartManager.ApplicationParts.OfType<AssemblyPart>().SelectMany(x => x.Types);

			var service = MvcPackServiceBuilder.Build(controllerTypes, types);
			builder.Services.AddSingleton(service);

			return builder;
		}
	}
}
