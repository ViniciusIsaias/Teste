using Questao5.Domain.Entities;

namespace Questao5.Infrastructure.Sqlite
{
    public interface IDatabaseContaCorrente
    {
        ContaCorrente RetornaContaCorrente(string idContaCorrente);

        ContaCorrente RetornaSaldo(string numero);

        int InserirMovimentacao(ContaCorrente conta);
    }
}
