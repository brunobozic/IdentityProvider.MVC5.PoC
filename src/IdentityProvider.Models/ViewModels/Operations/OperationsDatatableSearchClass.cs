using System;

namespace IdentityProvider.Models
{
    public class OperationsDatatableSearchClass
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Active { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string Actions { get; set; }
        public bool Deleted { get; set; }
    }
}