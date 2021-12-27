using System;
using System.Collections.Generic;
using System.Linq;
using App.SerializedClasses;
using App.UIClasses;
using Domain;

namespace App
{
    public class CardApi
    {
        private readonly Converter converter;
        private readonly IRepository repository;

        public CardApi(IRepository repository, Converter converter)
        {
            this.repository = repository;
            this.converter = converter;
        }

        public void SaveCard(User user, string deckId, string front, string back)
        {
            var deck = converter.ToDeck(repository.GetDeck(deckId));
            var card = new Card(user, Guid.Parse(deckId), front, back,
                deck.LearnMethod.GetParameters());
            repository.SaveCard(new DbCard(card));
        }

        public void LearnCard(string cardId, int answer)
        {
            var card = converter.ToCard(repository.GetCard(cardId));
            card.Parameters.LearnCard(card, answer);
            card.LastLearnTime = DateTime.Now;
            repository.UpdateCard(new DbCard(card));
        }

        public IEnumerable<UICard> GetCardsToLearn(string deckId)
        {
            var deck = converter.ToDeck(repository.GetDeck(deckId));
            return deck.GetCardsToLearn()
                .Select(c => new UICard(c.Id.ToString(), c.Front, c.Back, c.DeckId.ToString()));
        }

        public IEnumerable<UICard> GetCardsByDeckId(string deckId)
        {
            var deck = converter.ToDeck(repository.GetDeck(deckId));
            return deck.Cards.Select(c => new UICard(c.Id.ToString(), c.Front, c.Back, c.DeckId.ToString()));
        }

        public void DeleteCard(string cardId)
        {
            repository.DeleteCard(cardId);
        }
    }
}