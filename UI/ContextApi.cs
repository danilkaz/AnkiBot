using System.Linq;
using App;
using Domain;
using Newtonsoft.Json;
using UI.Commands;

namespace UI
{
    public class ContextApi
    {
        private readonly ContextRepository contextRepository;

        public ContextApi(ContextRepository contextRepository)
        {
            this.contextRepository = contextRepository;
        }

        public ICommandInfo Get(User user)
        {
            var dbContext = contextRepository.Search(c => c.UserId == user.Id).FirstOrDefault();
            if (dbContext is null) return null;

            return JsonConvert.DeserializeObject<ICommandInfo>(dbContext.Command, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Objects
            });
        }

        public void Update(User user, ICommandInfo commandInfo)
        {
            contextRepository.Update(new(user.Id, JsonConvert.SerializeObject(commandInfo)));
        }
    }
}