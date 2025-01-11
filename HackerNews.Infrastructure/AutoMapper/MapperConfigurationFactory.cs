using AutoMapper;
using System.Reflection;
using System.Text;

namespace HackerNews.Infrastructure.AutoMapper
{
    /// <summary>
    /// The mapper configuration factory.
    /// </summary>
    public class MapperConfigurationFactory
    {
        /// <summary>
        /// Creates the mapper configuration.
        /// </summary>
        /// <param name="assemblyNamesToScan">The assembly names to scan.</param>
        /// <returns>A MapperConfiguration.</returns>
        internal static MapperConfiguration CreateMapperConfiguration(IEnumerable<AssemblyName> assemblyNamesToScan)
        {
            var assembliesToScan = assemblyNamesToScan
                .Select(Assembly.Load);

            return CreateMapperConfiguration(assembliesToScan);
        }

        /// <summary>
        /// Creates the mapper configuration.
        /// </summary>
        /// <param name="assembliesToScan">The assemblies to scan.</param>
        /// <returns>A MapperConfiguration.</returns>
        private static MapperConfiguration CreateMapperConfiguration(IEnumerable<Assembly> assembliesToScan)
        {
            return new MapperConfiguration(configuration => ConfigureMapper(configuration, assembliesToScan));
        }

        /// <summary>
        /// Configures the mapper.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="assembliesToScan">The assemblies to scan.</param>
        private static void ConfigureMapper(IMapperConfigurationExpression configuration, IEnumerable<Assembly> assembliesToScan)
        {
            var profiles = GetProfilesFromTheAssemblies(assembliesToScan);

            foreach (var profile in profiles)
            {
                var addProfileMethod = typeof(IMapperConfigurationExpression)
                    .GetMethods(BindingFlags.Public | BindingFlags.Instance)
                    .First(method => method.Name == nameof(IMapperConfigurationExpression.AddProfile) && method.IsGenericMethod)
                    .MakeGenericMethod(profile);

                addProfileMethod.Invoke(configuration, new object[] { });
            }
        }

        /// <summary>
        /// Gets the profiles from the assemblies.
        /// </summary>
        /// <param name="assembliesToScan">The assemblies to scan.</param>
        /// <returns>A list of Types.</returns>
        private static IEnumerable<Type> GetProfilesFromTheAssemblies(IEnumerable<Assembly> assembliesToScan)
        {
            return assembliesToScan
                .SelectMany(GetTypes)
                .Where(type => type.IsSubclassOf(typeof(Profile)));
        }

        private static Type[] GetTypes(Assembly assembly)
        {
            try
            {
                return assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException ex)
            {
                var builder = new StringBuilder($"Failed to load assembly: {assembly.FullName}");
                builder.AppendLine();
                builder.AppendLine(ex.ToString());
                builder.AppendLine("Loader exceptions:");
                foreach (var loaderEx in ex.LoaderExceptions)
                {
                    builder.AppendLine(loaderEx.Message);
                    if (loaderEx.InnerException == null)
                    {
                        continue;
                    }

                    builder.AppendLine();
                    builder.AppendLine("Inner exception:");
                    builder.AppendLine(loaderEx.InnerException.Message);
                }

                throw new ReflectionTypeLoadException(ex.Types, ex.LoaderExceptions, builder.ToString());
            }
        }
    }
}
