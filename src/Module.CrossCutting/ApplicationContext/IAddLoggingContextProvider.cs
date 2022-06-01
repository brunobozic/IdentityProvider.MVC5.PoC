namespace Module.CrossCutting.ApplicationContext
{
    public interface IAddLoggingContextProvider
    {
        string FakeUserNameForTestingPurposes { get; }

        string GetContextualFullFilePath(string fileName);
        string GetUserName();
        ContextDataModel GetContextProperties();
    }
}