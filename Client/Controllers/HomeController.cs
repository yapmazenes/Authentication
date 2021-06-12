using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;

namespace Client.Controllers
{
    public class HomeController : Controller
    {
        private readonly HttpClient _client;

        public HomeController(IHttpClientFactory httpClientFactory)
        {
            _client = httpClientFactory.CreateClient();
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public async Task<IActionResult> Secret()
        {
            var token = await HttpContext.GetTokenAsync("access_token");

            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            //https://localhost:44320/ Api Url
            //https://localhost:44375 Auth Server
            var serverResponse = await _client.GetAsync("https://localhost:44375/secret/index");
            var apiResponse = await _client.GetAsync("https://localhost:44320/secret/index");

            return View();
        }
    }
}
