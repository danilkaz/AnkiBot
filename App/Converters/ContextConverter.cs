using App.SerializedClasses;
using Domain;
using Newtonsoft.Json;

namespace App.Converters
{
    public class ContextConverter
    {
        public Context ToDomainClass(DbContext dbContext)
        {
            return JsonConvert.DeserializeObject<Context>(dbContext.Context);
        }

        public DbContext ToDbContext(User user, Context context)
        {
            return new(user.Id, JsonConvert.SerializeObject(context));
        }
    }
}