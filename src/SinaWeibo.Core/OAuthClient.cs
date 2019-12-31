using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using SinaWeibo.Core.Extensions;
using SinaWeibo.Core.Utilities;

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
        public static string BaseApiUrl => "https://api.weibo.com/2/";

        /// <summary>
        /// The address of getting Access Token.
        /// </summary>
        public static string AccessTokenUrl => "https://api.weibo.com/oauth2/access_token";

        /// <summary>
        /// The address of getting authorization code.
        /// </summary>
        public static string AuthorizationCodeUrl => "https://api.weibo.com/oauth2/authorize";

        /// <summary>
        /// Client's name.
        /// </summary>
        public string ClientName { get; set; }

        /// <summary>
        /// Client' id.
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// Client' secret.
        /// </summary>
        public string ClientSecret { get; set; }

        /// <summary>
        /// Callback url.
        /// </summary>
        public string CallbackUrl { get; set; }

        /// <summary>
        /// Access token.
        /// </summary>
        public string AccessToken { get; set; }

        /// <summary>
        /// The flag of the access token's setting.
        /// </summary>
        public bool IsAccessTokenSet { get; set; }

        /// <summary>
        /// User id.
        /// </summary>
        public string Uid { get; set; }

        /// <summary>
        /// Access token expire time.
        /// </summary>
        public int ExpiresIn { get; set; }

        /// <summary>
        /// The time of the access token's acquisition.
        /// </summary>
        public DateTime TokenAcquisitionTime { get; set; }

        /// <summary>
        /// OpenAuth 2.0 client's constructor.
        /// </summary>
        public OAuthClient()
        {
            _http = new HttpClient(new HttpClientHandler
            {
                CookieContainer   = new CookieContainer(),
                UseCookies        = true,
                AllowAutoRedirect = false
            }) {BaseAddress = new Uri(BaseApiUrl)};
        }

        #region GET

        /// <summary>
        /// Send a GET request to the specified Uri as an asynchronous operation.
        /// </summary>
        /// <param name="api">The api url for the request is sent to.</param>
        /// <param name="parameters">The parameters of the api.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public Task<HttpResponseMessage> GetAsync(string api, NameValueCollection parameters = null)
        {
            return _http.GetAsync(parameters is null ? api : $"{api}?{parameters.ToQueryString()}");
        }

        /// <summary>
        /// Send a GET request to the specified Uri as an asynchronous operation.
        /// </summary>
        /// <param name="api">The api url for the request is sent to.</param>
        /// <param name="parameters">The parameters of the api.</param>
        /// <returns>The GET request's return.</returns>
        public HttpResponseMessage Get(string api, NameValueCollection parameters = null)
            => this.GetAsync(api, parameters).Result;

        #endregion

        #region POST

        /// <summary>
        /// Send a POST request to the specified Uri as an asynchronous operation.
        /// </summary>
        /// <param name="api">The api url for the request is sent to.</param>
        /// <param name="parameters">The parameters of the api.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public Task<HttpResponseMessage> PostAsync(string api, IDictionary<string, object> parameters)
        {
            if (parameters is null)
            {
                parameters = new Dictionary<string, object>();
            }

            if (parameters.All(p => p.Value is string))
            {
                return _http.PostAsync(api, new FormUrlEncodedContent(parameters.ToKeyValuePairs()));
            }

            var content = new MultipartFormDataContent();

            foreach (var (key, value) in parameters)
            {
                switch (value)
                {
                    case byte[] bytes: // bytes
                        content.Add(new ByteArrayContent(bytes), key, OAuthUtility.GenerateNonceString());
                        break;
                    case MemoryFileContent memoryContent: // memory content
                        content.Add(new ByteArrayContent(memoryContent.Content), key, memoryContent.FileName);
                        break;
                    case FileInfo file: // file
                        content.Add(new ByteArrayContent(File.ReadAllBytes(file.FullName)), key, file.Name);
                        break;
                    default:
                        content.Add(new StringContent($"{value}"), key);
                        break;
                }
            }

            return _http.PostAsync(api, content);
        }

        #endregion
    }
}
