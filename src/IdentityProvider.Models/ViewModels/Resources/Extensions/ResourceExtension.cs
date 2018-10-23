using System;
using IdentityProvider.Models.Domain.Account;
using IdentityProvider.Models.ViewModels.Operations;

namespace IdentityProvider.Models.ViewModels.Resources.Extensions
{
    public static class ResourceExtension
    {
        public static OperationVm ConvertToViewModel( this ApplicationResource resource )
        {
            var vm = new OperationVm
            {
                Operation = new OperationDto
                {

                    Deleted = resource?.IsDeleted ?? false ,
                    Active = resource?.Active ?? false ,
                    CreatedBy = resource?.CreatedById ?? "" ,
                    DeletedBy = resource?.DeletedById ?? "" ,
                    ModifiedBy = resource?.ModifiedById ?? "" ,
                    DateCreated = resource?.CreatedDate ?? DateTime.MinValue ,
                    DateModified = resource?.ModifiedDate ?? DateTime.MinValue ,
                    DateDeleted = resource?.DeletedDate ?? DateTime.MinValue ,
                    Description = resource?.Description ?? "" ,
                    Id = resource?.Id ,
                    Name = resource.Name ?? "" ,
                    UserMayViewCreatedProp = true ,
                    UserMayViewDeletedProp = true ,
                    UserMayViewLastModifieddProp = true

                }
            };

            return vm;
        }
    }
}
