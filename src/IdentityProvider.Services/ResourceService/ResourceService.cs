using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using IdentityProvider.Infrastructure.Logging.Log4Net;
using IdentityProvider.Models.Domain.Account;
using Module.Repository.EF.UnitOfWorkInterfaces;

namespace IdentityProvider.Services.ResourceService
{

    public class ResourceService
    {
        private readonly ILog4NetLoggingService _loggingService;
        private readonly IMapper _mapper;
        private readonly ApplicationSignInManager _signInManager;
        private readonly IUnitOfWorkAsync _unitOfWorkAsync;
        private readonly ApplicationUserManager _userManager;

        [StructureMap.DefaultConstructor] // Set Default Constructor for StructureMap
        public ResourceService(
            IUnitOfWorkAsync unitOfWorkAsync
            , ILog4NetLoggingService loggingService
            , IMapper mapper
            , ApplicationSignInManager signInManager
            , ApplicationUserManager userManager
        )
        {
            _unitOfWorkAsync = unitOfWorkAsync;
            _loggingService = loggingService;
            _mapper = mapper;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public int AddResource(int id, string name, List<Operation> operations)
        {
            var resource = new Resource
            {
                Id = id,
                Name = name,
                Operations = operations
            };

            _unitOfWorkAsync.RepositoryAsync<Resource>().Insert(resource);
            _unitOfWorkAsync.Commit();

            return resource.Id;
        }


        public bool FoundResource(int id)
        {
            var resource = _unitOfWorkAsync.RepositoryAsync<Resource>().Queryable().Single(i => i.Id.Equals(id));

            return resource != null;
        }



    

   
    }
}