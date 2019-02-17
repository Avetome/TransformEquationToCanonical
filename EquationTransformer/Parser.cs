using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace EquationTransformer
{
    public class Parser
    {
        private Tokenizer _tokenizer;
        private int _parensCount = 0;

        public Parser(Tokenizer tokenizer)
        {
            _tokenizer = tokenizer;
        }

        public Parser(string str) : this(new Tokenizer(new StringReader(str)))
        {
        }

        public IEnumerable<Summand> GetSummand()
        {
            var summands = ParseEquation().ToList();

            if (_parensCount != 0)
            {
                throw new Exception("Invalid count of parens");
            }

            var equalsReached = false;

            if (_tokenizer.Token == Token.Equals)
            {
                equalsReached = true;
            }
            else
            {                
                // throw new Exception("Equals not found");
            }

            List<Summand> rightSummands = new List<Summand>();
            if (equalsReached)
            {
                _tokenizer.NextToken();
                rightSummands = ParseEquation().ToList();
            }

            foreach (var s in rightSummands)
            {
                s.Multiply(-1);
            }

            summands.AddRange(rightSummands);

            return summands;
        }

        private IEnumerable<Summand> ParseEquation(sbyte sign = 1, bool isInBrackets = false)
        {
            var result = new List<Summand>();

            Summand lastSummand;
            while(true)
            {
                var summand = ParseSummand();
                
                if (summand != null)
                {
                    result.Add(summand);
                    summand.Multiplier *= sign;
                }

                if (_tokenizer.Token == Token.Subtract)
                {
                    sign *= -1;
                }

                if (_tokenizer.Token == Token.Add)
                {
                    sign = 1;
                }

                if (_tokenizer.Token == Token.EOF
                    || _tokenizer.Token == Token.Equals)
                {
                    return result;
                }

                if (_tokenizer.Token == Token.CloseParens)
                {
                    _parensCount--;
                    return result;
                }

                if (_tokenizer.Token == Token.OpenParens)
                {
                    var prevToken = _tokenizer.PrevToken;

                    _tokenizer.NextToken();
                    _parensCount++;                   

                    var summands = ParseEquation(1, true);

                    if (prevToken == Token.Number
                        || prevToken == Token.Variable
                        || prevToken == Token.Multiply)
                    {
                        var multiplier = result.Last();

                        result.RemoveAt(result.Count - 1);

                        if (multiplier != null)
                        {
                            foreach(var s in summands)
                            {
                                s.Multiply(summand);
                            }
                        }
                    }

                    if (prevToken == Token.Subtract)
                    {
                        foreach (var s in summands)
                        {
                            s.Multiply(-1);
                        }
                    }

                    result.AddRange(summands);
                }

                _tokenizer.NextToken();
            }
        }

        private Summand ParseSummand()
        {
            if (_tokenizer.Token != Token.Number
                && _tokenizer.Token != Token.Variable
                && _tokenizer.Token != Token.Subtract)
            {
                return null;
            }

            var summand = new Summand();
            summand.Multiplier = 1;

            if (_tokenizer.Token == Token.Subtract)
            {
                summand.Multiplier = -1;
                _tokenizer.NextToken();
            }

            bool isValidSummand = false;

            while (true)
            {
                if (_tokenizer.Token == Token.Number)
                {
                    summand.Multiplier *= _tokenizer.Number;

                    isValidSummand = true;

                    _tokenizer.NextToken();
                    continue;
                }

                // skip '*'
                if (_tokenizer.Token == Token.Multiply)
                {
                    _tokenizer.NextToken();
                    continue;
                }

                if (_tokenizer.Token == Token.Variable)
                {
                    // TODO: make it with one parameter
                    summand.AddVariable(_tokenizer.Variable.Name, _tokenizer.Variable.Power);

                    isValidSummand = true;

                    _tokenizer.NextToken();
                    continue;
                }

                return isValidSummand ? summand : null;
            }
        }
    }
}
