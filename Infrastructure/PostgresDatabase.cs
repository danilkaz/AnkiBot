using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Infrastructure.Attributes;
using Npgsql;

namespace Infrastructure
{
    public class PostgresDatabase<T> : IDatabase<T>, IDisposable
    {
        private static readonly IEnumerable<FieldAttribute> fields;
        private static readonly IEnumerable<PropertyInfo> propertyInfos;
        private static readonly string tableName;

        private NpgsqlConnection connection;

        static PostgresDatabase()
        {
            tableName = typeof(T).GetCustomAttributes<TableAttribute>().FirstOrDefault()?.Name;
            if (tableName is null) throw new ArgumentException("Attribute Table must be initialized in class");
            fields = typeof(T).GetProperties().SelectMany(p => p.GetCustomAttributes<FieldAttribute>());
            propertyInfos = typeof(T).GetProperties().Where(p => p.GetCustomAttributes<FieldAttribute>().Any());
        }

        public void Save(T item)
        {
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
                throw new ArgumentException("Constructor Attributes must be initialized in constructor class");
            if (reader.Read())
                return (T) constructor.Invoke(fields.Select(f => reader[f.Name]).ToArray());
            throw new ArgumentException("Can't create element");
        }

        public void Delete(string id)
        {
            var command = new NpgsqlCommand
            {
                Connection = connection,
                CommandText = $"DELETE FROM {tableName} WHERE id == \"{id}\""
            };
            command.ExecuteNonQuery();
        }

        public IEnumerable<T> GetAll(Func<T, bool> filter)
        {
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
            var result = new List<T>();
            while (reader.Read())
            {
                var elem = (T) constructor.Invoke(fields.Select(f => reader[f.Name]).ToArray());
                if (filter(elem))
                    result.Add(elem);
            }

            return result;
        }

        public void CreateTable()
        {
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

        public void Dispose()
        {
            connection?.Dispose();
        }
    }
}