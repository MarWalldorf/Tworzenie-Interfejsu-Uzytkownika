using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Lab4_Sekcja1.Models
{
    // Klasa mapująca odpowiedź JSON Schema z LLM
    public class LlmEvaluationResult
    {
        [JsonPropertyName("isCorrect")]
        public bool IsCorrect { get; set; }

        [JsonPropertyName("reason")]
        public string Reason { get; set; } = string.Empty;
    }

    public class TestModel
    {
        // 1. Pole tekstowe
        public string Pyt1 { get; set; } = string.Empty;

        // 2. Pole tekstowe (textarea) -> weryfikowane przez LLM
        public string Pyt2 { get; set; } = string.Empty;

        // 3. Lista rozwijana
        public string Pyt3 { get; set; } = string.Empty;

        // 4. Lista rozwinięta jednokrotnego wyboru
        public string Pyt4 { get; set; } = string.Empty;

        // 5. Radio buttons
        public string Pyt5 { get; set; } = string.Empty;

        // 6. Lista rozwinięta wielokrotnego wyboru
        public List<string> Pyt6 { get; set; } = new List<string>();

        // 7. Zestaw checkboxów o RÓŻNYCH nazwach
        public bool Pyt7_Odp1 { get; set; }
        public bool Pyt7_Odp2 { get; set; }
        public bool Pyt7_Odp3 { get; set; }
        public bool Pyt7_Odp4 { get; set; }

        // 8. Zestaw checkboxów o TEJ SAMEJ nazwie
        public List<string> Pyt8 { get; set; } = new List<string>();

        // 9. Pole tekstowe
        public string Pyt9 { get; set; } = string.Empty;

        // Wyniki i podsumowanie
        public int WynikPunkty { get; set; }
        public int MaksPunktow { get; set; } = 9;
        public string Ocena { get; set; } = string.Empty;
        public bool CzyWyslanaOdpowiedz { get; set; } = false;
        public string OcenaLlmKomentarz { get; set; } = string.Empty;

        // Metoda obliczająca punktację i wyznaczająca ocenę
        public async Task ObliczWynikAsync(HttpClient httpClient, string apiKey)
        {
            WynikPunkty = 0;

            // Pytanie 1: Pytanie tekstowe ("AMD")
            if (!string.IsNullOrWhiteSpace(Pyt1) && Pyt1.Trim().Equals("AMD", StringComparison.OrdinalIgnoreCase))
            {
                WynikPunkty++;
            }

            // Pytanie 2: Otwarta odpowiedź sprawdzana przez API LLM ze ścisłym JSON Schema
            // Usuwamy wszelkie białe znaki oraz znaki nowej linii (\r\n) przesyłane przez textarea
            string czystaOdp2 = Pyt2?.Trim() ?? string.Empty;
            bool pyt2Poprawne = await WeryfikujOdpowiedzPrzezLlmAsync(czystaOdp2, httpClient, apiKey);
            if (pyt2Poprawne)
            {
                WynikPunkty++;
            }

            // Pytanie 3: Lista rozwijana ("odp1" - UEFI)
            if (Pyt3 == "odp1")
            {
                WynikPunkty++;
            }

            // Pytanie 4: Lista rozwinięta 1-wyboru ("odp1" - HTTPS)
            if (Pyt4 == "odp1")
            {
                WynikPunkty++;
            }

            // Pytanie 5: Radio button ("odp1" - USB-C)
            if (Pyt5 == "odp1")
            {
                WynikPunkty++;
            }

            // Pytanie 6: Lista rozwinięta wielokrotnego wyboru (OLED, IPS -> odp1, odp2)
            if (Pyt6 != null && Pyt6.Contains("odp1") && Pyt6.Contains("odp2") && Pyt6.Count == 2)
            {
                WynikPunkty++;
            }

            // Pytanie 7: Checkboxy o różnych nazwach (SSD, HDD -> Odp1, Odp2 prawdziwe, reszta fałszywa)
            if (Pyt7_Odp1 && Pyt7_Odp2 && !Pyt7_Odp3 && !Pyt7_Odp4)
            {
                WynikPunkty++;
            }

            // Pytanie 8: Checkboxy o tej samej nazwie (Wi-Fi, Bluetooth -> odp1, odp2)
            if (Pyt8 != null && Pyt8.Contains("odp1") && Pyt8.Contains("odp2") && Pyt8.Count == 2)
            {
                WynikPunkty++;
            }

            // Pytanie 9: Pole tekstowe ("papugę" / "papuga" / "papuge" / "ptak")
            string czystaOdp9 = Pyt9?.Trim().ToLower() ?? string.Empty;
            if (czystaOdp9 == "papugę" || czystaOdp9 == "papuga" || czystaOdp9 == "papuge" || czystaOdp9 == "ptak")
            {
                WynikPunkty++;
            }

            // Wyznaczenie oceny na podstawie progu procentowego
            double procent = (double)WynikPunkty / MaksPunktow * 100;
            if (procent >= 90) Ocena = "bardzo dobra (5.0)";
            else if (procent >= 75) Ocena = "dobra plus (4.5)";
            else if (procent >= 65) Ocena = "dobra (4.0)";
            else if (procent >= 55) Ocena = "dostateczna plus (3.5)";
            else if (procent >= 50) Ocena = "dostateczna (3.0)";
            else Ocena = "niedostateczna (2.0)";

            CzyWyslanaOdpowiedz = true;
        }

        // Sprawdzanie odpowiedzi z wykorzystaniem OpenAI Structured Outputs (JSON Schema)
        private async Task<bool> WeryfikujOdpowiedzPrzezLlmAsync(string odpowiedz, HttpClient client, string apiKey)
        {
            if (string.IsNullOrWhiteSpace(odpowiedz))
            {
                OcenaLlmKomentarz = "Brak odpowiedzi.";
                return false;
            }

            // Weryfikacja lokalna (fallback), gdy klucz API nie jest podany lub wystąpi błąd
            bool fallbackCheck = odpowiedz.ToLower().Contains("ram");

            if (string.IsNullOrEmpty(apiKey))
            {
                OcenaLlmKomentarz = fallbackCheck ? "Poprawnie (weryfikacja lokalna)." : "Niepoprawnie (weryfikacja lokalna).";
                return fallbackCheck;
            }

            try
            {
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

                // Żądanie wymuszające strukturę JSON zgodną ze schematem
                var payload = new
                {
                    model = "gpt-4o-mini",
                    messages = new[]
                    {
                        new { role = "system", content = "Jesteś egzaminatorem. Pytanie brzmi: 'Wpisz skrót nazwy pamięci operacyjnej o dostępie swobodnym'. Prawidłowy skrót to RAM. Oceń czy odpowiedź użytkownika (niezależnie od wielkości liter czy otoczenia tekstem) jest poprawna." },
                        new { role = "user", content = odpowiedz }
                    },
                    response_format = new
                    {
                        type = "json_schema",
                        json_schema = new
                        {
                            name = "evaluation_schema",
                            strict = true,
                            schema = new
                            {
                                type = "object",
                                properties = new
                                {
                                    isCorrect = new { type = "boolean", description = "Czy odpowiedź jest poprawna" },
                                    reason = new { type = "string", description = "Krótkie uzasadnienie oceny" }
                                },
                                required = new[] { "isCorrect", "reason" },
                                additionalProperties = false
                            }
                        }
                    }
                };

                var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
                var response = await client.PostAsync("https://api.openai.com/v1/chat/completions", content);

                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    using var doc = JsonDocument.Parse(jsonResponse);
                    var messageContent = doc.RootElement
                        .GetProperty("choices")[0]
                        .GetProperty("message")
                        .GetProperty("content")
                        .GetString();

                    var result = JsonSerializer.Deserialize<LlmEvaluationResult>(messageContent!);
                    OcenaLlmKomentarz = result?.Reason ?? "";
                    return result?.IsCorrect ?? false;
                }
            }
            catch
            {
                // Obsługa błędu połączenia z API OpenAI
            }

            OcenaLlmKomentarz = fallbackCheck ? "Poprawnie." : "Niepoprawnie.";
            return fallbackCheck;
        }
    }
}