using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Web.Mvc;
using System.Web.Razor;
using System.Web.Routing;
using Microsoft.CSharp;

namespace FunnelWeb.Web.Application.Markup.Macros
{
    public class RazorMacroExecutor : IMacroExecutor
    {
        private static readonly Dictionary<int, Assembly> compiledAssemblyCache = new Dictionary<int, Assembly>();
        private static readonly object @lock = new object();

        public RazorMacroExecutor()
        {
        }

        public StringBuilder ExecuteMacro(string templateContent, HtmlHelper html)
        {
            var key = templateContent.GetHashCode();

            Assembly assembly;
            lock (@lock)
            {
                if (!compiledAssemblyCache.TryGetValue(key, out assembly))
                {
                    assembly = CompileAssembly(templateContent);
                    compiledAssemblyCache.Add(key, assembly);
                }
            }

            var type = assembly.GetType("Generated.TemplateInstance");
            var instance = (MacroBase)Activator.CreateInstance(type);
            var writer = new StringBuilder();
            instance.Html = html;
            instance.Initialize(writer);
            instance.Execute();

            return writer;
        }

        private Assembly CompileAssembly(string templateContent)
        {
            var host = new RazorEngineHost(RazorCodeLanguage.GetLanguageByExtension("cshtml"));
            host.DefaultBaseClass = typeof(MacroBase).FullName;
            host.NamespaceImports.Add("System.Web.Mvc");
            host.NamespaceImports.Add("System.Web.Mvc.Ajax");
            host.NamespaceImports.Add("System.Web.Mvc.Html");
            host.NamespaceImports.Add("System.Net");
            host.NamespaceImports.Add("System");
            host.NamespaceImports.Add("System.Web.Routing");
            host.NamespaceImports.Add("FunnelWeb.Model");
            host.NamespaceImports.Add("FunnelWeb.Web.Application.Extensions");
            host.NamespaceImports.Add("FunnelWeb.Web.Application.Mvc");

            var eng = new RazorTemplateEngine(host);
            var res = eng.GenerateCode(new StringReader(templateContent), "TemplateInstance", "Generated", "TemplateInstance.cs");

            var compiler = new CSharpCodeProvider();
            var param = new CompilerParameters();
            param.GenerateInMemory = true;
            param.ReferencedAssemblies.Add(typeof(MacroBase).Assembly.Location);
            param.ReferencedAssemblies.Add(typeof(Microsoft.CSharp.RuntimeBinder.Binder).Assembly.Location);
            param.ReferencedAssemblies.Add(typeof(System.Runtime.CompilerServices.CallSite).Assembly.Location);
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                try
                {
                    param.ReferencedAssemblies.Add(assembly.Location);
                }
                catch (NotSupportedException) { }
            }

            var results = compiler.CompileAssemblyFromDom(param, res.GeneratedCode);
            if (results.Errors.HasErrors)
            {
                throw new Exception("Error compiling template: " + results.Errors[0].ErrorText);
            }

            return results.CompiledAssembly;
        }
    }

    public class CustomViewDataContainer : IViewDataContainer
    {
        public ViewDataDictionary ViewData { get; set; }
    }
}