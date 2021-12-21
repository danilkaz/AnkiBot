using AnkiBot.Domain.Parameters;

namespace AnkiBot.Domain.Visitors
{
    public interface IVisitor
    {
        void VisitEmptyParameters(EmptyParameters parameters);
        void VisitSuperMemo2Parameters(SuperMemo2Parameters parameters);
    }
}