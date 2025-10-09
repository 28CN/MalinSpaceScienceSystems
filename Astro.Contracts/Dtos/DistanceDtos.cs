using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astro.Contracts.Dtos
{
    // Parallax distance in parsec
    // request Sent from client to server.
    public sealed class DistanceRequest
    {
        [Range(double.Epsilon, double.MaxValue)] //must be positive
        public double ParallaxArcseconds { get; set; }
    }

    // Distance in parsec
    // response sent from server to client.
    public sealed class DistanceResponse
    {
        public double DistanceParsec { get; set; }
    }
}
