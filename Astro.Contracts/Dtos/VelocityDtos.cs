using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astro.Contracts.Dtos
{
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
}
