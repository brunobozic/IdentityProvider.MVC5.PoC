using IdentityProvider.Repository.EFCore.EFDataContext;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using IdentityProvider.Repository.EFCore.Domain.Roles;

public class ApplicationRoleManager : RoleManager<AppRole>
{
    private readonly AppDbContext _context;

    public ApplicationRoleManager(IRoleStore<AppRole> roleStore, AppDbContext context) : base(roleStore)
    {
        _context = context;
    }

}