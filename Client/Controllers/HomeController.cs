using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Client.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public HomeController(IHttpClientFactory httpClientFactory)
        {
            this._httpClientFactory = httpClientFactory;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public async Task<IActionResult> Secret()
        {
            var serverResponse = await AccessTokenRefreshWrapper(() => SecuredGetRequest("https://localhost:44375/secret/index"));

            var apiResponse = await AccessTokenRefreshWrapper(() => SecuredGetRequest("https://localhost:44320/secret/index"));

            return View();
        }

        private async Task<HttpResponseMessage> SecuredGetRequest(string url)
        {
            var client = _httpClientFactory.CreateClient();

            var token = await HttpContext.GetTokenAsync("access_token");

            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            return await client.GetAsync(url);
        }

        public async Task<HttpResponseMessage> AccessTokenRefreshWrapper(Func<Task<HttpResponseMessage>> initialRequest)
        {
            var responseAction = await initialRequest();

            if (responseAction.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                await RefreshAccessToken();
                responseAction = await initialRequest();
            }

            return responseAction;
        }

        public async Task<string> RefreshAccessToken()
        {
            var refreshToken = await HttpContext.GetTokenAsync("refresh_token");

            var refreshTokenClient = _httpClientFactory.CreateClient();

            var requestData = new Dictionary<string, string>
            {
                ["grant_type"] = "refresh_token",
                ["refresh_token"] = refreshToken
            };

            var request = new HttpRequestMessage(HttpMethod.Post, "https://localhost:44375/oauth/token")
            {
                Content = new FormUrlEncodedContent(requestData)
            };

            var basicCredentails = "username:password";
            var encodedCredentails = Encoding.UTF8.GetBytes(basicCredentails);
            var base64Credentails = Convert.ToBase64String(encodedCredentails);

            request.Headers.Add("Authorization", $"Basic {base64Credentails}");

            var response = await refreshTokenClient.SendAsync(request);

            var responseString = await response.Content.ReadAsStringAsync();

            var responseData = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseString);

            var newAccessToken = responseData.GetValueOrDefault("access_token");
            var newRefreshToken = responseData.GetValueOrDefault("refresh_token");

            var authInfo = await HttpContext.AuthenticateAsync("ClientCookie");

            authInfo.Properties.UpdateTokenValue("access_token", newAccessToken);
            authInfo.Properties.UpdateTokenValue("refresh_token", newRefreshToken);

            await HttpContext.SignInAsync("ClientCookie", authInfo.Principal, authInfo.Properties);
            return "";

        }
    }
}
