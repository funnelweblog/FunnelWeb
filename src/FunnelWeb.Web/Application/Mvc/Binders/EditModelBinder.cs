using System.Web.Mvc;
using FunnelWeb.Web.Views.Wiki;

namespace FunnelWeb.Web.Application.Mvc.Binders
{
    [ModelBinder(typeof(EditModel))]
    public class EditModelBinder : DefaultModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var model = base.BindModel(controllerContext, bindingContext) as EditModel;
            model.TagsString =
                controllerContext.HttpContext.Request["SelectedTags-Ids"];
            return model;
        }
    }
}