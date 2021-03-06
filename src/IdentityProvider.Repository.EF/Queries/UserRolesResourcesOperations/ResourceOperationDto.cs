﻿using IdentityProvider.Models.Domain.Account;
using System.Collections.Generic;

namespace IdentityProvider.Repository.EF.Queries.UserRolesResourcesOperations
{
    public class ResourceOperationDto
    {
        public Operation Operation { get; set; }
        public string ResourceName { get; set; }
        public string ResourceDescription { get; set; }
        public bool ResourceActive { get; set; }
        public ICollection<Operation> Operations { get; set; }
        public bool ResourceDeleted { get; set; }
    }
}
