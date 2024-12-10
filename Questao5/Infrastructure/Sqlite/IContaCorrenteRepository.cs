namespace Questao5.Infrastructure.Sqlite
{
    public interface IContaCorrenteRepository
    {
        bool CheckIdempotencia(string chaveIdempotencia);
        object GetContaCorrente(string idContaCorrente);
        Task AddMovimento(object movimento);
        Task AddIdempotencia(object idempotencia);
    }
}
