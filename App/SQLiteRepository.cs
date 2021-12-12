// using System;
// using System.Collections.Generic;
// using System.Text;
// using AnkiBot.App;
// using AnkiBot.Domain;
// using Microsoft.Data.Sqlite;
//
// namespace App
// {
//     public class SQLiteRepository : IRepository, IDisposable
//     {
//         private bool isDisposed;
//         private readonly SqliteConnection connection;
//         public SQLiteRepository(string connectionString)
//         {
//             connection = new SqliteConnection(connectionString);
//             connection.Open();
//             var command = new SqliteCommand
//             {
//                 Connection = connection,
//                 CommandText =
//                     "CREATE TABLE IF NOT EXISTS Decks(_id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE," +
//                     " userId TEXT NOT NULL," +
//                     " name TEXT NOT NULL," +
//                     " LearnMethod TEXT NOT NULL)"
//             };
//             command.ExecuteNonQuery();
//             command = new SqliteCommand
//             {
//                 Connection = connection,
//                 CommandText =
//                     "CREATE TABLE IF NOT EXISTS Cards(_id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE," +
//                     " userId TEXT NOT NULL," +
//                     " front TEXT NOT NULL," +
//                     " back TEXT NOT NULL, " +
//                     " deckId TEXT NOT NULL," +
//                     " TimeBeforeLearn INTEGER NOT NULL," +
//                     " LastLearnTime INTEGER NOT NULL)"
//             };
//             command.ExecuteNonQuery();
//         }
//
//         ~SQLiteRepository()
//         {
//             Dispose();
//         }
//
//         public void SaveCard(Card card)
//         {
//             var command = new SqliteCommand
//             {
//                 Connection = connection,
//                 CommandText = $"INSERT INTO Cards VALUES ({card.UserId}, {card.Front}, {card.Back}, {card.DeckId}, {card.TimeBeforeLearn}, {card.LastLearnTime})"
//             };
//             command.ExecuteNonQuery();
//         }
//
//         public Card GetCard(string cardId)
//         {
//             throw new NotImplementedException();
//         }
//
//         public void SaveDeck(Deck deck)
//         {
//             var command = new SqliteCommand
//             {
//                 Connection = connection,
//                 CommandText = $"INSERT INTO Decks (userId, name, LearnMethod) VALUES (\"{deck.UserId}\", \"{deck.Name}\", \"{deck.LearnMethod.Name}\")"
//             };
//             command.ExecuteNonQuery();
//         }
//
//         public Deck GetDeck(string deckId)
//         {
//             throw new NotImplementedException();
//         }
//
//         public IEnumerable<Deck> GetDecksByUserId(string userId)
//         {
//             throw new NotImplementedException();
//         }
//
//         public IEnumerable<Card> GetCardsByDeckId(string deckId)
//         {
//             throw new NotImplementedException();
//         }
//
//         public IEnumerable<Card> GetCardsToLearn(string deckId)
//         {
//             throw new NotImplementedException();
//         }
//
//         public void Dispose()
//         {
//             if (isDisposed) return;
//             connection.Close();
//             isDisposed = true;
//         }
//     }
// }

