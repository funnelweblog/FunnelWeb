using System.Web.Mvc;
using FunnelWeb.Model;

namespace FunnelWeb.Web.Application.Mvc.Binders
{
    [ModelBinder(typeof(EntryRevision))]
    public class EntryRevisionBinder : DefaultModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var model = base.BindModel(controllerContext, bindingContext) as EntryRevision;
            model.TagsCommaSeparated =
                controllerContext.HttpContext.Request["SelectedTags-Ids"];
            return model;
        }
    }
}