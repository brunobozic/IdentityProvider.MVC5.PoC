namespace Module.CrossCutting.Models.ViewModels.Permissions
{
    public class PermissionGroupDto
    {
        public string Name { get; set; }
        public List<PermissionDto> Permissions { get; set; }
    }
}