using System;
using System.Collections.Generic;
using System.Linq;
using App.Converters;
using App.SerializedClasses;
using App.UIClasses;
using Domain;

namespace App
{
    public class CardApi
    {
        private readonly IConverter<DbCard, UICard, Card> cardConverter;
        private readonly IRepository<DbCard> cardRepository;
        private readonly IConverter<DbDeck, UIDeck, Deck> deckConverter;
        private readonly IRepository<DbDeck> deckRepository;

        public CardApi(IRepository<DbCard> cardRepository, IRepository<DbDeck> deckRepository,
            IConverter<DbCard, UICard, Card> cardConverter, IConverter<DbDeck, UIDeck, Deck> deckConverter)
        {
            this.cardRepository = cardRepository;
            this.deckRepository = deckRepository;
            this.cardConverter = cardConverter;
            this.deckConverter = deckConverter;
        }

        public void SaveCard(User user, string deckId, string front, string back)
        {
            var deck = deckConverter.ToDomainClass(deckRepository.Get(deckId));
            var card = new Card(user, Guid.Parse(deckId), front, back,
                deck.LearnMethod.GetParameters());
            cardRepository.Save(new DbCard(card));
        }

        public void LearnCard(string cardId, int answer)
        {
            var card = cardConverter.ToDomainClass(cardRepository.Get(cardId));
            card.Parameters.LearnCard(card, answer);
            card.LastLearnTime = DateTime.Now;
            cardRepository.Update(new DbCard(card));
        }

        public IEnumerable<UICard> GetCardsToLearn(string deckId)
        {
            var deck = deckConverter.ToDomainClass(deckRepository.Get(deckId));
            return deck.GetCardsToLearn()
                .Select(c => new UICard(c.Id.ToString(), c.Front, c.Back, c.DeckId.ToString()));
        }

        public IEnumerable<UICard> GetCardsByDeckId(string deckId)
        {
            var deck = deckConverter.ToDomainClass(deckRepository.Get(deckId));
            return deck.Cards.Select(c => new UICard(c.Id.ToString(), c.Front, c.Back, c.DeckId.ToString()));
        }

        public void DeleteCard(string cardId)
        {
            cardRepository.Delete(cardId);
        }
    }
}