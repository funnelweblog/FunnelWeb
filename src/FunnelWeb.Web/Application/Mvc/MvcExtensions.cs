using System.Web.Mvc;

namespace FunnelWeb.Web.Application.Mvc
{
    public static class MvcExtensions
    {
        public static FlashResult<TResult> AndFlash<TResult>(this TResult result, string messageFormat, params object[] args) where TResult : ActionResult
        {
            return new FlashResult<TResult>(result, string.Format(messageFormat, args));
        }

        public class FlashResult<TResult> : ActionResult where TResult : ActionResult
        {
            private readonly TResult result;
            private readonly string message;

            public FlashResult(TResult result, string message)
            {
                this.result = result;
                this.message = message;
            }

            public override void ExecuteResult(ControllerContext context)
            {
                context.Controller.TempData["Flash"] = message;
                result.ExecuteResult(context);
            }
        }
    }
}