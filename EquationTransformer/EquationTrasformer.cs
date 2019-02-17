using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EquationTransformer
{
    public class EquationTrasformer
    {
        public string Transform(string equation)
        {
            var tokenizer = new Tokenizer(equation);
            var parser = new Parser(tokenizer);

            var summands = parser.GetSummand().ToList();

            if (!summands.Any())
            {
                return string.Empty;
            }

            var equalsReached = false;

            if (tokenizer.Token == Token.Equals)
            {
                equalsReached = true;
            }
            else
            {
                throw new Exception("Equals not found");
            }

            List<Summand> rightSummands = new List<Summand>();
            if (equalsReached)
            {
                tokenizer.NextToken();
                rightSummands = parser.GetSummand().ToList();
            }

            foreach (var s in rightSummands)
            {
                s.Multiply(-1);
            }

            summands.AddRange(rightSummands);

            summands = summands.OrderByDescending(s => s.Power).ToList();

            return StringifySummands(SimplifySummands(summands));
        }

        /// <summary>
        /// Summarize summands with equals power and remove summands with zero multiplier.
        /// </summary>
        /// <param name="summands"></param>
        /// <returns></returns>
        private List<Summand> SimplifySummands(List<Summand> summands)
        {
            var result = new List<Summand>();

            var excluded = new HashSet<int>();

            for (var i = 0; i < summands.Count; i++)
            {
                if (excluded.Contains(i))
                {
                    continue;
                }

                var currentSummand = summands[i];

                for (var j = i + 1; j < summands.Count; j++)
                {
                    if (currentSummand.Power != summands[j].Power)
                    {
                        break;
                    }

                    if (currentSummand.IsEqualent(summands[j]))
                    {
                        excluded.Add(j);
                        currentSummand.Multiplier += summands[j].Multiplier;
                    }
                }

                if (currentSummand.Multiplier != 0)
                {
                    result.Add(currentSummand);
                }
            }

            return result;
        }

        private string StringifySummands(List<Summand> summands)
        {
            if (!summands.Any())
            {
                return "0 = 0;";
            }

            var sb = new StringBuilder();

            sb.Append(summands[0].ToString());

            for (var i = 1; i < summands.Count; i++)
            {
                if (summands[i].Multiplier > 0)
                {
                    sb.Append($" + {summands[i].ToString()}");
                }
                else
                {
                    sb.Append($" - {summands[i].ToString(isInEquation: true)}");
                }
            }

            sb.Append(" = 0;");

            return sb.ToString();
        }
    }
}
