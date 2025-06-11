using System.Threading.Tasks;
using InsuranceFunctionApp.Models;

namespace InsuranceFunctionApp.Services
{
    public interface IVehicleClient
    {
        Task<Vehicle?> GetVehicleAsync(string registrationNumber);
    }
}