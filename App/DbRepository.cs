using System.Collections.Generic;
using System.Linq;
using AnkiBot.App;
using AnkiBot.Domain;
using AnkiBot.Domain.LearnMethods;
using AnkiBot.Infrastructure;
using App.SerializedClasses;

namespace App
{
    public class DbRepository : IRepository
    {
        private readonly IDatabase<DbCard> cardDatabase;
        private readonly IDatabase<DbDeck> deckDatabase;

        public DbRepository(IDatabase<DbCard> cardDatabase, IDatabase<DbDeck> deckDatabase)
        {
            this.cardDatabase = cardDatabase;
            this.deckDatabase = deckDatabase;
        }

        public void SaveCard(Card card)
        {
            cardDatabase.Save(new DbCard(card));
        }

        public DbCard GetCard(string cardId)
        {
            var dbCard = cardDatabase.Get(cardId);
            return dbCard;
        }

        public void UpdateCard(Card card)
        {
            DeleteCard(card.Id.ToString());
            SaveCard(card);
        }

        public void DeleteCard(string cardId)
        {
            cardDatabase.Delete(cardId);
        }

        public void SaveDeck(Deck deck)
        {
            deckDatabase.Save(new DbDeck(deck));
        }

        public DbDeck GetDeck(string deckId)
        {
            var dbDeck = deckDatabase.Get(deckId);
            return dbDeck;
            // return ConvertDbDeckToDeck(dbDeck, GetCardsByDeckId(deckId));
        }

        public void DeleteDeck(string deckId)
        {
            foreach (var card in GetCardsByDeckId(deckId))
                cardDatabase.Delete(card.Id);
            deckDatabase.Delete(deckId);
        }

        public IEnumerable<string> GetDecksNamesByUser(User user)
        {
            return deckDatabase.GetAll(d => d.UserId == user.Id)
                .Select(d => d.Name);
        }

        public IEnumerable<DbDeck> GetDecksByUser(User user)
        {
            return deckDatabase.GetAll(d => d.UserId == user.Id);
        }

        public IEnumerable<DbCard> GetCardsByDeckId(string deckId)
        {
            return cardDatabase.GetAll(c => c.DeckId == deckId);
        }

        // private Deck ConvertDbDeckToDeck(DbDeck dbDeck, IEnumerable<Card> cards)
        // {
        //     var method = learnMethods.FirstOrDefault(m => m.Name == dbDeck.LearnMethod);
        //     return new Deck(Guid.Parse(dbDeck.Id), new User(dbDeck.UserId), dbDeck.Name, method, cards);
        // }
        //
        // private static Card ConvertDbCardToCard(DbCard dbCard)
        // {
        //     var parameters = JsonConvert.DeserializeObject<IParameters>(dbCard.Parameters);
        //     return new Card(Guid.Parse(dbCard.Id), new User(dbCard.UserId), Guid.Parse(dbCard.DeckId), dbCard.Front,
        //         dbCard.Back,
        //         TimeSpan.Parse(dbCard.TimeBeforeLearn),
        //         DateTime.Parse(dbCard.LastLearnTime, CultureInfo.InvariantCulture), parameters);
        // }
    }
}