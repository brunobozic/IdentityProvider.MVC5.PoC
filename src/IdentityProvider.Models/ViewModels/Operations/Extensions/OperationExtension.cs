using System;
using IdentityProvider.Models.Domain.Account;

namespace IdentityProvider.Models.ViewModels.Operations.Extensions
{
    public static class OperationExtension
    {
        public static OperationDto ConvertToViewModel( this Operation operation )
        {
            var vm = new OperationDto
            {
                Deleted = operation?.IsDeleted ?? false ,
                Active = operation?.Active ?? false ,
                ActiveFrom = operation?.ActiveFrom ?? DateTime.MinValue ,
                ActiveTo = operation?.ActiveFrom ?? DateTime.MinValue ,
                CreatedBy = operation?.CreatedById ?? "" ,
                DeletedBy = operation?.DeletedById ?? "" ,
                ModifiedBy = operation?.ModifiedById ?? "" ,
                DateCreated = operation?.CreatedDate ?? DateTime.MinValue ,
                DateModified = operation?.ModifiedDate ?? DateTime.MinValue ,
                DateDeleted = operation?.DeletedDate ?? DateTime.MinValue ,
                Description = operation?.Description ?? "" ,
                Id = operation?.Id ,
                Name = operation.Name ?? "" ,
                UserMayViewCreatedProp = true ,
                UserMayViewDeletedProp = true ,
                UserMayViewLastModifieddProp = true
            };

            return vm;
        }

    }
}
