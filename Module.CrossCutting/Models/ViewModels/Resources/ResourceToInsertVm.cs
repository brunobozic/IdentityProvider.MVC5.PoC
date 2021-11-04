using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace IdentityProvider.Models.ViewModels.Resources
{
    public class ResourceToInsertVm
    {
        public ResourceToInsertVm()
        {
            MakeActive = false;
        }

        [Required(ErrorMessage = "You need to set the item as either active or inactive.")]
        [StringLength(150, MinimumLength = 1,
            ErrorMessage = "The number of characters in the name may not exceed 150. ")]
        public string Name { get; set; }

        [Required(ErrorMessage = "An operation description is required")]
        [StringLength(150, MinimumLength = 1,
            ErrorMessage = "The number of characters in the description may not exceed 150. ")]
        public string Description { get; set; }

        [DisplayName("The item will be made active (default: active). ")]
        public bool MakeActive { get; set; }

        [DisplayName("(Optional) The date when the item becomes inactive.")]
        public DateTime? ActiveUntil { get; set; }
    }
}