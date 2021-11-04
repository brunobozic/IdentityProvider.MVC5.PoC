﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using IdentityProvider.Models.Domain.Account;
using IdentityProvider.Repository.EF.EFDataContext;
using IdentityProvider.Repository.EF.Queries.UserRolesResourcesOperations.RoleOperationResource;
using IdentityProvider.Repository.EF.Queries.UserRolesResourcesOperations.UserRoleResourcesOperations;
using Microsoft.AspNet.Identity;
using StructureMap;
using TrackableEntities;
using URF.Core.Services;

namespace IdentityProvider.Services.ApplicationRoleService
{
    public class ApplicationRoleService : Service<ApplicationRole>, IApplicationRoleService
    {
        // private readonly ILog4NetLoggingService _loggingService;
        private readonly IMapper _mapper;
        private readonly ApplicationRoleManager _roleManager;
        private readonly ApplicationSignInManager _signInManager;
        private readonly IUnitOfWorkAsync _unitOfWorkAsync;
        private readonly ApplicationUserManager _userManager;

        [DefaultConstructor] // This is the attribute you need to add on the constructor
        public ApplicationRoleService(
            IUnitOfWorkAsync unitOfWorkAsync
            //, ILog4NetLoggingService loggingService
            , IMapper mapper
            , IRepositoryAsync<ApplicationRole> repository
            , ApplicationSignInManager signInManager
            , ApplicationUserManager userManager
            , ApplicationRoleManager roleManager
        ) : base(repository)
        {
            _unitOfWorkAsync = unitOfWorkAsync;
           // _loggingService = loggingService;
            _mapper = mapper;
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public Task<IdentityResult> AddRoleAsync(string roleName, string optionalDescription,
            bool startAsNonActive = false)
        {
            return _roleManager.CreateAsync(new ApplicationRole(roleName)
            {
                Name = roleName,
                Description = optionalDescription,
                Active = !startAsNonActive,
                ActiveFrom = DateTime.Now,
                TrackingState = TrackingState.Added
            });
        }

 



        public bool Exists(string roleName)
        {
            return _roleManager.RoleExists(roleName);
        }


        public ApplicationRole FetchRole(string byRoleName)
        {
            return _roleManager.FindByName(byRoleName);
        }

        /// <summary>
        ///     Get all active application roles.
        /// </summary>
        /// <returns></returns>
        public async Task<List<ApplicationRole>> GetList()
        {
            return await Queryable().Where(i => i.Active).ToListAsync();
        }

        /// <summary>
        ///     Get all deactivated application roles.
        /// </summary>
        /// <returns></returns>
        public async Task<List<ApplicationRole>> GetListOfDeactivated()
        {
            return await Queryable().Where(i => i.Active == false).ToListAsync();
        }

   
    }
}