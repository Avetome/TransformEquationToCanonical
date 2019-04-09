using EquationTransformer;
using EquationTransformerTests.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace EquationTransformerTests
{
    public class Parser2Tests
    {
        [Theory(DisplayName = "Can parse one summand with numbers, variables and unary operator")]
        [MemberData(nameof(GetNumbers))]
        [MemberData(nameof(GetVariable))]
        [MemberData(nameof(GetVariableWithUnaryOperator))]
        [MemberData(nameof(GetSimpleSummandsWithMultiplier))]
        [MemberData(nameof(GetSimpleSummandsWithStarMultiplierSign))]
        [MemberData(nameof(GetSimpleSummandsWithPower))]
        [MemberData(nameof(GetSimpleSummandsWithNegativePower))]
        public void ParseSimpleOperands(string equations, Summand answer)
        {
            var parser = new Parser2(equations);

            var summand = parser.GetSummand().First();

            CompareSummand(answer, summand);
        }

        [Theory(DisplayName = "Can parse equation with several summands")]
        [MemberData(nameof(GetSimpleEquation))]
        [MemberData(nameof(GetEquationWithBrackets))]
        [MemberData(nameof(GetEquationWithBracketsWithMultiplier))]
        [MemberData(nameof(GetEquationWithUnaryOperator))]
        [MemberData(nameof(GetComplexEquation))]
        public void ParseTestSimpleEquation(string equations, List<Summand> answer)
        {
            var parser = new Parser2(equations);

            var summands = parser.GetSummand().ToList();

            Assert.Equal(answer.Count(), summands.Count());

            for (var i = 0; i < answer.Count(); i++)
            {
                CompareSummand(answer[i], summands[i]);
            }
        }

        [Theory(DisplayName = "Can't parse equation with abcent operands or unclosed brackets")]
        [MemberData(nameof(GetEquationsWithInvalidBracketsCount))]
        [MemberData(nameof(GetEquationsWithInvalidOperands))]
        public void ErrorParsing(string equations)
        {
            var parser = new Parser2(equations);

            Exception ex = Assert.Throws<Exception>(() => parser.GetSummand().First());
        }

        public static IEnumerable<object[]> GetEquationsWithInvalidBracketsCount()
        {
            yield return new object[] { "y2(" };
            yield return new object[] { "y2(x + 5(y + )" };
        }

        public static IEnumerable<object[]> GetEquationsWithInvalidOperands()
        {
            yield return new object[] { "y2 *" };
            yield return new object[] { "y2 +" };
            yield return new object[] { "y2* + y" };
        }

        public static IEnumerable<object[]> GetNumbers()
        {
            yield return new object[] { "5", SummandsHelper.CreateSummand(5) };
            yield return new object[] { "-10", SummandsHelper.CreateSummand(-10) };
        }

        public static IEnumerable<object[]> GetVariable()
        {
            yield return new object[] { "y", SummandsHelper.CreateSummand(('y', 1)) };
            yield return new object[] { "x", SummandsHelper.CreateSummand(('x', 1)) };
            yield return new object[] { "yx", SummandsHelper.CreateSummand(('y', 1), ('x', 1)) };
        }
        public static IEnumerable<object[]> GetVariableWithUnaryOperator()
        {
            yield return new object[] { "-y", SummandsHelper.CreateSummand(-1, ('y', 1)) };
            yield return new object[] { "-yx", SummandsHelper.CreateSummand(-1, ('y', 1), ('x', 1)) };
        }

        public static IEnumerable<object[]> GetSimpleSummandsWithMultiplier()
        {
            yield return new object[] { "2y", SummandsHelper.CreateSummand(2, ('y', 1)) };
            yield return new object[] { "-5x", SummandsHelper.CreateSummand(-5, ('x', 1)) };
            yield return new object[] { "3x5y", SummandsHelper.CreateSummand(15, ('x', 1), ('y', 1)) };
            yield return new object[] { "x2X", SummandsHelper.CreateSummand(2, ('x', 2)) };
        }

        public static IEnumerable<object[]> GetSimpleSummandsWithStarMultiplierSign()
        {
            yield return new object[] { "y2 * 10", SummandsHelper.CreateSummand(20, ('y', 1)) };
            yield return new object[] { "5*2x", SummandsHelper.CreateSummand(10, ('x', 1)) };
            yield return new object[] { "x^2 * y^3", SummandsHelper.CreateSummand(('x', 2), ('y', 3)) };
        }

        public static IEnumerable<object[]> GetSimpleSummandsWithPower()
        {
            yield return new object[] { "y^2", SummandsHelper.CreateSummand(('y', 2)) };
            yield return new object[] { "yx^5", SummandsHelper.CreateSummand(('x', 5), ('y', 1)) };
            yield return new object[] { "y^7x^5", SummandsHelper.CreateSummand(('x', 5), ('y', 7)) };

            yield return new object[] { "y^-2", SummandsHelper.CreateSummand(('y', -2)) };
        }

        public static IEnumerable<object[]> GetSimpleSummandsWithNegativePower()
        {
            yield return new object[] { "y^-2", SummandsHelper.CreateSummand(('y', -2)) };
            yield return new object[] { "5y^-2", SummandsHelper.CreateSummand(5, ('y', -2)) };
            yield return new object[] { "y^-2x", SummandsHelper.CreateSummand(('y', -2), ('x', 1)) };
            yield return new object[] { "y^-2 * 10", SummandsHelper.CreateSummand(10, ('y', -2)) };
        }

        public static IEnumerable<object[]> GetSimpleEquation()
        {
            var summands = new List<Summand>();
            summands.Add(SummandsHelper.CreateSummand(('x', 2)));
            summands.Add(SummandsHelper.CreateSummand(2, ('x', 1)));
            yield return new object[] { "x^2 + 2x", summands };

            summands = new List<Summand>();
            summands.Add(SummandsHelper.CreateSummand(('x', 2)));
            summands.Add(SummandsHelper.CreateSummand(-2, ('x', 1)));
            yield return new object[] { "x^2 - 2x", summands };

            summands = new List<Summand>();
            summands.Add(SummandsHelper.CreateSummand(('x', -2)));
            summands.Add(SummandsHelper.CreateSummand(-2, ('x', 1)));
            yield return new object[] { "x^-2 - 2x", summands };

            summands = new List<Summand>();
            summands.Add(SummandsHelper.CreateSummand(2, ('x', 1)));
            summands.Add(SummandsHelper.CreateSummand(-3, ('y', 1)));
            yield return new object[] { "x2 - 3y", summands };

            summands = new List<Summand>();
            summands.Add(SummandsHelper.CreateSummand(2, ('x', 1)));
            summands.Add(SummandsHelper.CreateSummand(-3, ('x', 1)));
            yield return new object[] { "x2 - 3X", summands };

            summands = new List<Summand>();
            summands.Add(SummandsHelper.CreateSummand(2, ('x', 1)));
            summands.Add(SummandsHelper.CreateSummand(4));
            summands.Add(SummandsHelper.CreateSummand(-3, ('y', 1)));
            yield return new object[] { "x2 + 4 - 3y", summands };

            summands = new List<Summand>();
            summands.Add(SummandsHelper.CreateSummand(('y', 2)));
            summands.Add(SummandsHelper.CreateSummand(-1, ('x', 1), ('y', 1)));
            summands.Add(SummandsHelper.CreateSummand(('y', 1)));
            yield return new object[] { "y ^ 2 - xy + y", summands };

            summands = new List<Summand>();
            summands.Add(SummandsHelper.CreateSummand(('y', -2)));
            summands.Add(SummandsHelper.CreateSummand(('x', 1), ('y', 1)));
            summands.Add(SummandsHelper.CreateSummand(('y', 1)));
            yield return new object[] { "y ^ - 2 + xy + y", summands };
        }

        public static IEnumerable<object[]> GetEquationWithBrackets()
        {
            var summands = new List<Summand>();
            summands.Add(SummandsHelper.CreateSummand(2, ('y', 1)));
            summands.Add(SummandsHelper.CreateSummand(5));
            summands.Add(SummandsHelper.CreateSummand(('x', 1)));
            yield return new object[] { "y2 + (5 + x)", summands };

            summands = new List<Summand>();
            summands.Add(SummandsHelper.CreateSummand(2, ('y', 1)));
            summands.Add(SummandsHelper.CreateSummand(-5));
            summands.Add(SummandsHelper.CreateSummand(-1, ('x', 1)));
            yield return new object[] { "y2 - (5 + x)", summands };

            summands = new List<Summand>();
            summands.Add(SummandsHelper.CreateSummand(2, ('y', 1)));
            summands.Add(SummandsHelper.CreateSummand(-5));
            summands.Add(SummandsHelper.CreateSummand(('x', 1)));
            yield return new object[] { "y2 - (5 - x)", summands };

            summands = new List<Summand>();
            summands.Add(SummandsHelper.CreateSummand(('x', 1)));
            summands.Add(SummandsHelper.CreateSummand(-1, ('y', 2)));
            summands.Add(SummandsHelper.CreateSummand(('x', 1)));
            yield return new object[] { "x - (y ^ 2 - x)", summands };
        }

        public static IEnumerable<object[]> GetEquationWithBracketsWithMultiplier()
        {
            var summands = new List<Summand>();
            summands.Add(SummandsHelper.CreateSummand(2, ('y', 1)));
            summands.Add(SummandsHelper.CreateSummand(('x', 2)));
            summands.Add(SummandsHelper.CreateSummand(2, ('x', 1)));
            yield return new object[] { "y2 + x(x + 2)", summands };

            summands = new List<Summand>();
            summands.Add(SummandsHelper.CreateSummand(2, ('y', 1)));
            summands.Add(SummandsHelper.CreateSummand(('x', 2), ('y', 1)));
            summands.Add(SummandsHelper.CreateSummand(2, ('x', 1), ('y', 1)));
            yield return new object[] { "y2 + x(x + 2)y", summands };
        }

        public static IEnumerable<object[]> GetEquationWithUnaryOperator()
        {
            var summands = new List<Summand>();
            summands.Add(SummandsHelper.CreateSummand(2, ('y', 1)));
            summands.Add(SummandsHelper.CreateSummand(-1, ('x', 1)));
            yield return new object[] { "2y + -x", summands };

            summands = new List<Summand>();
            summands.Add(SummandsHelper.CreateSummand(2, ('y', 1)));
            summands.Add(SummandsHelper.CreateSummand(-1, ('x', 1)));
            yield return new object[] { "2y + -+-+-x", summands };

            summands = new List<Summand>();
            summands.Add(SummandsHelper.CreateSummand(2, ('y', 1)));
            summands.Add(SummandsHelper.CreateSummand(5));
            summands.Add(SummandsHelper.CreateSummand(-1, ('x', 1)));
            yield return new object[] { "y2 - -(5 - x)", summands };
        }

        public static IEnumerable<object[]> GetComplexEquation()
        {
            var summands = new List<Summand>();
            summands.Add(SummandsHelper.CreateSummand(2, ('x', 4)));
            summands.Add(SummandsHelper.CreateSummand(3, ('x', 1), ('y', 1)));
            summands.Add(SummandsHelper.CreateSummand(-6, ('x', 2), ('y', 1)));
            summands.Add(SummandsHelper.CreateSummand(5, ('x', 1), ('y', 2)));
            summands.Add(SummandsHelper.CreateSummand(-3, ('x', 1), ('y', 1)));
            summands.Add(SummandsHelper.CreateSummand(15, ('y', 2)));

            yield return new object[] { "2x^4 + 3xy - (6x^2y + -5xy^2 + 3y(x - 5y))", summands };
        }

        private static void CompareSummand(Summand a, Summand b)
        {
            Assert.Equal(a.Multiplier, b.Multiplier);
            Assert.Equal(a.Variables.Count, b.Variables.Count);

            foreach (KeyValuePair<char, int> item in a.Variables)
            {
                Assert.True(b.Variables.ContainsKey(item.Key));
                Assert.Equal(a.Variables[item.Key], b.Variables[item.Key]);
            }
        }
    }
}
