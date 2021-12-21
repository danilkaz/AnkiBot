using Domain.Parameters;

namespace Domain.Visitors
{
    public interface IVisitor
    {
        void VisitEmptyParameters(EmptyParameters parameters);
        void VisitSuperMemo2Parameters(SuperMemo2Parameters parameters);
    }
}