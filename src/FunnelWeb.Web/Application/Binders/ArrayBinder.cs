using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace FunnelWeb.Web.Application.Binders
{
    public class ArrayBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var selectedIdentities = new List<int>();
            var formValues = controllerContext.HttpContext.Request.Form;
            foreach (var parameter in formValues)
            {
                var key = (string) parameter;
                if (!key.StartsWith(bindingContext.ModelName))
                    continue;

                var value = formValues[key] ?? string.Empty;
                var values = value.Split(',').Select(x => x.Trim());
                foreach (var item in values)
                {
                    var id = 0;
                    if (int.TryParse(item, out id))
                    {
                        selectedIdentities.Add(id);
                    }
                }
            }

            return selectedIdentities.ToArray();
        }
    }
}