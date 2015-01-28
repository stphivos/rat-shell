using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Company.Tools.RemoteAccessTrojan.Infrastructure.CrossCutting
{
    public static class Extensions
    {
        #region Generic

        public static bool IsDefaultValue<T>(this T source)
        {
            if ((object)source == null)
            {
                return true;
            }

            bool result = default(bool);
            T defaultValue = default(T);

            if (typeof(T).IsValueType)
            {
                result = ValueType.Equals(source, defaultValue);
            }
            else
            {
                result = source.Equals(defaultValue);
            }

            return result;
        }

        public static T ConvertTo<T>(this object source)
        {
            var result = default(T);
            if (typeof(T).IsEnum) result = source.ConvertToEnum<T>();
            else result = (T)Convert.ChangeType(source, typeof(T));
            return result;
        }

        public static T ConvertToEnum<T>(this object source)
        {
            var result = (T)Enum.Parse(typeof(T), source.GetNullSafe());
            return result;
        }

        #endregion

        #region String

        public static string GetNullSafe(this object source)
        {
            if (source == null) return string.Empty;
            else return source.ToString();
        }

        public static object GetLocalInstance(this string qualifiedTypeName, params object[] args)
        {
            var instance = Activator.CreateInstance(Type.GetType(qualifiedTypeName), args);
            return instance;
        }

        #endregion

        #region Exception

        public static Exception Append(this Exception source, Exception ex)
        {
            Exception result = null;
            if (source == null)
            {
                result = ex;
            }
            else
            {
                result = new AggregateException(ex, source);
            }
            return result;
        }

        #endregion
    }
}