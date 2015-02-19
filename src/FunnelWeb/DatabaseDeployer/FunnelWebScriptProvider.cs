using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using DbUp.Engine;

namespace FunnelWeb.DatabaseDeployer
{
    public class FunnelWebScriptProvider : IScriptProvider
    {
        private readonly _Assembly assembly;
        private readonly Func<string, bool> filter;
        private readonly string databaseProviderName;

        public FunnelWebScriptProvider(_Assembly assembly, Func<string, bool> filter, string databaseProviderName)
        {
            this.assembly = assembly;
            this.filter = filter;
            this.databaseProviderName = databaseProviderName.ToLower();
        }

        /// <summary>
        /// Gets all scripts that should be executed. 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<SqlScript> GetScripts(DbUp.Engine.Transactions.IConnectionManager connectionManager)
        {
            var providerRegex = new Regex(@"_(?<Provider>.*?)\.");
            return assembly
                .GetManifestResourceNames()
                .Where(filter)
                .GroupBy(f=> providerRegex.Replace(f, "."))
                .Select(g=>
                            {
                                if (g.Count() == 1)
                                    return g.Single();

                                var matchingProvider = g.FirstOrDefault(
                                    s =>
                                        {
                                            var @group = providerRegex.Match(s).Groups["Provider"];
                                            return @group.Success &&
                                                    @group.Value == databaseProviderName;
                                        });

                                return 
                                    matchingProvider ??
                                    g.FirstOrDefault(s => !providerRegex.Match(s).Groups["Provider"].Success);
                            })
                .OrderBy(x => x)
                .Select(ReadResourceAsScript)
                .ToList();
        }

        private SqlScript ReadResourceAsScript(string scriptName)
        {
            string contents;
            var resourceStream = assembly.GetManifestResourceStream(scriptName);
            using (var resourceStreamReader = new StreamReader(resourceStream))
            {
                contents = resourceStreamReader.ReadToEnd();
            }

            return new SqlScript(scriptName, contents);
        }
    }
}