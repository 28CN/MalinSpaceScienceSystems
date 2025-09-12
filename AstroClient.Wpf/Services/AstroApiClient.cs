using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Astro.Contracts.Dtos;
using System.Net.Http.Json;

namespace AstroClient.Wpf.Services
{
    public sealed class AstroApiClient
    {
        private readonly HttpClient _http;

        // TODO: Set to your Swagger "https" port when running.
        private const string BASE = "https://localhost:7183";

        public AstroApiClient()
        {
            _http = new HttpClient { BaseAddress = new Uri(BASE) };
        }

        public Task<VelocityResponse?> PostVelocityAsync(VelocityRequest body)
            => _http.PostAsJsonAsync("/api/astro/velocity", body).Result.Content.ReadFromJsonAsync<VelocityResponse>();

        public Task<DistanceResponse?> PostDistanceAsync(DistanceRequest body)
            => _http.PostAsJsonAsync("/api/astro/distance", body).Result.Content.ReadFromJsonAsync<DistanceResponse>();

        public Task<KelvinResponse?> PostKelvinAsync(KelvinRequest body)
            => _http.PostAsJsonAsync("/api/astro/kelvin", body).Result.Content.ReadFromJsonAsync<KelvinResponse>();

        public Task<EventHorizonResponse?> PostEventHorizonAsync(EventHorizonRequest body)
            => _http.PostAsJsonAsync("/api/astro/event-horizon", body).Result.Content.ReadFromJsonAsync<EventHorizonResponse>();

        // POST JSON and read double result (async)
        private async Task<double> PostAndReadDoubleAsync<TRequest>(string url, TRequest request, CancellationToken ct = default)
        {
            // NOTE: ensure BaseAddress is https://localhost:7120/  (see constructor or wherever you set it)
            using var resp = await _http.PostAsJsonAsync(url, request, ct); // async post
            if (!resp.IsSuccessStatusCode)
            {
                // unify error message for VM
                var body = await resp.Content.ReadAsStringAsync(ct);
                var message = string.IsNullOrWhiteSpace(body)
                    ? $"{(int)resp.StatusCode} {resp.ReasonPhrase}"
                    : body;
                throw new HttpRequestException(message);
            }

            // requirement: EnsureSuccessStatusCode (after custom check; harmless but explicit)
            resp.EnsureSuccessStatusCode();

            double? result = await resp.Content.ReadFromJsonAsync<double?>(cancellationToken: ct);
            if (result is null) throw new HttpRequestException("Empty response.");
            return result.Value; // keep as double; VM will format .ToString("E6")
        }

        // async version
        public Task<double> ComputeVelocityAsync(Astro.Contracts.Dtos.VelocityRequest request, CancellationToken ct = default)
            => PostAndReadDoubleAsync("api/astro/velocity", request, ct);

        // temporary sync wrapper (keep UI working now; will be removed after VM becomes async)
        public double ComputeVelocity(Astro.Contracts.Dtos.VelocityRequest request)
            => ComputeVelocityAsync(request).GetAwaiter().GetResult();

        public Task<double> ComputeDistanceAsync(Astro.Contracts.Dtos.DistanceRequest request, CancellationToken ct = default)
    => PostAndReadDoubleAsync("api/astro/distance", request, ct);

        public double ComputeDistance(Astro.Contracts.Dtos.DistanceRequest request)
            => ComputeDistanceAsync(request).GetAwaiter().GetResult();

        public Task<double> ComputeKelvinAsync(Astro.Contracts.Dtos.KelvinRequest request, CancellationToken ct = default)
    => PostAndReadDoubleAsync("api/astro/kelvin", request, ct);

        public double ComputeKelvin(Astro.Contracts.Dtos.KelvinRequest request)
            => ComputeKelvinAsync(request).GetAwaiter().GetResult();

        public Task<double> ComputeEventHorizonRadiusAsync(Astro.Contracts.Dtos.EventHorizonRequest request, CancellationToken ct = default)
    => PostAndReadDoubleAsync("api/astro/event-horizon", request, ct);

        public double ComputeEventHorizonRadius(Astro.Contracts.Dtos.EventHorizonRequest request)
            => ComputeEventHorizonRadiusAsync(request).GetAwaiter().GetResult();

    }
}
