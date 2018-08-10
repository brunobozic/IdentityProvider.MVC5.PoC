namespace IdentityProvider.Infrastructure.ApplicationContext
{
    public interface IAddLoggingContextProvider
    {
        string FakeUserNameForTestingPurposes { get; }

        string GetContextualFullFilePath(string fileName);
        string GetUserName();
        ContextDataModel GetContextProperties();
    }
}