using Astro.Contracts.Dtos;
using AstroMath;
using Microsoft.AspNetCore.Mvc;

namespace AstroServer.WebApi.Controllers
{
    [ApiController]
    [Route("api/v1/astro")]
    public class AstroController : ControllerBase
    {
        [HttpPost("velocity")]
        public ActionResult<VelocityResponse> Velocity([FromBody] VelocityRequest req)
        {
            if (req == null) return BadRequest("Invalid body.");
            if (req.RestWavelength <= 0 || req.ObservedWavelength <= 0)
                return BadRequest("Wavelengths must be positive.");

            double v = StarMath.ComputeVelocity(req.ObservedWavelength, req.RestWavelength);
            return Ok(new VelocityResponse { VelocityMps = v });
        }

        [HttpPost("distance")]
        public ActionResult<DistanceResponse> Distance([FromBody] DistanceRequest req)
        {
            if (req == null) return BadRequest("Invalid body.");
            if (req.ParallaxArcseconds <= 0)
                return BadRequest("Parallax must be positive.");

            double d = StarMath.ComputeDistanceParsec(req.ParallaxArcseconds);
            return Ok(new DistanceResponse { DistanceParsec = d });
        }

        [HttpPost("kelvin")]
        public ActionResult<KelvinResponse> Kelvin([FromBody] KelvinRequest req)
        {
            if (req == null) return BadRequest("Invalid body.");
            if (req.Celsius < -273.15d)
                return BadRequest("Celsius must be >= -273.15.");

            double k = StarMath.ToKelvin(req.Celsius);
            return Ok(new KelvinResponse { Kelvin = k });
        }

        [HttpPost("event-horizon")]
        public ActionResult<EventHorizonResponse> EventHorizon([FromBody] EventHorizonRequest req)
        {
            if (req == null) return BadRequest("Invalid body.");
            if (req.MassKg <= 0)
                return BadRequest("Mass must be posotive.");

            double r = StarMath.ComputeEventHorizon(req.MassKg);
            return Ok(new EventHorizonResponse { RadiusMeters = r });
        }
    }
}
