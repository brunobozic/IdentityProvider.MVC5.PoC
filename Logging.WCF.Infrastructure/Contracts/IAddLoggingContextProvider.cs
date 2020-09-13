using Logging.WCF.Models;

namespace Logging.WCF.Infrastructure.Contracts
{
    public interface IAddLoggingContextProvider
    {
        string FakeUserNameForTestingPurposes { get; }

        string GetContextualFullFilePath(string fileName);
        string GetUserName();
        ContextDataModel GetContextProperties();
    }
}