using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HMS.DesktopClient.Utils
{
    public static class RequestHelper
    {
        /// <summary>
        /// Creates an HttpRequestMessage with the Authorization header set to the current user's JWT token.
        /// </summary>
        /// <param name="method">The HTTP method (GET, POST, etc.)</param>
        /// <param name="url">The request URL</param>
        /// <returns>An HttpRequestMessage with the Authorization header set</returns>
        // Helper method to create an HttpRequestMessage with the Authorization header
        public static HttpRequestMessage CreateRequest(HttpMethod method, string url)
        {
            if (string.IsNullOrEmpty(App.CurrentUser.Token))
                throw new InvalidOperationException("JWT token is missing. Please log in first.");

            var request = new HttpRequestMessage(method, url);
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", App.CurrentUser.Token);
            return request;
        }
    }
}
