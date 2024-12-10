using MediatR;
using Questao5.Application.Queries.Responses;

namespace Questao5.Application.Queries.Requests
{
    public class ConsultaSaldoRequest : IRequest<ConsultaSaldoResponse>
    {
        public string IdContaCorrente { get; set; }
    }
}
