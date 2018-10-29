using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Runtime;
using Microsoft.ServiceFabric.Actors.Client;
using Variable.Interfaces;
using Expression.Interfaces;

namespace Variable
{
    /// <remarks>
    /// This class represents an actor.
    /// Every ActorID maps to an instance of this class.
    /// The StatePersistence attribute determines persistence and replication of actor state:
    ///  - Persisted: State is written to disk and replicated.
    ///  - Volatile: State is kept in memory only and replicated.
    ///  - None: State is kept in memory only and not replicated.
    /// </remarks>
    [StatePersistence(StatePersistence.Persisted)]
    internal class Variable : Actor, IVariable
    {
        /// <summary>
        /// Initializes a new instance of Variable
        /// </summary>
        /// <param name="actorService">The Microsoft.ServiceFabric.Actors.Runtime.ActorService that will host this actor instance.</param>
        /// <param name="actorId">The Microsoft.ServiceFabric.Actors.ActorId for this actor instance.</param>
        public Variable(ActorService actorService, ActorId actorId)
            : base(actorService, actorId)
        {
        }

        /// <summary>
        /// This method is called whenever an actor is activated.
        /// An actor is activated the first time any of its methods are invoked.
        /// </summary>
        protected override Task OnActivateAsync()
        {
            ActorEventSource.Current.ActorMessage(this, "Actor activated.");
            return Task.CompletedTask;
        }

        public Task<string> ReplaceVariablesAsync(string expression, Dictionary<string, string> values, ExpressionPart[] parts)
        {
            var expressionActor = ActorProxy.Create<IExpression>(new ActorId("default"), new Uri("fabric:/TechInterviewTask/Actor1ActorService"));

            var variables = parts.Where(x => x.Type == Expression.Interfaces.Type.Variable).ToList();

            foreach (var variable in variables.OrderByDescending(x => x.Index))
            {
                if (!values.TryGetValue(variable.Value, out var value))
                    continue;

                expression = expression.Remove(variable.Index, variable.Value.Length).Insert(variable.Index, value);
            }

            return Task.FromResult(expression);
        }
    }
}
