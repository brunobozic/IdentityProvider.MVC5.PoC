using System.ComponentModel;

namespace KafkaModule.Core.Enums.EnumExtensions
{
    public static class EnumExtensions
    {
        public static string GetOracleParamName<TEnum>(this TEnum @enum)
        {
            try
            {
                var info = @enum.GetType().GetField(@enum.ToString());
                var attributes = (DescriptionAttribute[])info.GetCustomAttributes(typeof(DescriptionAttribute), false);

                return attributes?[0].Description ?? @enum.ToString();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static string GetDescriptionString<TEnum>(this TEnum @enum)
        {
            try
            {
                var info = @enum.GetType().GetField(@enum.ToString());
                var attributes = (DescriptionAttribute[])info.GetCustomAttributes(typeof(DescriptionAttribute), false);

                return attributes?[0].Description ?? @enum.ToString();
            }
            catch (Exception)
            {
                return "Nepoznato";
            }
        }

        public static T GetValueFromDescription<T>(this string description)
        {
            var type = typeof(T);
            if (!type.IsEnum) throw new InvalidOperationException();
            foreach (var field in type.GetFields())
            {
                var attribute = Attribute.GetCustomAttribute(field,
                    typeof(DescriptionAttribute)) as DescriptionAttribute;
                if (attribute != null)
                {
                    if (attribute.Description == description)
                        return (T)field.GetValue(null);
                }
                else
                {
                    if (field.Name == description)
                        return (T)field.GetValue(null);
                }
            }

            throw new ArgumentException("Not found.", nameof(description));
            // or return default(T);
        }
    }
}