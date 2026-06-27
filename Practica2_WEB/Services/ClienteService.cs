using Practica2_WEB.Models;
using System.Text;
using System.Text.Json;

namespace Practica2_WEB.Services
{
    public interface IClienteService
    {
        Task<List<Cliente>> GetAllClientesAsync();
        Task<Cliente> GetClienteByIdAsync(long id);
        Task<bool> CreateClienteAsync(Cliente cliente);
        Task<bool> UpdateClienteAsync(long id, Cliente cliente);
        Task<bool> DeleteClienteAsync(long id);
    }

    public class ClienteService : IClienteService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly string _apiBaseUrl;

        public ClienteService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _apiBaseUrl = configuration["Valores:UrlApi"];
        }

        public async Task<List<Cliente>> GetAllClientesAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_apiBaseUrl}clientes");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    var apiResponse = JsonSerializer.Deserialize<ApiResponse<List<Cliente>>>(content, jsonOptions);
                    return apiResponse?.Data ?? new List<Cliente>();
                }
                return new List<Cliente>();
            }
            catch
            {
                return new List<Cliente>();
            }
        }

        public async Task<Cliente> GetClienteByIdAsync(long id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_apiBaseUrl}clientes/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    var apiResponse = JsonSerializer.Deserialize<ApiResponse<Cliente>>(content, jsonOptions);
                    return apiResponse?.Data;
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        public async Task<bool> CreateClienteAsync(Cliente cliente)
        {
            try
            {
                var request = new { cliente.Cedula, cliente.Nombre, cliente.Correo };
                var content = new StringContent(
                    JsonSerializer.Serialize(request),
                    Encoding.UTF8,
                    "application/json");

                var response = await _httpClient.PostAsync($"{_apiBaseUrl}clientes", content);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateClienteAsync(long id, Cliente cliente)
        {
            try
            {
                var request = new { cliente.Cedula, cliente.Nombre, cliente.Correo };
                var content = new StringContent(
                    JsonSerializer.Serialize(request),
                    Encoding.UTF8,
                    "application/json");

                var response = await _httpClient.PutAsync($"{_apiBaseUrl}clientes/{id}", content);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteClienteAsync(long id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"{_apiBaseUrl}clientes/{id}");
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }
    }
}
