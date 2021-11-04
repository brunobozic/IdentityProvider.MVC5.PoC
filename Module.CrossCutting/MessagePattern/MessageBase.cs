namespace IdentityProvider.Infrastructure.MessagePattern
{
    public abstract class MessageBase
    {
        protected MessageBase()
        {
            Success = false;
        }

        public int? TotalCountForPager { get; set; }
        public bool Success { get; set; }

        public string ErrorMessage { get; set; } = string.Empty;

        public long? TimeTaken { get; set; } = -1;
    }
}