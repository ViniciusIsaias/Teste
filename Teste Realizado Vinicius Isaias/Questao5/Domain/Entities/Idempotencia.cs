namespace Questao5.Domain.Entities
{
    public class Idempotencia
    {
        public int ChaveIdempotencia { get; set; }

        public string Requisicao { get; set; }

        public string Resultado { get; set; }
    }
}
