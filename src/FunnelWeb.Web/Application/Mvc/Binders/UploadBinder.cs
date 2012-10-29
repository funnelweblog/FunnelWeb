using System.Web.Mvc;

namespace FunnelWeb.Web.Application.Mvc.Binders
{
    public class UploadBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var files = controllerContext.HttpContext.Request.Files;
            var file = files.Get(bindingContext.ModelName);

            if (file == null)
                return null;
            return new FileUpload(file);
        }
    }
}
