using System.Web.Mvc;

namespace IdentityProvider.Infrastructure.SweetAlert
{
    public class AlertDecoratorResult : ActionResult
    {
        public AlertDecoratorResult(ActionResult innerResult, string command, string message)
        {
            InnerResult = innerResult;
            Command = command;
            Message = message;
        }

        public ActionResult InnerResult { get; set; }
        public string Command { get; set; }
        public string Message { get; set; }

        public override void ExecuteResult(ControllerContext context)
        {
            var alerts = context.Controller.TempData.GetAlerts();
            alerts.Add(new Alert(Command, Message));
            InnerResult.ExecuteResult(context);
        }
    }
}