using LoanProcessing.Application.Authentication.Commands.Login;
using LoanProcessing.Application.Authentication.Commands.RegisterApprover;
using LoanProcessing.Application.Authentication.Commands.RegisterUser;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LoanProcessing.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class AuthenticationController : ControllerBase
    {
        private readonly IMediator _mediator;
        public AuthenticationController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpPost("RegisterUser")]
        public async Task<IActionResult> Register([FromBody] RegisterUserCommand request, CancellationToken cancellationToken)
            => Ok(await _mediator.Send(request, cancellationToken).ConfigureAwait(false));

        
        [HttpPost("RegisterApprover")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RegisterApprover([FromBody] RegisterApproverCommand request, CancellationToken cancellationToken)
            => Ok(await _mediator.Send(request, cancellationToken).ConfigureAwait(false));

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginCommand request, CancellationToken cancellationToken)
    => Ok(await _mediator.Send(request, cancellationToken).ConfigureAwait(false));
    }
}
