namespace Expression.Interfaces
{
    public class ExpressionPart
    {
        public Type Type { get; set; }
        public string Value { get; set; }
        public int Index { get; set; }

        public ExpressionPart()
        {

        }

        public override string ToString()
        {
            return "new ExpressionPart { Index = "+Index+", Type = Interfaces.Type."+Type+", Value = \""+Value+"\" }";
        }

        public ExpressionPart(Type type, string value, int index)
        {
            Type = type;
            Value = value;
            Index = index;
        }
    }
}
