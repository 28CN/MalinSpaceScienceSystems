using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astro.Contracts.Dtos
{
    public sealed class EventHorizonRequest
    {
        [Range(double.Epsilon, double.MaxValue)] // Mass > 0
        public double MassKg { get; set; }
    }

    public sealed class EventHorizonResponse
    {
        public double RadiusMeters { get; set; }
    }
}
