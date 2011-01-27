using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace Uruchie.Core.Helpers
{
    public static class ReflectionHelper
    {
        /// <summary>
        /// Returns collection of datamembers in specified types in string form separated by ','
        /// </summary>
        public static string GetActiveDataMembers(Type type)
        {
            var names = GetPrimitiveMembersList(type)
                .SelectMany(p => p.GetCustomAttributes(typeof(DataMemberAttribute), true))
                .OfType<DataMemberAttribute>()
                .Select(i => i.Name)
                .Distinct()
                .ToArray();

            string result = string.Empty;
            string joined = string.Join(",", names);
            if (!string.IsNullOrEmpty(joined) && 
                !string.IsNullOrEmpty(result))
                result += ",";
            result += joined;

            return result;
        }
        
        private static IEnumerable<PropertyInfo> GetPrimitiveMembersList(Type type)
        {
            foreach (var property in type.GetProperties())
            {
                if (property.PropertyType.IsPrimitive || 
                    property.PropertyType == typeof(string)) //string is an array of chars! ваш кэп.
                    yield return property;
                else if (property.PropertyType.IsClass)
                {
                    Type propertyType = property.PropertyType.IsArray
                                            ? property.PropertyType.GetElementType()
                                            : property.PropertyType;

                    foreach (var item in GetPrimitiveMembersList(propertyType))
                        yield return item;
                }
            }
        }
    }
}