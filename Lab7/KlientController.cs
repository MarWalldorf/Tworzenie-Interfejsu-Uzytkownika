using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;

namespace LabTiu7.Controllers
{
    public class KlientController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public KlientController(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<IActionResult> Bitcoin()
        {
            var client = _httpClientFactory.CreateClient();

            try
            {
                var responseUsd = await client.GetStringAsync("https://api.coinbase.com/v2/prices/BTC-USD/spot");
                var responsePln = await client.GetStringAsync("https://api.coinbase.com/v2/prices/BTC-PLN/spot");

                using var docUsd = JsonDocument.Parse(responseUsd);
                using var docPln = JsonDocument.Parse(responsePln);

                ViewBag.CenaUsd = docUsd.RootElement.GetProperty("data").GetProperty("amount").GetString();
                ViewBag.CenaPln = docPln.RootElement.GetProperty("data").GetProperty("amount").GetString();
                ViewBag.CzasPobrania = DateTime.Now.ToString("HH:mm:ss dd.MM.yyyy");
            }
            catch (Exception ex)
            {
                ViewBag.Blad = $"Nie udało się pobrać ceny Bitcoina: {ex.Message}";
            }

            return View();
        }

        [HttpGet]
        public IActionResult LLM()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Baza()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> LLM(string prompt)
        {
            ViewBag.Prompt = prompt;

            if (string.IsNullOrWhiteSpace(prompt))
            {
                ViewBag.Blad = "Wpisz treść zapytania (prompt).";
                return View();
            }

            var apiKey = _configuration["Gemini:ApiKey"];
            if (string.IsNullOrWhiteSpace(apiKey) || apiKey.StartsWith("TUTAJ_"))
            {
                ViewBag.Blad = "Brak poprawnego klucza API w pliku appsettings.json.";
                return View();
            }

            var client = _httpClientFactory.CreateClient();
            var url = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.5-flash:generateContent?key={apiKey}";

            var requestBody = new
            {
                contents = new[]
                {
                    new
                    {
                        parts = new[]
                        {
                            new { text = prompt }
                        }
                    }
                }
            };

            var jsonContent = new StringContent(
                JsonSerializer.Serialize(requestBody),
                Encoding.UTF8,
                "application/json"
            );

            try
            {
                var response = await client.PostAsync(url, jsonContent);
                var responseString = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    ViewBag.Blad = $"Błąd API ({response.StatusCode}): {responseString}";
                    return View();
                }

                using var doc = JsonDocument.Parse(responseString);
                var odpowiedzText = doc.RootElement
                    .GetProperty("candidates")[0]
                    .GetProperty("content")
                    .GetProperty("parts")[0]
                    .GetProperty("text")
                    .GetString();

                ViewBag.Odpowiedz = odpowiedzText;
            }
            catch (Exception ex)
            {
                ViewBag.Blad = $"Wystąpił błąd podczas komunikacji z Gemini API: {ex.Message}";
            }

            return View();
        }
    }
}