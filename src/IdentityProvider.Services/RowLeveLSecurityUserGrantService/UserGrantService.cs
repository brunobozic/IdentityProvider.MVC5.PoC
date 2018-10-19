using System.Web.Mvc;
using AutoMapper;
using IdentityProvider.Infrastructure.Logging.Log4Net;
using IdentityProvider.Infrastructure.Logging.Serilog;
using IdentityProvider.Models.Domain.Account;
using IdentityProvider.Repository.EF.EFDataContext;
using IdentityProvider.Repository.EF.Queries.UserGrants;
using Module.Repository.EF.Repositories;
using Module.Repository.EF.UnitOfWorkInterfaces;
using StructureMap;


namespace IdentityProvider.Services.RowLeveLSecurityUserGrantService
{
    public class UserGrantService : IUserGrantService
    {
        private readonly ILog4NetLoggingService _loggingService;
        private readonly IMapper _mapper;
        private readonly ApplicationSignInManager _signInManager;
        private readonly IUnitOfWorkAsync _unitOfWorkAsync;
        private readonly ApplicationUserManager _userManager;
        private readonly ApplicationRoleManager _roleManager;

        [DefaultConstructor]  //This is the attribute you need to add on the constructor
        public UserGrantService(
            IUnitOfWorkAsync unitOfWorkAsync
            , ILog4NetLoggingService loggingService
            , IMapper mapper
            , IRepositoryAsync<ApplicationRole> repository
            , ApplicationSignInManager signInManager
            , ApplicationUserManager userManager
            , ApplicationRoleManager roleManager
        )
        {
            _unitOfWorkAsync = unitOfWorkAsync;
            _loggingService = loggingService;
            _mapper = mapper;
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public GrantedPriviligesResponse OrgUnitGrantedPriviligesByUser( string userId )
        {
            var retVal = new GrantedPriviligesResponse();

            var context = ( AppDbContext ) DependencyResolver.Current.GetService(typeof(AppDbContext));
            var loggingFactory = ( ISerilogLoggingFactory ) DependencyResolver.Current.GetService(typeof(ISerilogLoggingFactory));

            var q = new UserGrantQuery(context , loggingFactory) { UserId = userId };

            var queryResponse = q.Execute();

            if (queryResponse.Succes)
            {
                retVal.Success = true;
                retVal.GrantedPriviliges = queryResponse.GrantedPriviliges;

                return retVal;
            }
            else
            {
                retVal.Message = queryResponse.Message;
            }

            return retVal;
        }

        public GrantedPriviligesResponse OrgUnitGrantedPriviligesByEmployee( int employeeId )
        {
            var retVal = new GrantedPriviligesResponse();

            var context = ( AppDbContext ) DependencyResolver.Current.GetService(typeof(AppDbContext));
            var loggingFactory = ( ISerilogLoggingFactory ) DependencyResolver.Current.GetService(typeof(ISerilogLoggingFactory));

            var q = new UserGrantQuery(context , loggingFactory) { EmployeeId = employeeId };

            var queryResponse = q.Execute();

            if (queryResponse.Succes)
            {
                retVal.Success = true;
                retVal.GrantedPriviliges = queryResponse.GrantedPriviliges;

                return retVal;
            }
            else
            {
                retVal.Message = queryResponse.Message;
            }

            return retVal;
        }
    }
}