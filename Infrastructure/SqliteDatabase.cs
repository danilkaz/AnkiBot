using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AnkiBot.Infrastructure;
using Infrastructure.Attributes;
using Microsoft.Data.Sqlite;

namespace Infrastructure
{
    public class SqLiteDatabase<T> : IDatabase<T>, IDisposable
    {
        private readonly SqliteConnection connection;

        private readonly string tableName;
        private readonly IEnumerable<FieldAttribute> fields;
        private readonly IEnumerable<PropertyInfo> propertyInfos;

        public SqLiteDatabase(string connectionString)
        {
            connection = new SqliteConnection(connectionString);
            connection.Open();
            tableName = typeof(T).GetCustomAttributes<TableAttribute>().FirstOrDefault()?.Name;
            if (tableName is null) throw new ArgumentException("Attribute Table must be ");
            fields = typeof(T).GetProperties().SelectMany(p => p.GetCustomAttributes<FieldAttribute>());
            propertyInfos = typeof(T).GetProperties().Where(p => p.GetCustomAttributes<FieldAttribute>().Any());
            CreateTable();
        }


        public void Save(T item)
        {
            var command = new SqliteCommand
            {
                Connection = connection,
                CommandText =
                    $"INSERT INTO {tableName} VALUES (" +
                    $"{string.Join(", ", propertyInfos.Select(p => $"\"{p.GetValue(item)}\""))})"
            };
            command.ExecuteNonQuery();
        }

        public T Get(string id)
        {
            connection.Open();
            var command = new SqliteCommand()
            {
                Connection = connection,
                CommandText = $"SELECT * FROM {tableName} WHERE id == \"{id}\""
            };
            var reader = command.ExecuteReader();
            var constructor = typeof(T)
                .GetConstructors()
                .FirstOrDefault(c => c.GetCustomAttributes<ConstructorAttribute>().Any());
            if (constructor is null)
                throw new ArgumentException();
            if (reader.Read())
                return (T) constructor.Invoke(fields.Select(f => reader[f.Name]).ToArray());
            throw new ArgumentException();
        }

        public IEnumerable<T> Where(Func<T, bool> filter)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> GetAll()
        {
            var command = new SqliteCommand()
            {
                Connection = connection,
                CommandText = $"SELECT * FROM {tableName}"
            };
            var reader = command.ExecuteReader();
            var constructor = typeof(T)
                .GetConstructors()
                .FirstOrDefault(c => c.GetCustomAttributes<ConstructorAttribute>().Any());
            if (constructor is null)
                throw new ArgumentException();
            while (reader.Read())
                yield return (T) constructor.Invoke(fields.Select(f => reader[f.Name]).ToArray());
        }

        private void CreateTable()
        {
            var createFields = fields
                .Select(f => f.IsUnique ? $"\"{f.Name}\" TEXT UNIQUE" : $"\"{f.Name}\" TEXT");
            var command = new SqliteCommand
            {
                Connection = connection,
                CommandText = $"CREATE TABLE IF NOT EXISTS  {tableName} (" +
                              $"{string.Join(", ", createFields)})"
            };
            command.ExecuteNonQuery();
        }

        ~SqLiteDatabase() => Dispose();

        public void Dispose()
        {
            connection.Close();
            connection.Dispose();
        }
    }
}