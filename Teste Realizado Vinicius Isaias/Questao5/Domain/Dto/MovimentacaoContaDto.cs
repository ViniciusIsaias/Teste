namespace Questao5.Domain.ViewModel
{
    public class MovimentacaoContaDto
    {
        public string IdRequisicao { get; set; }

        public string IdContaCorrente { get; set; }

        public decimal Valor { get; set; }

        public string TipoMovimento { get; set; }
    }
}
