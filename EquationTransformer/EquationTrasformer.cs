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

            summands = summands.OrderByDescending(s => s.Power).ToList();

            var excluded = new HashSet<int>();
            var newSummands = new List<Summand>();

            for(var i = 0; i < summands.Count; i++)
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
                    newSummands.Add(currentSummand);
                }                
            }

            return StringifySummands(newSummands);
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
