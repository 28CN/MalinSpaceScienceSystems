using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astro.Contracts.Dtos
{
    public sealed class KelvinRequest
    {
        [Range(-273.15, double.MaxValue)] // C >= -273.15
        public double Celsius { get; set; }
    }

    public sealed class KelvinResponse
    {
        public double Kelvin { get; set; }
    }
}
