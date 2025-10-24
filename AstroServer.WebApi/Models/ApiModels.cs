using System.ComponentModel.DataAnnotations;

namespace AstroServer.WebApi.Models
{

    public enum CalculationType
    {
        Unknown,
        Velocity,
        Distance,
        Kelvin,
        EventHorizon
    }

    public class AstroDataTransfer
    {
        public CalculationType Type { get; set; } = CalculationType.Unknown;

        public double? ObservedWavelength { get; set; }

        public double? RestWavelength { get; set; }

        public double? ParallaxArcseconds { get; set; }

        public double? Celsius { get; set; }

        public double? MassKg { get; set; }

        // possible Output Fields
        public double? VelocityMps { get; set; }
        public double? DistanceParsec { get; set; }
        public double? Kelvin { get; set; }
        public double? RadiusMeters { get; set; }

        // Error Message
        public string? ErrorMessage { get; set; }
    }
}