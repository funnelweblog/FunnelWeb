using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using Autofac;

namespace FunnelWeb
{
    public interface IFunnelWebExtension
    {
        void Initialize(ContainerBuilder builder);
    }

    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class FunnelWebExtensionAttribute : ExportAttribute
    {
        public FunnelWebExtensionAttribute()
            : base(typeof(IFunnelWebExtension))
        {
            
        }

        public FunnelWebExtensionAttribute(IDictionary<string, object> something)
            : base(typeof(IFunnelWebExtension))
        {
            
        }

        public string FullName { get; set; }
        public string SupportLink { get; set; }
        public string Publisher { get; set; }
    }

    public class ExtensionsModule : Module
    {
        private readonly string extensionsPath;

        public ExtensionsModule(string extensionsPath)
        {
            this.extensionsPath = extensionsPath;
        }

        [ImportMany]
        public IEnumerable<Lazy<IFunnelWebExtension, FunnelWebExtensionAttribute>> Extensions { get; set; }

        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            if (!Directory.Exists(extensionsPath))
                return;

            var container = new CompositionContainer(
                new DirectoryCatalog(extensionsPath)
                );

            container.SatisfyImportsOnce(this);
            foreach (var export in Extensions)
            {
                var extension = export.Value;
                extension.Initialize(builder);
                builder.RegisterInstance(export.Metadata).As<FunnelWebExtensionAttribute>();
            }
        }
    }
}
