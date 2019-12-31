using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using static System.String;
using static System.Web.HttpUtility;

namespace SinaWeibo.Core.Extensions
{
    /// <summary>
    /// The extension for string.
    /// </summary>
    public static class CollectionExtension
    {
        /// <summary>
        /// Converts a collection into a query string.
        /// </summary>
        /// <param name="source">The collection for converting</param>
        /// <param name="removeEmptyEntries">A value that indicates whether the empty will be removed.</param>
        /// <returns>A Url query string.</returns>
        public static string ToQueryString(this NameValueCollection source, bool removeEmptyEntries = true)
        {
            return source != null
                ? Join("&", source.AllKeys.Where(key => !removeEmptyEntries ||
                                                        (source.GetValues(key) ?? throw new InvalidOperationException())
                                                       .Any(value => !IsNullOrEmpty(value)))
                                  .SelectMany(key => source
                                                    .GetValues(key)
                                                   ?.Where(value => !removeEmptyEntries || !IsNullOrEmpty(value))
                                                    .Select(value =>
                                                                $"{UrlEncode(key)}={(value != null ? UrlEncode(value) : Empty)}"))
                                  .ToArray())
                : Empty;
        }

        /// <summary>
        /// Converts a collection into KeyValuePairs.
        /// </summary>
        /// <param name="source">The collection for converting</param>
        /// <returns>KeyValuePair collection.</returns>
        public static IEnumerable<KeyValuePair<string, string>> ToKeyValuePairs(this IDictionary<string, object> source)
            => source.ToDictionary(k => k.Key, v => $"{v.Value}");
    }
}
