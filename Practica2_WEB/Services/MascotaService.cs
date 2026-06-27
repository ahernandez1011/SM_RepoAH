using Practica2_WEB.Models;
using System.Text;
using System.Text.Json;

namespace Practica2_WEB.Services
{
    public interface IMascotaService
    {
        Task<List<Mascota>> GetAllMascotasAsync();
        Task<Mascota> GetMascotaByIdAsync(long id);
        Task<List<Mascota>> GetMascotasByClienteAsync(long idCliente);
        Task<bool> CreateMascotaAsync(Mascota mascota);
        Task<bool> UpdateMascotaAsync(long id, Mascota mascota);
        Task<bool> DeleteMascotaAsync(long id);
    }

    public class MascotaService : IMascotaService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly string _apiBaseUrl;

        public MascotaService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _apiBaseUrl = configuration["Valores:UrlApi"];
        }

        public async Task<List<Mascota>> GetAllMascotasAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_apiBaseUrl}mascotas");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    var apiResponse = JsonSerializer.Deserialize<ApiResponse<List<Mascota>>>(content, jsonOptions);
                    return apiResponse?.Data ?? new List<Mascota>();
                }
                return new List<Mascota>();
            }
            catch
            {
                return new List<Mascota>();
            }
        }

        public async Task<Mascota> GetMascotaByIdAsync(long id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_apiBaseUrl}mascotas/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    var apiResponse = JsonSerializer.Deserialize<ApiResponse<Mascota>>(content, jsonOptions);
                    return apiResponse?.Data;
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        public async Task<List<Mascota>> GetMascotasByClienteAsync(long idCliente)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_apiBaseUrl}mascotas/cliente/{idCliente}");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    var apiResponse = JsonSerializer.Deserialize<ApiResponse<List<Mascota>>>(content, jsonOptions);
                    return apiResponse?.Data ?? new List<Mascota>();
                }
                return new List<Mascota>();
            }
            catch
            {
                return new List<Mascota>();
            }
        }

        public async Task<bool> CreateMascotaAsync(Mascota mascota)
        {
            try
            {
                var request = new { mascota.Nombre, mascota.Especie, mascota.Raza, mascota.Peso, mascota.IdCliente };
                var content = new StringContent(
                    JsonSerializer.Serialize(request),
                    Encoding.UTF8,
                    "application/json");

                var response = await _httpClient.PostAsync($"{_apiBaseUrl}mascotas", content);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateMascotaAsync(long id, Mascota mascota)
        {
            try
            {
                var request = new { mascota.Nombre, mascota.Especie, mascota.Raza, mascota.Peso, mascota.IdCliente };
                var content = new StringContent(
                    JsonSerializer.Serialize(request),
                    Encoding.UTF8,
                    "application/json");

                var response = await _httpClient.PutAsync($"{_apiBaseUrl}mascotas/{id}", content);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteMascotaAsync(long id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"{_apiBaseUrl}mascotas/{id}");
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }
    }
}
