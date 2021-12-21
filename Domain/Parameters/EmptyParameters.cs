using Domain.Visitors;

namespace Domain.Parameters
{
    public class EmptyParameters : IParameters
    {
        public void Accept(IVisitor visitor)
        {
            visitor.VisitEmptyParameters(this);
        }
    }
}