using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Threading.Tasks;
using Expression.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Client;
using Variable.Interfaces;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public partial class ValuesController : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult> ExtractVariables([FromBody] string expression)
        {
            var actor = await GetExpressionActor("default");
            await actor.ExtractVariablesAsync(expression);

            return Ok();
        }

        [HttpPut]
        public async Task<ActionResult> ReplaceVariables([FromBody] PutModel model)
        {
            var parts = MemoryCache.Default.Get("result:" + model.Expression) as ExpressionPart[];
            if (parts == null)
                return NotFound();

            var actor = GetVariableActor("default");
            var values = model.Values.ToDictionary(x => x.Key, x => x.Value);
            var result = await actor.ReplaceVariablesAsync(model.Expression, values, parts);

            return Ok(result);
        }

        [HttpPost("progress")]
        public ActionResult<ProcessResult> Progress([FromBody] string expression)
        {
            var progress = MemoryCache.Default.Get("progress:" + expression) as decimal?;
            var variables = MemoryCache.Default.Get("result:" + expression) as IEnumerable<ExpressionPart>;

            if (progress == null)
                return NotFound();

            return Ok(new ProcessResult
            {
                Progress = progress.Value,
                Variables = variables?.Select(x => x.Value)
            });
        }

        private async Task<IExpression> GetExpressionActor(string actorId)
        {
            var proxy = ActorProxy.Create<IExpression>(new ActorId(actorId), new Uri("fabric:/ServiceFabricApplication/ExpressionActorService"));
            await proxy.SubscribeAsync<IExpressionEvents>(new ExpressionEventsHandler());

            return proxy;
        }

        private IVariable GetVariableActor(string actorId)
        {
            return ActorProxy.Create<IVariable>(new ActorId(actorId), new Uri("fabric:/ServiceFabricApplication/VariableActorService"));
        }
    }
}
