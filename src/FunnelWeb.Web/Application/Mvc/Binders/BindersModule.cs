using System.Collections.Generic;
using System.Web.Mvc;
using FunnelWeb.Web.Model.Strings;
using Autofac;

namespace FunnelWeb.Web.Application.Mvc.Binders
{
    public class BindersModule : Module
    {
        private readonly ModelBinderDictionary binders;

        public BindersModule(ModelBinderDictionary binders)
        {
            this.binders = binders;
        }

        protected override void Load(ContainerBuilder builder)
        {
            binders.Add(typeof(PageName), new ImplicitAssignmentBinder());
            binders.Add(typeof(Dictionary<string, string>), new DictionaryBinder());
            binders.Add(typeof(int[]), new ArrayBinder());
            binders.Add(typeof(Upload), new UploadBinder());
        }
    }
}
