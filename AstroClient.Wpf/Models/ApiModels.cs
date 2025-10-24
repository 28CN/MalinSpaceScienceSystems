using System.ComponentModel.DataAnnotations;

namespace AstroClient.Wpf.Models
{

    // VelocityDtos
    public sealed class VelocityRequest
    {
        [Range(double.Epsilon, double.MaxValue)]
        public double ObservedWavelength { get; set; }
        [Range(double.Epsilon, double.MaxValue)]
        public double RestWavelength { get; set; }
    }
    public sealed class VelocityResponse
    {
        public double VelocityMps { get; set; }
    }

    // DistanceDtos
    public sealed class DistanceRequest
    {
        [Range(double.Epsilon, double.MaxValue)]
        public double ParallaxArcseconds { get; set; }
    }
    public sealed class DistanceResponse
    {
        public double DistanceParsec { get; set; }
    }

    // KelvinDtos.cs
    public sealed class KelvinRequest
    {
        [Range(-273.15, double.MaxValue)]
        public double Celsius { get; set; }
    }
    public sealed class KelvinResponse
    {
        public double Kelvin { get; set; }
    }

    // EventHorizonDtos.cs
    public sealed class EventHorizonRequest
    {
        [Range(double.Epsilon, double.MaxValue)]
        public double MassKg { get; set; }
    }
    public sealed class EventHorizonResponse
    {
        public double RadiusMeters { get; set; }
    }
}
