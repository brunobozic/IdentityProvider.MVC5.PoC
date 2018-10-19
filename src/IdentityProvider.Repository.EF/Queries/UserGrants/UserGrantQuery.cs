using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using IdentityProvider.Infrastructure.Logging.Serilog;
using IdentityProvider.Repository.EF.EFDataContext;

namespace IdentityProvider.Repository.EF.Queries.UserGrants
{
    public class UserGrantQuery
    {
        private readonly AppDbContext _context;
        private readonly ISerilogLoggingFactory _loggingFactory;

        public UserGrantQuery(
            AppDbContext context
            , ISerilogLoggingFactory loggingFactory
        )
        {
            _context = context;
            _loggingFactory = loggingFactory;
        }

        public string UserId { private get; set; } = "";

        public int EmployeeId { private get; set; } = 0;

        public UserGrantQueryResponse Execute()
        {
            var retVal = new UserGrantQueryResponse
            {
                Succes = false ,
                Message = "" ,
            };

            try
            {
                if (EmployeeId > 0)
                {
                    var query = ( from employeeOrgUnit in _context.EmployeesBelongToOgranizationalUnits
                                  join employee in _context.Employee on employeeOrgUnit.EmployeeId equals employee.Id
                                  join orgUnit in _context.OrganisationalUnit on employeeOrgUnit.OrganisationalUnitId equals
                                      orgUnit.Id
                                  join user in _context.Users on employee.ApplicationUser.Id equals user.Id
                                  where employeeOrgUnit.Active
                                        && employee.Active
                                        && orgUnit.Active
                                        && user.Active
                                        && employee.Id == EmployeeId
                                  select new GrantedPriviligesDto
                                  {
                                      //EplicitGrants = ( from qs in cl_sa.ChecklistSysauditQuestion
                                      //    orderby qs.Id
                                      //    select new ExplicitGrantsDto
                                      //    {
                                      //        ExplicitGrantId = 1,
                                      //        ExplicitGrantName = ""
                                      //    } ) ,
                                      OrganizationalUnits = ( List<OrganizationalUnitsDto> ) ( from ou in orgUnit.RoleGroups
                                                                                               select new OrganizationalUnitsDto
                                                                                               {
                                                                                                   OrganizationalUnitName = ou.OrganisationalUnit.Name ,
                                                                                                   OrganizationalUnitId = ou.OrganisationalUnit.Id ,
                                                                                                   OrganizationalUnitSecurityWeight = ou.OrganisationalUnit.SecurityWeight
                                                                                               } ) ,
                                      UserFirstName = user.FirstName ,
                                      UserLastName = user.LastName ,
                                      UserId = user.Id ,
                                      EmployeeTitle = user.UserName ,
                                      EmployeeId = user.Id ,
                                  } ).ToList();

                    retVal.GrantedPriviliges = query;
                }
                else if (!string.IsNullOrEmpty(UserId))
                {
                    var query = ( from employeeOrgUnit in _context.EmployeesBelongToOgranizationalUnits
                                  join employee in _context.Employee on employeeOrgUnit.EmployeeId equals employee.Id
                                  join orgUnit in _context.OrganisationalUnit on employeeOrgUnit.OrganisationalUnitId equals
                                      orgUnit.Id
                                  join user in _context.Users on employee.ApplicationUser.Id equals user.Id
                                  where employeeOrgUnit.Active
                                        && employee.Active
                                        && orgUnit.Active
                                        && user.Active
                                        && user.Id == UserId
                                  select new GrantedPriviligesDto
                                  {
                                      OrganizationalUnits = ( List<OrganizationalUnitsDto> ) ( from ou in orgUnit.RoleGroups
                                                                                               select new OrganizationalUnitsDto
                                                                                               {
                                                                                                   OrganizationalUnitName = ou.OrganisationalUnit.Name ,
                                                                                                   OrganizationalUnitId = ou.OrganisationalUnit.Id ,
                                                                                                   OrganizationalUnitSecurityWeight = ou.OrganisationalUnit.SecurityWeight
                                                                                               } ) ,
                                      UserFirstName = user.FirstName ,
                                      UserLastName = user.LastName ,
                                      UserId = user.Id ,
                                      EmployeeTitle = user.UserName ,
                                      EmployeeId = user.Id ,
                                  } ).ToList();

                    retVal.GrantedPriviliges = query;
                }


                retVal.Succes = true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Debug.WriteLine(e);
                _loggingFactory.GetLogger(SerilogLogTypesEnum.ErrorDbLog).Error(e , "{Exception}");
                retVal.Message = e.Message;
            }

            return retVal;
        }
    }
}
