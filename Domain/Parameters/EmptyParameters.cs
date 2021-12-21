using AnkiBot.Domain.Visitors;

namespace AnkiBot.Domain.Parameters
{
    public class EmptyParameters : IParameters
    {
        public void Accept(IVisitor visitor)
        {
            visitor.VisitEmptyParameters(this);
        }
    }
}