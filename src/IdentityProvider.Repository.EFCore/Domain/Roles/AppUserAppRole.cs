﻿using IdentityProvider.Repository.EFCore.Domain.Account;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityProvider.Repository.EFCore.Domain.Roles
{
    public class AppUserAppRole: IdentityUserRole<string>
    {
      [Key]
        public int Id { get; set; }
        public virtual ApplicationUser User { get; set; }
        public virtual AppRole Role { get; set; }
        public string UserId { get; set; }
        public string RoleId { get; set; }
    }
}
