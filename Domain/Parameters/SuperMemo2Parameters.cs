using Domain.Visitors;

namespace Domain.Parameters
{
    public record SuperMemo2Parameters : IParameters
    {
        public SuperMemo2Parameters(double ef = 2.5)
        {
            EF = ef;
        }

        public double EF { get; set; }

        public void Accept(IVisitor visitor)
        {
            visitor.VisitSuperMemo2Parameters(this);
        }
    }
}