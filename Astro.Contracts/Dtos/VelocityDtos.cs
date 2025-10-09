using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astro.Contracts.Dtos
{
    // request Sent from client to server.
    public sealed class VelocityRequest
    {
        [Range(double.Epsilon, double.MaxValue)] //must be positive
        public double ObservedWavelength { get; set; }

        [Range(double.Epsilon, double.MaxValue)] //must be positive
        public double RestWavelength { get; set; }
    }

    // response sent from server to client.
    public sealed class VelocityResponse
    {
        public double VelocityMps { get; set; }
    }
}
