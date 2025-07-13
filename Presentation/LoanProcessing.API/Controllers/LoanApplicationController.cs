using LoanProcessing.Application.Authentication.Commands.RegisterApprover;
using LoanProcessing.Application.Authentication.Commands.RegisterUser;
using LoanProcessing.Application.LoanApplication.Commands.ApproveLoanApplication;
using LoanProcessing.Application.LoanApplication.Commands.CreateLoanApplication;
using LoanProcessing.Application.LoanApplication.Commands.DeleteLoanApplication;
using LoanProcessing.Application.LoanApplication.Commands.EditLoanApplication;
using LoanProcessing.Application.LoanApplication.Commands.SubmitLoanApplication;
using LoanProcessing.Application.LoanApplication.Query.GetLoanApplicationsList;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LoanProcessing.API.Controllers
{ 
    [ApiController]
    [Route("api/[controller]")]
    public class LoanApplicationController : ControllerBase
    {
        private readonly IMediator _mediator;
        public LoanApplicationController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }
         
        [HttpPost("CreateLoanApplication")]
        public async Task<IActionResult> CreateLoanApplication([FromBody] CreateLoanApplicationCommand request, CancellationToken cancellationToken)
            => Ok(await _mediator.Send(request, cancellationToken).ConfigureAwait(false));

        [HttpPut("UpdateLoanApplication")]
        public async Task<IActionResult> UpdateLoanApplication([FromBody] EditLoanApplicationCommand request, CancellationToken cancellationToken)
            => Ok(await _mediator.Send(request, cancellationToken).ConfigureAwait(false));


        [HttpPut("ApproveLoanApplication")]
        [Authorize(Roles = "Approver")]
        public async Task<IActionResult> ApproveLoanApplication([FromBody] ApproveLoanApplicationCommand request, CancellationToken cancellationToken)
            => Ok(await _mediator.Send(request, cancellationToken).ConfigureAwait(false));


        [HttpDelete("RejectLoanApplication")]

        public async Task<IActionResult> RejectLoanApplication([FromBody] RejectLoanApplicationCommand request, CancellationToken cancellationToken)
            => Ok(await _mediator.Send(request, cancellationToken).ConfigureAwait(false));

        [HttpPost("SubmitLoanApplication")]

        public async Task<IActionResult> SubmitLoanApplication([FromBody] SubmitLoanApplicationCommand request, CancellationToken cancellationToken)
            => Ok(await _mediator.Send(request, cancellationToken).ConfigureAwait(false));

        [HttpGet("GetLoanApplicationsList")]
        public async Task<IActionResult> GetLoanApplicationsList([FromQuery] GetLoanApplicationsQuery request, CancellationToken cancellationToken)
            => Ok(await _mediator.Send(request, cancellationToken).ConfigureAwait(false));

        [HttpGet("GetAllLoanApplicationsList")]
        public async Task<IActionResult> GetAllLoanApplicationsList([FromQuery] GetLoanApplicationsListQuery request, CancellationToken cancellationToken)
            => Ok(await _mediator.Send(request, cancellationToken).ConfigureAwait(false));

    }
}
