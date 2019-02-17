using EquationTransformer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace EquationTransformerTests
{
    public class ParserTests
    {
        [Fact]
        public void Test1()
        {
            var str = "3x5y";

            var parser = new Parser(str);

            var summand = parser.GetSummand().First();

            Assert.Equal(15, summand.Multiplier);
            Assert.Equal(2, summand.Variables.Count);
            Assert.True(summand.Variables.ContainsKey('x'));
            Assert.True(summand.Variables.ContainsKey('y'));
        }

        [Fact]
        public void Test2()
        {
            var str = "x^2";

            var parser = new Parser(str);

            var summand = parser.GetSummand().First();

            Assert.Equal(1, summand.Multiplier);
            Assert.Single(summand.Variables);
            Assert.True(summand.Variables.ContainsKey('x'));
            Assert.Equal(2, summand.Variables['x']);
        }

        [Fact]
        public void Test3()
        {
            var str = "yx^2";

            var parser = new Parser(str);

            var summand = parser.GetSummand().First();

            Assert.Equal(1, summand.Multiplier);
            Assert.Equal(2, summand.Variables.Count);
            Assert.True(summand.Variables.ContainsKey('x'));
            Assert.Equal(2, summand.Variables['x']);
            Assert.True(summand.Variables.ContainsKey('y'));
            Assert.Equal(1, summand.Variables['y']);
        }

        [Fact]
        public void Testx()
        {
            var str = "y* y * x^2";

            var parser = new Parser(str);

            var summand = parser.GetSummand().First();

            Assert.Equal(1, summand.Multiplier);
            Assert.Equal(2, summand.Variables.Count);
            Assert.True(summand.Variables.ContainsKey('x'));
            Assert.Equal(2, summand.Variables['x']);
            Assert.True(summand.Variables.ContainsKey('y'));
            Assert.Equal(2, summand.Variables['y']);
        }

        [Fact]
        public void Testx2()
        {
            var str = "yy ^2 * x^2";

            var parser = new Parser(str);

            var summand = parser.GetSummand().First();

            Assert.Equal(1, summand.Multiplier);
            Assert.Equal(2, summand.Variables.Count);
            Assert.True(summand.Variables.ContainsKey('x'));
            Assert.Equal(2, summand.Variables['x']);
            Assert.True(summand.Variables.ContainsKey('y'));
            Assert.Equal(3, summand.Variables['y']);
        }

        [Fact]
        public void Test5()
        {
            var str = "y2";

            var parser = new Parser(str);

            var summand = parser.GetSummand().First();

            Assert.Equal(2, summand.Multiplier);
            Assert.Single(summand.Variables);
            Assert.True(summand.Variables.ContainsKey('y'));
            Assert.Equal(1, summand.Variables['y']);
        }

        [Fact]
        public void Test55()
        {
            var str = "y2 * 10";

            var parser = new Parser(str);

            var summand = parser.GetSummand().First();

            Assert.Equal(20, summand.Multiplier);
            Assert.Single(summand.Variables);
            Assert.True(summand.Variables.ContainsKey('y'));
            Assert.Equal(1, summand.Variables['y']);
        }

        [Fact]
        public void Test6()
        {
            var str = "y2 +";

            var parser = new Parser(str);

            var summand = parser.GetSummand().First();

            Assert.Equal(2, summand.Multiplier);
            Assert.Single(summand.Variables);
            Assert.True(summand.Variables.ContainsKey('y'));
            Assert.Equal(1, summand.Variables['y']);
        }

        [Fact]
        public void Test64()
        {
            var str = "2y + -x";

            var parser = new Parser(str);

            var summands = parser.GetSummand();

            var summand1 = summands.ElementAt(0);
            var summand2 = summands.ElementAt(1);

            Assert.Equal(2, summand1.Multiplier);
            Assert.Single(summand1.Variables);
            Assert.True(summand1.Variables.ContainsKey('y'));
            Assert.Equal(1, summand1.Variables['y']);

            Assert.Equal(-1, summand2.Multiplier);
            Assert.Single(summand2.Variables);
            Assert.True(summand2.Variables.ContainsKey('x'));
            Assert.Equal(1, summand2.Variables['x']);
        }

        [Fact]
        public void Test7()
        {
            var str = "y2(";

            var parser = new Parser(str);

            Exception ex = Assert.Throws<Exception>(() => parser.GetSummand().First());

            Assert.Equal("Invalid count of parens", ex.Message);
        }

        [Fact]
        public void Test77()
        {
            var str = "y2 + (5 + x)";

            var parser = new Parser(str);

            var summands = parser.GetSummand();
            Assert.Equal(3, summands.Count());

            var summand1 = summands.First();
            var summand2 = summands.ElementAt(1);
            var summand3 = summands.Last();

            Assert.Equal(2, summand1.Multiplier);
            Assert.Single(summand1.Variables);
            Assert.True(summand1.Variables.ContainsKey('y'));
            Assert.Equal(1, summand1.Variables['y']);

            Assert.Equal(5, summand2.Multiplier);
            Assert.Empty(summand2.Variables);

            Assert.Equal(1, summand3.Multiplier);
            Assert.Single(summand3.Variables);
            Assert.True(summand3.Variables.ContainsKey('x'));
            Assert.Equal(1, summand3.Variables['x']);
        }

        [Fact]
        public void Test777()
        {
            var str = "y2 - (5 + x)";

            var parser = new Parser(str);

            var summands = parser.GetSummand();
            Assert.Equal(3, summands.Count());

            var summand1 = summands.First();
            var summand2 = summands.ElementAt(1);
            var summand3 = summands.Last();

            Assert.Equal(2, summand1.Multiplier);
            Assert.Single(summand1.Variables);
            Assert.True(summand1.Variables.ContainsKey('y'));
            Assert.Equal(1, summand1.Variables['y']);

            Assert.Equal(-5, summand2.Multiplier);
            Assert.Empty(summand2.Variables);

            Assert.Equal(-1, summand3.Multiplier);
            Assert.Single(summand3.Variables);
            Assert.True(summand3.Variables.ContainsKey('x'));
            Assert.Equal(1, summand3.Variables['x']);
        }

        [Fact]
        public void Test7773()
        {
            var str = "x - (y^2 - x)";

            var parser = new Parser(str);

            var summands = parser.GetSummand();
            Assert.Equal(3, summands.Count());

            var summand1 = summands.First();
            var summand2 = summands.ElementAt(1);
            var summand3 = summands.Last();

            Assert.Equal(1, summand1.Multiplier);
            Assert.Single(summand1.Variables);
            Assert.True(summand1.Variables.ContainsKey('x'));
            Assert.Equal(1, summand1.Variables['x']);

            Assert.Equal(-1, summand2.Multiplier);
            Assert.Single(summand2.Variables);
            Assert.True(summand2.Variables.ContainsKey('y'));
            Assert.Equal(2, summand2.Variables['y']);

            Assert.Equal(1, summand3.Multiplier);
            Assert.Single(summand3.Variables);
            Assert.True(summand3.Variables.ContainsKey('x'));
            Assert.Equal(1, summand3.Variables['x']);
        }

        [Fact]
        public void Test7774()
        {
            var str = "y2 - (5 - x)";

            var parser = new Parser(str);

            var summands = parser.GetSummand();
            Assert.Equal(3, summands.Count());

            var summand1 = summands.First();
            var summand2 = summands.ElementAt(1);
            var summand3 = summands.Last();

            Assert.Equal(2, summand1.Multiplier);
            Assert.Single(summand1.Variables);
            Assert.True(summand1.Variables.ContainsKey('y'));
            Assert.Equal(1, summand1.Variables['y']);

            Assert.Equal(-5, summand2.Multiplier);
            Assert.Empty(summand2.Variables);

            Assert.Equal(1, summand3.Multiplier);
            Assert.Single(summand3.Variables);
            Assert.True(summand3.Variables.ContainsKey('x'));
            Assert.Equal(1, summand3.Variables['x']);
        }

        [Fact]
        public void Test7775()
        {
            var str = "y2 + x * (x + 2)";

            var parser = new Parser(str);

            var summands = parser.GetSummand();

            Assert.Equal(3, summands.Count());

            var summand1 = summands.First();
            var summand2 = summands.ElementAt(1);
            var summand3 = summands.Last();

            Assert.Equal(2, summand1.Multiplier);
            Assert.Single(summand1.Variables);
            Assert.True(summand1.Variables.ContainsKey('y'));
            Assert.Equal(1, summand1.Variables['y']);

            Assert.Equal(1, summand2.Multiplier);
            Assert.Single(summand2.Variables);
            Assert.True(summand2.Variables.ContainsKey('x'));
            Assert.Equal(2, summand2.Variables['x']);

            Assert.Equal(2, summand3.Multiplier);
            Assert.Single(summand3.Variables);
            Assert.True(summand3.Variables.ContainsKey('x'));
            Assert.Equal(1, summand3.Variables['x']);
        }

        [Fact]
        public void Test7776s()
        {
            var str = "y2 - -(5 - x)";

            var parser = new Parser(str);

            var summands = parser.GetSummand();
            Assert.Equal(3, summands.Count());

            var summand1 = summands.First();
            var summand2 = summands.ElementAt(1);
            var summand3 = summands.Last();

            Assert.Equal(2, summand1.Multiplier);
            Assert.Single(summand1.Variables);
            Assert.True(summand1.Variables.ContainsKey('y'));
            Assert.Equal(1, summand1.Variables['y']);

            Assert.Equal(5, summand2.Multiplier);
            Assert.Empty(summand2.Variables);

            Assert.Equal(-1, summand3.Multiplier);
            Assert.Single(summand3.Variables);
            Assert.True(summand3.Variables.ContainsKey('x'));
            Assert.Equal(1, summand3.Variables['x']);
        }

        [Fact]
        public void Test8()
        {
            var str = "x^2 + 2x";

            var parser = new Parser(str);

            var summands = parser.GetSummand();
            var summand1 = summands.First();
            var summand2 = summands.Last();

            Assert.Equal(1, summand1.Multiplier);
            Assert.Single(summand1.Variables);
            Assert.True(summand1.Variables.ContainsKey('x'));
            Assert.Equal(2, summand1.Variables['x']);

            Assert.Equal(2, summand2.Multiplier);
            Assert.Single(summand2.Variables);
            Assert.True(summand2.Variables.ContainsKey('x'));
            Assert.Equal(1, summand2.Variables['x']);
        }

        [Fact]
        public void Test9()
        {
            var str = "x^2 - 2x";

            var parser = new Parser(str);

            var summands = parser.GetSummand();
            var summand1 = summands.First();
            var summand2 = summands.Last();

            Assert.Equal(1, summand1.Multiplier);
            Assert.Single(summand1.Variables);
            Assert.True(summand1.Variables.ContainsKey('x'));
            Assert.Equal(2, summand1.Variables['x']);

            Assert.Equal(-2, summand2.Multiplier);
            Assert.Single(summand2.Variables);
            Assert.True(summand2.Variables.ContainsKey('x'));
            Assert.Equal(1, summand2.Variables['x']);
        }
        
        [Fact]
        public void Test10()
        {
            var str = "x2 = 3y";

            var parser = new Parser(str);

            var summands = parser.GetSummand();
            var summand1 = summands.First();
            var summand2 = summands.Last();

            Assert.Equal(2, summand1.Multiplier);
            Assert.Single(summand1.Variables);
            Assert.True(summand1.Variables.ContainsKey('x'));
            Assert.Equal(1, summand1.Variables['x']);

            Assert.Equal(-3, summand2.Multiplier);
            Assert.Single(summand2.Variables);
            Assert.True(summand2.Variables.ContainsKey('y'));
            Assert.Equal(1, summand2.Variables['y']);
        }

        [Fact]
        public void Test11()
        {
            var str = "x2 + 4 = 3y";

            var parser = new Parser(str);

            var summands = parser.GetSummand();
            var summand1 = summands.First();
            var summand2 = summands.ElementAt(1);
            var summand3 = summands.Last();

            Assert.Equal(2, summand1.Multiplier);
            Assert.Single(summand1.Variables);
            Assert.True(summand1.Variables.ContainsKey('x'));
            Assert.Equal(1, summand1.Variables['x']);

            Assert.Equal(4, summand2.Multiplier);
            Assert.Equal(0, summand2.Variables.Count);

            Assert.Equal(-3, summand3.Multiplier);
            Assert.Single(summand3.Variables);
            Assert.True(summand3.Variables.ContainsKey('y'));
            Assert.Equal(1, summand3.Variables['y']);
        }

        [Fact]
        public void Test111()
        {
            var str = "y^2 - xy + y";

            var parser = new Parser(str);

            var summands = parser.GetSummand();
            var summand1 = summands.First();
            var summand2 = summands.ElementAt(1);
            var summand3 = summands.Last();

            Assert.Equal(1, summand1.Multiplier);
            Assert.Single(summand1.Variables);
            Assert.True(summand1.Variables.ContainsKey('y'));
            Assert.Equal(2, summand1.Variables['y']);

            Assert.Equal(-1, summand2.Multiplier);
            Assert.Equal(2, summand2.Variables.Count);
            Assert.True(summand2.Variables.ContainsKey('x'));
            Assert.Equal(1, summand2.Variables['x']);
            Assert.True(summand2.Variables.ContainsKey('y'));
            Assert.Equal(1, summand2.Variables['y']);

            Assert.Equal(1, summand3.Multiplier);
            Assert.Single(summand3.Variables);
            Assert.True(summand3.Variables.ContainsKey('y'));
            Assert.Equal(1, summand3.Variables['y']);
        }

        [Fact]
        public void Test12()
        {
            var str = "2x^4 + 3xy - (6x^2y + -5xy^2 + 3y(x - 5y))";

            var parser = new Parser(str);

            var summands = parser.GetSummand();
            var summand1 = summands.ElementAt(0);
            var summand2 = summands.ElementAt(1);
            var summand3 = summands.ElementAt(2);
            var summand4 = summands.ElementAt(3);
            var summand5 = summands.ElementAt(4);
            var summand6 = summands.ElementAt(5);

            Assert.Equal(6, summands.Count());

            Assert.Equal(2, summand1.Multiplier);
            Assert.Single(summand1.Variables);
            Assert.True(summand1.Variables.ContainsKey('x'));
            Assert.Equal(4, summand1.Variables['x']);

            Assert.Equal(3, summand2.Multiplier);
            Assert.Equal(2, summand2.Variables.Count);
            Assert.True(summand2.Variables.ContainsKey('x'));
            Assert.Equal(1, summand2.Variables['x']);
            Assert.True(summand2.Variables.ContainsKey('y'));
            Assert.Equal(1, summand2.Variables['y']);

            Assert.Equal(-6, summand3.Multiplier);
            Assert.Equal(2, summand3.Variables.Count);
            Assert.True(summand3.Variables.ContainsKey('x'));
            Assert.Equal(2, summand3.Variables['x']);
            Assert.True(summand3.Variables.ContainsKey('y'));
            Assert.Equal(1, summand3.Variables['y']);

            Assert.Equal(5, summand4.Multiplier);
            Assert.Equal(2, summand4.Variables.Count);
            Assert.True(summand4.Variables.ContainsKey('x'));
            Assert.Equal(1, summand4.Variables['x']);
            Assert.True(summand4.Variables.ContainsKey('y'));
            Assert.Equal(2, summand4.Variables['y']);

            Assert.Equal(15, summand6.Multiplier);
            Assert.Single(summand6.Variables);
            Assert.True(summand6.Variables.ContainsKey('y'));
            Assert.Equal(2, summand6.Variables['y']);
        }

        [Fact]
        public void Test13()
        {
            var str = "x2X";

            var parser = new Parser(str);

            var summands = parser.GetSummand();
            var summand1 = summands.First();
            var summand2 = summands.Last();

            Assert.Equal(2, summand1.Multiplier);
            Assert.Single(summand1.Variables);
            Assert.True(summand1.Variables.ContainsKey('x'));
            Assert.Equal(2, summand1.Variables['x']);
        }

        [Fact]
        public void Test14()
        {
            var str = "x2 = 3X";

            var parser = new Parser(str);

            var summands = parser.GetSummand();
            var summand1 = summands.First();
            var summand2 = summands.Last();

            Assert.Equal(2, summand1.Multiplier);
            Assert.Single(summand1.Variables);
            Assert.True(summand1.Variables.ContainsKey('x'));
            Assert.Equal(1, summand1.Variables['x']);

            Assert.Equal(-3, summand2.Multiplier);
            Assert.Single(summand2.Variables);
            Assert.True(summand2.Variables.ContainsKey('x'));
            Assert.Equal(1, summand2.Variables['x']);
        }
    }
}
