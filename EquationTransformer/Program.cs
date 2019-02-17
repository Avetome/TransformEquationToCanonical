using System;
using System.Linq;

namespace EquationTransformer
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            /*var testString = "10 + 20 - 30.123 - -1";
            var t = new Tokenizer(new StringReader(testString));

            while(t.Token != Token.EOF)
            {
                Console.WriteLine($"{t.Number} {t.Token}");
                t.NextToken();
            }*/

            var testString = "2x";
            var parser = new Parser(testString);

            var summand = parser.GetSummand().First();

            Console.WriteLine($"{summand.Multiplier} {summand.Variables.FirstOrDefault().Key} ^ {summand.Variables.FirstOrDefault().Value}");

            Console.ReadLine();
        }
    }
}
