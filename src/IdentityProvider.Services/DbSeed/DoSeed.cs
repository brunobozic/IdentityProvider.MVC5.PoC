using System;
using System.Collections.Generic;
using System.Linq;
using IdentityProvider.Models;
using IdentityProvider.Models.Domain.Account;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using TrackableEntities;
using AppDbContext = IdentityProvider.Repository.EF.EFDataContext.AppDbContext;

namespace IdentityProvider.Services.DbSeed
{
    public class DoSeed : IDoSeed
    {
        private readonly AppDbContext _context;

        [StructureMap.DefaultConstructor] // Set Default Constructor for StructureMap
        public DoSeed(AppDbContext context)
        {
            _context = context;
        }

        public bool Seed()
        {

            #region Operations

            if (!_context.Operation.Any(u => u.Name == "R"))
                _context.Operation.Add(new Operation
                {
                    Name = "R",
                    Description = "Read",
                    Active = true,
                    ActiveFrom = DateTime.Now,
                    ActiveTo = DateTime.Now.AddMonths(6),
                    TrackingState = TrackingState.Added
                });

            if (!_context.Operation.Any(u => u.Name == "W"))
                _context.Operation.Add(new Operation
                {
                    Name = "W",
                    Description = "Write",
                    Active = true,
                    ActiveFrom = DateTime.Now,
                    ActiveTo = DateTime.Now.AddMonths(6),
                    TrackingState = TrackingState.Added
                });

            if (!_context.Operation.Any(u => u.Name == "U"))
                _context.Operation.Add(new Operation
                {
                    Name = "U",
                    Description = "Update",
                    Active = true,
                    ActiveFrom = DateTime.Now,
                    ActiveTo = DateTime.Now.AddMonths(6),
                    TrackingState = TrackingState.Added
                });

            if (!_context.Operation.Any(u => u.Name == "D"))
                _context.Operation.Add(new Operation
                {
                    Name = "D",
                    Description = "Delete",
                    Active = true,
                    ActiveFrom = DateTime.Now,
                    ActiveTo = DateTime.Now.AddMonths(6),
                    TrackingState = TrackingState.Added
                });

            if (!_context.Operation.Any(u => u.Name == "C"))
                _context.Operation.Add(new Operation
                {
                    Name = "C",
                    Description = "Insert",
                    Active = true,
                    ActiveFrom = DateTime.Now,
                    ActiveTo = DateTime.Now.AddMonths(6),
                    TrackingState = TrackingState.Added
                });

            if (!_context.Operation.Any(u => u.Name == "V"))
                _context.Operation.Add(new Operation
                {
                    Name = "V",
                    Description = "ViewResult",
                    Active = true,
                    ActiveFrom = DateTime.Now,
                    ActiveTo = DateTime.Now.AddMonths(6),
                    TrackingState = TrackingState.Added
                });

            if (!_context.Operation.Any(u => u.Name == "Upl"))
                _context.Operation.Add(new Operation
                {
                    Name = "Upl",
                    Description = "UploadBlob",
                    Active = true,
                    ActiveFrom = DateTime.Now,
                    ActiveTo = DateTime.Now.AddMonths(6),
                    TrackingState = TrackingState.Added
                });

            if (!_context.Operation.Any(u => u.Name == "Test"))
                _context.Operation.Add(new Operation
                {
                    Name = "Test",
                    Description = "Test",
                    Active = true,
                    ActiveFrom = DateTime.Now,
                    ActiveTo = DateTime.Now.AddMonths(6),
                    TrackingState = TrackingState.Added
                });

            if (!_context.Operation.Any(u => u.Name == "Test2"))
                _context.Operation.Add(new Operation
                {
                    Name = "Test2",
                    Description = "Test2",
                    Active = true,
                    ActiveFrom = DateTime.Now,
                    ActiveTo = DateTime.Now.AddMonths(6),
                    TrackingState = TrackingState.Added
                });

            _context.SaveChanges();

            var writeOperation = _context.Operation.SingleOrDefault(i => i.Name.Equals("W"));
            var deleteOperation = _context.Operation.SingleOrDefault(i => i.Name.Equals("D"));
            var readOperation = _context.Operation.SingleOrDefault(i => i.Name.Equals("R"));
            var updateOperation = _context.Operation.SingleOrDefault(i => i.Name.Equals("U"));
            var createOperation = _context.Operation.SingleOrDefault(i => i.Name.Equals("C"));
            var viewOperation = _context.Operation.SingleOrDefault(i => i.Name.Equals("V"));
            var uploadOperation = _context.Operation.SingleOrDefault(i => i.Name.Equals("Upl"));
            var test1Operation = _context.Operation.SingleOrDefault(i => i.Name.Equals("Test"));

            //writeOperation.TrackingState = TrackingState.Unchanged;
            //deleteOperation.TrackingState = TrackingState.Unchanged;
            //readOperation.TrackingState = TrackingState.Unchanged;
            //updateOperation.TrackingState = TrackingState.Unchanged;
            //createOperation.TrackingState = TrackingState.Unchanged;


            #endregion Operations

            #region Resources
            var resources = new List<Resource>();

            if (!_context.Resource.Any(u => u.Name == "ManageController"))
            {
                var r = new Resource
                {
                    Name = "ManageController",
                    Active = true,
                    ActiveFrom = DateTime.Now,
                    ActiveTo = DateTime.Now.AddMonths(6),
                    TrackingState = TrackingState.Added
                };

                r.Operations.Add(readOperation);
                r.Operations.Add(deleteOperation);
                r.Operations.Add(createOperation);
                r.Operations.Add(updateOperation);

                _context.Resource.Add(r);
                _context.SaveChanges();
            }

            _context.SaveChanges();


            if (!_context.Resource.Any(u => u.Name == "AccountController"))
            {
                var r = new Resource
                {
                    Name = "AccountController",
                    Active = true,
                    ActiveFrom = DateTime.Now,
                    ActiveTo = DateTime.Now.AddMonths(6),
                    TrackingState = TrackingState.Added,
                };

                r.Operations.Add(readOperation);
                r.Operations.Add(deleteOperation);
                r.Operations.Add(createOperation);
                r.Operations.Add(updateOperation);

                _context.Resource.Add(r);
                _context.SaveChanges();
            }

            if (!_context.Resource.Any(u => u.Name == "HomeController"))
            {
                var r = new Resource
                {
                    Name = "HomeController",
                    Active = true,
                    ActiveFrom = DateTime.Now,
                    ActiveTo = DateTime.Now.AddMonths(6),
                    TrackingState = TrackingState.Added,
                };

                r.Operations.Add(readOperation);
                r.Operations.Add(deleteOperation);
                r.Operations.Add(createOperation);
                r.Operations.Add(updateOperation);

                _context.Resource.Add(r);
                _context.SaveChanges();
            }

            if (!_context.Resource.Any(u => u.Name == "AdministrationController"))
            {
                var r = new Resource
                {
                    Name = "AdministrationController",
                    Active = true,
                    ActiveFrom = DateTime.Now,
                    ActiveTo = DateTime.Now.AddMonths(6),
                    TrackingState = TrackingState.Added,
                };

                r.Operations.Add(readOperation);
                r.Operations.Add(deleteOperation);
                r.Operations.Add(createOperation);
                r.Operations.Add(updateOperation);

                _context.Resource.Add(r);
                _context.SaveChanges();
            }

            if (!_context.Resource.Any(u => u.Name == "UserProfileController"))
            {
                var r = new Resource
                {
                    Name = "UserProfileController",
                    Active = true,
                    ActiveFrom = DateTime.Now,
                    ActiveTo = DateTime.Now.AddMonths(6),
                    TrackingState = TrackingState.Added
                };

                r.Operations.Add(readOperation);
                r.Operations.Add(deleteOperation);
                r.Operations.Add(createOperation);
                r.Operations.Add(updateOperation);

                _context.Resource.Add(r);
                _context.SaveChanges();
            }

            if (!_context.Resource.Any(u => u.Name == "ListUserProfiles"))
            {
                var r = new Resource
                {
                    Name = "ListUserProfiles",
                    Active = true,
                    ActiveFrom = DateTime.Now,
                    ActiveTo = DateTime.Now.AddMonths(6),
                    TrackingState = TrackingState.Added,
                };

                r.Operations.Add(readOperation);

                _context.Resource.Add(r);
                _context.SaveChanges();
            }

            if (!_context.Resource.Any(u => u.Name == "ListLockedUserAccounts"))
            {
                var r = new Resource
                {
                    Name = "ListLockedUserAccounts",
                    Active = true,
                    ActiveFrom = DateTime.Now,
                    ActiveTo = DateTime.Now.AddMonths(6),
                    TrackingState = TrackingState.Added
                };

                r.Operations.Add(readOperation);

                _context.Resource.Add(r);
                _context.SaveChanges();
            }

            if (!_context.Resource.Any(u => u.Name == "UnlockLockedUserAccount"))
            {
                var r = new Resource
                {
                    Name = "UnlockLockedUserAccount",
                    Active = true,
                    ActiveFrom = DateTime.Now,
                    ActiveTo = DateTime.Now.AddMonths(6),
                    TrackingState = TrackingState.Added
                };

                r.Operations.Add(updateOperation);

                _context.Resource.Add(r);
                _context.SaveChanges();
            }

            if (!_context.Resource.Any(u => u.Name == "UploadUserProfileImage"))
            {
                var r = new Resource
                {
                    Name = "UploadUserProfileImage",
                    Active = true,
                    ActiveFrom = DateTime.Now,
                    ActiveTo = DateTime.Now.AddMonths(6),
                    TrackingState = TrackingState.Added,
                };

                r.Operations.Add(uploadOperation);

                _context.Resource.Add(r);
                _context.SaveChanges();
            }

            if (!_context.Resource.Any(u => u.Name == "DeleteUserProfileImage"))
            {
                var r = new Resource
                {
                    Name = "DeleteUserProfileImage",
                    Active = true,
                    ActiveFrom = DateTime.Now,
                    ActiveTo = DateTime.Now.AddMonths(6),
                    TrackingState = TrackingState.Added
                };

                r.Operations.Add(deleteOperation);

                _context.Resource.Add(r);
                _context.SaveChanges();
            }



            #endregion Resources

            var manageController = _context.Resource.Single(u => u.Name == "ManageController");
            var homeController = _context.Resource.Single(u => u.Name == "HomeController");
            var userProfileController = _context.Resource.Single(u => u.Name == "UserProfileController");
            var listUserProfiles = _context.Resource.Single(u => u.Name == "ListUserProfiles");
            var listLockedUserAccounts = _context.Resource.Single(u => u.Name == "ListLockedUserAccounts");
            var unlockLockedUserAccount = _context.Resource.Single(u => u.Name == "UnlockLockedUserAccount");
            var uploadUserProfileImage = _context.Resource.Single(u => u.Name == "UploadUserProfileImage");

            if (!_context.Roles.Any(r => r.Name == "Admin"))
            {
                var store = new RoleStore<ApplicationRole>(_context);
                var manager = new ApplicationRoleManager(store, _context);
                var role = new ApplicationRole("Admin")
                {
                    Name = "Admin",
                    Description = "Admin",
                    Active = true,
                    ActiveFrom = DateTime.Now,
                    ActiveTo = DateTime.Now.AddMonths(6),
                    TrackingState = TrackingState.Added
                };

                role.Resources.Add(manageController);
                role.Resources.Add(homeController);
                role.Resources.Add(userProfileController);
                role.Resources.Add(listUserProfiles);
                role.Resources.Add(listLockedUserAccounts);
                role.Resources.Add(unlockLockedUserAccount);
                role.Resources.Add(uploadUserProfileImage);

                manager.Create(role);
            }

            if (!_context.Roles.Any(r => r.Name == "Standard"))
            {
                var store = new RoleStore<ApplicationRole>(_context);
                var manager = new ApplicationRoleManager(store, _context);
                var role = new ApplicationRole("Standard")
                {
                    Name = "Standard",
                    Description = "Standard",
                    Active = true,
                    ActiveFrom = DateTime.Now,
                    ActiveTo = DateTime.Now.AddMonths(6),
                    TrackingState = TrackingState.Added
                };

                role.Resources.Add(manageController);
                role.Resources.Add(homeController);
                role.Resources.Add(userProfileController);
                role.Resources.Add(listUserProfiles);
                role.Resources.Add(listLockedUserAccounts);
                role.Resources.Add(unlockLockedUserAccount);
                role.Resources.Add(uploadUserProfileImage);

                manager.Create(role);

            }

            #region Roles

            if (!_context.Roles.Any(r => r.Name == "Guest"))
            {
                var store = new RoleStore<ApplicationRole>(_context);
                var manager = new ApplicationRoleManager(store, _context);
                var role = new ApplicationRole("Guest")
                {
                    Name = "Guest",
                    Description = "Guest",
                    Active = true,
                    ActiveFrom = DateTime.Now,
                    ActiveTo = DateTime.Now.AddMonths(6),
                    TrackingState = TrackingState.Added
                };

                role.Resources.Add(manageController);
                role.Resources.Add(homeController);

                manager.Create(role);

            }

            _context.SaveChanges();

            #endregion Roles

            if (!_context.Users.Any(u => u.UserName == "AppAutomation"))
            {
                var store = new UserStore<ApplicationUser>(_context);
                var manager = new UserManager<ApplicationUser>(store);

                var user = new ApplicationUser
                {
                    UserName = "AppAutomation",
                    FirstName = "Application",
                    LastName = "AutomatedTasks",
                    Active = true,
                    ActiveFrom = DateTime.Now,
                    ActiveTo = DateTime.Now.AddMonths(6),
                    TrackingState = TrackingState.Added
                };

                manager.Create(user, "AppAutomation!");
                manager.AddToRole(user.Id, "Admin");
            }

            if (!_context.Users.Any(u => u.UserName == "AdminUser"))
            {
                var store = new UserStore<ApplicationUser>(_context);
                var manager = new UserManager<ApplicationUser>(store);
                var user = new ApplicationUser
                {
                    Id = Guid.NewGuid().ToString(),
                    UserName = "AdminUser",
                    FirstName = "Application",
                    LastName = "Administrator",
                    Active = true,
                    ActiveFrom = DateTime.Now,
                    ActiveTo = DateTime.Now.AddMonths(6),
                    TrackingState = TrackingState.Added
                };

                manager.Create(user, "AdminUser!");
                manager.AddToRole(user.Id, "Admin");
            }

            if (!_context.Users.Any(u => u.UserName == "StandardUser"))
            {
                var store = new UserStore<ApplicationUser>(_context);
                var manager = new UserManager<ApplicationUser>(store);
                var user = new ApplicationUser
                {
                    Id = Guid.NewGuid().ToString(),
                    UserName = "StandardUser",
                    FirstName = "Application",
                    LastName = "Standard User",
                    Active = true,
                    ActiveFrom = DateTime.Now,
                    ActiveTo = DateTime.Now.AddMonths(6),
                    TrackingState = TrackingState.Added
                };

                manager.Create(user, "StandardUser!");
                manager.AddToRole(user.Id, "Standard");
            }

            if (!_context.Users.Any(u => u.UserName == "bruno.bozic@blink.hr"))
            {
                var store = new UserStore<ApplicationUser>(_context);
                var manager = new UserManager<ApplicationUser>(store);
                var user = new ApplicationUser
                {
                    Id = Guid.NewGuid().ToString(),
                    UserName = "bruno.bozic@blink.hr",
                    FirstName = "Bruno",
                    LastName = "Božić",
                    Active = true,
                    ActiveFrom = DateTime.Now,
                    TrackingState = TrackingState.Added
                };

                manager.Create(user, "bruno123!");
                manager.AddToRole(user.Id, "Admin");
            }

            _context.SaveChanges();

            return true;
        }
    }
}