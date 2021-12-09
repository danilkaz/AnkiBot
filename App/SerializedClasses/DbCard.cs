using System;
using System.ComponentModel.DataAnnotations;
using AnkiBot.Domain;
using Infrastructure.Attributes;

namespace App.SerializedClasses
{
    [Table("cards")]
    public class DbCard
    {
        [Field("id", isUnique: true)] public Guid Id { get; }

        [Field("front")] public string Front { get; }

        [Field("back")] public string Back { get; }

        [Field("userId")] public string UserId { get; }

        [Field("deckId")] public string DeckId { get; }

        [Field("timeBeforeLearn")] public TimeSpan TimeBeforeLearn { get; }

        [Field("lastLearnTime")] public DateTime LastLearnTime { get; }

        public DateTime NextLearnTime => LastLearnTime + TimeBeforeLearn;

        public DbCard(Card card)
        {
            Id = card.Id;
            Front = card.Front;
            Back = card.Back;
            UserId = card.UserId;
            DeckId = card.DeckId;
            TimeBeforeLearn = card.TimeBeforeLearn;
            LastLearnTime = card.LastLearnTime;
        }
    }
}