using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using IdentityProvider.Infrastructure.Logging.Log4Net;
using IdentityProvider.Models.Domain.Account;
using Module.Repository.EF.UnitOfWorkInterfaces;
using Module.ServicePattern;

namespace IdentityProvider.Services.ResourceService
{

    public class ApplicationResourceService : Service<ApplicationResource>, IApplicationResourceService
    {
        private readonly ILog4NetLoggingService _loggingService;
        private readonly IMapper _mapper;
        private readonly ApplicationSignInManager _signInManager;
        private readonly IUnitOfWorkAsync _unitOfWorkAsync;
        private readonly ApplicationUserManager _userManager;

        [StructureMap.DefaultConstructor] // Set Default Constructor for StructureMap
        public ApplicationResourceService(
            IUnitOfWorkAsync unitOfWorkAsync
            , ILog4NetLoggingService loggingService
            , IMapper mapper
            , ApplicationSignInManager signInManager
            , ApplicationUserManager userManager
        ) : base(unitOfWorkAsync.RepositoryAsync<ApplicationResource>())
        {
            _unitOfWorkAsync = unitOfWorkAsync;
            _loggingService = loggingService;
            _mapper = mapper;
            _signInManager = signInManager;
            _userManager = userManager;
        }

   
    }
}