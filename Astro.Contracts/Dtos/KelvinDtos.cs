using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astro.Contracts.Dtos
{
    public sealed class KelvinRequest
    {
        public double Celsius { get; set; }
    }

    public sealed class KelvinResponse
    {
        public double Kelvin { get; set; }
    }
}
