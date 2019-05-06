using System;
using System.Collections.Generic;
using System.Linq;
using IdentityProvider.Models.Domain.Account;
using IdentityProvider.Repository.EF.EFDataContext;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using TrackableEntities;

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

            // Adding a list of basic (commonly used) Operations to the Db

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

            _context.SaveChanges();

            var deleteOperation = _context.Operation.SingleOrDefault(i => i.Name.Equals("D"));
            var readOperation = _context.Operation.SingleOrDefault(i => i.Name.Equals("R"));
            var updateOperation = _context.Operation.SingleOrDefault(i => i.Name.Equals("U"));
            var createOperation = _context.Operation.SingleOrDefault(i => i.Name.Equals("C"));
            var uploadOperation = _context.Operation.SingleOrDefault(i => i.Name.Equals("Upl"));

            #endregion Operations

            #region ApplicationResource

            // Adding a list of basic application resources (in the form of either mvc controllers or abstract resources, see examples)

            var resources = new List<ApplicationResource>();

            if (!_context.ApplicationResource.Any(u => u.Name == "ManageController"))
            {
                var r = new ApplicationResource
                {
                    Name = "ManageController",
                    Active = true,
                    ActiveFrom = DateTime.Now,
                    ActiveTo = DateTime.Now.AddMonths(6),
                    TrackingState = TrackingState.Added,
                    Description = "ManageController"
                };

                _context.ApplicationResource.Add(r);
            }

            if (!_context.ApplicationResource.Any(u => u.Name == "AccountController"))
            {
                var r = new ApplicationResource
                {
                    Name = "AccountController",
                    Active = true,
                    ActiveFrom = DateTime.Now,
                    ActiveTo = DateTime.Now.AddMonths(6),
                    TrackingState = TrackingState.Added,
                    Description = "AccountController"
                };

                _context.ApplicationResource.Add(r);
            }

            if (!_context.ApplicationResource.Any(u => u.Name == "HomeController"))
            {
                var r = new ApplicationResource
                {
                    Name = "HomeController",
                    Active = true,
                    ActiveFrom = DateTime.Now,
                    ActiveTo = DateTime.Now.AddMonths(6),
                    TrackingState = TrackingState.Added,
                    Description = "HomeController"
                };

                _context.ApplicationResource.Add(r);
            }

            if (!_context.ApplicationResource.Any(u => u.Name == "UserProfileController"))
            {
                var r = new ApplicationResource
                {
                    Name = "UserProfileController",
                    Active = true,
                    ActiveFrom = DateTime.Now,
                    ActiveTo = DateTime.Now.AddMonths(6),
                    TrackingState = TrackingState.Added,
                    Description = "UserProfileController"
                };

                _context.ApplicationResource.Add(r);
            }

            if (!_context.ApplicationResource.Any(u => u.Name == "LockedUserAccounts"))
            {
                var r = new ApplicationResource
                {
                    Name = "LockedUserAccounts",
                    Active = true,
                    ActiveFrom = DateTime.Now,
                    ActiveTo = DateTime.Now.AddMonths(6),
                    TrackingState = TrackingState.Added,
                    Description = "LockedUserAccounts"
                };

                _context.ApplicationResource.Add(r);

            }

            if (!_context.ApplicationResource.Any(u => u.Name == "UserProfileImage"))
            {
                var r = new ApplicationResource
                {
                    Name = "UserProfileImage",
                    Active = true,
                    ActiveFrom = DateTime.Now,
                    ActiveTo = DateTime.Now.AddMonths(6),
                    TrackingState = TrackingState.Added,
                    Description = "UserProfileImage"
                };

                _context.ApplicationResource.Add(r);

            }

            if (!_context.ApplicationResource.Any(u => u.Name == "OperationController"))
            {
                var r = new ApplicationResource
                {
                    Name = "OperationController",
                    Active = true,
                    ActiveFrom = DateTime.Now,
                    ActiveTo = DateTime.Now.AddMonths(6),
                    TrackingState = TrackingState.Added,
                    Description = "OperationController"
                };

                _context.ApplicationResource.Add(r);

            }

            if (!_context.ApplicationResource.Any(u => u.Name == "RoleGroupController"))
            {
                var r = new ApplicationResource
                {
                    Name = "RoleGroupController",
                    Active = true,
                    ActiveFrom = DateTime.Now,
                    ActiveTo = DateTime.Now.AddMonths(6),
                    TrackingState = TrackingState.Added,
                    Description = "RoleGroupController"
                };

                _context.ApplicationResource.Add(r);

            }

            if (!_context.ApplicationResource.Any(u => u.Name == "PermissionGroupController"))
            {
                var r = new ApplicationResource
                {
                    Name = "PermissionGroupController",
                    Active = true,
                    ActiveFrom = DateTime.Now,
                    ActiveTo = DateTime.Now.AddMonths(6),
                    TrackingState = TrackingState.Added,
                    Description = "PermissionGroupController"
                };

                _context.ApplicationResource.Add(r);

            }

            if (!_context.ApplicationResource.Any(u => u.Name == "PermissionController"))
            {
                var r = new ApplicationResource
                {
                    Name = "PermissionController",
                    Active = true,
                    ActiveFrom = DateTime.Now,
                    ActiveTo = DateTime.Now.AddMonths(6),
                    TrackingState = TrackingState.Added,
                    Description = "PermissionController"
                };

                _context.ApplicationResource.Add(r);

            }

            if (!_context.ApplicationResource.Any(u => u.Name == "ResourceController"))
            {
                var r = new ApplicationResource
                {
                    Name = "ResourceController",
                    Active = true,
                    ActiveFrom = DateTime.Now,
                    ActiveTo = DateTime.Now.AddMonths(6),
                    TrackingState = TrackingState.Added,
                    Description = "ResourceController"
                };

                _context.ApplicationResource.Add(r);

            }

            if (!_context.ApplicationResource.Any(u => u.Name == "OrganizationalUnitController"))
            {
                var r = new ApplicationResource
                {
                    Name = "OrganizationalUnitController",
                    Active = true,
                    ActiveFrom = DateTime.Now,
                    ActiveTo = DateTime.Now.AddMonths(6),
                    TrackingState = TrackingState.Added,
                    Description = "OrganizationalUnitController"
                };

                _context.ApplicationResource.Add(r);

            }

            if (!_context.ApplicationResource.Any(u => u.Name == "EmployeeController"))
            {
                var r = new ApplicationResource
                {
                    Name = "EmployeeController",
                    Active = true,
                    ActiveFrom = DateTime.Now,
                    ActiveTo = DateTime.Now.AddMonths(6),
                    TrackingState = TrackingState.Added,
                    Description = "EmployeeController"
                };

                _context.ApplicationResource.Add(r);

            }

            _context.SaveChanges();

            #endregion ApplicationResource

            #region Resource Permissions

            // Adding a basic list of permissions to the Db, the application resources and application operation are being connected via this table...
            // the "Permission" table is therefore a link table between "Resource" and "Operation"

            var res1 = _context.ApplicationResource.First(u => u.Name == "UserProfileImage");

            if (!_context.ApplicationResource.Any(u => u.Name == "UserProfileImageDelete"))
            {
                var rp = new Permission
                {
                    Name = "UserProfileImageDelete",
                    Active = true,
                    ActiveFrom = DateTime.Now,
                    ActiveTo = DateTime.Now.AddMonths(6),
                    TrackingState = TrackingState.Added,
                    ApplicationResource = res1,
                    Operation = deleteOperation
                };

                _context.Permission.Add(rp);
            }

            var res2 = _context.ApplicationResource.First(u => u.Name == "UserProfileImage");

            if (!_context.ApplicationResource.Any(u => u.Name == "UserProfileImageUpload"))
            {
                var rp2 = new Permission
                {
                    Name = "UserProfileImageUpload",
                    Active = true,
                    ActiveFrom = DateTime.Now,
                    ActiveTo = DateTime.Now.AddMonths(6),
                    TrackingState = TrackingState.Added,
                    ApplicationResource = res2,
                    Operation = uploadOperation
                };

                _context.Permission.Add(rp2);
            }

            var res3 = _context.ApplicationResource.First(u => u.Name == "EmployeeController");

            if (!_context.ApplicationResource.Any(u => u.Name == "EmployeeControllerCreate"))
            {
                var rp3 = new Permission
                {
                    Name = "EmployeeControllerCreate",
                    Active = true,
                    ActiveFrom = DateTime.Now,
                    ActiveTo = DateTime.Now.AddMonths(6),
                    TrackingState = TrackingState.Added,
                    ApplicationResource = res3,
                    Operation = createOperation
                };

                _context.Permission.Add(rp3);
            }

            if (!_context.ApplicationResource.Any(u => u.Name == "EmployeeControllerDelete"))
            {
                var rp31 = new Permission
                {
                    Name = "EmployeeControllerDelete",
                    Active = true,
                    ActiveFrom = DateTime.Now,
                    ActiveTo = DateTime.Now.AddMonths(6),
                    TrackingState = TrackingState.Added,
                    ApplicationResource = res3,
                    Operation = deleteOperation
                };

                _context.Permission.Add(rp31);
            }

            if (!_context.ApplicationResource.Any(u => u.Name == "EmployeeControllerUpdate"))
            {
                var rp32 = new Permission
                {
                    Name = "EmployeeControllerUpdate",
                    Active = true,
                    ActiveFrom = DateTime.Now,
                    ActiveTo = DateTime.Now.AddMonths(6),
                    TrackingState = TrackingState.Added,
                    ApplicationResource = res3,
                    Operation = updateOperation
                };

                _context.Permission.Add(rp32);
            }

            if (!_context.ApplicationResource.Any(u => u.Name == "EmployeeControllerRead"))
            {
                var rp33 = new Permission
                {
                    Name = "EmployeeControllerRead",
                    Active = true,
                    ActiveFrom = DateTime.Now,
                    ActiveTo = DateTime.Now.AddMonths(6),
                    TrackingState = TrackingState.Added,
                    ApplicationResource = res3,
                    Operation = readOperation
                };

                _context.Permission.Add(rp33);
            }

            var res4 = _context.ApplicationResource.First(u => u.Name == "EmployeeController");

            if (!_context.ApplicationResource.Any(u => u.Name == "EmployeeDelete"))
            {
                var rp4 = new Permission
                {
                    Name = "EmployeeDelete",
                    Active = true,
                    ActiveFrom = DateTime.Now,
                    ActiveTo = DateTime.Now.AddMonths(6),
                    TrackingState = TrackingState.Added,
                    ApplicationResource = res4,
                    Operation = deleteOperation
                };

                _context.Permission.Add(rp4);
            }

            var res5 = _context.ApplicationResource.First(u => u.Name == "EmployeeController");

            if (!_context.ApplicationResource.Any(u => u.Name == "EmployeeCreate"))
            {
                var rp5 = new Permission
                {
                    Name = "EmployeeCreate",
                    Active = true,
                    ActiveFrom = DateTime.Now,
                    ActiveTo = DateTime.Now.AddMonths(6),
                    TrackingState = TrackingState.Added,
                    ApplicationResource = res5,
                    Operation = createOperation
                };

                _context.Permission.Add(rp5);
            }

            if (!_context.ApplicationResource.Any(u => u.Name == "EmployeeUpdate"))
            {
                var rp5 = new Permission
                {
                    Name = "EmployeeUpdate",
                    Active = true,
                    ActiveFrom = DateTime.Now,
                    ActiveTo = DateTime.Now.AddMonths(6),
                    TrackingState = TrackingState.Added,
                    ApplicationResource = res5,
                    Operation = updateOperation
                };

                _context.Permission.Add(rp5);
            }

            if (!_context.ApplicationResource.Any(u => u.Name == "EmployeeRead"))
            {
                var rp5 = new Permission
                {
                    Name = "EmployeeRead",
                    Active = true,
                    ActiveFrom = DateTime.Now,
                    ActiveTo = DateTime.Now.AddMonths(6),
                    TrackingState = TrackingState.Added,
                    ApplicationResource = res5,
                    Operation = readOperation
                };

                _context.Permission.Add(rp5);
            }

            var res6 = _context.ApplicationResource.First(u => u.Name == "LockedUserAccount");

            if (!_context.ApplicationResource.Any(u => u.Name == "UnlockLockedUserAccount"))
            {
                var rp6 = new Permission
                {
                    Name = "UnlockLockedUserAccount",
                    Active = true,
                    ActiveFrom = DateTime.Now,
                    ActiveTo = DateTime.Now.AddMonths(6),
                    TrackingState = TrackingState.Added,
                    ApplicationResource = res6,
                    Operation = updateOperation
                };

                _context.Permission.Add(rp6);
            }

            var res7 = _context.ApplicationResource.First(u => u.Name == "LockedUserAccounts");

            if (!_context.ApplicationResource.Any(u => u.Name == "ViewLockedUserAccounts"))
            {
                var rp7 = new Permission
                {
                    Name = "ViewLockedUserAccounts",
                    Active = true,
                    ActiveFrom = DateTime.Now,
                    ActiveTo = DateTime.Now.AddMonths(6),
                    TrackingState = TrackingState.Added,
                    ApplicationResource = res7,
                    Operation = readOperation
                };

                _context.Permission.Add(rp7);
            }

            var res8 = _context.ApplicationResource.First(u => u.Name == "HomeController");

            if (!_context.ApplicationResource.Any(u => u.Name == "HomeControllerCreate"))
            {
                var rp8 = new Permission
                {
                    Name = "HomeControllerCreate",
                    Active = true,
                    ActiveFrom = DateTime.Now,
                    ActiveTo = DateTime.Now.AddMonths(6),
                    TrackingState = TrackingState.Added,
                    ApplicationResource = res8,
                    Operation = createOperation
                };

                _context.Permission.Add(rp8);
            }

            if (!_context.ApplicationResource.Any(u => u.Name == "HomeControlleDelete"))
            {
                var rp81 = new Permission
                {
                    Name = "HomeControlleDelete",
                    Active = true,
                    ActiveFrom = DateTime.Now,
                    ActiveTo = DateTime.Now.AddMonths(6),
                    TrackingState = TrackingState.Added,
                    ApplicationResource = res8,
                    Operation = deleteOperation
                };

                _context.Permission.Add(rp81);
            }

            if (!_context.ApplicationResource.Any(u => u.Name == "HomeControllerRead"))
            {
                var rp82 = new Permission
                {
                    Name = "HomeControllerRead",
                    Active = true,
                    ActiveFrom = DateTime.Now,
                    ActiveTo = DateTime.Now.AddMonths(6),
                    TrackingState = TrackingState.Added,
                    ApplicationResource = res8,
                    Operation = readOperation
                };

                _context.Permission.Add(rp82);
            }

            if (!_context.ApplicationResource.Any(u => u.Name == "HomeControllerUpdate"))
            {
                var rp83 = new Permission
                {
                    Name = "HomeControllerUpdate",
                    Active = true,
                    ActiveFrom = DateTime.Now,
                    ActiveTo = DateTime.Now.AddMonths(6),
                    TrackingState = TrackingState.Added,
                    ApplicationResource = res8,
                    Operation = updateOperation
                };

                _context.Permission.Add(rp83);
            }

            var res9 = _context.ApplicationResource.First(u => u.Name == "OperationController");

            if (!_context.ApplicationResource.Any(u => u.Name == "OperationCreate"))
            {
                var rp9 = new Permission
                {
                    Name = "OperationCreate",
                    Active = true,
                    ActiveFrom = DateTime.Now,
                    ActiveTo = DateTime.Now.AddMonths(6),
                    TrackingState = TrackingState.Added,
                    ApplicationResource = res9,
                    Operation = createOperation
                };

                _context.Permission.Add(rp9);
            }

            if (!_context.ApplicationResource.Any(u => u.Name == "OperationDelete"))
            {
                var rp91 = new Permission
                {
                    Name = "OperationDelete",
                    Active = true,
                    ActiveFrom = DateTime.Now,
                    ActiveTo = DateTime.Now.AddMonths(6),
                    TrackingState = TrackingState.Added,
                    ApplicationResource = res9,
                    Operation = deleteOperation
                };

                _context.Permission.Add(rp91);
            }

            if (!_context.ApplicationResource.Any(u => u.Name == "OperationRead"))
            {
                var rp92 = new Permission
                {
                    Name = "OperationRead",
                    Active = true,
                    ActiveFrom = DateTime.Now,
                    ActiveTo = DateTime.Now.AddMonths(6),
                    TrackingState = TrackingState.Added,
                    ApplicationResource = res9,
                    Operation = readOperation
                };

                _context.Permission.Add(rp92);
            }

            if (!_context.ApplicationResource.Any(u => u.Name == "OperationUpdate"))
            {
                var rp93 = new Permission
                {
                    Name = "OperationUpdate",
                    Active = true,
                    ActiveFrom = DateTime.Now,
                    ActiveTo = DateTime.Now.AddMonths(6),
                    TrackingState = TrackingState.Added,
                    ApplicationResource = res9,
                    Operation = updateOperation
                };

                _context.Permission.Add(rp93);
            }

            var res10 = _context.ApplicationResource.First(u => u.Name == "ResourceController");

            if (!_context.ApplicationResource.Any(u => u.Name == "ResourceCreate"))
            {
                var rp10 = new Permission
                {
                    Name = "ResourceCreate",
                    Active = true,
                    ActiveFrom = DateTime.Now,
                    ActiveTo = DateTime.Now.AddMonths(6),
                    TrackingState = TrackingState.Added,
                    ApplicationResource = res10,
                    Operation = createOperation
                };

                _context.Permission.Add(rp10);
            }

            if (!_context.ApplicationResource.Any(u => u.Name == "ResourceDelete"))
            {
                var rp101 = new Permission
                {
                    Name = "ResourceDelete",
                    Active = true,
                    ActiveFrom = DateTime.Now,
                    ActiveTo = DateTime.Now.AddMonths(6),
                    TrackingState = TrackingState.Added,
                    ApplicationResource = res10,
                    Operation = deleteOperation
                };

                _context.Permission.Add(rp101);
            }

            if (!_context.ApplicationResource.Any(u => u.Name == "ResourceRead"))
            {
                var rp102 = new Permission
                {
                    Name = "ResourceRead",
                    Active = true,
                    ActiveFrom = DateTime.Now,
                    ActiveTo = DateTime.Now.AddMonths(6),
                    TrackingState = TrackingState.Added,
                    ApplicationResource = res10,
                    Operation = readOperation
                };

                _context.Permission.Add(rp102);
            }

            if (!_context.ApplicationResource.Any(u => u.Name == "ResourceUpdate"))
            {
                var rp103 = new Permission
                {
                    Name = "ResourceUpdate",
                    Active = true,
                    ActiveFrom = DateTime.Now,
                    ActiveTo = DateTime.Now.AddMonths(6),
                    TrackingState = TrackingState.Added,
                    ApplicationResource = res10,
                    Operation = updateOperation
                };

                _context.Permission.Add(rp103);
            }

            var res12 = _context.ApplicationResource.First(u => u.Name == "OrganizationalUnitController");

            if (!_context.ApplicationResource.Any(u => u.Name == "OrganizationalUnitCreate"))
            {
                var rp12 = new Permission
                {
                    Name = "OrganizationalUnitCreate",
                    Active = true,
                    ActiveFrom = DateTime.Now,
                    ActiveTo = DateTime.Now.AddMonths(6),
                    TrackingState = TrackingState.Added,
                    ApplicationResource = res12,
                    Operation = createOperation
                };

                _context.Permission.Add(rp12);
            }

            if (!_context.ApplicationResource.Any(u => u.Name == "OrganizationalUnitDelete"))
            {
                var rp121 = new Permission
                {
                    Name = "OrganizationalUnitDelete",
                    Active = true,
                    ActiveFrom = DateTime.Now,
                    ActiveTo = DateTime.Now.AddMonths(6),
                    TrackingState = TrackingState.Added,
                    ApplicationResource = res12,
                    Operation = deleteOperation
                };

                _context.Permission.Add(rp121);
            }

            if (!_context.ApplicationResource.Any(u => u.Name == "OrganizationalUnitRead"))
            {
                var rp122 = new Permission
                {
                    Name = "OrganizationalUnitRead",
                    Active = true,
                    ActiveFrom = DateTime.Now,
                    ActiveTo = DateTime.Now.AddMonths(6),
                    TrackingState = TrackingState.Added,
                    ApplicationResource = res12,
                    Operation = readOperation
                };

                _context.Permission.Add(rp122);
            }

            if (!_context.ApplicationResource.Any(u => u.Name == "OrganizationalUnitUpdate"))
            {
                var rp123 = new Permission
                {
                    Name = "OrganizationalUnitUpdate",
                    Active = true,
                    ActiveFrom = DateTime.Now,
                    ActiveTo = DateTime.Now.AddMonths(6),
                    TrackingState = TrackingState.Added,
                    ApplicationResource = res12,
                    Operation = updateOperation
                };

                _context.Permission.Add(rp123);
            }

            var res13 = _context.ApplicationResource.First(u => u.Name == "RoleGroupController");

            if (!_context.ApplicationResource.Any(u => u.Name == "RoleGroupCreate"))
            {
                var rp13 = new Permission
                {
                    Name = "RoleGroupCreate",
                    Active = true,
                    ActiveFrom = DateTime.Now,
                    ActiveTo = DateTime.Now.AddMonths(6),
                    TrackingState = TrackingState.Added,
                    ApplicationResource = res13,
                    Operation = createOperation
                };

                _context.Permission.Add(rp13);
            }

            if (!_context.ApplicationResource.Any(u => u.Name == "RoleGroupDelete"))
            {
                var rp131 = new Permission
                {
                    Name = "RoleGroupDelete",
                    Active = true,
                    ActiveFrom = DateTime.Now,
                    ActiveTo = DateTime.Now.AddMonths(6),
                    TrackingState = TrackingState.Added,
                    ApplicationResource = res13,
                    Operation = deleteOperation
                };

                _context.Permission.Add(rp131);
            }

            if (!_context.ApplicationResource.Any(u => u.Name == "RoleGroupRead"))
            {
                var rp132 = new Permission
                {
                    Name = "RoleGroupRead",
                    Active = true,
                    ActiveFrom = DateTime.Now,
                    ActiveTo = DateTime.Now.AddMonths(6),
                    TrackingState = TrackingState.Added,
                    ApplicationResource = res13,
                    Operation = readOperation
                };

                _context.Permission.Add(rp132);
            }

            if (!_context.ApplicationResource.Any(u => u.Name == "RoleGroupUpdate"))
            {
                var rp133 = new Permission
                {
                    Name = "RoleGroupUpdate",
                    Active = true,
                    ActiveFrom = DateTime.Now,
                    ActiveTo = DateTime.Now.AddMonths(6),
                    TrackingState = TrackingState.Added,
                    ApplicationResource = res13,
                    Operation = updateOperation
                };

                _context.Permission.Add(rp133);
            }

            var res14 = _context.ApplicationResource.First(u => u.Name == "UserProfileController");

            if (!_context.ApplicationResource.Any(u => u.Name == "UserProfileCreate"))
            {
                var rp14 = new Permission
                {
                    Name = "UserProfileCreate",
                    Active = true,
                    ActiveFrom = DateTime.Now,
                    ActiveTo = DateTime.Now.AddMonths(6),
                    TrackingState = TrackingState.Added,
                    ApplicationResource = res14,
                    Operation = createOperation
                };

                _context.Permission.Add(rp14);
            }

            if (!_context.ApplicationResource.Any(u => u.Name == "UserProfileDelete"))
            {
                var rp141 = new Permission
                {
                    Name = "UserProfileDelete",
                    Active = true,
                    ActiveFrom = DateTime.Now,
                    ActiveTo = DateTime.Now.AddMonths(6),
                    TrackingState = TrackingState.Added,
                    ApplicationResource = res14,
                    Operation = deleteOperation
                };

                _context.Permission.Add(rp141);
            }

            if (!_context.ApplicationResource.Any(u => u.Name == "UserProfileRead"))
            {
                var rp142 = new Permission
                {
                    Name = "UserProfileRead",
                    Active = true,
                    ActiveFrom = DateTime.Now,
                    ActiveTo = DateTime.Now.AddMonths(6),
                    TrackingState = TrackingState.Added,
                    ApplicationResource = res14,
                    Operation = readOperation
                };

                _context.Permission.Add(rp142);
            }

            if (!_context.ApplicationResource.Any(u => u.Name == "UserProfileUpdate"))
            {
                var rp143 = new Permission
                {
                    Name = "UserProfileUpdate",
                    Active = true,
                    ActiveFrom = DateTime.Now,
                    ActiveTo = DateTime.Now.AddMonths(6),
                    TrackingState = TrackingState.Added,
                    ApplicationResource = res14,
                    Operation = updateOperation
                };

                _context.Permission.Add(rp143);
            }

            _context.SaveChanges();

            #endregion Resource Permissions

            #region Resource Permissions Group

            // "Permissions" each contain a "Resource" and an "Operations"
            // examnple: a Permission named: "DeleteEmployee" contains an "Employee" resource and a "Delete" operation
            // "ResourceGroups" each contain an n-number of "Permissions"
            // example: "EmployeeResourceGroup" contains "DeleteEmployee", "CreateEmployee", "EditEmployee" Permissions, that are in turn comprised of an "Employee" resource coupled with (respectively) a "Delete", "Create" and "Edit" "Operation".

            var rperm1 = _context.Permission.First(u => u.Name == "EmployeeControllerTotalGrant");
            var rperm2 = _context.Permission.First(u => u.Name == "EmployeeDelete");
            var rperm3 = _context.Permission.First(u => u.Name == "EmployeeCreate");
            var rperm4 = _context.Permission.First(u => u.Name == "EmployeeUpdate");
            var rperm5 = _context.Permission.First(u => u.Name == "EmployeeRead");

            var rpg1 = new PermissionGroup
            {
                Name = "EmployeePermissionGroup",
                Active = true,
                ActiveFrom = DateTime.Now,
                ActiveTo = DateTime.Now.AddMonths(6),
                TrackingState = TrackingState.Added,
            };

            var rpgrpLink11 = new PermissionGroupOwnsPermissionLink
            {
                Active = true,
                ActiveFrom = DateTime.Now,
                ActiveTo = DateTime.Now.AddMonths(6),
                TrackingState = TrackingState.Added,
                PermissionGroup = rpg1,
                Permission = rperm1
            };

            var rpgrpLink12 = new PermissionGroupOwnsPermissionLink
            {
                Active = true,
                ActiveFrom = DateTime.Now,
                ActiveTo = DateTime.Now.AddMonths(6),
                TrackingState = TrackingState.Added,
                PermissionGroup = rpg1,
                Permission = rperm2
            };

            var rpgrpLink13 = new PermissionGroupOwnsPermissionLink
            {
                Active = true,
                ActiveFrom = DateTime.Now,
                ActiveTo = DateTime.Now.AddMonths(6),
                TrackingState = TrackingState.Added,
                PermissionGroup = rpg1,
                Permission = rperm3
            };

            var rpgrpLink14 = new PermissionGroupOwnsPermissionLink
            {
                Active = true,
                ActiveFrom = DateTime.Now,
                ActiveTo = DateTime.Now.AddMonths(6),
                TrackingState = TrackingState.Added,
                PermissionGroup = rpg1,
                Permission = rperm4
            };

            var rpgrpLink15 = new PermissionGroupOwnsPermissionLink
            {
                Active = true,
                ActiveFrom = DateTime.Now,
                ActiveTo = DateTime.Now.AddMonths(6),
                TrackingState = TrackingState.Added,
                PermissionGroup = rpg1,
                Permission = rperm5
            };

            _context.PermissionGroupOwnsPermissionLink.Add(rpgrpLink11);
            _context.PermissionGroupOwnsPermissionLink.Add(rpgrpLink12);
            _context.PermissionGroupOwnsPermissionLink.Add(rpgrpLink13);
            _context.PermissionGroupOwnsPermissionLink.Add(rpgrpLink14);
            _context.PermissionGroupOwnsPermissionLink.Add(rpgrpLink15);

            var rperm41 = _context.Permission.First(u => u.Name == "UserProfileControllerTotalGrant");
            var rperm51 = _context.Permission.First(u => u.Name == "UserProfileImageUpload");
            var rperm6 = _context.Permission.First(u => u.Name == "UserProfileImageDelete");

            var rpg21 = new PermissionGroup
            {
                Name = "UserProfilePermissionGroup",
                Active = true,
                ActiveFrom = DateTime.Now,
                ActiveTo = DateTime.Now.AddMonths(6),
                TrackingState = TrackingState.Added,
            };

            var rpgrpLink21 = new PermissionGroupOwnsPermissionLink
            {
                Active = true,
                ActiveFrom = DateTime.Now,
                ActiveTo = DateTime.Now.AddMonths(6),
                TrackingState = TrackingState.Added,
                PermissionGroup = rpg21,
                Permission = rperm4
            };

            var rpgrpLink22 = new PermissionGroupOwnsPermissionLink
            {
                Active = true,
                ActiveFrom = DateTime.Now,
                ActiveTo = DateTime.Now.AddMonths(6),
                TrackingState = TrackingState.Added,
                PermissionGroup = rpg21,
                Permission = rperm5
            };

            var rpgrpLink23 = new PermissionGroupOwnsPermissionLink
            {
                Active = true,
                ActiveFrom = DateTime.Now,
                ActiveTo = DateTime.Now.AddMonths(6),
                TrackingState = TrackingState.Added,
                PermissionGroup = rpg21,
                Permission = rperm6
            };

            _context.PermissionGroupOwnsPermissionLink.Add(rpgrpLink21);
            _context.PermissionGroupOwnsPermissionLink.Add(rpgrpLink22);
            _context.PermissionGroupOwnsPermissionLink.Add(rpgrpLink23);

            _context.SaveChanges();

            #endregion Resource Permissions Group

            #region Organizational Units

            if (!_context.OrganisationalUnit.Any(u => u.Name == "Developers"))
            {
                var orgUnit = new OrganizationalUnit
                {
                    Name = "Developers",
                    Description = "DescriptionDescriptionDescriptionDescription",
                    SecurityWeight = 0,
                    Active = true,
                    ActiveFrom = DateTime.Now,
                    ActiveTo = DateTime.Now.AddMonths(6),
                    TrackingState = TrackingState.Added
                };

                _context.OrganisationalUnit.Add(orgUnit);
            }

            if (!_context.OrganisationalUnit.Any(u => u.Name == "DirectorsOffice"))
            {
                var orgUnit = new OrganizationalUnit
                {
                    Name = "DirectorsOffice",
                    Description = "DescriptionDescriptionDescriptionDescription",
                    SecurityWeight = 1,
                    Active = true,
                    ActiveFrom = DateTime.Now,
                    ActiveTo = DateTime.Now.AddMonths(6),
                    TrackingState = TrackingState.Added
                };

                _context.OrganisationalUnit.Add(orgUnit);
            }

            if (!_context.OrganisationalUnit.Any(u => u.Name == "HR"))
            {
                var orgUnit = new OrganizationalUnit
                {
                    Name = "HR",
                    Description = "DescriptionDescriptionDescriptionDescription",
                    SecurityWeight = 2,
                    Active = true,
                    ActiveFrom = DateTime.Now,
                    ActiveTo = DateTime.Now.AddMonths(6),
                    TrackingState = TrackingState.Added
                };

                _context.OrganisationalUnit.Add(orgUnit);
            }

            if (!_context.OrganisationalUnit.Any(u => u.Name == "Administration"))
            {
                var orgUnit = new OrganizationalUnit
                {
                    Name = "Administration",
                    Description = "DescriptionDescriptionDescriptionDescription",
                    SecurityWeight = 3,
                    Active = true,
                    ActiveFrom = DateTime.Now,
                    ActiveTo = DateTime.Now.AddMonths(6),
                    TrackingState = TrackingState.Added
                };

                _context.OrganisationalUnit.Add(orgUnit);
            }

            if (!_context.OrganisationalUnit.Any(u => u.Name == "PointOfSales"))
            {
                var orgUnit = new OrganizationalUnit
                {
                    Name = "PointOfSales",
                    Description = "DescriptionDescriptionDescriptionDescription",
                    SecurityWeight = 4,
                    Active = true,
                    ActiveFrom = DateTime.Now,
                    ActiveTo = DateTime.Now.AddMonths(6),
                    TrackingState = TrackingState.Added
                };

                _context.OrganisationalUnit.Add(orgUnit);
            }

            _context.SaveChanges();

            #endregion Organizational Units

            #region Roles

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

                manager.Create(role);
            }

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

                manager.Create(role);
            }

            // a role can optionally belong to either a group of roles or *directly* to an organizational unit (or an individual user)
            // organizational units will normally contain an n-number of role groups (each role group having an n-number of roles).

            #region Add basic roles to the Developers OU

            var org1 = _context.OrganisationalUnit.First(u => u.Name == "Developers");
            var role1 = _context.Roles.First(u => u.Name == "Admin");

            var orgUnitRoleLink1 = new OrgUnitContainsRoleLink
            {
                OrganizationalUnit = org1,
                Role = (ApplicationRole)role1
            };

            var org2 = _context.OrganisationalUnit.First(u => u.Name == "Developers");
            var role2 = _context.Roles.First(u => u.Name == "Standard");

            var orgUnitRoleLink2 = new OrgUnitContainsRoleLink
            {
                OrganizationalUnit = org2,
                Role = (ApplicationRole)role2
            };

            var org3 = _context.OrganisationalUnit.First(u => u.Name == "Developers");
            var role3 = _context.Roles.First(u => u.Name == "Guest");

            var orgUnitRoleLink3 = new OrgUnitContainsRoleLink
            {
                OrganizationalUnit = org3,
                Role = (ApplicationRole)role3
            };

            _context.OrgUnitRoleLink.Add(orgUnitRoleLink1);
            _context.OrgUnitRoleLink.Add(orgUnitRoleLink2);
            _context.OrgUnitRoleLink.Add(orgUnitRoleLink3);

            #endregion Add basic roles to the Developers OU

            #region Add basic roles to the Directors Office OU

            var org11 = _context.OrganisationalUnit.First(u => u.Name == "DirectorsOffice");
            var role11 = _context.Roles.First(u => u.Name == "Admin");

            var orgUnitRoleLink11 = new OrgUnitContainsRoleLink
            {
                OrganizationalUnit = org11,
                Role = (ApplicationRole)role11
            };

            var org21 = _context.OrganisationalUnit.First(u => u.Name == "DirectorsOffice");
            var role21 = _context.Roles.First(u => u.Name == "Standard");

            var orgUnitRoleLink21 = new OrgUnitContainsRoleLink
            {
                OrganizationalUnit = org21,
                Role = (ApplicationRole)role21
            };

            var org31 = _context.OrganisationalUnit.First(u => u.Name == "DirectorsOffice");
            var role31 = _context.Roles.First(u => u.Name == "Guest");

            var orgUnitRoleLink31 = new OrgUnitContainsRoleLink
            {
                OrganizationalUnit = org31,
                Role = (ApplicationRole)role31
            };

            _context.OrgUnitRoleLink.Add(orgUnitRoleLink11);
            _context.OrgUnitRoleLink.Add(orgUnitRoleLink21);
            _context.OrgUnitRoleLink.Add(orgUnitRoleLink31);

            #endregion Add basic roles to the Directors Office OU

            #region Add basic roles to the PointOfSales OU

            var org12 = _context.OrganisationalUnit.First(u => u.Name == "PointOfSales");
            var role12 = _context.Roles.First(u => u.Name == "Admin");

            var orgUnitRoleLink12 = new OrgUnitContainsRoleLink
            {
                OrganizationalUnit = org12,
                Role = (ApplicationRole)role12
            };

            var org22 = _context.OrganisationalUnit.First(u => u.Name == "PointOfSales");
            var role22 = _context.Roles.First(u => u.Name == "Standard");

            var orgUnitRoleLink22 = new OrgUnitContainsRoleLink
            {
                OrganizationalUnit = org22,
                Role = (ApplicationRole)role22
            };

            var org32 = _context.OrganisationalUnit.First(u => u.Name == "PointOfSales");
            var role32 = _context.Roles.First(u => u.Name == "Guest");

            var orgUnitRoleLink32 = new OrgUnitContainsRoleLink
            {
                OrganizationalUnit = org32,
                Role = (ApplicationRole)role32
            };

            _context.OrgUnitRoleLink.Add(orgUnitRoleLink12);
            _context.OrgUnitRoleLink.Add(orgUnitRoleLink22);
            _context.OrgUnitRoleLink.Add(orgUnitRoleLink32);

            #endregion Add basic roles to the PointOfSales OU

            _context.SaveChanges();

            #endregion Roles

            #region Users

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

            #endregion Users

            #region Employee

            // Create test employees out of existing test users

            var user1 = _context.Users.SingleOrDefault(u => u.FirstName == "Bruno" && u.LastName == "Božić");

            if (!_context.Employee.Any(u => u.Name == "Bruno" && u.Surname == "Božić"))
            {
                var emp = new Employee
                {
                    Name = "Bruno",
                    Surname = "Božić",
                    Active = true,
                    ActiveFrom = DateTime.Now,
                    ActiveTo = DateTime.Now.AddMonths(6),
                    TrackingState = TrackingState.Added,
                    ApplicationUser = user1
                };

                _context.Employee.Add(emp);
            }

            var user2 = _context.Users.SingleOrDefault(u => u.FirstName == "Application" && u.LastName == "Standard User");

            if (!_context.Employee.Any(u => u.Name == "Application" && u.Surname == "Standard User"))
            {
                var emp = new Employee
                {
                    Name = "Application",
                    Surname = "Standard User",
                    Active = true,
                    ActiveFrom = DateTime.Now,
                    ActiveTo = DateTime.Now.AddMonths(6),
                    TrackingState = TrackingState.Added,
                    ApplicationUser = user2
                };

                _context.Employee.Add(emp);
            }

            var user3 = _context.Users.SingleOrDefault(u => u.FirstName == "Application" && u.LastName == "Administrator");

            if (!_context.Employee.Any(u => u.Name == "Application" && u.Surname == "Administrator"))
            {
                var emp = new Employee
                {
                    Name = "Application",
                    Surname = "Administrator",
                    Active = true,
                    ActiveFrom = DateTime.Now,
                    ActiveTo = DateTime.Now.AddMonths(6),
                    TrackingState = TrackingState.Added,
                    ApplicationUser = user3
                };

                _context.Employee.Add(emp);
            }

            var user4 = _context.Users.SingleOrDefault(u => u.FirstName == "Application" && u.LastName == "Administrator");

            if (!_context.Employee.Any(u => u.Name == "Application" && u.Surname == "AutomatedTasks"))
            {
                var emp = new Employee
                {
                    Name = "Application",
                    Surname = "AutomatedTasks",
                    Active = true,
                    ActiveFrom = DateTime.Now,
                    ActiveTo = DateTime.Now.AddMonths(6),
                    TrackingState = TrackingState.Added,
                    ApplicationUser = user4
                };

                _context.Employee.Add(emp);
            }

            _context.SaveChanges();

            // Attach Employees to Organizational Units

            var myOrgUnit = _context.OrganisationalUnit.SingleOrDefault(u => u.Name == "Developers");
            var myOrgUnit2 = _context.OrganisationalUnit.SingleOrDefault(u => u.Name == "PointOfSales");

            var employee1 = _context.Employee.SingleOrDefault(u => u.Name == "Bruno" && u.Surname == "Božić");
            var employee2 = _context.Employee.SingleOrDefault(u => u.Name == "Application" && u.Surname == "Administrator");
            var employee3 = _context.Employee.SingleOrDefault(u => u.Name == "Application" && u.Surname == "Standard User");

            var empOrgUnit1 = new EmployeeBelongsToOrgUnitLink
            {
                Employee = employee1,
                OrganizationalUnit = myOrgUnit,
                Active = true,
                ActiveFrom = DateTime.Now,
                ActiveTo = DateTime.Now.AddMonths(6),
                TrackingState = TrackingState.Added
            };

            var empOrgUnit2 = new EmployeeBelongsToOrgUnitLink
            {
                Employee = employee2,
                OrganizationalUnit = myOrgUnit,
                Active = true,
                ActiveFrom = DateTime.Now,
                ActiveTo = DateTime.Now.AddMonths(6),
                TrackingState = TrackingState.Added
            };

            var empOrgUnit3 = new EmployeeBelongsToOrgUnitLink
            {
                Employee = employee3,
                OrganizationalUnit = myOrgUnit,
                Active = true,
                ActiveFrom = DateTime.Now,
                ActiveTo = DateTime.Now.AddMonths(6),
                TrackingState = TrackingState.Added
            };

            var empOrgUnit11 = new EmployeeBelongsToOrgUnitLink
            {
                Employee = employee1,
                OrganizationalUnit = myOrgUnit2,
                Active = true,
                ActiveFrom = DateTime.Now,
                ActiveTo = DateTime.Now.AddMonths(6),
                TrackingState = TrackingState.Added
            };

            var empOrgUnit22 = new EmployeeBelongsToOrgUnitLink
            {
                Employee = employee2,
                OrganizationalUnit = myOrgUnit2,
                Active = true,
                ActiveFrom = DateTime.Now,
                ActiveTo = DateTime.Now.AddMonths(6),
                TrackingState = TrackingState.Added
            };

            var empOrgUnit33 = new EmployeeBelongsToOrgUnitLink
            {
                Employee = employee3,
                OrganizationalUnit = myOrgUnit2,
                Active = true,
                ActiveFrom = DateTime.Now,
                ActiveTo = DateTime.Now.AddMonths(6),
                TrackingState = TrackingState.Added
            };

            _context.EmployeesBelongToOgranizationalUnits.Add(empOrgUnit1);
            _context.EmployeesBelongToOgranizationalUnits.Add(empOrgUnit2);
            _context.EmployeesBelongToOgranizationalUnits.Add(empOrgUnit3);
            _context.EmployeesBelongToOgranizationalUnits.Add(empOrgUnit11);
            _context.EmployeesBelongToOgranizationalUnits.Add(empOrgUnit22);
            _context.EmployeesBelongToOgranizationalUnits.Add(empOrgUnit33);

            _context.SaveChanges();

            #endregion Employee



            return true;
        }
    }
}