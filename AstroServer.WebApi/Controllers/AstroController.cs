using AstroServer.WebApi.Models;
using Microsoft.AspNetCore.Mvc;
using AstroMath;

namespace AstroServer.WebApi.Controllers
{
    [ApiController]
    [Route("api/v1/astro")]
    public class AstroController : ControllerBase
    {
        private readonly IAstroMathService _astroMathService;

        // Initialises the controller with the injected calculation service.
        public AstroController(IAstroMathService astroMathService)
        {
            _astroMathService = astroMathService;
        }

        [HttpPost("velocity")]
        public ActionResult<VelocityResponse> Velocity([FromBody] VelocityRequest req)
        {
            //if (req == null) return BadRequest("Invalid body."); no need anymore, coulb be managed by apicontroller
            if (req.RestWavelength <= 0 || req.ObservedWavelength <= 0)
                return BadRequest("Wavelengths must be positive.");

            double v = _astroMathService.ComputeVelocity(req.ObservedWavelength, req.RestWavelength);
            return Ok(new VelocityResponse { VelocityMps = v });
        }

        [HttpPost("distance")]
        public ActionResult<DistanceResponse> Distance([FromBody] DistanceRequest req)
        {
            if (req.ParallaxArcseconds <= 0)
                return BadRequest("Parallax must be positive.");

            double d = _astroMathService.ComputeDistanceParsec(req.ParallaxArcseconds);
            return Ok(new DistanceResponse { DistanceParsec = d });
        }

        [HttpPost("kelvin")]
        public ActionResult<KelvinResponse> Kelvin([FromBody] KelvinRequest req)
        {
            if (req.Celsius < -273.15d)
                return BadRequest("Celsius must be >= -273.15.");

            double k = _astroMathService.ToKelvin(req.Celsius);
            return Ok(new KelvinResponse { Kelvin = k });
        }

        [HttpPost("event-horizon")]
        public ActionResult<EventHorizonResponse> EventHorizon([FromBody] EventHorizonRequest req)
        {
            if (req.MassKg <= 0)
                return BadRequest("Mass must be posotive.");

            double r = _astroMathService.ComputeEventHorizon(req.MassKg);
            return Ok(new EventHorizonResponse { RadiusMeters = r });
        }
    }
}
