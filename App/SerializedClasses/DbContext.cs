using Infrastructure.Attributes;

namespace App.SerializedClasses
{
    [Table("contexts")]
    public class DbContext
    {
        [Constructor]
        public DbContext(string userId, string context)
        {
            UserId = userId;
            Context = context;
        }

        [Field("id", true)] public string UserId { get; }
        [Field("context")] public string Context { get; }
    }
}