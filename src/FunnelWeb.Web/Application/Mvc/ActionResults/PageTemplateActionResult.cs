using System.Web.Mvc;

namespace FunnelWeb.Web.Application.Mvc.ActionResults
{
    public class PageTemplateActionResult : ActionResult
    {
        private readonly string _actionName;
        private readonly string _pageTemplate;

        public PageTemplateActionResult(string pageTemplate = null, string actionName = null)
        {
            _actionName = actionName;
            _pageTemplate = "PageTemplates/" + (pageTemplate ?? "Default");
        }

        public override void ExecuteResult(ControllerContext context)
        {
            new ViewResult
                {
                    MasterName = _pageTemplate,
                    ViewData = context.Controller.ViewData,
                    ViewName = _actionName,
                    TempData = context.Controller.TempData,
                }.ExecuteResult(context);
        }
    }
}