using System.Linq;
using App.Converters;
using App.SerializedClasses;
using Domain;

namespace App.APIs
{
    public class ContextApi
    {
        private readonly ContextConverter contextConverter;
        private readonly IRepository<DbContext> stateRepository;

        public ContextApi(IRepository<DbContext> stateRepository, ContextConverter contextConverter)
        {
            this.stateRepository = stateRepository;
            this.contextConverter = contextConverter;
        }

        public Context Get(User user)
        {
            var dbContext = stateRepository.Search(s => s.UserId == user.Id).FirstOrDefault();
            return dbContext is null ? null : contextConverter.ToDomainClass(dbContext);
        }

        public void Update(User user, Context context)
        {
            var dbContext = contextConverter.ToDbContext(user, context);
            stateRepository.Update(dbContext);
        }
    }
}