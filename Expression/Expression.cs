using System.Linq;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Runtime;
using Expression.Interfaces;
using System;
using Expression.Logic;

namespace Expression
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
    internal class Expression : Actor, IExpression, IRemindable
    {
        /// <summary>
        /// Initializes a new instance of Expression
        /// </summary>
        /// <param name="actorService">The Microsoft.ServiceFabric.Actors.Runtime.ActorService that will host this actor instance.</param>
        /// <param name="actorId">The Microsoft.ServiceFabric.Actors.ActorId for this actor instance.</param>
        public Expression(ActorService actorService, ActorId actorId)
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

        public async Task ExtractVariablesAsync(string expression)
        {
            await RegisterReminderAsync(expression, null, TimeSpan.Zero, TimeSpan.FromMilliseconds(-1));
        }

        public async Task ReceiveReminderAsync(string expression, byte[] state, TimeSpan dueTime, TimeSpan period)
        {
            if (state == null)
                throw new ArgumentNullException(nameof(state));

            var logic = new ExpressionLogic(GetEvent<IExpressionEvents>());
            var parts = await logic.GetExpressionPartsAsync(expression);
            var variables = parts.Where(x => x.Type == Interfaces.Type.Variable).ToArray();
            this.GetEvent<IExpressionEvents>().ProcessCompleted(expression, variables);
        }
    }
}
