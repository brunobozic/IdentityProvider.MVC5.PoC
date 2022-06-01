using IdentityProvider.Repository.EFCore.Domain.ResourceOperations;
using Module.CrossCutting.Models.ViewModels.Operations;
using System;

namespace IdentityProvider.ServiceLayer.Services.OperationsService
{
    public static class OperationExtension
    {
        public static OperationDto ConvertToViewModel(this Operation operation)
        {
            var vm = new OperationDto
            {
                Deleted = operation?.IsDeleted ?? false,
                Active = operation?.Active ?? false,
                ActiveFrom = operation?.ActiveFrom ?? DateTime.MinValue,
                ActiveTo = operation?.ActiveFrom ?? DateTime.MinValue,
                CreatedBy = operation?.CreatedById ?? string.Empty,
                DeletedBy = operation?.DeletedById ?? string.Empty,
                ModifiedBy = operation?.ModifiedById ?? string.Empty,
                DateCreated = operation?.CreatedDate ?? DateTime.MinValue,
                DateModified = operation?.ModifiedDate ?? DateTime.MinValue,
                DateDeleted = operation?.DeletedDate ?? DateTime.MinValue,
                Description = operation?.Description ?? string.Empty,
                Id = operation?.Id,
                Name = operation.Name ?? string.Empty,
                UserMayViewCreatedProp = true,
                UserMayViewDeletedProp = true,
                UserMayViewLastModifieddProp = true,
                RowVersion = operation.RowVersion
            };

            return vm;
        }
    }
}