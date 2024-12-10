using Dapper;
using MediatR;
using Questao5.Application.Queries.Requests;
using Questao5.Application.Queries.Responses;
using Questao5.Infrastructure.Sqlite;

namespace Questao5.Application.Handlers
{
    public class ConsultaSaldoHandler : IRequestHandler<ConsultaSaldoRequest, ConsultaSaldoResponse>
    {
        private readonly ISqliteConnectionFactory _connectionFactory;

        public ConsultaSaldoHandler(ISqliteConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<ConsultaSaldoResponse> Handle(ConsultaSaldoRequest request, CancellationToken cancellationToken)
        {
            using var connection = _connectionFactory.CreateConnection();

            // Verificar se a conta existe e está ativa
            var conta = await connection.QueryFirstOrDefaultAsync("SELECT * FROM contacorrente WHERE idcontacorrente = @IdContaCorrente AND ativo = 1", new { request.IdContaCorrente });
            if (conta == null)
            {
                throw new ArgumentException("Conta inválida ou inativa.", "INVALID_ACCOUNT");
            }

            // Calcular saldo (Créditos - Débitos)
            var saldo = await connection.QuerySingleOrDefaultAsync<decimal>(
                 "SELECT IFNULL(SUM(CASE WHEN tipomovimento = 'C' THEN valor ELSE 0 END), 0) - IFNULL(SUM(CASE WHEN tipomovimento = 'D' THEN valor ELSE 0 END), 0) " +
                 "FROM movimento WHERE idcontacorrente = @IdContaCorrente",
                 new { request.IdContaCorrente }
            );

            return new ConsultaSaldoResponse
            {
                NumeroConta = conta.numero,
                NomeTitular = conta.nome,
                Saldo = saldo,  // Aqui, 'saldo' é do tipo 'decimal', garantindo a compatibilidade
                DataHoraConsulta = DateTime.Now
            };
        }
    }
}
