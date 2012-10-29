using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using Autofac.Core;
using Autofac.Features.Indexed;

namespace FunnelWeb.Providers
{
    public class ProviderInfo
    {
        public static IProviderInfo<T> For<T>(IComponentContext componentContext)
        {
            var keyNames = componentContext.ComponentRegistry.Registrations
                .SelectMany(c=> c.Services)
                .OfType<KeyedService>()
                .Where(s => s.ServiceType == typeof(T))
                .Select(s => (string)s.ServiceKey)
                .ToList();

            return new ProviderInfoImpl<T>(keyNames, componentContext.Resolve<IIndex<string, Lazy<T, IProviderMetadata>>>());
        }

        class ProviderInfoImpl<T> : IProviderInfo<T>
        {
            private readonly IIndex<string, Lazy<T, IProviderMetadata>> factory;

            public ProviderInfoImpl(IEnumerable<string> keyNames, IIndex<string, Lazy<T, IProviderMetadata>> factory)
            {
                this.factory = factory;
                Keys = keyNames;
            }

            public IEnumerable<string> Keys { get; private set; }

            public T GetProviderByName(string key)
            {
                return factory[key].Value;
            }

            public IProviderMetadata GetMetaData(string key)
            {
                return factory[key].Metadata;
            }
        }
    }
}