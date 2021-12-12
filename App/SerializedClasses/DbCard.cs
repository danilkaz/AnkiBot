using System.Globalization;
using AnkiBot.Domain;
using Infrastructure.Attributes;
using Newtonsoft.Json;

namespace App.SerializedClasses
{
    [Table("cards")]
    public class DbCard
    {
        public DbCard(Card card)
        {
            Id = card.Id.ToString();
            Front = card.Front;
            Back = card.Back;
            UserId = card.UserId;
            DeckId = card.DeckId;
            TimeBeforeLearn = card.TimeBeforeLearn.ToString();
            LastLearnTime = card.LastLearnTime.ToString(CultureInfo.InvariantCulture);
            Parameters = JsonConvert.SerializeObject(card.Parameters);
        }

        [Constructor]
        public DbCard(string id, string front, string back, string userId, string deckId, string timeBeforeLearn,
            string lastLearnTime, string parameters)
        {
            Id = id;
            Front = front;
            Back = back;
            UserId = userId;
            DeckId = deckId;
            TimeBeforeLearn = timeBeforeLearn;
            LastLearnTime = lastLearnTime;
            Parameters = parameters;
        }

        [Field("id", true)] public string Id { get; }
        [Field("front")] public string Front { get; }
        [Field("back")] public string Back { get; }
        [Field("userId")] public string UserId { get; }
        [Field("deckId")] public string DeckId { get; }
        [Field("timeBeforeLearn")] public string TimeBeforeLearn { get; }
        [Field("lastLearnTime")] public string LastLearnTime { get; }

        [Field("parameters")] public string Parameters { get; }
    }
}