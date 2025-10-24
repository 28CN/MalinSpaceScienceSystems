using AstroServer.WebApi.Models; // Use the new Models namespace
using Microsoft.AspNetCore.Mvc;
using AstroMath; // Ensure this using exists for IAstroMathService

namespace AstroServer.WebApi.Controllers
{
    [ApiController]
    [Route("api/v1/astro")] // Keep the base route
    public class AstroController : ControllerBase
    {
        private readonly IAstroMathService _astroMathService; // Injected service

        public AstroController(IAstroMathService astroMathService)
        {
            _astroMathService = astroMathService;
        }

        // Unified endpoint for all calculations.
        [HttpPost("calculate")] // New route for the unified method
        public ActionResult<AstroDataTransfer> Calculate([FromBody] AstroDataTransfer request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Prepare the response object, preserving the request type.
            var response = new AstroDataTransfer { Type = request.Type };

            try
            {
                // Perform calculation based on the request Type.
                switch (request.Type)
                {
                    case CalculationType.Velocity:
                        // Check if required fields are provided in the request payload.
                        if (!request.ObservedWavelength.HasValue || !request.RestWavelength.HasValue)
                        {
                            response.ErrorMessage = "ObservedWavelength and RestWavelength are required.";
                        }
                        else
                        {
                            // Call the calculation service.
                            response.VelocityMps = _astroMathService.ComputeVelocity(
                                request.ObservedWavelength.Value,
                                request.RestWavelength.Value);
                        }
                        break;

                    case CalculationType.Distance:
                        if (!request.ParallaxArcseconds.HasValue)
                        {
                            response.ErrorMessage = "ParallaxArcseconds is required.";
                        }
                        else
                        {
                            response.DistanceParsec = _astroMathService.ComputeDistanceParsec(
                                request.ParallaxArcseconds.Value);
                        }
                        break;

                    case CalculationType.Kelvin:
                        if (!request.Celsius.HasValue)
                        {
                            response.ErrorMessage = "Celsius is required.";
                        }
                        else
                        {
                            response.Kelvin = _astroMathService.ToKelvin(
                                request.Celsius.Value);
                        }
                        break;

                    case CalculationType.EventHorizon:
                        if (!request.MassKg.HasValue)
                        {
                            response.ErrorMessage = "MassKg is required.";
                        }
                        else
                        {
                            response.RadiusMeters = _astroMathService.ComputeEventHorizon(
                                request.MassKg.Value);
                        }
                        break;

                    default: // Handle unknown type.
                        response.ErrorMessage = "Invalid calculation type specified.";
                        break;
                }
            }
            catch (Exception ex) // Catch potential errors.
            {
                response.ErrorMessage = $"Calculation error: {ex.Message}";
            }

            return Ok(response);
        }
    }
}