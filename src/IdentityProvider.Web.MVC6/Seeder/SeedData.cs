using IdentityProvider.Repository.EFCore.Domain.Account;
using IdentityProvider.Repository.EFCore.Domain.Account.Employees;
using IdentityProvider.Repository.EFCore.Domain.OrganizationalUnits;
using IdentityProvider.Repository.EFCore.Domain.Permissions;
using IdentityProvider.Repository.EFCore.Domain.ResourceOperations;
using IdentityProvider.Repository.EFCore.Domain.Roles;
using IdentityProvider.Repository.EFCore.EFDataContext;
using IdentityProvider.Web.MVC6.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrackableEntities.Common.Core;

public class SeedData
{
    public static async Task Initialize(AppDbContext dbContext, IServiceScope scope, string testUserPw)
    {
        try
        {
            await SeedOrganizationUnits(dbContext);
            var numSaved0 = dbContext.SaveChanges();

            await EnsureRoles(scope);
            await EnsureUsers(scope, testUserPw);
            await AddRolesToUsers(scope, testUserPw);
            var numSaved = dbContext.SaveChanges();

            await EnsureEmployees(scope, testUserPw);
            var numSaved2 = dbContext.SaveChanges();

            await AttachEmployeeToOrganizationUnitByName(dbContext, "Bruno", "Bozic", "IT Department");

            var numSaved3 = dbContext.SaveChanges();

            await SeedOperationsAndResources(dbContext);

            var numSaved4 = dbContext.SaveChanges();

            await SeedPermissionsAndRoles(scope, dbContext); // Seed permissions and roles
             
            var numSaved5 = dbContext.SaveChanges();

            // await SeedEmployeeRoles(dbContext); // Seed employee-role relationship
            // var numSaved5 = dbContext.SaveChanges();
        }
        catch (Exception seeedingException)
        {
            throw;
        }

    }

    // EnsureRoles method
    private static async Task EnsureRoles(IServiceScope scope)
    {
        var roleManager = scope.ServiceProvider.GetService<RoleManager<AppRole>>();

        if (roleManager == null)
        {
            throw new Exception("roleManager null");
        }

        var roles = new List<string>
        {
            IdentityHelpingConstants.SuperUser,
            IdentityHelpingConstants.StandardRole,
            IdentityHelpingConstants.ContactManagersRole,
            IdentityHelpingConstants.ContactAdministratorsRole
        };

        foreach (var roleName in roles)
        {
            var roleExists = await roleManager.RoleExistsAsync(roleName);
            if (!roleExists)
            {
                var role = new AppRole(roleName)
                {
                    Name = roleName
                };

                var identityResult = await roleManager.CreateAsync(role);

                if (!identityResult.Succeeded)
                {
                    throw new Exception($"Failed to create role: {roleName}");
                }
            }
        }
    }
    public static async Task SeedEmployeeRoles(AppDbContext dbContext)
    {
        var employees = await dbContext.Employee.ToListAsync();
        var roles = await dbContext.Role.ToListAsync();

        // Assuming each employee is assigned all roles
        foreach (var employee in employees)
        {
            var employeeRoles = roles.Select(role => new EmployeeOwnsRoles
            {
                EmployeeId = employee.Id,
                RoleId = role.Id
            }).ToList();

            dbContext.EmployeeOwnsRoles.AddRange(employeeRoles);
        }

        await dbContext.SaveChangesAsync();
    }
    // EnsureUsers method
    private static async Task EnsureUsers(IServiceScope scope, string testUserPw)
    {
        var userManager = scope.ServiceProvider.GetService<UserManager<ApplicationUser>>();

        if (userManager == null)
        {
            throw new Exception("userManager null");
        }

        var users = new List<(string FirstName, string LastName, string Email)>
        {
            ("Bruno", "Bozic", "bruno.bozic@gmail.com"),
            // Add other user data here
        };

        foreach (var (firstName, lastName, email) in users)
        {
            var user = await userManager.FindByEmailAsync(email);

            if (user == null)
            {
                user = new ApplicationUser
                {
                    UserName = email,
                    Email = email,
                    EmailConfirmed = true,
                    Id = Guid.NewGuid(),
                    PasswordHash = "hfhfhfhf",
                    FirstName = firstName,
                    LastName = lastName,
                    Active = true,
                    ActiveFrom = DateTime.Now,
                    TrackingState = TrackingState.Added
                };

                var identityResult = await userManager.CreateAsync(user, testUserPw);
                if (!identityResult.Succeeded)
                {
                    throw new Exception($"Failed to create user: {email}");
                }
            }
        }
    }

    private static async Task AddRolesToUsers(IServiceScope scope, string testUserPw)
    {
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<AppRole>>();

        // Check if user manager and role manager are available
        if (userManager == null || roleManager == null)
        {
            throw new Exception("UserManager or RoleManager is null");
        }

        // Retrieve other roles
        var roles = new List<string>
        {
            IdentityHelpingConstants.StandardRole,
            IdentityHelpingConstants.ContactManagersRole,
            IdentityHelpingConstants.ContactAdministratorsRole
        };

        // Iterate over each role
        foreach (var roleName in roles)
        {
            // Check if the role exists
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                throw new Exception($"Role {roleName} does not exist");
            }

            // Get all users
            var users = await userManager.Users.ToListAsync();

            // Assign roles to users who don't have the role already
            foreach (var user in users)
            {
                // Check if the user already has the role
                var isInRole = await userManager.IsInRoleAsync(user, roleName);
                if (!isInRole)
                {
                    var identityResult = await userManager.AddToRoleAsync(user, roleName);

                    if (!identityResult.Succeeded)
                    {
                        throw new Exception($"Failed to assign role {roleName} to user {user.Email}");
                    }
                }
            }
        }
    }


    private static async Task EnsureEmployees(IServiceScope scope, string testUserPw)
    {
        var userManager = scope.ServiceProvider.GetService<UserManager<ApplicationUser>>();
        var dbContext = scope.ServiceProvider.GetService<AppDbContext>();

        if (userManager == null || dbContext == null)
        {
            throw new Exception("userManager or dbContext null");
        }

        var users = await userManager.Users.ToListAsync();

        foreach (var user in users)
        {
            var existingEmployee = await dbContext.Employee.FirstOrDefaultAsync(e => e.ApplicationUserId == user.Id);
            if (existingEmployee == null)
            {
                dbContext.Employee.Add(new Employee
                {
                    Active = true,
                    IsDeleted = false,
                    Name = user.FirstName,
                    Surname = user.LastName,
                    CreatedById = user.Id,
                    ApplicationUserId = user.Id,
                    ActiveFrom = DateTime.UtcNow,
                    ActiveTo = DateTime.UtcNow.AddYears(3)
                });
            }
        }

        var numSave = dbContext.SaveChanges();

        if (numSave <= 0)
        {
            throw new Exception("Employees not saved");
        }
    }


    public static async Task SeedOrganizationUnits(AppDbContext dbContext)
    {
        // Seed Organization Units
        var organizationUnits = new List<OrganizationalUnit>
        {
            new() { Name = "IT Department", Description = "Information Technology Department" },
            new() { Name = "HR Department", Description = "Human Resources Department" },
            new() { Name = "Finance Department", Description = "Finance Department" },
            new() { Name = "Marketing Department", Description = "Marketing Department" },
            new() { Name = "Operations Department", Description = "Operations Department" }
        };

        dbContext.OrganisationalUnit.AddRange(organizationUnits);
        var numSave = dbContext.SaveChanges();

        if (numSave <= 0)
        {
            throw new Exception("Organization units not saved");
        }
    }

    public static async Task AttachEmployeeToOrganizationUnitByName(AppDbContext dbContext, string firstName, string lastName, string organizationalUnitName)
    {
        var employee = await dbContext.Employee.FirstOrDefaultAsync(e => e.Name == firstName && e.Surname == lastName);
        var organizationalUnit = await dbContext.OrganisationalUnit.FirstOrDefaultAsync(ou => ou.Name == organizationalUnitName);

        if (employee != null && organizationalUnit != null)
        {
            var existingRelation = await dbContext.EmployeesBelongToOgranizationalUnits
                .FirstOrDefaultAsync(e => e.EmployeeId == employee.Id && e.OrganizationalUnitId == organizationalUnit.Id);

            if (existingRelation == null)
            {
                var employeeOrgUnit = new EmployeeBelongsToOrgUnit
                {
                    EmployeeId = employee.Id,
                    OrganizationalUnitId = organizationalUnit.Id,
                    Active = true,
                    ActiveFrom = DateTime.UtcNow
                };

                dbContext.EmployeesBelongToOgranizationalUnits.Add(employeeOrgUnit);
                var numSave = dbContext.SaveChanges();

                if (numSave <= 0)
                {
                    throw new Exception("Employee organization unit not saved");
                }
            }
            // else: relation already exists, no need to add again
        }
        else
        {
            throw new Exception("Employee or organizational unit not found");
        }
    }


    public static async Task SeedOperationsAndResources(AppDbContext dbContext)
    {
        // Seed Operations
        var operations = new List<Operation>
        {
            new() { Name = "Read" },
            new() { Name = "Delete" },
            new() { Name = "Create" },
            new() { Name = "Edit" }
        };

        dbContext.Operation.AddRange(operations);

        var resources = new List<Resource>
        {
            new() { Name = "HomeController/Index" },
            new() { Name = "UserController/Create" },
            new() { Name = "ProductController/Details" },
            new() { Name = "OrderController/View" },
            new() { Name = "AdminController/Manage" },
            new() { Name = "ReportController/Generate" }

        };


        dbContext.ApplicationResource.AddRange(resources);
        var numSave = dbContext.SaveChanges();

        if (numSave <= 0)
        {
            throw new Exception("Operations and resources not saved");
        }
    }

    public static async Task SeedPermissionsAndRoles(IServiceScope scope, AppDbContext dbContext)
    {
        // Retrieve existing resources and operations from the database
        var resources = await dbContext.ApplicationResource.ToListAsync();
        var operations = await dbContext.Operation.ToListAsync();

        if (!resources.Any() || !operations.Any())
        {
            throw new Exception("Resources or operations not found in the database.");
        }

        // Create permissions by associating each resource with each operation
        var permissions = new List<Permission>();
        foreach (var resource in resources)
        {
            foreach (var operation in operations)
            {
                // Construct permission name based on resource and operation
                var permissionName = $"Can{operation.Name}{resource.Name.Replace("/", "")}";
                var permissionDescription = $"Permission to {operation.Name} {resource.Name}";

                permissions.Add(new Permission
                {
                    Name = permissionName,
                    Description = permissionDescription,
                    ResourceId = resource.Id,
                    OperationId = operation.Id,
                    Active = true,
                    ActiveFrom = DateTime.UtcNow
                });
            }
        }

        dbContext.Permission.AddRange(permissions);
        var numSave = dbContext.SaveChanges();

        if (numSave <= 0)
        {
            throw new Exception("Permissions not saved");
        }

        // Define roles corresponding to these permissions
        var rolePermissions = new Dictionary<string, List<string>>
        {
            { "StandardRole", permissions.Select(p => p.Name).ToList() }, // Assign all permissions to StandardRole
            { "AdminRole", new List<string> { "CanCreateUserController", "CanViewOrderController" } },
            // Add more roles and their associated permissions as needed
        };

        // Get role manager
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<AppRole>>();

        foreach (var (roleName, permissionNames) in rolePermissions)
        {
            // Check if the role exists
            var role = await roleManager.FindByNameAsync(roleName);
            if (role == null)
            {
                throw new Exception($"Role {roleName} not found");
            }

            // Assign permissions to the role
            foreach (var permissionName in permissionNames)
            {
                var permission = await dbContext.Permission.FirstOrDefaultAsync(p => p.Name == permissionName);
                if (permission == null)
                {
                    throw new Exception($"Permission {permissionName} not found");
                }

                // Create a mapping between the role and the permission
                dbContext.RoleContainsPermissions.Add(new RoleContainsPermissions
                {
                    RoleId = role.Id,
                    PermissionId = permission.Id,
                    Active = true,
                    ActiveFrom = DateTime.UtcNow
                });
            }
        }

        // Save changes
        numSave = dbContext.SaveChanges();

        if (numSave <= 0)
        {
            throw new Exception("Role permissions not saved");
        }
    }

}
