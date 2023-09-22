using Dapper;
using Microsoft.Data.Sqlite;
using Questao5.Domain.Entities;
using System.Data;

namespace Questao5.Infrastructure.Sqlite
{
    public class DatabaseContaCorrente : IDatabaseContaCorrente
    {
        private readonly DatabaseConfig databaseConfig;

        public DatabaseContaCorrente(DatabaseConfig databaseConfig)
        {
            this.databaseConfig = databaseConfig;
        }

        public ContaCorrente RetornaContaCorrente(string idContaCorrente)
        {
            using var connection = new SqliteConnection(databaseConfig.Name);

            DynamicParameters parameters = new();
            parameters.Add("@idContaCorrente", idContaCorrente, DbType.String);

                var contaCorrente = connection.QueryFirstOrDefault<ContaCorrente>("SELECT idcontacorrente, numero, nome, ativo FROM contacorrente WHERE idContaCorrente = @idContaCorrente", parameters);

            if (contaCorrente == null)
            {
                throw new ArgumentException("INVALID_ACCOUNT.");
            }
            else if (!contaCorrente.Ativo)
            {
                throw new ArgumentException("INACTIVE_ACCOUNT.");
            }

            return contaCorrente;
        }

        public ContaCorrente RetornaSaldo(string idContaCorrente)
        {
            using var connection = new SqliteConnection(databaseConfig.Name);

            DynamicParameters parameters = new();
            parameters.Add("@idContaCorrente", idContaCorrente, DbType.String);

            var contaCorrente = connection.QueryFirstOrDefault<ContaCorrente>(
                "SELECT idcontacorrente, numero, nome, ativo, " +
                "(SELECT ifnull(SUM(VALOR),0) FROM MOVIMENTO WHERE idcontacorrente = a.idcontacorrente and tipomovimento = 'C') - " +
                "(SELECT ifnull(SUM(VALOR), 0) FROM MOVIMENTO WHERE idcontacorrente = a.idcontacorrente and tipomovimento = 'D') as saldo, " +
                "datetime('now') as DataConsulta " +
                "FROM contacorrente as a " +
                "WHERE idContaCorrente = @idContaCorrente", parameters);

            return contaCorrente;
        }

        public int InserirMovimentacao(ContaCorrente conta)
        {
            using var connection = new SqliteConnection(databaseConfig.Name);

            DynamicParameters parameters = new();
            parameters.Add("@idContaCorrente", conta.IdContaCorrente, DbType.String);
            parameters.Add("@tipomovimento", conta.TipoMovimento, DbType.Int32);
            parameters.Add("@valor", conta.Valor, DbType.Decimal);

            var idmovimento = connection.QueryFirstOrDefault<int>(
                "INSERT INTO movimento ( idcontacorrente, datamovimento, tipomovimento, valor) " +
                "VALUES (@idContaCorrente, strftime('%d/%m/%Y', datetime('now')), @tipomovimento, @valor); " +
                "select last_insert_rowid()", parameters);

            return idmovimento;
        }
    }
}
