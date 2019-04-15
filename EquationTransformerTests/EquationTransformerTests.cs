using EquationTransformer;
using System;
using System.Collections.Generic;
using Xunit;

namespace EquationTransformerTests
{
    /// <summary>
    /// TODO: I'm too lazy to write display names to other tests in other files.
    /// It's only test work, after all.
    /// And I also known what "Test1" - not a best name for test methods, but...
    /// </summary>
    [Collection("Full equation transform tests")]
    public class EquationTransformerTests
    {
        [Theory(DisplayName = "Can transform equation to canonical form")]
        [MemberData(nameof(GetEquations))]
        public void TransformEquationTest(string equation, string equationInCanonicalForm)
        {
            var trasformer = new EquationTrasformer();

            var result = trasformer.Transform(equation);

            Assert.Equal(equationInCanonicalForm, result);
        }

        [Theory(DisplayName = "Should raise exception if equation is invalid")]
        [MemberData(nameof(GetInvalidEquations))]
        public void TransformEquationExceptionTest(string equation)
        {
            var trasformer = new EquationTrasformer();

            Exception ex = Assert.Throws<Exception>(() => trasformer.Transform(equation));
        }

        public static IEnumerable<object[]> GetEquations()
        {
            var examples = GetEquationsData();

            foreach(var example in examples)
            {
                yield return new object[] { example.equation, example.normalEquation };
            }
        }

        public static IEnumerable<(string equation, string normalEquation)> GetEquationsData()
        {
            yield return ("3x5y = 4y7z", "15xy - 28yz = 0;");
            yield return ("3x5y = -4y7z", "15xy + 28yz = 0;");
            yield return ("3x5y = 4y7x", "-13xy = 0;");
            yield return ("x^2 + 3.5xy + y = y^2 - xy + y", "x^2 - y^2 + 4.5xy = 0;");
            yield return ("x = y", "x - y = 0;");
            yield return ("x - (y^2 - x) = 0", "-y^2 + 2x = 0;");
            yield return ("x - (0 - (0 - x)) = 0", "0 = 0;");
            yield return ("x2 = 3X", "-x = 0;");
            yield return ("-7 + 4x^2 + xy - x^4 + z = y - 2xy + w^2", "-x^4 + 4x^2 - w^2 + 3xy + z - y - 7 = 0;");
            yield return ("2 - 1 = x", "-x + 1 = 0;");
            yield return ("x^-1 = 5", "x^-1 - 5 = 0;");
            yield return ("x + y = 1", "x + y - 1 = 0;");
        }

        public static IEnumerable<object[]> GetInvalidEquations()
        {
            yield return new object[] { "y2(x + 5(y + ) = yx" };

            yield return new object[] { "y2(x + 5(y + 5)) - yx" };
        }
    }
}
