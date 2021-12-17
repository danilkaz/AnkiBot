using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AnkiBot.Infrastructure;
using Infrastructure.Attributes;
using Npgsql;

namespace Infrastructure
{
    public class PostgresDatabase<T> : IDatabase<T>
    {
        private string connectionString;
        
        private static readonly IEnumerable<FieldAttribute> fields;
        private static readonly IEnumerable<PropertyInfo> propertyInfos;
        private static readonly string tableName;

        static PostgresDatabase()
        {
            tableName = typeof(T).GetCustomAttributes<TableAttribute>().FirstOrDefault()?.Name;
            if (tableName is null) throw new ArgumentException("Attribute Table must be "); // TODO поправить
            fields = typeof(T).GetProperties().SelectMany(p => p.GetCustomAttributes<FieldAttribute>());
            propertyInfos = typeof(T).GetProperties().Where(p => p.GetCustomAttributes<FieldAttribute>().Any());
        }
        
        public void Save(T item)
        {
            using var connection = new NpgsqlConnection(connectionString);
            connection.Open();
            var command = new NpgsqlCommand
            {
                Connection = connection,
                CommandText =
                    $"INSERT INTO {tableName} VALUES (" +
                    $"{string.Join(", ", propertyInfos.Select(p => $"'{p.GetValue(item)}'"))})"
            };
            command.ExecuteNonQuery();
        }

        public T Get(string id)
        {
            using var connection = new NpgsqlConnection(connectionString);
            connection.Open();
            var command = new NpgsqlCommand
            {
                Connection = connection,
                CommandText = $"SELECT * FROM {tableName} WHERE id == \"{id}\""
            };
            using var reader = command.ExecuteReader();
            var constructor = typeof(T)
                .GetConstructors()
                .FirstOrDefault(c => c.GetCustomAttributes<ConstructorAttribute>().Any());
            if (constructor is null)
                throw new ArgumentException(); // TODO написать
            if (reader.Read())
                return (T) constructor.Invoke(fields.Select(f => reader[f.Name]).ToArray());
            throw new ArgumentException();
        }

        public void Delete(string id)
        {
            using var connection = new NpgsqlConnection(connectionString);
            connection.Open();
            var command = new NpgsqlCommand
            {
                Connection = connection,
                CommandText = $"DELETE FROM {tableName} WHERE id == \"{id}\""
            };
            command.ExecuteNonQuery();
        }
        
        public IEnumerable<T> GetAll()
        {
            using var connection = new NpgsqlConnection(connectionString);
            connection.Open();
            var command = new NpgsqlCommand
            {
                Connection = connection,
                CommandText = $"SELECT * FROM {tableName}"
            };
            using var reader = command.ExecuteReader();
            var constructor = typeof(T)
                .GetConstructors()
                .FirstOrDefault(c => c.GetCustomAttributes<ConstructorAttribute>().Any());
            if (constructor is null)
                throw new ArgumentException();
            while (reader.Read())
                yield return (T) constructor.Invoke(fields.Select(f => reader[f.Name]).ToArray());
        }

        public void CreateTable(string connectionString)
        {
            this.connectionString = connectionString;
            
            using var connection = new NpgsqlConnection(connectionString);
            connection.Open();
            var createFields = fields
                .Select(f => f.IsUnique ? $"\"{f.Name}\" TEXT UNIQUE" : $"\"{f.Name}\" TEXT");
            var command = new NpgsqlCommand
            {
                Connection = connection,
                CommandText = $"CREATE TABLE IF NOT EXISTS  {tableName} (" +
                              $"{string.Join(", ", createFields)})"
            };
            command.ExecuteNonQuery();
        }
    }
}