using Dapper;
using MediatR;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Commands.Responses;
using Questao5.Domain.Enumerators.Questao5.Domain.Enumerators;
using Questao5.Infrastructure.Sqlite;

namespace Questao5.Application.Handlers
{

    public class MovimentacaoHandler : IRequestHandler<MovimentacaoRequest, MovimentacaoResponse>
    {
        private readonly IContaCorrenteRepository _repository;

        public MovimentacaoHandler(IContaCorrenteRepository repository)
        {
            _repository = repository;
        }

        public async Task<MovimentacaoResponse> Handle(MovimentacaoRequest request, CancellationToken cancellationToken)
        {
            // Valida tipo de movimento
            if (!Enum.IsDefined(typeof(TipoMovimento), request.TipoMovimento))
            {
                return new MovimentacaoResponse
                {
                    Error = "Tipo de movimento inválido",
                    ErrorType = "INVALID_TYPE"
                };
            }

            // Valida valor positivo
            if (request.Valor <= 0)
            {
                return new MovimentacaoResponse
                {
                    Error = "Valor inválido. Deve ser positivo",
                    ErrorType = "INVALID_VALUE"
                };
            }

            // Verifica idempotência
            var isDuplicateRequest = _repository.CheckIdempotencia(request.Idempotencia);
            if (isDuplicateRequest)
            {
                return new MovimentacaoResponse
                {
                    Error = "Requisição já processada",
                    ErrorType = "IDEMPOTENCY_VIOLATED"
                };
            }

            // Verifica conta corrente
            var conta = _repository.GetContaCorrente(request.IdContaCorrente);
            if (conta == null)
            {
                return new MovimentacaoResponse
                {
                    Error = "Conta corrente não cadastrada ou inativa",
                    ErrorType = "INVALID_ACCOUNT"
                };
            }

            // Insere o movimento
            var idMovimento = Guid.NewGuid().ToString();
            await _repository.AddMovimento(new
            {
                IdMovimento = idMovimento,
                IdContaCorrente = request.IdContaCorrente,
                DataMovimento = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"),
                TipoMovimento = request.TipoMovimento.ToString(),
                Valor = request.Valor
            });

            // Registra a idempotência
            await _repository.AddIdempotencia(new
            {
                ChaveIdempotencia = request.Idempotencia,
                Requisicao = "Movimentação",
                Resultado = "Sucesso"
            });

            return new MovimentacaoResponse { IdMovimento = idMovimento };
        }
    }
}