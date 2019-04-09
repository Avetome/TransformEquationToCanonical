using System;
using System.Collections.Generic;
using EquationTransformer;
using EquationTransformerTests.Helpers;
using Xunit;

namespace EquationTransformerTests
{
    public class SummandTests
    {
        [Theory(DisplayName = "Summands ToString() get readable and correct stringified summand")]
        [MemberData(nameof(GetSimpleNumbersAndVariables))]
        [MemberData(nameof(GetSummandsWithPower))]
        [MemberData(nameof(GetSummandsWithNegativePower))]
        [MemberData(nameof(GetSummandsWithZeroPower))]
        [MemberData(nameof(GetSummandsToCheckCorrectVariablesSorting))]
        [MemberData(nameof(GetSummandsToCheckSpecialMultipliers))]
        public void SummandsToStringTests(Summand summand, string stringifiedSummand)
        {
            Assert.Equal(stringifiedSummand, summand.ToString());
        }

        [Theory(DisplayName = "Summands ToString() should correct handle isInEquation parameter")]
        [MemberData(nameof(GetDataForCheckWithIsInEquation))]
        public void SummandsToStringWithisInEquationTests(
            Summand summand,
            string stringifiedSummand,
            string stringifiedWithIsInEquationSummand)
        {
            Assert.Equal(stringifiedSummand, summand.ToString());
            Assert.Equal(stringifiedWithIsInEquationSummand, summand.ToString(isInEquation: true));
        }

        public static IEnumerable<object[]> GetSimpleNumbersAndVariables()
        {
            yield return new object[] { SummandsHelper.CreateSummand(0), "0" };
            yield return new object[] { SummandsHelper.CreateSummand(1), "1" };
            yield return new object[] { SummandsHelper.CreateSummand(-1), "-1" };
            yield return new object[] { SummandsHelper.CreateSummand(15), "15" };
            yield return new object[] { SummandsHelper.CreateSummand(-15), "-15" };

            yield return new object[] { SummandsHelper.CreateSummand(('y', 1)), "y"};
            yield return new object[] { SummandsHelper.CreateSummand(-1, ('y', 1)), "-y" };

            yield return new object[] { SummandsHelper.CreateSummand(('y', 2)), "y^2" };
            yield return new object[] { SummandsHelper.CreateSummand(-1, ('y', 2)), "-y^2" };

            yield return new object[] { SummandsHelper.CreateSummand(-1, ('y', 2), ('x', 1)), "-xy^2" };
            yield return new object[] { SummandsHelper.CreateSummand(-1, ('y', 2), ('x', 3), ('z', 1)), "-zy^2x^3" };
        }

        public static IEnumerable<object[]> GetSummandsWithPower()
        {
            yield return new object[] { SummandsHelper.CreateSummand(('y', 2)), "y^2" };
            yield return new object[] { SummandsHelper.CreateSummand(-1, ('y', 2)), "-y^2" };
        }

        public static IEnumerable<object[]> GetSummandsWithNegativePower()
        {
            yield return new object[] { SummandsHelper.CreateSummand(('y', -1)), "y^-1" };
            yield return new object[] { SummandsHelper.CreateSummand(('y', -2)), "y^-2" };
            yield return new object[] { SummandsHelper.CreateSummand(-1, ('y', -2)), "-y^-2" };
        }

        public static IEnumerable<object[]> GetSummandsWithZeroPower()
        {
            yield return new object[] { SummandsHelper.CreateSummand(('y', 0)), "1" };
            yield return new object[] { SummandsHelper.CreateSummand(5, ('y', 0)), "5" };
        }

        public static IEnumerable<object[]> GetSummandsToCheckCorrectVariablesSorting()
        {
            yield return new object[] { SummandsHelper.CreateSummand(-1, ('y', 2), ('x', 1)), "-xy^2" };
            yield return new object[] { SummandsHelper.CreateSummand(-1, ('y', 2), ('x', 3), ('z', 1)), "-zy^2x^3" };
            yield return new object[] { SummandsHelper.CreateSummand(-1, ('y', 2), ('x', 2), ('z', 1)), "-zx^2y^2" };
        }

        public static IEnumerable<object[]> GetSummandsToCheckSpecialMultipliers()
        {
            yield return new object[] { SummandsHelper.CreateSummand(1, ('y', 2), ('x', 1)), "xy^2" };
            yield return new object[] { SummandsHelper.CreateSummand(-1, ('y', 2), ('x', 1)), "-xy^2" };
            yield return new object[] { SummandsHelper.CreateSummand(0, ('y', 2), ('x', 1)), string.Empty };
        }

        public static IEnumerable<object[]> GetDataForCheckWithIsInEquation()
        {
            yield return new object[] { SummandsHelper.CreateSummand(5), "5", "5" };
            yield return new object[] { SummandsHelper.CreateSummand(1, ('x', 1)), "x", "x" };
            yield return new object[] { SummandsHelper.CreateSummand(-1, ('y', 2), ('x', 1)), "-xy^2", "xy^2" };
        }
    }
}
