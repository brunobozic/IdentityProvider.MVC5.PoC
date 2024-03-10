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
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class SeedData
{
    public static async Task Initialize(IServiceProvider serviceProvider, string testUserPw)
    {
        using var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<AppRole>>();

        await SeedOrganizationUnits(dbContext);
        await EnsureRoles(scope);
        await EnsureUsers(serviceProvider, testUserPw);
        await AddRolesToUsers(serviceProvider, testUserPw);
        await EnsureEmployees(serviceProvider, testUserPw);
        await AttachEmployeeToOrganizationUnitByName(dbContext, "Bruno", "Božić", "IT Department");
        await AttachEmployeeToOrganizationUnitByName(dbContext, "Goran", "Goranić", "IT Department");
        await AttachEmployeeToOrganizationUnitByName(dbContext, "Ivan", "Ivić", "IT Department");
        await SeedOperationsAndResources(dbContext);
        await SeedPermissionsAndRoles(scope, dbContext);
    }

    private static async Task EnsureRoles(IServiceScope scope)
    {
        // Use GetRequiredService instead of GetService to automatically throw if service is not found
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<AppRole>>();

        var roles = new List<string>
        {
            IdentityHelpingConstants.SuperUser,
            IdentityHelpingConstants.StandardRole,
            IdentityHelpingConstants.ContactManagersRole,
            IdentityHelpingConstants.ContactAdministratorsRole,
            IdentityHelpingConstants.ReadOnlyUserRole
        };

        foreach (var roleName in roles)
        {
            // Simplify the check and creation process
            var roleExists = await roleManager.RoleExistsAsync(roleName);
            if (!roleExists)
            {
                // Directly instantiate AppRole within the method call for brevity
                var result = await roleManager.CreateAsync(new AppRole { Name = roleName });
                if (!result.Succeeded)
                {
                    // Utilize string interpolation for clearer exception message
                    throw new InvalidOperationException($"Failed to create role: {roleName}. Errors: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                }
            }
        }
    }

    public static async Task SeedEmployeeRoles(AppDbContext dbContext)
    {
        var employees = await dbContext.Employee.ToListAsync();
        var roles = await dbContext.Role.ToListAsync();

        // Check if the EmployeeOwnsRoles already contains specific mappings to avoid duplicates
        var existingMappings = await dbContext.EmployeeOwnsRoles.ToListAsync();
        var newMappings = new List<EmployeeOwnsRoles>();

        foreach (var employee in employees)
        {
            foreach (var role in roles)
            {
                // Check if the mapping already exists
                bool mappingExists = existingMappings.Any(x => x.EmployeeId == employee.Id && x.RoleId == role.Id);
                if (!mappingExists)
                {
                    newMappings.Add(new EmployeeOwnsRoles
                    {
                        EmployeeId = employee.Id,
                        RoleId = role.Id
                    });
                }
            }
        }

        if (newMappings.Any())
        {
            await dbContext.EmployeeOwnsRoles.AddRangeAsync(newMappings);
            var numSaved = dbContext.SaveChanges();
        }
    }

    private static async Task EnsureUsers(IServiceProvider serviceProvider, string testUserPw)
    {
        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        var users = new List<(string FirstName, string LastName, string Email)>
        {
            ("Bruno", "Božić", "bruno.bozic@gmail.com"),
            ("Ivan", "Ivić", "ivan.ivic@gmail.com"),
            ("Goran", "Goranić", "goran.goranic@gmail.com"),
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
                    // Avoid setting Id manually if it's auto-generated
                    FirstName = firstName,
                    LastName = lastName,
                    Active = true,
                    ActiveFrom = DateTime.UtcNow, // Use DateTime.UtcNow for consistency in global applications
                                                  // TrackingState = TrackingState.Added // Consider removing if not explicitly used or managed by EF
                };

                var identityResult = await userManager.CreateAsync(user, testUserPw);

                if (!identityResult.Succeeded)
                {
                    // Consider logging the error details for better debugging
                    throw new Exception($"Failed to create user: {email}. Errors: {string.Join(", ", identityResult.Errors.Select(e => e.Description))}");
                }
            }
        }
    }

    private static async Task AddRolesToUsers(IServiceProvider serviceProvider, string testUserPw)
    {
        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = serviceProvider.GetRequiredService<RoleManager<AppRole>>();

        // Assuming roles are predefined and exist.
        var roles = new List<string>
        {
            IdentityHelpingConstants.StandardRole,
            IdentityHelpingConstants.ContactManagersRole,
            IdentityHelpingConstants.ContactAdministratorsRole
        };

        // Optimize by retrieving all users at once rather than in each iteration.
        var users = await userManager.Users.ToListAsync();

        foreach (var roleName in roles)
        {
            // This check ensures the role exists without throwing an exception, 
            // which would be more graceful than interrupting the flow.
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                // Consider logging this situation instead of throwing an exception.
                Log.Warning($"Role {roleName} does not exist and will be skipped.");
                continue;
            }

            foreach (var user in users)
            {
                // Check and assign roles more efficiently.
                if (!await userManager.IsInRoleAsync(user, roleName))
                {
                    var result = await userManager.AddToRoleAsync(user, roleName);
                    if (!result.Succeeded)
                    {
                        // Consider logging this issue rather than throwing, to allow the process to continue.
                        Log.Error($"Failed to assign role {roleName} to user {user.Email}: {result.Errors.FirstOrDefault()?.Description}");
                    }
                }
            }
        }
    }

    private static async Task EnsureEmployees(IServiceProvider serviceProvider, string testUserPw)
    {
        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var dbContext = serviceProvider.GetRequiredService<AppDbContext>();

        // No need for null checks after GetRequiredService, it throws if service not found

        var users = await userManager.Users.ToListAsync();
        var employeeUserIds = new HashSet<Guid>(await dbContext.Employee
                                                    .Select(e => e.ApplicationUserId)
                                                    .ToListAsync());

        var employeesToAdd = new List<Employee>();

        foreach (var user in users.Where(u => !employeeUserIds.Contains(u.Id)))
        {
            employeesToAdd.Add(new Employee
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

        if (employeesToAdd.Any())
        {
            await dbContext.Employee.AddRangeAsync(employeesToAdd);
            var numSaved = dbContext.SaveChanges();
        }
    }

    public static async Task SeedOrganizationUnits(AppDbContext dbContext)
    {
        // Prepare a list of organization units to seed.
        var organizationUnits = new List<OrganizationalUnit>
        {
            new() { Name = "IT Department", Description = "Information Technology Department" },
            new() { Name = "HR Department", Description = "Human Resources Department" },
            new() { Name = "Finance Department", Description = "Finance Department" },
            new() { Name = "Marketing Department", Description = "Marketing Department" },
            new() { Name = "Operations Department", Description = "Operations Department" }
        };

        // Fetch all existing units once to reduce database calls.
        var existingUnitsNames = new HashSet<string>(await dbContext.OrganisationalUnit
            .Select(ou => ou.Name).ToListAsync());

        // Filter out the units that already exist to prevent duplicate entries.
        var unitsToAdd = organizationUnits.Where(ou => !existingUnitsNames.Contains(ou.Name));

        // Add new units to the database.
        await dbContext.OrganisationalUnit.AddRangeAsync(unitsToAdd);

        // Save changes to the database.
        var numSaved = dbContext.SaveChanges();
    }

    public static async Task AttachEmployeeToOrganizationUnitByName(AppDbContext dbContext, string firstName, string lastName, string organizationalUnitName)
    {
        // Normalize input to improve matching reliability.
        var normalizedFirstName = firstName.ToUpper().Trim();
        var normalizedLastName = lastName.ToUpper().Trim();
        var normalizedUnitName = organizationalUnitName.Trim();

        var employee = await dbContext.Employee.FirstOrDefaultAsync(e => e.Name.ToUpper().Trim() == normalizedFirstName && e.Surname.ToUpper().Trim() == normalizedLastName);
        if (employee == null)
        {
            throw new Exception($"Employee with name {firstName} {lastName} not found.");
        }

        var organizationalUnit = await dbContext.OrganisationalUnit.FirstOrDefaultAsync(ou => ou.Name.Trim() == normalizedUnitName);
        if (organizationalUnit == null)
        {
            throw new Exception($"Organizational unit with name {organizationalUnitName} not found.");
        }

        bool existingRelation = await dbContext.EmployeesBelongToOgranizationalUnits.AnyAsync(e => e.EmployeeId == employee.Id && e.OrganizationalUnitId == organizationalUnit.Id);
        if (!existingRelation)
        {
            var employeeOrgUnit = new EmployeeBelongsToOrgUnit
            {
                EmployeeId = employee.Id,
                OrganizationalUnitId = organizationalUnit.Id,
                Active = true,
                ActiveFrom = DateTime.UtcNow
            };

            await dbContext.EmployeesBelongToOgranizationalUnits.AddAsync(employeeOrgUnit);
            var numSaved = dbContext.SaveChanges();

            if (numSaved <= 0)
            {
                // Consider logging this condition instead of throwing an exception if it's not critical.
                throw new Exception("Failed to save the employee to organization unit relationship.");
            }
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

        foreach (var operation in operations)
        {
            // Check if the operation already exists in the database
            var existingOperation = await dbContext.Operation.FirstOrDefaultAsync(o => o.Name == operation.Name);

            if (existingOperation == null)
            {
                // Add the operation to the database if it doesn't exist
                dbContext.Operation.Add(operation);
            }
        }

        // Seed Resources
        var resources = new List<Resource>
        {
            new() { Name = "HomeController/Index" },
            new() { Name = "UserController/Create" },
            new() { Name = "ProductController/Details" },
            new() { Name = "OrderController/View" },
            new() { Name = "AdminController/Manage" },
            new() { Name = "ReportController/Generate" },
            new() { Name = "OperationController/OperationsGetAll" },
            new() { Name = "OperationController/Edit" },
            new() { Name = "OperationController/Insert" },
            new() { Name = "OperationController/FetchInfoOnOperations" },
            new() { Name = "OperationController/Delete" },
            new() { Name = "OperationController/Detail" },
            new() { Name = "ResurceController/ResurcesGetAll" },
            new() { Name = "ResurceController/Edit" },
            new() { Name = "ResurceController/Insert" },
            new() { Name = "ResurceController/FetchInfoOnResurces" },
            new() { Name = "ResurceController/Delete" },
            new() { Name = "ResurceController/Detail" },
            new() { Name = "PermissionController/PermissionsGetAll" },
            new() { Name = "PermissionController/Edit" },
            new() { Name = "PermissionController/Insert" },
            new() { Name = "PermissionController/FetchInfoOnPermissions" },
            new() { Name = "PermissionController/Delete" },
            new() { Name = "PermissionController/Detail" },
        };

        foreach (var resource in resources)
        {
            // Check if the resource already exists in the database
            var existingResource = await dbContext.ApplicationResource.FirstOrDefaultAsync(r => r.Name == resource.Name);

            if (existingResource == null)
            {
                // Add the resource to the database if it doesn't exist
                dbContext.ApplicationResource.Add(resource);
            }
        }

        var numSave = dbContext.SaveChanges();
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
        var permissionsToAdd = new List<Permission>();
        foreach (var resource in resources)
        {
            foreach (var operation in operations)
            {
                // Construct permission name based on resource and operation
                var permissionName = $"Can{operation.Name}{resource.Name.Replace("/", "")}";
                var permissionDescription = $"Permission to {operation.Name} {resource.Name}";

                // Check if the permission already exists
                var existingPermission = await dbContext.Permission.FirstOrDefaultAsync(p => p.Name == permissionName);
                if (existingPermission == null)
                {
                    permissionsToAdd.Add(new Permission
                    {
                        Name = permissionName,
                        Description = permissionDescription,
                        ResourceId = resource.Id,
                        OperationId = operation.Id,
                        Active = true,
                        ActiveFrom = DateTime.UtcNow,
                        TrackingState = TrackableEntities.Common.Core.TrackingState.Added
                    });
                }
            }
        }

        dbContext.Permission.AddRange(permissionsToAdd);
        await dbContext.SaveChangesAsync();

        // Define roles corresponding to these permissions
        var rolePermissions = new Dictionary<string, List<string>>
        {
            { "Admin", permissionsToAdd.Where(p => p.Name.StartsWith("Can")).Select(p => p.Name).ToList() }, // Assign all permissions starting with "Can"
            { "Standard", permissionsToAdd.Where(p => p.Name.StartsWith("CanRead")).Select(p => p.Name).ToList() }, // Assign all permissions starting with "CanRead"       
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
                // Create the role if it doesn't exist
                role = new AppRole { Name = roleName };
                var result = await roleManager.CreateAsync(role);
                if (!result.Succeeded)
                {
                    throw new Exception($"Failed to create role {roleName}");
                }
            }

            // Assign permissions to the role
            foreach (var permissionName in permissionNames)
            {
                var permission = await dbContext.Permission.FirstOrDefaultAsync(p => p.Name == permissionName);
                if (permission != null)
                {
                    // Check if the role already has this permission
                    var roleContainsPermission = await dbContext.RoleContainsPermissions.FirstOrDefaultAsync(r => r.RoleId == role.Id && r.PermissionId == permission.Id);
                    if (roleContainsPermission == null)
                    {
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
            }
        }

        await dbContext.SaveChangesAsync();
    }

}
