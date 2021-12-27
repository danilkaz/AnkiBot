using System.Collections.Generic;
using System.Linq;
using App.SerializedClasses;
using App.UIClasses;
using Domain;
using Domain.LearnMethods;

namespace App
{
    public class DeckApi
    {
        private readonly Converter converter;
        private readonly IRepository repository;

        public DeckApi(IRepository repository, Converter converter)
        {
            this.repository = repository;
            this.converter = converter;
        }

        public IEnumerable<UIDeck> GetDecksByUser(User user)
        {
            return repository.GetDecksByUser(user).Select(d => converter.ToUiDeck(d));
        }

        public void SaveDeck(User user, string name, ILearnMethod learnMethod, IEnumerable<Card> cards)
        {
            var deck = new Deck(user, name, learnMethod, cards);
            repository.SaveDeck(new DbDeck(deck));
        }

        public void DeleteDeck(string deckId)
        {
            repository.DeleteDeck(deckId);
        }
    }
}