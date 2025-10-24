using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AstroClient.Wpf.Models;
using System.Net.Http.Json;

namespace AstroClient.Wpf.Services
{
    public sealed class AstroApiClient
    {
        private readonly HttpClient _http;

        // use swagger https port
        private const string BASE = "https://localhost:7120";
        private const string API = "api/v1/astro/";  // base api path

        public AstroApiClient()
        {
            _http = new HttpClient { BaseAddress = new Uri(BASE) };
        }

        // posts request and return calculated results.
        public async Task<double> ComputeVelocityAsync(VelocityRequest request, CancellationToken ct = default)
        {
            using var resp = await _http.PostAsJsonAsync(API + "velocity", request, ct);
            resp.EnsureSuccessStatusCode();
            var payload = await resp.Content.ReadFromJsonAsync<VelocityResponse>(cancellationToken: ct);
            if (payload == null) throw new HttpRequestException("Empty response.");
            return payload.VelocityMps;
        }
        public async Task<double> ComputeDistanceAsync(DistanceRequest request, CancellationToken ct = default)
        {
            using var resp = await _http.PostAsJsonAsync(API + "distance", request, ct);
            resp.EnsureSuccessStatusCode();
            var payload = await resp.Content.ReadFromJsonAsync<DistanceResponse>(cancellationToken: ct);
            if (payload == null) throw new HttpRequestException("Empty response.");
            return payload.DistanceParsec;
        }

        public async Task<double> ComputeKelvinAsync(KelvinRequest request, CancellationToken ct = default)
        {
            using var resp = await _http.PostAsJsonAsync(API + "kelvin", request, ct);
            resp.EnsureSuccessStatusCode();
            var payload = await resp.Content.ReadFromJsonAsync<KelvinResponse>(cancellationToken: ct);
            if (payload == null) throw new HttpRequestException("Empty response.");
            return payload.Kelvin;
        }

        public async Task<double> ComputeEventHorizonRadiusAsync(EventHorizonRequest request, CancellationToken ct = default)
        {
            using var resp = await _http.PostAsJsonAsync(API + "event-horizon", request, ct);
            resp.EnsureSuccessStatusCode();
            var payload = await resp.Content.ReadFromJsonAsync<EventHorizonResponse>(cancellationToken: ct);
            if (payload == null) throw new HttpRequestException("Empty response.");
            return payload.RadiusMeters;
        }
    }
}
