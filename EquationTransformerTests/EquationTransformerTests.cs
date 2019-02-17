using EquationTransformer;
using Xunit;

namespace EquationTransformerTests
{
    /// <summary>
    /// TODO: I'm too lazy to write display names to other tests in other files.
    /// It's only test work, after all.
    /// </summary>
    [Collection("Full equation transform tests")]
    public class EquationTransformerTests
    {
        [Fact(DisplayName = "Equation: 3x5y = 4y7z")]
        public void Test1()
        {
            var str = "3x5y = 4y7z";

            var trasformer = new EquationTrasformer();

            var result = trasformer.Transform(str);

            Assert.Equal("15xy - 28yz = 0;", result);
        }

        [Fact(DisplayName = "Equation: 3x5y = -4y7z")]
        public void Test11()
        {
            var str = "3x5y = -4y7z";

            var trasformer = new EquationTrasformer();

            var result = trasformer.Transform(str);

            Assert.Equal("15xy + 28yz = 0;", result);
        }

        [Fact(DisplayName = "Equation: 3x5y = 4y7x")]
        public void Test2()
        {
            var str = "3x5y = 4y7x";

            var trasformer = new EquationTrasformer();

            var result = trasformer.Transform(str);

            Assert.Equal("-13xy = 0;", result);
        }


        [Fact(DisplayName = "Equation: x^2 + 3.5xy + y = y^2 - xy + y")]
        public void Test3()
        {
            var str = "x^2 + 3.5xy + y = y^2 - xy + y";

            var trasformer = new EquationTrasformer();

            var result = trasformer.Transform(str);

            Assert.Equal("x^2 - y^2 + 4.5xy = 0;", result);            
        }

        [Fact(DisplayName = "Equation: x = y")]
        public void Test4()
        {
            var str = "x = y";

            var trasformer = new EquationTrasformer();

            var result = trasformer.Transform(str);

            Assert.Equal("x - y = 0;", result);
        }

        [Fact(DisplayName = "Equation: x - (y^2 - x) = 0")]
        public void Test5()
        {
            var str = "x - (y^2 - x) = 0";

            var trasformer = new EquationTrasformer();

            var result = trasformer.Transform(str);

            Assert.Equal("-y^2 + 2x = 0;", result);
        }

        [Fact(DisplayName = "Equation: x - (0 - (0 - x)) = 0")]
        public void Test6()
        {
            var str = "x - (0 - (0 - x)) = 0";

            var trasformer = new EquationTrasformer();

            var result = trasformer.Transform(str);

            Assert.Equal("0 = 0;", result);
        }

        [Fact(DisplayName = "Equation: x2 = 3X")]
        public void Test7()
        {
            var str = "x2 = 3X";

            var trasformer = new EquationTrasformer();

            var result = trasformer.Transform(str);

            Assert.Equal("-x = 0;", result);
        }
    }
}
