using Dapper;
using Microsoft.Data.Sqlite;
using Questao5.Domain.Entities;
using System.Data;

namespace Questao5.Infrastructure.Sqlite
{
    public class DatabaseIdempotencia : IDatabaseIdempotencia
    {
        private readonly DatabaseConfig databaseConfig;

        public DatabaseIdempotencia(DatabaseConfig databaseConfig)
        {
            this.databaseConfig = databaseConfig;
        }

        public bool ValidaIdempotencia(string idRequisicao)
        {
            using var connection = new SqliteConnection(databaseConfig.Name);

            DynamicParameters parameters = new();
            parameters.Add("@idRequisicao", idRequisicao, DbType.String);

            var idempotencia = connection.QueryFirstOrDefault<bool>("SELECT chave_idempotencia FROM idempotencia WHERE requisicao = @idRequisicao", parameters);

            return idempotencia;
        }

        public int InsereIdempotencia(string idRequisicao)
        {
            using var connection = new SqliteConnection(databaseConfig.Name);

            DynamicParameters parameters = new();
            parameters.Add("@idRequisicao", idRequisicao, DbType.String);

            var ididempotencia = connection.QueryFirstOrDefault<int>("INSERT INTO IDEMPOTENCIA (REQUISICAO) VALUES (@idRequisicao); " +
                "select last_insert_rowid()", parameters);

            return ididempotencia;
        }

        public void UpdateIdempotencia(int idIdempotencia, string resultado)
        {
            using var connection = new SqliteConnection(databaseConfig.Name);

            DynamicParameters parameters = new();
            parameters.Add("@idIdempotencia", idIdempotencia, DbType.Int32);
            parameters.Add("@resultado", resultado, DbType.String);

            var ididempotencia = connection.Query(
                "UPDATE IDEMPOTENCIA set resultado = @resultado " +
                "WHERE chave_idempotencia = @idIdempotencia", parameters);
        }

        public Idempotencia RetornaIdempotencia(string idRequisicao)
        {
            using var connection = new SqliteConnection(databaseConfig.Name);

            DynamicParameters parameters = new();
            parameters.Add("@idRequisicao", idRequisicao, DbType.String);

            var idempotencia = connection.QueryFirstOrDefault<Idempotencia>("SELECT chave_idempotencia as chaveidempotencia, requisicao, resultado " + 
                "FROM idempotencia WHERE requisicao = @idRequisicao", parameters);

            return idempotencia;
        }
        
    }
}
