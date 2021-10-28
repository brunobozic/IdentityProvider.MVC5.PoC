using System;
using System.Linq.Expressions;

namespace IdentityProvider.Infrastructure.GlobalAsaxHelpers
{
    public static class ReflectionExtensions
    {
        public static T GetPropertyValue<T>(object o, string propertyName)
        {
            return (T) o.GetType().GetProperty(propertyName)?.GetValue(o, null);
        }

        public static TOut GetPropertyValueStatic<TIn, TOut>(string propertyName)
        {
            return (TOut) typeof(TIn).GetProperty(propertyName)?.GetValue(null);
        }

        public static object GetPropertyValue(object o, string propertyName)
        {
            return o.GetType().GetProperty(propertyName)?.GetValue(o, null);
        }

        public static void SetPropertyValueWithTypeChange<T>(object o, string propertyName, T propertyValue)
        {
            var propertyInfo = o.GetType().GetProperty(propertyName);
            propertyInfo.SetValue(o, Convert.ChangeType(propertyValue, propertyInfo.PropertyType), null);
        }

        public static void SetPropertyValue(object o, string propertyName, object propertyValue)
        {
            var propertyInfo = o.GetType().GetProperty(propertyName);
            propertyInfo.SetValue(o, propertyValue);
        }

        public static object ConvertValue(Type originalType, object value)
        {
            var underlyingType = Nullable.GetUnderlyingType(originalType);

            var instance = Convert.ChangeType(value, underlyingType ?? originalType);

            return instance;
        }

        //public static string GetPropertyValue( object o , FieldMetadata fieldMetadata )
        //{
        //    var propertyValue = o.GetType().GetProperty(fieldMetadata.Field)?.GetValue(o , null);
        //    var propertyValueString = propertyValue != null ? propertyValue.ToString() : string.Empty;
        //    if (fieldMetadata.Type == "date")
        //    {
        //        DateTime tmpDate;
        //        if (DateTime.TryParse(propertyValueString , Thread.CurrentThread.CurrentCulture , DateTimeStyles.None , out tmpDate))
        //        {
        //            propertyValueString = !string.IsNullOrWhiteSpace(fieldMetadata.Format) ? string.Format(fieldMetadata.Format , tmpDate) : tmpDate.ToShortDateString();
        //        }
        //    }
        //    else if (fieldMetadata.Type == "boolean")
        //    {
        //        propertyValueString = propertyValueString == "True" ? "Da" : "Ne";
        //    }

        //    return !string.IsNullOrWhiteSpace(propertyValueString) ? propertyValueString : "-";
        //}

        public static Func<TMnModel, Guid?> CreateSelectExpression<TMnModel>(string fieldName)
        {
            var parameterExp = Expression.Parameter(typeof(TMnModel), "x");
            var cast = Expression.Convert(parameterExp, typeof(TMnModel));
            var fieldProp = Expression.PropertyOrField(cast, fieldName);
            var lambda = Expression.Lambda<Func<TMnModel, Guid?>>(fieldProp, parameterExp);

            return lambda.Compile();
        }

        public static T To<T>(this IConvertible obj)
        {
            var t = typeof(T);
            var underlyingType = Nullable.GetUnderlyingType(t);

            return underlyingType != null
                ? obj == null ? default : (T) Convert.ChangeType(obj, underlyingType)
                : (T) Convert.ChangeType(obj, t);
        }
    }
}