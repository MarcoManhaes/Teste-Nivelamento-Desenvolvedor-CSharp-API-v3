using MediatR;
using Microsoft.AspNetCore.Mvc;
using Questao5.Application.Queries.Requests;

namespace Questao5.Infrastructure.Services.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SaldoController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SaldoController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{idContaCorrente}")]
        public async Task<IActionResult> ConsultarSaldo(string idContaCorrente)
        {
            try
            {
                var response = await _mediator.Send(new ConsultaSaldoRequest { IdContaCorrente = idContaCorrente });
                return Ok(new
                {
                    response.NumeroConta,
                    response.NomeTitular,
                    response.Saldo,
                    response.DataHoraConsulta
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { ex.Message, ErrorType = ex.ParamName });
            }
        }
    }
}
