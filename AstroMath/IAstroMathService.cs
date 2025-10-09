using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace AstroMath
{
    // Defines the contract for the astronomical calculation service.
    public interface IAstroMathService
    {
        double ComputeVelocity(double observedWavelength, double restWavelength);
        double ComputeDistanceParsec(double parallaxArcseconds);
        double ToKelvin(double celsius);
        double ComputeEventHorizon(double massKg);
    }
}