using Dapper;
using Questao5.Infrastructure.Sqlite;

namespace Questao5.Infrastructure.Repositories
{
    public class ContaCorrenteRepository : IContaCorrenteRepository
    {
        private readonly ISqliteConnectionFactory _connectionFactory;

        public ContaCorrenteRepository(ISqliteConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public bool CheckIdempotencia(string chaveIdempotencia)
        {
            using var connection = _connectionFactory.CreateConnection();
            return connection.QueryFirstOrDefault<int>(
                "SELECT COUNT(1) FROM idempotencia WHERE chave_idempotencia = @ChaveIdempotencia",
                new { ChaveIdempotencia = chaveIdempotencia }) > 0;
        }

        public object GetContaCorrente(string idContaCorrente)
        {
            using var connection = _connectionFactory.CreateConnection();
            return connection.QueryFirstOrDefault(
                "SELECT * FROM contacorrente WHERE idcontacorrente = @IdContaCorrente AND ativo = 1",
                new { IdContaCorrente = idContaCorrente });
        }

        public async Task AddMovimento(object movimento)
        {
            using var connection = _connectionFactory.CreateConnection();
            await connection.ExecuteAsync(
                "INSERT INTO movimento (idmovimento, idcontacorrente, datamovimento, tipomovimento, valor) VALUES (@IdMovimento, @IdContaCorrente, @DataMovimento, @TipoMovimento, @Valor)",
                movimento);
        }

        public async Task AddIdempotencia(object idempotencia)
        {
            using var connection = _connectionFactory.CreateConnection();
            await connection.ExecuteAsync(
                "INSERT INTO idempotencia (chave_idempotencia, requisicao, resultado) VALUES (@ChaveIdempotencia, @Requisicao, @Resultado)",
                idempotencia);
        }
    }
}
