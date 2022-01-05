using System.Collections.Generic;
using System.Linq;
using App.Converters;
using App.SerializedClasses;
using App.UIClasses;
using Domain;
using Domain.LearnMethods;

namespace App.APIs
{
    public class DeckApi
    {
        private readonly IConverter<DbDeck, UIDeck, Deck> deckConverter;
        private readonly IRepository<DbDeck> deckRepository;

        public DeckApi(IRepository<DbDeck> deckRepository, IConverter<DbDeck, UIDeck, Deck> deckConverter)
        {
            this.deckRepository = deckRepository;
            this.deckConverter = deckConverter;
        }

        public IEnumerable<UIDeck> GetDecksByUser(User user)
        {
            return deckRepository.Search(d => d.UserId == user.Id).Select(d => deckConverter.ToUi(d));
        }

        public void SaveDeck(User user, string name, ILearnMethod learnMethod, IEnumerable<Card> cards)
        {
            var deck = new Deck(user, name, learnMethod, cards);
            deckRepository.Save(new DbDeck(deck));
        }

        public void DeleteDeck(string deckId)
        {
            deckRepository.Delete(deckId);
        }
    }
}