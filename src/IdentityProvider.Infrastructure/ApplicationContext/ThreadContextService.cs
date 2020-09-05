using AutoMapper;
using System;
using System.IO;
using System.Threading;

namespace IdentityProvider.Infrastructure.ApplicationContext
{
    public class ThreadContextService : IContextProvider
    {
        private IMapper _mapper;

        public ThreadContextService(IMapper mapper)
        {
            _mapper = mapper;
        }

        public string GetContextualFullFilePath(string fileName)
        {
            var dir = Directory.GetCurrentDirectory();
            var resourceFileInfo = new FileInfo(Path.Combine(dir, fileName));
            return resourceFileInfo.FullName;
        }

        public string GetUserName()
        {
            var userName = "<null>";

            try
            {
                if (Thread.CurrentPrincipal != null)
                    userName = Thread.CurrentPrincipal.Identity.IsAuthenticated
                        ? Thread.CurrentPrincipal.Identity.Name
                        : "<null>";
            }
            catch
            {
            }

            return userName;
        }

        public ContextDataModel GetContextProperties()
        {
            return new ContextDataModel();
        }

        public string FakeUserNameForTestingPurposes
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

        public string FakeGetContextualFullFilePath
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

        public void Dispose()
        {
            // nothing to dispose of at the moment...
        }
    }


    public class FakeThreadContextService : IContextProvider
    {
        public string GetContextualFullFilePath { get; set; } = @"FileNameForTestingPurposes";

        public string FakeUserNameForTestingPurposes { get; set; } = "TestUser";

        public string FakeGetContextualFullFilePath
        {
            get => GetContextualFullFilePath;
            set => GetContextualFullFilePath = value;
        }

        string IContextProvider.GetContextualFullFilePath(string fileName)
        {
            throw new NotImplementedException();
        }

        public string GetUserName()
        {
            var userName = FakeUserNameForTestingPurposes;

            return userName;
        }

        public ContextDataModel GetContextProperties()
        {
            return new ContextDataModel();
        }

        public void Dispose()
        {
            // nothing to dispose of at the moment...
        }
    }
}