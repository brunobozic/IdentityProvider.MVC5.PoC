using System;

namespace IdentityProvider.Infrastructure.ApplicationContext
{
    public interface IContextProvider : IDisposable
    {
        string FakeUserNameForTestingPurposes { get; set; }
        string FakeGetContextualFullFilePath { get; set; }
        string GetContextualFullFilePath(string fileName);
        string GetUserName();
        ContextDataModel GetContextProperties();
    }
}