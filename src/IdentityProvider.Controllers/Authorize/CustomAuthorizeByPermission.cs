﻿using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using IdentityProvider.Models.Domain.Account;

namespace IdentityProvider.Controllers.Authorize
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class CustomAuthorizeByPermission : AuthorizeAttribute
    {
        private Permission _permission;

        public CustomAuthorizeByPermission(Permission permission)
        {
            _permission = permission;
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            // Get the current claims principal
            var prinicpal = (ClaimsPrincipal) Thread.CurrentPrincipal;
            // Make sure they are authenticated
            if (!prinicpal.Identity.IsAuthenticated)
                return false;
            // Get the roles from the claims
            var roles = prinicpal.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToArray();
            //Check if they are authorized
            // return PermissionService.Authorize(_permission , roles);

            return true;
        }
    }
}