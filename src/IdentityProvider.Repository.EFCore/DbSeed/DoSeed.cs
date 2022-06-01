using IdentityProvider.Repository.EFCore.Domain.Account.Employees;
using IdentityProvider.Repository.EFCore.Domain.OrganizationalUnits;
using IdentityProvider.Repository.EFCore.Domain.ResourceOperations;
using IdentityProvider.Repository.EFCore.Domain.Roles;
using IdentityProvider.Repository.EFCore.EFDataContext;
using StructureMap;
using System;
using System.Collections.Generic;
using System.Linq;
using TrackableEntities.Common.Core;

namespace IdentityProvider.Repository.EFCore.DbSeed
{
    public class DoSeed : IDoSeed
    {
        private readonly AppDbContext _context;

        [DefaultConstructor] // Set Default Constructor for StructureMap
        public DoSeed(AppDbContext context)
        {
            _context = context;
        }

        public bool Seed()
        {
            // IMPORTANT!
            // Please note that the identity users, several employee entities and basic roles have already been seeded (see program.cs, the code for that can be found there)

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

            #region Resource

            // Adding a list of basic application resources (in the form of either mvc controllers or abstract resources, see examples)

            var resources = new List<Resource>();

            if (!_context.ApplicationResource.Any(u => u.Name == "ManageController"))
            {
                var r = new Resource
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
                var r = new Resource
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
                var r = new Resource
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
                var r = new Resource
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
                var r = new Resource
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
                var r = new Resource
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
                var r = new Resource
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
                var r = new Resource
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
                var r = new Resource
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
                var r = new Resource
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
                var r = new Resource
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
                var r = new Resource
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
                var r = new Resource
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

            if (!_context.ApplicationResource.Any(u => u.Name == "LockedUserAccount"))
            {
                var r = new Resource
                {
                    Name = "LockedUserAccount",
                    Active = true,
                    ActiveFrom = DateTime.Now,
                    ActiveTo = DateTime.Now.AddMonths(6),
                    TrackingState = TrackingState.Added,
                    Description = "LockedUserAccount"
                };

                _context.ApplicationResource.Add(r);
            }

            _context.SaveChanges();

            #endregion Resource

            // a role can optionally belong to either a group of roles or *directly* to an organizational unit (or an individual user)
            // organizational units will normally contain an n-number of role groups (each role group having an n-number of roles).

            #region Add basic roles to the Developers OU

            var org1 = _context.OrganisationalUnit.First(u => u.Name == "Developers");
            var role1 = _context.Role.First(u => u.Name == "Admin");

            var orgUnitRoleLink1 = new OrgUnitContainsRole
            {
                OrganizationalUnit = org1,
                Role = (AppRole)role1
            };

            var org2 = _context.OrganisationalUnit.First(u => u.Name == "Developers");
            var role2 = _context.Role.First(u => u.Name == "Standard");

            var orgUnitRoleLink2 = new OrgUnitContainsRole
            {
                OrganizationalUnit = org2,
                Role = (AppRole)role2
            };

            var org3 = _context.OrganisationalUnit.First(u => u.Name == "Developers");
            var role3 = _context.Role.First(u => u.Name == "Guest");

            var orgUnitRoleLink3 = new OrgUnitContainsRole
            {
                OrganizationalUnit = org3,
                Role = (AppRole)role3
            };

            _context.OrgUnitRoleLink.Add(orgUnitRoleLink1);
            _context.OrgUnitRoleLink.Add(orgUnitRoleLink2);
            _context.OrgUnitRoleLink.Add(orgUnitRoleLink3);

            #endregion Add basic roles to the Developers OU

            #region Add basic roles to the Directors Office OU

            var org11 = _context.OrganisationalUnit.First(u => u.Name == "DirectorsOffice");
            var role11 = _context.Role.First(u => u.Name == "Admin");

            var orgUnitRoleLink11 = new OrgUnitContainsRole
            {
                OrganizationalUnit = org11,
                Role = (AppRole)role11
            };

            var org21 = _context.OrganisationalUnit.First(u => u.Name == "DirectorsOffice");
            var role21 = _context.Role.First(u => u.Name == "Standard");

            var orgUnitRoleLink21 = new OrgUnitContainsRole
            {
                OrganizationalUnit = org21,
                Role = (AppRole)role21
            };

            var org31 = _context.OrganisationalUnit.First(u => u.Name == "DirectorsOffice");
            var role31 = _context.Role.First(u => u.Name == "Guest");

            var orgUnitRoleLink31 = new OrgUnitContainsRole
            {
                OrganizationalUnit = org31,
                Role = (AppRole)role31
            };

            _context.OrgUnitRoleLink.Add(orgUnitRoleLink11);
            _context.OrgUnitRoleLink.Add(orgUnitRoleLink21);
            _context.OrgUnitRoleLink.Add(orgUnitRoleLink31);

            #endregion Add basic roles to the Directors Office OU

            #region Add basic roles to the PointOfSales OU

            var org12 = _context.OrganisationalUnit.First(u => u.Name == "PointOfSales");
            var role12 = _context.Role.First(u => u.Name == "Admin");

            var orgUnitRoleLink12 = new OrgUnitContainsRole
            {
                OrganizationalUnit = org12,
                Role = (AppRole)role12
            };

            var org22 = _context.OrganisationalUnit.First(u => u.Name == "PointOfSales");
            var role22 = _context.Role.First(u => u.Name == "Standard");

            var orgUnitRoleLink22 = new OrgUnitContainsRole
            {
                OrganizationalUnit = org22,
                Role = (AppRole)role22
            };

            var org32 = _context.OrganisationalUnit.First(u => u.Name == "PointOfSales");
            var role32 = _context.Role.First(u => u.Name == "Guest");

            var orgUnitRoleLink32 = new OrgUnitContainsRole
            {
                OrganizationalUnit = org32,
                Role = (AppRole)role32
            };

            _context.OrgUnitRoleLink.Add(orgUnitRoleLink12);
            _context.OrgUnitRoleLink.Add(orgUnitRoleLink22);
            _context.OrgUnitRoleLink.Add(orgUnitRoleLink32);

            #endregion Add basic roles to the PointOfSales OU

            _context.SaveChanges();

            //#region Role Group

            //if (!_context.RoleGroup.Any(r => r.Name == "CEO Office"))
            //{
            //    var rg0 = new RoleGroup
            //    {
            //        Name = "CEO Office",
            //        Active = true,
            //        ActiveFrom = DateTime.Now,
            //        ActiveTo = DateTime.Now.AddMonths(6),
            //        TrackingState = TrackingState.Added
            //    };

            //    _context.RoleGroup.Add(rg0);
            //    _context.SaveChanges();

            //}

            //if (!_context.RoleGroup.Any(r => r.Name == "COO Sector"))
            //{
            //    var rg1 = new RoleGroup
            //    {
            //        Name = "COO Sector",
            //        Active = true,
            //        ActiveFrom = DateTime.Now,
            //        ActiveTo = DateTime.Now.AddMonths(6),
            //        TrackingState = TrackingState.Added
            //    };

            //    _context.RoleGroup.Add(rg1);
            //    _context.SaveChanges();

            //}

            //if (!_context.RoleGroup.Any(r => r.Name == "Sales"))
            //{
            //    var rg2 = new RoleGroup
            //    {
            //        Name = "Sales",
            //        Active = true,
            //        ActiveFrom = DateTime.Now,
            //        ActiveTo = DateTime.Now.AddMonths(6),
            //        TrackingState = TrackingState.Added
            //    };

            //    _context.RoleGroup.Add(rg2);
            //    _context.SaveChanges();
            //}

            //if (!_context.RoleGroup.Any(r => r.Name == "Marketing"))
            //{
            //    var rg3 = new RoleGroup
            //    {
            //        Name = "Marketing",
            //        Active = true,
            //        ActiveFrom = DateTime.Now,
            //        ActiveTo = DateTime.Now.AddMonths(6),
            //        TrackingState = TrackingState.Added
            //    };

            //    _context.RoleGroup.Add(rg3);
            //    _context.SaveChanges();

            //}

            //#endregion

            //#region RoleGroupContainsRoleLink

            //var store2 = new RoleStore<IdentityFrameworkRole>(_context);

            //var myRg0 = _context.RoleGroup.Where(r => r.Name == "CEO Office").Single();
            //var myRg1 = _context.RoleGroup.Where(r => r.Name == "COO Sector").Single();
            //var myRg2 = _context.RoleGroup.Where(r => r.Name == "Sales").Single();
            //var myRg3 = _context.RoleGroup.Where(r => r.Name == "Marketing").Single();
            //var myRole1 = manager2.Roles.Where(role => role.Name == "Standard").Single();
            //var myRole2 = manager2.Roles.Where(role => role.Name == "Admin").Single();
            //var myRole3 = manager2.Roles.Where(role => role.Name == "Guest").Single();

            //if (_context.RoleGroup.Any(rg => rg.Name == "CEO Office"))
            //{
            //    var rgcrl0 = new RoleGroupContainsRole
            //    {
            //        RoleGroup = myRg0,
            //        Role = myRole2,
            //        Active = true,
            //        ActiveFrom = DateTime.Now,
            //        ActiveTo = DateTime.Now.AddMonths(6),
            //        TrackingState = TrackingState.Added
            //    };

            //    var rgcrl6 = new RoleGroupContainsRole
            //    {
            //        RoleGroup = myRg0,
            //        Role = myRole1,
            //        Active = true,
            //        ActiveFrom = DateTime.Now,
            //        ActiveTo = DateTime.Now.AddMonths(6),
            //        TrackingState = TrackingState.Added
            //    };
            //}

            //if (_context.RoleGroup.Any(rg => rg.Name == "COO Sector"))
            //{
            //    var rgcrl1 = new RoleGroupContainsRole
            //    {
            //        RoleGroup = myRg1,
            //        Role = myRole1,
            //        Active = true,
            //        ActiveFrom = DateTime.Now,
            //        ActiveTo = DateTime.Now.AddMonths(6),
            //        TrackingState = TrackingState.Added
            //    };
            //}

            //if (_context.RoleGroup.Any(rg => rg.Name == "Sales"))
            //{
            //    var rgcrl2 = new RoleGroupContainsRole
            //    {
            //        RoleGroup = myRg1,
            //        Role = myRole3,
            //        Active = true,
            //        ActiveFrom = DateTime.Now,
            //        ActiveTo = DateTime.Now.AddMonths(6),
            //        TrackingState = TrackingState.Added
            //    };

            //    var rgcrl7 = new RoleGroupContainsRole
            //    {
            //        RoleGroup = myRg1,
            //        Role = myRole1,
            //        Active = true,
            //        ActiveFrom = DateTime.Now,
            //        ActiveTo = DateTime.Now.AddMonths(6),
            //        TrackingState = TrackingState.Added
            //    };
            //}

            //if (_context.RoleGroup.Any(rg => rg.Name == "Marketing"))
            //{
            //    var rgcrl3 = new RoleGroupContainsRole
            //    {
            //        RoleGroup = myRg3,
            //        Role = myRole1,
            //        Active = true,
            //        ActiveFrom = DateTime.Now,
            //        ActiveTo = DateTime.Now.AddMonths(6),
            //        TrackingState = TrackingState.Added
            //    };

            //    var rgcrl8 = new RoleGroupContainsRole
            //    {
            //        RoleGroup = myRg3,
            //        Role = myRole3,
            //        Active = true,
            //        ActiveFrom = DateTime.Now,
            //        ActiveTo = DateTime.Now.AddMonths(6),
            //        TrackingState = TrackingState.Added
            //    };
            //}

            //#endregion

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

            _context.SaveChanges();

            var user2 = _context.Users.SingleOrDefault(u =>
                u.FirstName == "Application" && u.LastName == "Standard User");

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

            _context.SaveChanges();

            var user3 = _context.Users.SingleOrDefault(u =>
                u.FirstName == "Application" && u.LastName == "Administrator");

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

            _context.SaveChanges();

            //var user4 = _context.Users.SingleOrDefault(u => u.FirstName == "Application" && u.LastName == "Administrator");

            //if (!_context.Employee.Any(u => u.Name == "Application" && u.Surname == "AutomatedTasks"))
            //{
            //    var emp = new Employee
            //    {
            //        Name = "Application",
            //        Surname = "AutomatedTasks",
            //        Active = true,
            //        ActiveFrom = DateTime.Now,
            //        ActiveTo = DateTime.Now.AddMonths(6),
            //        TrackingState = TrackingState.Added,
            //        ApplicationUser = user4
            //    };

            //    _context.Employee.Add(emp);
            //}

            //_context.SaveChanges();

            // Attach Employees to Organizational Units

            var myOrgUnit = _context.OrganisationalUnit.SingleOrDefault(u => u.Name == "Developers");
            var myOrgUnit2 = _context.OrganisationalUnit.SingleOrDefault(u => u.Name == "PointOfSales");

            var employee1 = _context.Employee.SingleOrDefault(u => u.Name == "Bruno" && u.Surname == "Božić");
            var employee2 =
                _context.Employee.SingleOrDefault(u => u.Name == "Application" && u.Surname == "Administrator");
            var employee3 =
                _context.Employee.SingleOrDefault(u => u.Name == "Application" && u.Surname == "Standard User");

            var empOrgUnit1 = new EmployeeBelongsToOrgUnit
            {
                Employee = employee1,
                OrganizationalUnit = myOrgUnit,
                Active = true,
                ActiveFrom = DateTime.Now,
                ActiveTo = DateTime.Now.AddMonths(6),
                TrackingState = TrackingState.Added
            };

            _context.SaveChanges();

            var empOrgUnit2 = new EmployeeBelongsToOrgUnit
            {
                Employee = employee2,
                OrganizationalUnit = myOrgUnit,
                Active = true,
                ActiveFrom = DateTime.Now,
                ActiveTo = DateTime.Now.AddMonths(6),
                TrackingState = TrackingState.Added
            };

            _context.SaveChanges();

            var empOrgUnit3 = new EmployeeBelongsToOrgUnit
            {
                Employee = employee3,
                OrganizationalUnit = myOrgUnit,
                Active = true,
                ActiveFrom = DateTime.Now,
                ActiveTo = DateTime.Now.AddMonths(6),
                TrackingState = TrackingState.Added
            };

            _context.SaveChanges();

            var empOrgUnit11 = new EmployeeBelongsToOrgUnit
            {
                Employee = employee1,
                OrganizationalUnit = myOrgUnit2,
                Active = true,
                ActiveFrom = DateTime.Now,
                ActiveTo = DateTime.Now.AddMonths(6),
                TrackingState = TrackingState.Added
            };

            _context.SaveChanges();

            var empOrgUnit22 = new EmployeeBelongsToOrgUnit
            {
                Employee = employee2,
                OrganizationalUnit = myOrgUnit2,
                Active = true,
                ActiveFrom = DateTime.Now,
                ActiveTo = DateTime.Now.AddMonths(6),
                TrackingState = TrackingState.Added
            };

            _context.SaveChanges();

            var empOrgUnit33 = new EmployeeBelongsToOrgUnit
            {
                Employee = employee3,
                OrganizationalUnit = myOrgUnit2,
                Active = true,
                ActiveFrom = DateTime.Now,
                ActiveTo = DateTime.Now.AddMonths(6),
                TrackingState = TrackingState.Added
            };

            _context.SaveChanges();

            _context.EmployeesBelongToOgranizationalUnits.Add(empOrgUnit1);
            _context.SaveChanges();
            _context.EmployeesBelongToOgranizationalUnits.Add(empOrgUnit2);
            _context.SaveChanges();
            _context.EmployeesBelongToOgranizationalUnits.Add(empOrgUnit3);
            _context.SaveChanges();
            _context.EmployeesBelongToOgranizationalUnits.Add(empOrgUnit11);
            _context.SaveChanges();
            _context.EmployeesBelongToOgranizationalUnits.Add(empOrgUnit22);
            _context.SaveChanges();
            _context.EmployeesBelongToOgranizationalUnits.Add(empOrgUnit33);
            _context.SaveChanges();

            _context.SaveChanges();

            #endregion Employee

            return true;
        }
    }
}