using System.ComponentModel.DataAnnotations;

namespace AstroServer.WebApi.Models
{

    public class AstroDataTransfer
    {
        public double? ObservedWavelength { get; set; }

        public double? RestWavelength { get; set; }

        public double? ParallaxArcseconds { get; set; }

        public double? Celsius { get; set; }

        public double? MassKg { get; set; }


        public double? VelocityMps { get; set; }
        public double? DistanceParsec { get; set; }
        public double? Kelvin { get; set; }
        public double? RadiusMeters { get; set; }
    }
}