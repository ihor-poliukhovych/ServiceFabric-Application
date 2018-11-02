using Expression.Interfaces;
using Expression.Logic;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Expression.Tests
{
    public class ExpressionLogicTests
    {
        private ExpressionLogic _logic;

        public ExpressionLogicTests()
        {
            var events = new Mock<IExpressionEvents>();
            _logic = new ExpressionLogic(events.Object);
        }

        [Fact]
        public async Task GetExpressionPartsAsync()
        {
            //Arrage
            var expression = "(x + max(x1, 5)) / d – sqrt(z) + b * CalculateSalary(\"Ivanov\", -1+x)";

            var expected = new List<ExpressionPart>()
            {
                new ExpressionPart { Index = 1, Type = Interfaces.Type.Variable, Value = "x" },
                new ExpressionPart { Index = 5, Type = Interfaces.Type.Function, Value = "max" },
                new ExpressionPart { Index = 8, Type = Interfaces.Type.Variable, Value = "x1" },
                new ExpressionPart { Index = 12, Type = Interfaces.Type.Constant, Value = "5" },
                new ExpressionPart { Index = 19, Type = Interfaces.Type.Variable, Value = "d" },
                new ExpressionPart { Index = 23, Type = Interfaces.Type.Function, Value = "sqrt" },
                new ExpressionPart { Index = 28, Type = Interfaces.Type.Variable, Value = "z" },
                new ExpressionPart { Index = 33, Type = Interfaces.Type.Variable, Value = "b" },
                new ExpressionPart { Index = 37, Type = Interfaces.Type.Function, Value = "CalculateSalary" },
                new ExpressionPart { Index = 53, Type = Interfaces.Type.Constant, Value = "Ivanov" },
                new ExpressionPart { Index = 64, Type = Interfaces.Type.Constant, Value = "1" },
                new ExpressionPart { Index = 66, Type = Interfaces.Type.Variable, Value = "x" }
            };

            //Act
            var actual = await _logic.GetExpressionPartsAsync(expression);

            //Assert
            actual.SequenceEqual(expected);
        }
    }
}
