using Questao5.Domain.Entities;

namespace Questao5.Infrastructure.Sqlite
{
    public interface IDatabaseIdempotencia
    {
        bool ValidaIdempotencia(string idRequisicao);

        int InsereIdempotencia(string idRequisicao);

        void UpdateIdempotencia(int idIdempotencia, string resultado);

        Idempotencia RetornaIdempotencia(string idRequisicao);
    }
}
