using Domain.Parameters;

namespace Domain.LearnMethods
{
    public class SuperMemo2LearnMethod : ILearnMethod
    {
        public string Name => "Алгоритм SuperMemo 2";

        public string Description => "Алгоритм SuperMemo 2\n" +
                                     "Cледующий интервал вычисляется на основе предыдущего интервала и ответа пользователя";

        public IParameters GetParameters()
        {
            return new SuperMemo2Parameters();
        }
    }
}