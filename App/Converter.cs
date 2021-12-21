using System;
using System.Globalization;
using System.Linq;
using AnkiBot.App;
using AnkiBot.Domain;
using AnkiBot.Domain.LearnMethods;
using AnkiBot.Domain.Parameters;
using App.SerializedClasses;
using App.UIClasses;
using Newtonsoft.Json;

namespace App
{
    public class Converter
    {
        private readonly IRepository repository;
        private readonly ILearnMethod[] learnMethods;

        public Converter(IRepository repository, ILearnMethod[] learnMethods)
        {
            this.repository = repository;
            this.learnMethods = learnMethods;
        }

        public UIDeck ToUiDeck(DbDeck deck)
        {
            return new UIDeck(deck.Id, deck.Name);
        }

        public UICard ToUiCard(DbCard card)
        {
            return new UICard(card.Id, card.Front, card.Back);
        }
        
        public Deck ToDeck(DbDeck dbDeck)
        {
            var method = learnMethods.FirstOrDefault(m => m.Name == dbDeck.LearnMethod);
            var cards = repository.GetCardsByDeckId(dbDeck.Id).Select(ToCard);
            return new Deck(Guid.Parse(dbDeck.Id), new User(dbDeck.UserId), dbDeck.Name, method, cards);
        }
        
        public Card ToCard(DbCard dbCard)
        {
            var parameters = JsonConvert.DeserializeObject<IParameters>(dbCard.Parameters);
            return new Card(Guid.Parse(dbCard.Id), 
                new User(dbCard.UserId), 
                Guid.Parse(dbCard.DeckId), 
                dbCard.Front,
                dbCard.Back,
                TimeSpan.Parse(dbCard.TimeBeforeLearn),
                DateTime.Parse(dbCard.LastLearnTime, CultureInfo.InvariantCulture), parameters);
        }
    }
}