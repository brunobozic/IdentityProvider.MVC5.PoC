using IdentityProvider.Repository.EFCore.Domain.Account;
using IdentityProvider.Repository.EFCore.Domain.Account.Employees;
using IdentityProvider.Repository.EFCore.Domain.Roles;
using IdentityProvider.Repository.EFCore.EFDataContext;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;
using TrackableEntities.Common.Core;

public class SeedData
{
    public static async Task Initialize(AppDbContext dbContext, IServiceScope scope, string testUserPw)
    {
        // Password is set using the following:
        // dotnet myIdentityUser-secrets set SeedUserPW <pw>

        // Ensure users are created and the roles have been assigned to these users.
        // In short the admin myIdentityUser has priviliges to do anything and everything
        // the guest myIdentityUser has read-only priviliges on certain resources
        // the standard myIdentityUser has r/w on certain resources
        var adminId = await EnsureUser(scope, testUserPw, "bruno.bozic@gmail.com");

        // Basic roles are the indentity framework roles - so we only have a "standard" and "admin" roles here
        // The admin role is basically a super user / developer role
        await EnsureBasicRole(scope, adminId, IdentityProvider.Web.MVC6.IdentityHelpingConstants.SuperUser);
        await EnsureBasicRole(scope, adminId, IdentityProvider.Web.MVC6.IdentityHelpingConstants.StandardRole);
         dbContext.SaveChanges();
        // The roles mentioned after this line are the application roles (not part of the identity framework)
        await EnsureRole(scope, adminId, IdentityProvider.Web.MVC6.IdentityHelpingConstants.ContactManagersRole);
        await EnsureRole(scope, adminId, IdentityProvider.Web.MVC6.IdentityHelpingConstants.ContactAdministratorsRole);
        dbContext.SaveChanges();
        var managerId = await EnsureUser(scope, testUserPw, "bruno.bozicmanager@gmail.com");
        var guestId = await EnsureUser(scope, testUserPw, "guest.user31337@gmail.com");
        dbContext.SaveChanges();
        // Now we need employees, one for each identity user, the employee entity holds information relevant to the business domain
        // while the identity user is used only for logon/authentication/authorization
        var employeeId1 = await EnsureSuperEmployee(scope, adminId, adminId, "bruno.bozic@gmail.com", "Bruno", "Bozic");
        var employeeId2 = await EnsureSuperEmployee(scope, adminId, managerId, "bruno.bozicmanager@gmail.com", "Manager", "Manager");
        var employeeId3 = await EnsureSuperEmployee(scope, adminId, guestId, "guest.user31337@gmail.com", "Guest", "Guest");
        dbContext.SaveChanges();
        // Proceed seeding the rest of the database
        SeedDB(dbContext, adminId);
        dbContext.SaveChanges();
    }

    private static async Task<int> EnsureSuperEmployee(IServiceScope scope, string adminUID, string applicationUserId, string employeeName, string userName, string userSurname)
    {
        if (string.IsNullOrEmpty(applicationUserId)) { throw new Exception("EnsureSuperEmployee applicationUserId null"); }
        if (string.IsNullOrEmpty(adminUID)) { throw new Exception("EnsureSuperEmployee adminUID null"); }
        if (string.IsNullOrEmpty(employeeName)) { throw new Exception("EnsureSuperEmployee employeeName null"); }
        if (string.IsNullOrEmpty(userName)) { throw new Exception("EnsureSuperEmployee userName null"); }
        if (string.IsNullOrEmpty(userSurname)) { throw new Exception("EnsureSuperEmployee userSurname null"); }

        var employeeId = -1;
        try
        {
            var dbContext = scope.ServiceProvider.GetService<AppDbContext>();

            if (dbContext == null)
            {
                throw new Exception("dbContext null");
            }

            if (!dbContext.Employee.Where(r => r.ApplicationUser.UserName == employeeName).Any())
            {
                dbContext.Employee.Add(new Employee
                {
                    Active = true,
                    IsDeleted = false,
                    Name = userName,
                    Surname = userSurname,
                    CreatedById = adminUID,
                    ApplicationUserId = applicationUserId,
                    ActiveFrom = DateTime.UtcNow,
                    ActiveTo = DateTime.UtcNow.AddYears(3)
                });

                dbContext.SaveChanges();
            }

            var postFactoEmployee = await dbContext.Employee.Where(e => e.ApplicationUserId == applicationUserId).SingleOrDefaultAsync();

            if (postFactoEmployee != null)
            {
                return postFactoEmployee.Id;
            }
        }
        catch
        {
            return employeeId;
        }

        return employeeId;
    }

    private static void SeedDB(AppDbContext context, string adminID)
    {
        if (context.Users.Any())
        {
            return; // DB has been seeded
        }

        context.Users.AddRange(
            new ApplicationUser()
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "StandardUser",
                FirstName = "Application",
                LastName = "Standard User",
                Active = true,
                ActiveFrom = DateTime.Now,
                ActiveTo = DateTime.Now.AddMonths(6),
                TrackingState = TrackingState.Added
            }, new ApplicationUser
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "AdminUser",
                FirstName = "Application",
                LastName = "Administrator",
                Active = true,
                ActiveFrom = DateTime.Now,
                ActiveTo = DateTime.Now.AddMonths(6),
                TrackingState = TrackingState.Added
            }, new ApplicationUser
            {
                UserName = "AppAutomation",
                FirstName = "Application",
                LastName = "AutomatedTasks",
                Active = true,
                ActiveFrom = DateTime.Now,
                ActiveTo = DateTime.Now.AddMonths(6),
                TrackingState = TrackingState.Added
            });
    }

    private static async Task<string> EnsureUser(IServiceScope scope, string testUserPw, string userName)
    {
        if (string.IsNullOrEmpty(testUserPw)) { testUserPw = "adminAdmin123456!"; }

        try
        {
            var userManager = scope.ServiceProvider.GetService<UserManager<ApplicationUser>>();
            var user = await userManager.FindByNameAsync(userName);

            if (user == null)
            {
                user = new ApplicationUser
                {
                    UserName = userName,
                    EmailConfirmed = true,
                    Id = Guid.NewGuid().ToString(),
                    FirstName = "Bruno",
                    LastName = "Božić",
                    Active = true,
                    ActiveFrom = DateTime.Now,
                    TrackingState = TrackingState.Added
                };

                var identityResult = await userManager.CreateAsync(user, testUserPw);

                if (user == null)
                {
                    throw new Exception("The password is probably not strong enough!");
                }

                return user.Id;
            }

            return user.Id;
        }
        catch
        {
            return null;
        }
    }

    private static async Task<IdentityResult> EnsureBasicRole(IServiceScope scope, string uid, string role)
    {
        IdentityResult ir = null;
        var roleManager = scope.ServiceProvider.GetService<RoleManager<AppRole>>();

        if (roleManager == null)
        {
            throw new Exception("roleManager null");
        }

        if (!await roleManager.RoleExistsAsync(role))
        {
            var r = new AppRole(role);
            var identityResult = await roleManager.CreateAsync(r);
        }

        var userManager = scope.ServiceProvider.GetService<UserManager<ApplicationUser>>();

        var user = await userManager.FindByIdAsync(uid);

        if (user == null)
        {
            throw new Exception("The testUserPw password was probably not strong enough!");
        }

        var anotherIdentityResult = await userManager.AddToRoleAsync(user, role);
        
        return anotherIdentityResult;
    }

    private static async Task<bool> EnsureRole(IServiceScope scope, string uid, string role)
    {
        var dbContext = scope.ServiceProvider.GetService<AppDbContext>();

        if (dbContext == null)
        {
            throw new Exception("dbContext null");
        }

        if (!dbContext.Role.Where(r => r.Name == role).Any())
        {
            dbContext.Role.Add(new AppRole
            {
                Active = true,
               // IsDeleted = false,
                Name = role,
               // CreatedById = uid,
                ActiveFrom = DateTime.UtcNow,
                ActiveTo = DateTime.UtcNow.AddYears(3)
            });

            dbContext.SaveChanges();
        }

        //var userManager = serviceProvider.GetService<UserManager<ApplicationUser>>();

        //var myIdentityUser = await userManager.FindByIdAsync(uid);

        //if (myIdentityUser == null)
        //{
        //    throw new Exception("The testUserPw password was probably not strong enough!");
        //}

        //var myRole = await dbContext.Role.Where(r => r.Name == role).SingleOrDefaultAsync();

        //var myEmployee = await dbContext.Employee.Where(e => e.ApplicationUser.Id == myIdentityUser.Id).SingleOrDefaultAsync();

        //var myEmployeeRole = new EmployeeOwnsRoles
        //{
        //    Name = "",
        //    IsDeleted = false,
        //    Employee = myEmployee,
        //    Role = myRole,
        //    Active = true,
        //    ActiveFrom = DateTime.UtcNow,
        //    ActiveTo = DateTime.UtcNow.AddYears(3)
        //};

        //dbContext.EmployeeOwnsRoles.Add(myEmployeeRole);

        //dbContext.SaveChanges();

        return true;
    }
}