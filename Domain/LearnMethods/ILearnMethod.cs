using Domain.Parameters;

namespace Domain.LearnMethods
{
    public interface ILearnMethod
    {
        string Name { get; }

        string Description { get; }

        IParameters GetParameters();
    }
}