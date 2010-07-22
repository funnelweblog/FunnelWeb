using System.Web.Mvc;

namespace FunnelWeb.Web.Application.Binders
{
    public class UploadBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var files = controllerContext.HttpContext.Request.Files;
            foreach (string fileName in files)
            {
                if (fileName != bindingContext.ModelName)
                    continue;

                var postedFile = files[fileName];
                if (postedFile.ContentLength == 0)
                    continue;

                return new Upload(postedFile);
            }
            return null;
        }
    }
}
