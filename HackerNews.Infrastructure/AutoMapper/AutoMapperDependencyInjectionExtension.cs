using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace HackerNews.Infrastructure.AutoMapper
{
    /// <summary>
    /// The auto mapper dependency injection extension.
    /// </summary>
    public static class AutoMapperDependencyInjectionExtension
    {
        private static readonly string[] AssemblyPrefixes = { "HackerNews" };

        /// <summary>
        /// Adds the auto mapper to service collection.
        /// </summary>
        /// <param name="services">The services.</param>
        public static void AddAutoMapper(this IServiceCollection services)
        {
            var executingAssembly = Assembly.GetEntryAssembly();

            if (executingAssembly != null)
            {
                var mapperInstance = CreateMapper(executingAssembly);
                services.AddSingleton(mapperInstance);
            }
        }

        /// <summary>
        /// Creates the mapper.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <returns>An IMapper.</returns>
        private static IMapper CreateMapper(Assembly assembly)
        {
            var referencedAssemblies = assembly
                .GetReferencedAssemblies()
                .Append(assembly.GetName())
                .Where(assemblyName => AssemblyPrefixes.Any(prefix => assemblyName.FullName.Contains(prefix)));

            return MapperConfigurationFactory
                .CreateMapperConfiguration(referencedAssemblies)
                .CreateMapper();
        }
    }
}
