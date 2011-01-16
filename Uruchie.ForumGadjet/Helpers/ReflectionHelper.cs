using System;
using System.Linq;
using System.Runtime.Serialization;

namespace Uruchie.ForumGadjet.Helpers
{
    public static class ReflectionHelper
    {
        /// <summary>
        /// Returns collection of datamembers in specified types in string form separated by ','
        /// </summary>
        public static string GetActiveDataMembers(params Type[] types)
        {
            string result = string.Empty;
            foreach (Type type in types)
            {
                string[] fields =
                    type.GetProperties().Where(i => i.PropertyType == typeof (string) || i.PropertyType.IsValueType)
                        .SelectMany(p => p.GetCustomAttributes(typeof (DataMemberAttribute), true)).OfType
                        <DataMemberAttribute>().Select(i => i.Name).Distinct().ToArray();
                string joined = string.Join(",", fields);
                if (!string.IsNullOrEmpty(joined) && !string.IsNullOrEmpty(result))
                    result += ",";
                result += joined;
            }
            return result;
        }
    }
}