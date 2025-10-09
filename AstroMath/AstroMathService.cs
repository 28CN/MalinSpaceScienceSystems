using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AstroMath
{
    // Provides services for performing astronomical calculations.
    public class AstroMathService : IAstroMathService
    {
        // Physical constants
        private const double C = 299_792_458d;    // Speed of light (m/s)
        private const double G = 6.674e-11;       // Gravitational constant (m^3·kg^-1·s^-2)

        public double ComputeVelocity(double observedWavelength, double restWavelength)
        {
            double delta = observedWavelength - restWavelength;
            return C * (delta / restWavelength);
        }

        public double ComputeDistanceParsec(double parallaxArcseconds)
        {
            return 1.0d / parallaxArcseconds;
        }

        public double ToKelvin(double celsius)
        {
            return celsius + 273.15d;
        }

        public double ComputeEventHorizon(double massKg)
        {
            return 2.0d * G * massKg / (C * C);
        }
    }
}
