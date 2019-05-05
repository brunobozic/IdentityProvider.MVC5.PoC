using System;
using IdentityProvider.Models.Domain.Account;

namespace IdentityProvider.Models.ViewModels.Resources.Extensions
{
    public static class ResourceExtension
    {
        public static ResourceDto ConvertToViewModel( this ApplicationResource resource )
        {
            var vm = new ResourceDto
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
               
            };

            return vm;
        }
    }
}
