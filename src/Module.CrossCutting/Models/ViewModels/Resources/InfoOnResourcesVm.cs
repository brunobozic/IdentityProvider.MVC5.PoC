namespace Module.CrossCutting.Models.ViewModels.Resources
{
    public class InfoOnResourcesVm
    {
        public int ActiveItemCount { get; set; }
        public int DeletedItemCount { get; set; }
        public int InactiveItemCount { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}