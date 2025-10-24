using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using AstroClient.Wpf.Models;

namespace AstroClient.Wpf.Services
{
    // Handles communication with the backend Astro API.
    public sealed class AstroApiClient
    {
        private readonly HttpClient _http;
        private const string BASE = "https://localhost:7120"; // API base URL.

        // API endpoint paths.
        private const string API_VELOCITY = "api/v1/astro/velocity";
        private const string API_DISTANCE = "api/v1/astro/distance";
        private const string API_KELVIN = "api/v1/astro/kelvin";
        private const string API_EVENTHORIZON = "api/v1/astro/eventhorizon";

        // Constructor initializes HttpClient.
        public AstroApiClient()
        {
            _http = new HttpClient { BaseAddress = new Uri(BASE) };
            _http.Timeout = TimeSpan.FromSeconds(60);
        }

        // 4 public calculation methods.
        public async Task<ApiResponse<AstroDataTransfer>> CalculateVelocityAsync(AstroDataTransfer request, CancellationToken ct = default)
        {
            return await PostCalculationAsync(API_VELOCITY, request, ct);
        }

        public async Task<ApiResponse<AstroDataTransfer>> CalculateDistanceAsync(AstroDataTransfer request, CancellationToken ct = default)
        {
            return await PostCalculationAsync(API_DISTANCE, request, ct);
        }

        public async Task<ApiResponse<AstroDataTransfer>> CalculateKelvinAsync(AstroDataTransfer request, CancellationToken ct = default)
        {
            return await PostCalculationAsync(API_KELVIN, request, ct);
        }

        public async Task<ApiResponse<AstroDataTransfer>> CalculateEventHorizonAsync(AstroDataTransfer request, CancellationToken ct = default)
        {
            return await PostCalculationAsync(API_EVENTHORIZON, request, ct);
        }



        // post calculation helper with error handling
        private async Task<ApiResponse<AstroDataTransfer>> PostCalculationAsync(string endpoint, AstroDataTransfer request, CancellationToken ct)
        {
            try
            {
                using var resp = await _http.PostAsJsonAsync(endpoint, request, ct);

                if (!resp.IsSuccessStatusCode)
                {
                    string error = await resp.Content.ReadAsStringAsync(ct);
                    return ApiResponse<AstroDataTransfer>.Fail(string.IsNullOrEmpty(error) ? resp.ReasonPhrase ?? "API Error" : error);
                }

                var payload = await resp.Content.ReadFromJsonAsync<AstroDataTransfer>(cancellationToken: ct);

                return payload != null
                    ? ApiResponse<AstroDataTransfer>.Success(payload)
                    : ApiResponse<AstroDataTransfer>.Fail("Received empty response.");
            }
            catch (Exception ex)
            {
                return ApiResponse<AstroDataTransfer>.Fail($"Client Error: {ex.Message}");
            }
        }
        public class ApiResponse<T>
        {
            public bool IsSuccess { get; private set; }
            public T? Data { get; private set; }
            public string? ErrorMessage { get; private set; }

            public static ApiResponse<T> Success(T data) => new ApiResponse<T> { IsSuccess = true, Data = data };
            public static ApiResponse<T> Fail(string message) => new ApiResponse<T> { IsSuccess = false, ErrorMessage = message };
        }
    }
}