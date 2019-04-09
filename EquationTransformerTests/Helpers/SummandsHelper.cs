using EquationTransformer;
using System;
using System.Collections.Generic;
using System.Text;

namespace EquationTransformerTests.Helpers
{
    public static class SummandsHelper
    {
        public static Summand CreateSummand(params (char name, int power)[] variables)
        {
            return CreateSummand(1, variables);
        }

        public static Summand CreateSummand(double multiplier, params (char name, int power)[] variables)
        {
            var summand = new Summand();
            summand.Multiplier = multiplier;

            foreach (var variable in variables)
            {
                summand.AddVariable(variable.name, variable.power);
            }

            return summand;
        }
    }
}
