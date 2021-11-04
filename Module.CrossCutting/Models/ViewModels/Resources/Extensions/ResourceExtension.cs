using System;
using IdentityProvider.Models.Domain.Account;

namespace IdentityProvider.Models.ViewModels.Resources.Extensions
{
    public static class ResourceExtension
    {
        public static ResourceDto ConvertToViewModel(this ApplicationResource resource)
        {
            var vm = new ResourceDto
            {
                Deleted = resource?.IsDeleted ?? false,
                Active = resource?.Active ?? false,
                CreatedBy = resource?.CreatedById ?? string.Empty,
                DeletedBy = resource?.DeletedById ?? string.Empty,
                ModifiedBy = resource?.ModifiedById ?? string.Empty,
                DateCreated = resource?.CreatedDate ?? DateTime.MinValue,
                DateModified = resource?.ModifiedDate ?? DateTime.MinValue,
                DateDeleted = resource?.DeletedDate ?? DateTime.MinValue,
                Description = resource?.Description ?? string.Empty,
                Id = resource?.Id,
                Name = resource.Name ?? string.Empty,
                UserMayViewCreatedProp = true,
                UserMayViewDeletedProp = true,
                UserMayViewLastModifieddProp = true
            };

            return vm;
        }
    }
}