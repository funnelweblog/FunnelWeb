using System.Collections.Generic;
using System.Web.Mvc;
using FunnelWeb.Web.Model.Strings;
using Autofac;

namespace FunnelWeb.Web.Application.Mvc.Binders
{
    public class BindersModule : Module
    {
        private readonly ModelBinderDictionary _binders;

        public BindersModule(ModelBinderDictionary binders)
        {
            _binders = binders;
        }

        protected override void Load(ContainerBuilder builder)
        {
            _binders.Add(typeof(PageName), new ImplicitAssignmentBinder());
            _binders.Add(typeof(Dictionary<string, string>), new DictionaryBinder());
            _binders.Add(typeof(int[]), new ArrayBinder());
            _binders.Add(typeof(Upload), new UploadBinder());
        }
    }
}
