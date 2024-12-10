namespace Questao5.Application.Queries.Responses
{
    public class ConsultaSaldoResponse
    {
        public long NumeroConta { get; set; }
        public string NomeTitular { get; set; }
        public decimal Saldo { get; set; }
        public DateTime DataHoraConsulta { get; set; }
    }
}
