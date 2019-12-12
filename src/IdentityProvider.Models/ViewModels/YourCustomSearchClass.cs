using System;

namespace IdentityProvider.Models
{
    public class YourCustomSearchClass
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string TableName { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string OldData { get; set; }
        public string NewData { get; set; }
        public string Action { get; set; }
        public long? TableId { get; set; }
        public string UserName { get; set; }
    }
}