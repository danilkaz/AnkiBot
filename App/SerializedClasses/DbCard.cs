using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using AnkiBot.Domain;
using Infrastructure.Attributes;

namespace App.SerializedClasses
{
    [Table("cards")]
    public class DbCard
    {
        [Field("id", isUnique: true)] public string Id { get; }
        [Field("front")] public string Front { get; }
        [Field("back")] public string Back { get; }
        [Field("userId")] public string UserId { get; }
        [Field("deckId")] public string DeckId { get; }
        [Field("timeBeforeLearn")] public string TimeBeforeLearn { get; }
        [Field("lastLearnTime")] public string LastLearnTime { get; }
        
        public DbCard(Card card)
        {
            Id = card.Id.ToString();
            Front = card.Front;
            Back = card.Back;
            UserId = card.UserId;
            DeckId = card.DeckId;
            TimeBeforeLearn = card.TimeBeforeLearn.ToString();
            LastLearnTime = card.LastLearnTime.ToString(CultureInfo.InvariantCulture);
        }

        [Constructor]
        public DbCard(string id, string front, string back, string userId, string deckId, string timeBeforeLearn,
            string lastLearnTime)
        {
            Id = id;
            Front = front;
            Back = back;
            UserId = userId;
            DeckId = deckId;
            TimeBeforeLearn = timeBeforeLearn;
            LastLearnTime = lastLearnTime;
        }
    }
}