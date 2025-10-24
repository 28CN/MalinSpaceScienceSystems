using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using AstroClient.Wpf.Models; // Use the client's Models namespace

namespace AstroClient.Wpf.Services
{
    // Handles communication with the backend Astro API.
    public sealed class AstroApiClient
    {
        private readonly HttpClient _http;
        private const string BASE = "https://localhost:7120"; // API base URL.
        private const string API_CALCULATE = "api/v1/astro/calculate"; // Unified API endpoint path.

        // Constructor initializes HttpClient.
        public AstroApiClient()
        {
            _http = new HttpClient { BaseAddress = new Uri(BASE) };
            _http.Timeout = TimeSpan.FromSeconds(60);
        }

        // Unified method to call the backend /calculate endpoint.
        public async Task<AstroDataTransfer> CalculateAsync(AstroDataTransfer request, CancellationToken ct = default)
        {
            try
            {
                // Send POST request with the AstroDataTransfer object as JSON.
                using var resp = await _http.PostAsJsonAsync(API_CALCULATE, request, ct);

                // Throw exception for HTTP error status codes (e.g., 4xx, 5xx).
                resp.EnsureSuccessStatusCode();

                // Read and deserialize the JSON response body into AstroDataTransfer.
                var payload = await resp.Content.ReadFromJsonAsync<AstroDataTransfer>(cancellationToken: ct);

                // Handle cases where the server returns an empty or invalid response.
                if (payload == null)
                {
                    return new AstroDataTransfer { Type = request.Type, ErrorMessage = "Received empty response." };
                }

                // Return the response DTO from the server.
                return payload;
            }
            catch (HttpRequestException httpEx) // network or HTTP errors.
            {
                return new AstroDataTransfer { Type = request.Type, ErrorMessage = $"API Error: {httpEx.Message}" };
            }
            catch (TaskCanceledException) // timeouts or cancellation.
            {
                return new AstroDataTransfer { Type = request.Type, ErrorMessage = ct.IsCancellationRequested ? "Cancelled." : "Timeout." };
            }
            catch (Exception ex) // other errors.
            {
                return new AstroDataTransfer { Type = request.Type, ErrorMessage = $"Error: {ex.Message}" };
            }
        }
    }
}