using Infrastructure.Attributes;

namespace App.SerializedClasses
{
    [Table("contexts")]
    public class DbContext
    {
        [Constructor]
        public DbContext(string userId, string command)
        {
            UserId = userId;
            Command = command;

        }

        [Field("id", true)] public string UserId { get; }
        [Field("command")] public string Command { get; }
    }
}