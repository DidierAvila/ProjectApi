using System.Threading.Tasks;
using System.Threading;
using Business.Security;
using Domain.Dtos.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SecurityController : ControllerBase
    {
        private readonly ILogger<SecurityController> _logger;
        private readonly ISecurityService _SecurityService;

        public SecurityController(ISecurityService securityService, ILogger<SecurityController> logger)
        {
            _SecurityService = securityService;
            _logger = logger;
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest autorizacion, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Login");
            LoginResponse? Response = await _SecurityService.Login(autorizacion, cancellationToken);
            if (Response == null)
                return BadRequest();

            return Ok(Response.Token);
        }
    }
}
