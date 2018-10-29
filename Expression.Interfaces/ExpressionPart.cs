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

        public ExpressionPart(Type type, string value, int index)
        {
            Type = type;
            Value = value;
            Index = index;
        }
    }
}
