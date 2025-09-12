using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astro.Contracts.Dtos
{
    public sealed class VelocityRequest
    {
        public double ObservedWavelength { get; set; }
        public double RestWavelength { get; set; }
    }

    public sealed class VelocityResponse
    {
        public double VelocityMps { get; set; }
    }
}
