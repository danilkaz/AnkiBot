using System;
using System.Linq;
using App.SerializedClasses;
using App.UIClasses;
using Domain;
using Domain.LearnMethods;

namespace App.Converters
{
    public class DeckConverter : IConverter<DbDeck, UIDeck, Deck>
    {
        private readonly IConverter<DbCard, UICard, Card> cardConverter;
        private readonly IRepository<DbCard> cardRepository;
        private readonly ILearnMethod[] learnMethods;

        public DeckConverter(ILearnMethod[] learnMethods, IRepository<DbCard> cardRepository,
            IConverter<DbCard, UICard, Card> cardConverter)
        {
            this.learnMethods = learnMethods;
            this.cardRepository = cardRepository;
            this.cardConverter = cardConverter;
        }

        public UIDeck ToUi(DbDeck dbDeck)
        {
            return new(dbDeck.Id, dbDeck.Name, dbDeck.LearnMethod);
        }

        public Deck ToDomainClass(DbDeck dbDeck)
        {
            var method = learnMethods.FirstOrDefault(m => m.Name == dbDeck.LearnMethod);
            var cards = cardRepository.Search(c => c.DeckId == dbDeck.Id).Select(cardConverter.ToDomainClass);
            return new(Guid.Parse(dbDeck.Id), new(dbDeck.UserId), dbDeck.Name, method, cards);
        }
    }
}