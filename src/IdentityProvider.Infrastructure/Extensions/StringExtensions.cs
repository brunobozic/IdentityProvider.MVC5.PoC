namespace IdentityProvider.Infrastructure.Extensions
{
    public static class StringExtensions
    {
        public static string CleanUpSerilogProperties(this string serilogProperty)
        {
            var returnValue = string.Empty;

            serilogProperty = serilogProperty.Replace("[", "");
            serilogProperty = serilogProperty.Replace("]", "");
            serilogProperty = serilogProperty.Replace(" ", "");
            serilogProperty = serilogProperty.Replace(",", "");
            serilogProperty = serilogProperty.Replace("\"", "");

            returnValue = serilogProperty;

            return returnValue;
        }
    }
}