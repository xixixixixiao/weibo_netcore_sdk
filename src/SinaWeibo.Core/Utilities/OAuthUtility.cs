using System;
using System.Text;

namespace SinaWeibo.Core.Utilities
{
    /// <summary>
    /// The utility class.
    /// </summary>
    public class OAuthUtility
    {
        /// <summary>
        /// Generate the nonce string.
        /// </summary>
        /// <param name="length">nonce string's length.</param>
        /// <returns>nonce string.</returns>
        public static string GenerateNonceString(int length = 8)
        {
            var builder = new StringBuilder();
            var random  = new Random();

            for (var i = 0; i < length; i++)
            {
                builder.Append((char) random.Next(97, 123));
            }

            return builder.ToString();
        }

        /// <summary>
        /// Returns the number of seconds that have elapsed since 1970-01-01T00:00:00Z.
        /// </summary>
        /// <returns>The number of seconds that have elapsed since 1970-01-01T00:00:00Z.</returns>
        public static string GetTimeStamp()
            => Convert.ToInt64(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0)).ToString();
    }
}