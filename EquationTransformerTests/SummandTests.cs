using System;
using EquationTransformer;
using Xunit;

namespace EquationTransformerTests
{
    public class SummandTests
    {
        [Fact]
        public void Test1()
        {
            var s1 = new Summand();
            s1.Multiplier = 5;
            s1.AddVariable('x', 2);
            s1.AddVariable('y', 2);

            var s2 = new Summand();
            s2.Multiplier = 5;
            s2.AddVariable('x', 2);
            s2.AddVariable('y', 2);

            Assert.True(s1.IsEqualent(s2));
        }

        [Fact]
        public void Test2()
        {
            var s1 = new Summand();
            s1.Multiplier = 5;
            s1.AddVariable('x', 2);
            s1.AddVariable('y', 2);

            var s2 = new Summand();
            s2.Multiplier = 5;
            s2.AddVariable('z', 2);
            s2.AddVariable('y', 2);

            Assert.False(s1.IsEqualent(s2));
        }

        [Fact]
        public void Test3()
        {
            var s1 = new Summand();
            s1.Multiplier = 2;
            s1.AddVariable('x', 2);
            s1.AddVariable('y', 2);
            s1.AddVariable('x', 2);

            var s2 = new Summand();
            s2.Multiplier = 5;
            s2.AddVariable('x', 4);
            s2.AddVariable('y', 2);

            Assert.True(s1.IsEqualent(s2));
        }

        [Fact]
        public void Test4()
        {
            var s1 = new Summand();
            s1.Multiplier = 2;
            s1.AddVariable('x', 2);
            s1.AddVariable('y', 2);
            s1.AddVariable('z', 2);

            var s2 = new Summand();
            s2.Multiplier = 5;
            s2.AddVariable('x', 2);
            s2.AddVariable('y', 2);

            Assert.False(s1.IsEqualent(s2));
        }

        [Fact]
        public void Test5()
        {
            var s1 = new Summand();
            s1.Multiplier = 2;
            s1.AddVariable('x', 2);
            s1.AddVariable('y', 2);
            s1.AddVariable('z', 2);
                        
            Assert.Equal("2x^2y^2z^2", s1.ToString());
        }

        [Fact]
        public void Test6()
        {
            var s1 = new Summand();
            s1.Multiplier = 2;
            s1.AddVariable('x', 1);

            Assert.Equal("2x", s1.ToString());
        }

        [Fact]
        public void Test7()
        {
            var s1 = new Summand();
            s1.Multiplier = 2;
            s1.AddVariable('y', 1);
            s1.AddVariable('x', 1);            

            Assert.Equal("2xy", s1.ToString());
        }
    }
}
