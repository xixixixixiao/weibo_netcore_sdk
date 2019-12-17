using System;
using System.Net;
using System.Net.Http;

namespace SinaWeibo.Core
{
    /// <summary>
    /// OpenAuth 2.0 client
    /// </summary>
    public class OAuthClient
    {
        /// <summary>
        /// HTTP client.
        /// </summary>
        private readonly HttpClient _http;

        /// <summary>
        /// OpenAuth Server Base API URL.
        /// </summary>
        public string BaseApiUrl => "https://api.weibo.com/2/";

        /// <summary>
        /// The address of getting Access Token.
        /// </summary>
        public string AccessTokenUrl => "https://api.weibo.com/oauth2/access_token";

        /// <summary>
        /// The address of getting authorization code.
        /// </summary>
        public string AuthorizationCodeUrl => "https://api.weibo.com/oauth2/authorize";

        public OAuthClient()
        {
            _http = new HttpClient(new HttpClientHandler
            {
                CookieContainer   = new CookieContainer(),
                UseCookies        = true,
                AllowAutoRedirect = false
            }) {BaseAddress = new Uri(BaseApiUrl)};
        }
    }
}
