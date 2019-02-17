using EquationTransformer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xunit;

namespace EquationTransformerTests
{
    public class TokenizerTests
    {
        [Fact]
        public void Test1()
        {
            var str = "3x^2";

            var tokenizer = new Tokenizer(new StringReader(str));

            Assert.Equal(Token.Number, tokenizer.Token);
            Assert.Equal(3, tokenizer.Number);

            tokenizer.NextToken();

            Assert.Equal(Token.Variable, tokenizer.Token);
            Assert.Equal('x', tokenizer.Variable.Name);
            Assert.Equal(2, tokenizer.Variable.Power);
        }

        [Fact]
        public void Test2()
        {
            var str = "3x ^ 2";

            var tokenizer = new Tokenizer(new StringReader(str));

            Assert.Equal(Token.Number, tokenizer.Token);
            Assert.Equal(3, tokenizer.Number);

            tokenizer.NextToken();

            Assert.Equal(Token.Variable, tokenizer.Token);
            Assert.Equal('x', tokenizer.Variable.Name);
            Assert.Equal(2, tokenizer.Variable.Power);
        }

        [Fact]
        public void Test3()
        {
            var str = "3x^2y^5";

            var tokenizer = new Tokenizer(new StringReader(str));

            Assert.Equal(Token.Number, tokenizer.Token);
            Assert.Equal(3, tokenizer.Number);

            tokenizer.NextToken();

            Assert.Equal(Token.Variable, tokenizer.Token);
            Assert.Equal('x', tokenizer.Variable.Name);
            Assert.Equal(2, tokenizer.Variable.Power);

            tokenizer.NextToken();

            Assert.Equal(Token.Variable, tokenizer.Token);
            Assert.Equal('y', tokenizer.Variable.Name);
            Assert.Equal(5, tokenizer.Variable.Power);
        }

        [Fact]
        public void Test4()
        {
            var str = "3xy";

            var tokenizer = new Tokenizer(new StringReader(str));

            Assert.Equal(Token.Number, tokenizer.Token);
            Assert.Equal(3, tokenizer.Number);

            tokenizer.NextToken();

            Assert.Equal(Token.Variable, tokenizer.Token);
            Assert.Equal('x', tokenizer.Variable.Name);
            Assert.Equal(1, tokenizer.Variable.Power);

            tokenizer.NextToken();

            Assert.Equal(Token.Variable, tokenizer.Token);
            Assert.Equal('y', tokenizer.Variable.Name);
            Assert.Equal(1, tokenizer.Variable.Power);
        }

        [Fact]
        public void Test5()
        {
            var str = "3x5y";

            var tokenizer = new Tokenizer(new StringReader(str));

            Assert.Equal(Token.Number, tokenizer.Token);
            Assert.Equal(3, tokenizer.Number);

            tokenizer.NextToken();

            Assert.Equal(Token.Variable, tokenizer.Token);
            Assert.Equal('x', tokenizer.Variable.Name);
            Assert.Equal(1, tokenizer.Variable.Power);

            tokenizer.NextToken();

            Assert.Equal(Token.Number, tokenizer.Token);
            Assert.Equal(5, tokenizer.Number);

            tokenizer.NextToken();

            Assert.Equal(Token.Variable, tokenizer.Token);
            Assert.Equal('y', tokenizer.Variable.Name);
            Assert.Equal(1, tokenizer.Variable.Power);
        }
    }
}
