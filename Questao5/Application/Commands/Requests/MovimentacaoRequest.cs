using MediatR;
using Questao5.Application.Commands.Responses;
using Questao5.Domain.Enumerators.Questao5.Domain.Enumerators;

namespace Questao5.Application.Commands.Requests
{
    public class MovimentacaoRequest : IRequest<MovimentacaoResponse>
    {
        public string IdContaCorrente { get; set; }
        public decimal Valor { get; set; }
        public TipoMovimento TipoMovimento { get; set; } // 'C' para Crédito, 'D' para Débito
        public string Idempotencia { get; set; } // Chave de idempotência
    }
}
