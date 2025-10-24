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

        public AstroController(IAstroMathService astroMathService)
        {
            _astroMathService = astroMathService;
        }

        // POST api/v1/astro/velocity
        [HttpPost("velocity")]
        public ActionResult<AstroDataTransfer> CalculateVelocity([FromBody] AstroDataTransfer request)
        {
            var response = new AstroDataTransfer();
            try
            {
                response.VelocityMps = _astroMathService.ComputeVelocity(request.ObservedWavelength.Value,request.RestWavelength.Value);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest($"Calculation error: {ex.Message}");
            }
        }

        // POST api/v1/astro/distance
        [HttpPost("distance")]
        public ActionResult<AstroDataTransfer> CalculateDistance([FromBody] AstroDataTransfer request)
        {
            var response = new AstroDataTransfer();
            try
            {
                response.DistanceParsec = _astroMathService.ComputeDistanceParsec(request.ParallaxArcseconds.Value);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest($"Calculation error: {ex.Message}");
            }
        }

        // POST api/v1/astro/kelvin
        [HttpPost("kelvin")]
        public ActionResult<AstroDataTransfer> CalculateKelvin([FromBody] AstroDataTransfer request)
        {
            var response = new AstroDataTransfer();
            try
            {
                response.Kelvin = _astroMathService.ToKelvin(request.Celsius.Value);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest($"Calculation error: {ex.Message}");
            }
        }

        // POST api/v1/astro/eventhorizon
        [HttpPost("eventhorizon")]
        public ActionResult<AstroDataTransfer> CalculateEventHorizon([FromBody] AstroDataTransfer request)
        {
            var response = new AstroDataTransfer();
            try
            {
                response.RadiusMeters = _astroMathService.ComputeEventHorizon(request.MassKg.Value);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest($"Calculation error: {ex.Message}");
            }
        }
    }
}