using Expression.Interfaces;
using System.Text;

namespace Expression
{
    public class ExpressionPartBuilder
    {
        private readonly StringBuilder _builder;

        public ExpressionPartBuilder()
        {
            _builder = new StringBuilder();
        }

        public void Append(char ch)
        {
            _builder.Append(ch);
        }

        public bool HasValue()
        {
            return _builder.Length > 0;
        }

        public ExpressionPart Build(int index)
        {
            var value = _builder.ToString();
            _builder.Clear();

            var type = value[0] == '"' || char.IsNumber(value[0]) ? Type.Constant : Type.Variable;
            return new ExpressionPart(type, value, index - value.Length);
        }

        public ExpressionPart BuildFunction(int index)
        {
            var value = _builder.ToString();
            _builder.Clear();

            return new ExpressionPart(Type.Function, value, index - value.Length);
        }
    }
}
