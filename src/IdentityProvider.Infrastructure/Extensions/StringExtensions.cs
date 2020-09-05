namespace IdentityProvider.Infrastructure.Extensions
{
    public static class StringExtensions
    {
        public static string CleanUpSerilogProperties(this string serilogProperty)
        {
            var returnValue = string.Empty;

            serilogProperty = serilogProperty.Replace("[", string.Empty);
            serilogProperty = serilogProperty.Replace("]", string.Empty);
            serilogProperty = serilogProperty.Replace(" ", string.Empty);
            serilogProperty = serilogProperty.Replace(",", string.Empty);
            serilogProperty = serilogProperty.Replace("\"", string.Empty);

            returnValue = serilogProperty;

            return returnValue;
        }
    }
}