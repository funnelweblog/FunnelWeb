using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web.Compilation;

namespace FunnelWeb.Web.Application
{
    public static class Extensibility
    {
        /// <summary>
        /// Finds all FunnelWeb extensions (in the <paramref name="extensionsPath"/>), and adds the assembly 
        /// that they are contained in to the ASP.NET BuildManager.
        /// </summary>
        /// <param name="extensionsPath">The extensions path.</param>
        public static void EnableAspNetIntegration(string extensionsPath)
        {
            TryOurBestToCreateExtensionsDirectory(extensionsPath);

            if (!Directory.Exists(extensionsPath)) return;

            var assemblies = DiscoverAssembliesThatContainExtensionsUsingMef(extensionsPath);
            foreach (var assembly in assemblies)
            {
                BuildManager.AddReferencedAssembly(assembly);
            }
        }

        private static IEnumerable<Assembly> DiscoverAssembliesThatContainExtensionsUsingMef(string extensionsPath)
        {
            var catalog = new DirectoryCatalog(extensionsPath);
            var compositionContainer = new CompositionContainer(catalog);
            
            try
            {
                var modules = compositionContainer.GetExportedValues<IFunnelWebExtension>();
                return modules.Select(m => m.GetType().Assembly).Distinct();
            }
            catch (ReflectionTypeLoadException ex)
            {
                if (ex.LoaderExceptions.Length > 0)
                    throw new FunnelWebExtensionLoadException(string.Format("Failed to load {0}", ex.Types[0].Assembly.FullName), ex.LoaderExceptions[0]);
                throw;
            }
        }

        private static void TryOurBestToCreateExtensionsDirectory(string extensionsPath)
        {
            if (!Directory.Exists(extensionsPath))
            {
                try
                {
                    Directory.CreateDirectory(extensionsPath);
                }
                catch (IOException ex)
                {
                    // oh, well nothing really we can do
                    Trace.WriteLine("Could not create extensions directory:" + extensionsPath + "\r\n" + ex.Message);
                }
            }
        }
    }
}