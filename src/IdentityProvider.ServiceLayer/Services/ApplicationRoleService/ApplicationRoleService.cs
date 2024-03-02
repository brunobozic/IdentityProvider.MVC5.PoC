using AutoMapper;
using IdentityProvider.Repository.EFCore.Domain.Roles;
using IdentityProvider.ServiceLayer.Services.ApplicationRoleService;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TrackableEntities.Common.Core;
using URF.Core.Abstractions;
using URF.Core.Abstractions.Trackable;
using URF.Core.Services;

public class ApplicationRoleService : Service<AppRole>, IRoleService
{
    #region Private Props

    private readonly IMapper _mapper;
    private readonly RoleManager<AppRole> _roleManager;
    private readonly IUnitOfWork _unitOfWorkAsync;

    #endregion Private Props

    #region Ctor

    public ApplicationRoleService(
        IUnitOfWork unitOfWorkAsync
        , IMapper mapper
        , ITrackableRepository<AppRole> repository
        , RoleManager<AppRole> roleManager

    ) : base(repository)
    {
        _unitOfWorkAsync = unitOfWorkAsync;
        _mapper = mapper;
        _roleManager = roleManager;

    }

    #endregion Ctor

    #region CRUD

    public Task<IdentityResult> AddRoleAsync(string roleName, string optionalDescription, bool startAsNonActive = false)
    {
        return _roleManager.CreateAsync(new AppRole(roleName)
        {
            Name = roleName,
            Description = optionalDescription,
            Active = !startAsNonActive,
            ActiveFrom = DateTime.Now,
            TrackingState = TrackingState.Added
        });
    }

    public async Task<bool> ExistsAsync(string roleName)
    {
        return await _roleManager.RoleExistsAsync(roleName);
    }


    public async Task<AppRole> FetchRoleAsync(string byRoleName)
    {
        return await _roleManager.FindByNameAsync(byRoleName);
    }

    /// <summary>
    /// Get all active application roles.
    /// </summary>
    /// <returns></returns>
    public async Task<List<AppRole>> GetList()
    {
        return await Queryable().Where(i => i.Active).ToListAsync();
    }

    /// <summary>
    /// Get all deactivated application roles.
    /// </summary>
    /// <returns></returns>
    public async Task<List<AppRole>> GetListOfDeactivated()
    {
        return await Queryable().Where(i => i.Active == false).ToListAsync();
    }

    #endregion CRUD
}