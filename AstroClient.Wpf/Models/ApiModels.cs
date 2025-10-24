using System.ComponentModel.DataAnnotations;

namespace AstroClient.Wpf.Models
{

    // VelocityDtos
    public sealed class VelocityRequest
    {
        public double ObservedWavelength { get; set; }
        public double RestWavelength { get; set; }
    }
    public sealed class VelocityResponse
    {
        public double VelocityMps { get; set; }
    }

    // DistanceDtos
    public sealed class DistanceRequest
    {
        public double ParallaxArcseconds { get; set; }
    }
    public sealed class DistanceResponse
    {
        public double DistanceParsec { get; set; }
    }

    // KelvinDtos
    public sealed class KelvinRequest
    {
        public double Celsius { get; set; }
    }
    public sealed class KelvinResponse
    {
        public double Kelvin { get; set; }
    }

    // EventHorizonDtos
    public sealed class EventHorizonRequest
    {
        public double MassKg { get; set; }
    }
    public sealed class EventHorizonResponse
    {
        public double RadiusMeters { get; set; }
    }
}
