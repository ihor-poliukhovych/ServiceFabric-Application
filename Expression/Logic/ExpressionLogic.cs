using Expression.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Expression.Logic
{
    public class ExpressionLogic
    {
        private IExpressionEvents _expressionEvents;

        public ExpressionLogic(IExpressionEvents expressionEvents)
        {
            this._expressionEvents = expressionEvents;
        }

        public async Task<IEnumerable<ExpressionPart>> GetExpressionPartsAsync(string expression, int startIdex = 0)
        {
            var builder = new ExpressionPartBuilder();
            var result = new List<ExpressionPart>();

            if (startIdex == 0)
                SendProgress(0, expression);

            for (var i = 0; i < expression.Length; i++)
            {
                var ch = expression[i];

                if (ch == '(')
                {
                    var endIndex = expression.IndexOf(')', i);
                    var innerParts = await GetExpressionPartsAsync(expression.Substring(i + 1, endIndex - i), i + 1);

                    if (builder.HasValue())
                    {
                        result.Add(builder.BuildFunction(startIdex + i));
                    }

                    result.AddRange(innerParts);
                    i = ++endIndex;
                }
                else if (ch == '"' || ch == '.' || ch == '_' || char.IsNumber(ch) || char.IsLetter(ch))
                {
                    builder.Append(ch);
                }
                else if (builder.HasValue())
                {
                    result.Add(builder.Build(startIdex + i));
                }

                await Task.Delay(500);

                if (startIdex == 0)
                    SendProgress(i, expression);
            }

            if (builder.HasValue())
            {
                result.Add(builder.Build(startIdex + expression.Length));
            }

            SendProgress(startIdex + expression.Length, expression);
            return result;
        }

        private void SendProgress(int index, string expression)
        {
            this._expressionEvents.ProgressUpdated(expression, 100m * index / expression.Length);
        }
    }
}
