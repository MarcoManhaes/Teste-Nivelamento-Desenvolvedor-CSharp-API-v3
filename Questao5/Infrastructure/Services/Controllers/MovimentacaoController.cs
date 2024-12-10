using MediatR;
using Microsoft.AspNetCore.Mvc;
using Questao5.Application.Commands.Requests;

namespace Questao5.Infrastructure.Services.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MovimentacaoController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MovimentacaoController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Movimentar([FromBody] MovimentacaoRequest request)
        {
            var response = await _mediator.Send(request);
            if (!string.IsNullOrEmpty(response.Error))
            {
                return BadRequest(new { response.Error, response.ErrorType });
            }
            return Ok(new { IdMovimento = response.IdMovimento });
        }
    }
}
