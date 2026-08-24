using System.Text;
using System.Text.Json;

namespace SM_API_AH.Services
{
    public class ModerationService(HttpClient _http, IConfiguration _config,
         ILogger<ModerationService> _logger) : IModerationService
    {
        public async Task<bool?> EsMensajeInapropiadoAsync(string mensaje)
        {
            string apiKey = _config["Gemini:ApiKey"]!;
            string prompt = $"¿El siguiente mensaje contiene insultos graves, amenazas, contenido sexual o discurso de odio? Palabras levemente informales NO se consideran inapropiadas. Mensaje: {mensaje}";

            StringContent content = new(
                JsonSerializer.Serialize(new
                {
                    systemInstruction = new
                    {
                        parts = new[] { new { text = "Eres un moderador de contenido para una plataforma universitaria de Costa Rica. Conoces el español costarricense y sus regionalismos. Responde ÚNICAMENTE con 'true' si el mensaje contiene insultos, amenazas, acoso, contenido sexual o discurso de odio (incluyendo jerga costarricense ofensiva como 'maje', 'zángano', 'patán' usados como insulto directo hacia alguien). Responde 'false' si el mensaje es apropiado. Nada más." } }
                    },
                    contents = new[]
                    {
                        new { parts = new[] { new { text = prompt } } }
                    },
                    generationConfig = new { temperature = 0, topK = 1, maxOutputTokens = 5 }
                }),
                Encoding.UTF8,
                "application/json");

            HttpResponseMessage response = await _http.PostAsync(
                $"https://generativelanguage.googleapis.com/v1beta/models/gemini-3.5-flash-lite:generateContent?key={apiKey}",
                content);

            if (!response.IsSuccessStatusCode)
            {
                string errorBody = await response.Content.ReadAsStringAsync();
                _logger.LogWarning("Gemini moderation {Status}: {Body}", (int)response.StatusCode, errorBody);
                return null;
            }

            using JsonDocument doc = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
            string respuesta = doc.RootElement
                                  .GetProperty("candidates")[0]
                                  .GetProperty("content")
                                  .GetProperty("parts")[0]
                                  .GetProperty("text")
                                  .GetString()!
                                  .Trim()
                                  .ToLower()
                                  .Replace(".", "")
                                  .Replace(",", "");

            // "false" takes priority — only block when the model explicitly says "true"
            if (respuesta.Contains("false")) return false;
            if (respuesta.Contains("true")) return true;

            // Response was neither true nor false — treat as service unavailable
            _logger.LogWarning("Gemini moderation respuesta inesperada: '{Respuesta}'", respuesta);
            return null;
        }
    }
}
