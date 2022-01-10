using System;
using System.Globalization;
using App.SerializedClasses;
using App.UIClasses;
using Domain;
using Domain.Parameters;
using Newtonsoft.Json;

namespace App.Converters
{
    public class CardConverter : IConverter<DbCard, UICard, Card>
    {
        public UICard ToUi(DbCard card)
        {
            return new(card.Id, card.Front, card.Back, card.DeckId);
        }

        public Card ToDomainClass(DbCard dbCard)
        {
            var parameters = JsonConvert.DeserializeObject<IParameters>(dbCard.Parameters);
            return new(Guid.Parse(dbCard.Id),
                new(dbCard.UserId),
                Guid.Parse(dbCard.DeckId),
                dbCard.Front,
                dbCard.Back,
                TimeSpan.Parse(dbCard.TimeBeforeLearn),
                DateTime.Parse(dbCard.LastLearnTime, CultureInfo.InvariantCulture), parameters);
        }
    }
}