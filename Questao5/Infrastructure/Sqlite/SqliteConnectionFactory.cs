using Microsoft.Data.Sqlite;

namespace Questao5.Infrastructure.Sqlite
{
    public class SqliteConnectionFactory : ISqliteConnectionFactory
    {
        private readonly string _connectionString;

        public SqliteConnectionFactory(DatabaseConfig databaseConfig)
        {
            _connectionString = databaseConfig.Name;
        }

        public SqliteConnection CreateConnection()
        {
            return new SqliteConnection(_connectionString);
        }
    }
}