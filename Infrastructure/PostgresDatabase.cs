#nullable enable
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
        private static readonly IEnumerable<FieldAttribute> Fields;
        private static readonly IEnumerable<PropertyInfo> PropertyInfos;
        private static readonly string? TableName;
        private static readonly ConstructorInfo? Constructor;

        private readonly NpgsqlConnection connection;

        static PostgresDatabase()
        {
            var type = typeof(T);
            Constructor = type.GetConstructors()
                .FirstOrDefault(c => c.GetCustomAttributes<ConstructorAttribute>().Any());
            TableName = type.GetCustomAttributes<TableAttribute>().FirstOrDefault()?.Name;
            if (TableName is null) throw new ArgumentException("Attribute Table must be initialized in class");
            Fields = type.GetProperties().SelectMany(p => p.GetCustomAttributes<FieldAttribute>());
            PropertyInfos = type.GetProperties().Where(p => p.GetCustomAttributes<FieldAttribute>().Any());
        }

        public PostgresDatabase(NpgsqlConnection connection)
        {
            this.connection = connection;
        }

        public void Save(T item)
        {
            var command = new NpgsqlCommand
            {
                Connection = connection,
                CommandText =
                    $"INSERT INTO {TableName} VALUES (" +
                    $"{string.Join(", ", PropertyInfos.Select(p => $"'{p.GetValue(item)}'"))})"
            };
            command.ExecuteNonQuery();
        }

        public T Get(string id)
        {
            var command = new NpgsqlCommand
            {
                Connection = connection,
                CommandText = $"SELECT * FROM {TableName} WHERE id == \"{id}\""
            };
            using var reader = command.ExecuteReader();
            if (Constructor is null)
                throw new ArgumentException("Constructor Attributes must be initialized in constructor class");
            if (reader.Read())
                return (T) Constructor.Invoke(Fields.Select(f => reader[f.Name]).ToArray());
            throw new ArgumentException("Can't create element");
        }

        public void Delete(string id)
        {
            var command = new NpgsqlCommand
            {
                Connection = connection,
                CommandText = $"DELETE FROM {TableName} WHERE id == \"{id}\""
            };
            command.ExecuteNonQuery();
        }

        public IEnumerable<T> GetAll(Func<T, bool> filter)
        {
            var command = new NpgsqlCommand
            {
                Connection = connection,
                CommandText = $"SELECT * FROM {TableName}"
            };
            using var reader = command.ExecuteReader();
            if (Constructor is null)
                throw new ArgumentException();
            var result = new List<T>();
            while (reader.Read())
            {
                var elem = (T) Constructor.Invoke(Fields.Select(f => reader[f.Name]).ToArray());
                if (filter(elem))
                    result.Add(elem);
            }

            return result;
        }

        public void CreateTable()
        {
            var createFields = Fields
                .Select(f => f.IsUnique ? $"\"{f.Name}\" TEXT UNIQUE" : $"\"{f.Name}\" TEXT");
            var command = new NpgsqlCommand
            {
                Connection = connection,
                CommandText = $"CREATE TABLE IF NOT EXISTS  {TableName} (" +
                              $"{string.Join(", ", createFields)})"
            };
            command.ExecuteNonQuery();
        }

        public void Dispose()
        {
            connection.Dispose();
        }
    }
}