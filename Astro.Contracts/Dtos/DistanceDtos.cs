using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astro.Contracts.Dtos
{
    public sealed class DistanceRequest
    {
        [Range(double.Epsilon, double.MaxValue)]
        public double ParallaxArcseconds { get; set; }
    }

    public sealed class DistanceResponse
    {
        public double DistanceParsec { get; set; }
    }
}
