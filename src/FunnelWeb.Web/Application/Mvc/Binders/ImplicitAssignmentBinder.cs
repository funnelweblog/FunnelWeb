using System;
using System.Linq;
using System.Reflection;
using FunnelWeb.Web.Model.Strings;

namespace FunnelWeb.Web.Application.Mvc.Binders
{
    /// <summary>
    /// This binder lets us assign basic types (like a string) to a more complicated type if an implicit assignment 
    /// operator exists. This is useful for <see cref="PageName"/>.
    /// </summary>
    public class ImplicitAssignmentBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName).AttemptedValue;

            var implicitAssignment = bindingContext.ModelType.GetMethods(BindingFlags.Public | BindingFlags.DeclaredOnly | BindingFlags.Static)
                .Where(x => x.Name == "op_Implicit")
                .Where(x => bindingContext.ModelType.IsAssignableFrom(x.ReturnType))
                .FirstOrDefault();

            if (implicitAssignment == null)
            {
                throw new ArgumentException(string.Format("The Implicit Assignment Binder was being applied to this request, but the target type was '{0}', which does not provide an implicit assignment operator.", bindingContext.ModelType));
            }

            var result = null as object;

            try
            {
                result = implicitAssignment.Invoke(null, new object[] { value });
            }
            catch (Exception ex)
            {
                var message = string.Format("An exception occurred when trying to convert the paramater named '{0}' to type '{1}'. {2}", 
                    bindingContext.ModelName, 
                    bindingContext.ModelType.Name,
                    ex.Message
                    );
                throw new ArgumentException(message);
            }

            return result;
        }
    }
}
