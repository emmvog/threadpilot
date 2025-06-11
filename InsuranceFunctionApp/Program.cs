using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using InsuranceFunctionApp.Services;
using System;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices(services =>
    {
        var vehicleApiBaseUrl = Environment.GetEnvironmentVariable("VEHICLE_API_BASE_URL")
                                ?? "http://localhost:7071/api/";

        services.AddHttpClient<IVehicleClient, VehicleClient>(client =>
        {
            client.BaseAddress = new Uri(vehicleApiBaseUrl);
        });
    })
    .Build();

host.Run();
