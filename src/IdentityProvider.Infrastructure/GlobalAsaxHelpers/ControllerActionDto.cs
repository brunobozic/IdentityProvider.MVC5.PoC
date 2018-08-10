namespace IdentityProvider.Infrastructure.GlobalAsaxHelpers
{
    public class ControllerActionDto
    {
        public string CurrentController { get; internal set; }
        public string CurrentAction { get; internal set; }
    }
}