using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnkiBot.Infrastructure;
using Microsoft.Data.Sqlite;

namespace Infrastructure
{
    public class SQLiteDatabase<T> : IDatabase<T>, IDisposable
    {
        private readonly SqliteConnection connection;
        private string tableName;
        public SQLiteDatabase(string connectionString)
        {
            connection = new SqliteConnection(connectionString);
            connection.Open();
            tableName = typeof(T).ToString().Split('.').Last();
            var a = typeof(T).GetProperties();
            var command = new SqliteCommand
            {
                Connection = connection,
                CommandText =
                    $"CREATE TABLE IF NOT EXISTS  {tableName} (" +
                    $"{string.Join(", ",typeof(T).GetProperties().Select(x=> x.Name + " TEXT"))})"
            };
            command.ExecuteNonQuery();
        }

        ~SQLiteDatabase()
        {
            Dispose();
        }

        public void Save(T item)
        {
            var command = new SqliteCommand
            {
                Connection = connection,
                CommandText =
                    $"INSERT INTO {tableName} VALUES (" +
                    $"{string.Join(", ", typeof(T).GetProperties().Select(x => "\""+ x.GetValue(item) + "\""))})"
            };
            command.ExecuteNonQuery();
        }

        public T Get(string id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> Where(Func<T, bool> filter)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            connection?.Dispose();
        }
    }
}
