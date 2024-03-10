using System;

namespace IdentityProvider.Repository.EFCore
{
    public class InternalCommand
    {
        public Guid Id { get; set; }

        public string Type { get; set; }

        public string Data { get; set; }

        public DateTime? ProcessedDate { get; set; }
        public DateTime? EnqueueDate { get; set; }
    }
}
