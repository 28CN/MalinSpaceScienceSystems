using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AstroMath
{
    public static class StarMath
    {
        // Physical constants
        private const double C = 299_792_458d;       // m/s
        private const double G = 6.674e-11;          // m^3·kg^-1·s^-2

        // Doppler velocity (approx, non-relativistic)
        public static double ComputeVelocity(double observedWavelength, double restWavelength)
        {
            // v ≈ c * (Δλ/λ0)
            double delta = observedWavelength - restWavelength;
            return C * (delta / restWavelength);
        }

        // Parallax distance in parsec
        public static double ComputeDistanceParsec(double parallaxArcseconds)
        {
            // d(pc) = 1 / p(arcsec)
            return 1.0d / parallaxArcseconds;
        }

        // Celsius to Kelvin
        public static double ToKelvin(double celsius)
        {
            return celsius + 273.15d;
        }

        // Schwarzschild radius (event horizon)
        public static double ComputeEventHorizon(double massKg)
        {
            // R = 2GM / c^2
            return 2.0d * G * massKg / (C * C);
        }
    }
}
