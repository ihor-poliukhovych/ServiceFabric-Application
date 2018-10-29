using Microsoft.ServiceFabric.Actors;

namespace Expression.Interfaces
{
    public interface IExpressionEvents : IActorEvents
    {
        void ProgressUpdated(string expression, decimal progress);
        void ProcessCompleted(string expression, ExpressionPart[] variables);
    }
}