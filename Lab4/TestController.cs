using Lab4_Sekcja1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Threading.Tasks;

namespace Lab4_Sekcja1.Controllers
{
    public class TestController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public TestController(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        // 1. Pierwsza wersja: Wysyłanie / odbieranie metodą GET
        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] TestModel model)
        {
            // Jeśli przynajmniej jedno pole zostało przesłane wquery string (np. pyt1)
            if (Request.Query.Count > 0)
            {
                var client = _httpClientFactory.CreateClient();
                string apiKey = _configuration["OpenAI:ApiKey"] ?? "";
                await model.ObliczWynikAsync(client, apiKey);
            }

            return View(model);
        }

        // 2. Docelowa wersja: Wysyłanie / odbieranie metodą POST
        [HttpPost]
        public async Task<IActionResult> IndexPost(TestModel model)
        {
            var client = _httpClientFactory.CreateClient();
            string apiKey = _configuration["OpenAI:ApiKey"] ?? "";
            await model.ObliczWynikAsync(client, apiKey);

            return View("Index", model);
        }
    }
}
