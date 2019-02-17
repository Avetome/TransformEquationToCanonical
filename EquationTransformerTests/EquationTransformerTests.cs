using EquationTransformer;
using Xunit;

namespace EquationTransformerTests
{
    public class EquationTransformerTests
    {
        [Fact]
        public void Test1()
        {
            var str = "3x5y = 4y7z";

            var trasformer = new EquationTrasformer();

            var result = trasformer.Transform(str);

            Assert.Equal("15xy - 28yz = 0;", result);
        }

        [Fact]
        public void Test11()
        {
            var str = "3x5y = -4y7z";

            var trasformer = new EquationTrasformer();

            var result = trasformer.Transform(str);

            Assert.Equal("15xy + 28yz = 0;", result);
        }

        [Fact]
        public void Test2()
        {
            var str = "3x5y = 4y7x";

            var trasformer = new EquationTrasformer();

            var result = trasformer.Transform(str);

            Assert.Equal("-13xy = 0;", result);
        }


        [Fact]
        public void Test3()
        {
            var str = "x^2 + 3.5xy + y = y^2 - xy + y";

            var trasformer = new EquationTrasformer();

            var result = trasformer.Transform(str);

            Assert.Equal("x^2 - y^2 + 4.5xy = 0;", result);            
        }

        [Fact]
        public void Test4()
        {
            var str = "x = y";

            var trasformer = new EquationTrasformer();

            var result = trasformer.Transform(str);

            Assert.Equal("x - y = 0;", result);
        }

        [Fact]
        public void Test5()
        {
            var str = "x - (y^2 - x) = 0";

            var trasformer = new EquationTrasformer();

            var result = trasformer.Transform(str);

            Assert.Equal("-y^2 + 2x = 0;", result);
        }

        [Fact]
        public void Test6()
        {
            var str = "x - (0 - (0 - x)) = 0";

            var trasformer = new EquationTrasformer();

            var result = trasformer.Transform(str);

            Assert.Equal("0 = 0;", result);
        }
    }
}
