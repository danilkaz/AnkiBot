using System;
using System.Linq;
using App;
using App.SerializedClasses;
using Domain;
using Newtonsoft.Json;
using UI.Commands;
using UI.Data;

namespace UI
{
    public class ContextApi
    {
        private readonly ContextRepository contextRepository;

        public ContextApi(ContextRepository contextRepository)
        {
            this.contextRepository = contextRepository;
        }

        public INext Get(User user)
        {
            var dbContext = contextRepository.Search(c => c.UserId == user.Id).FirstOrDefault();
            if (dbContext is null)
            {
                return null;
            }

            return JsonConvert.DeserializeObject<INext>(dbContext.Command, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Objects
            });
        }

        public void Update(User user, INext next)
        {
            contextRepository.Update(new(user.Id, JsonConvert.SerializeObject(next)));
        }
    }
}