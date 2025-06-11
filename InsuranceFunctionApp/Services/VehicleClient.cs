using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using InsuranceFunctionApp.Models;

namespace InsuranceFunctionApp.Services
{
    public class VehicleClient : IVehicleClient
    {
        private readonly HttpClient _httpClient;

        public VehicleClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Vehicle?> GetVehicleAsync(string registrationNumber)
        {
            return await _httpClient.GetFromJsonAsync<Vehicle>($"vehicle/{registrationNumber}");
        }
    }
}