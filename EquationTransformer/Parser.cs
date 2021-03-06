﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace EquationTransformer
{
    public class Parser
    {
        private Tokenizer _tokenizer;

        public Parser(Tokenizer tokenizer)
        {
            _tokenizer = tokenizer;
        }

        public Parser(string str) : this(new Tokenizer(new StringReader(str)))
        {
        }

        public IEnumerable<Summand> GetSummand()
        {
            var result = new List<Summand>();

            result = ParseAddAndSubstract();

            if (_tokenizer.Token != Token.EOF
                && _tokenizer.Token != Token.Equals)
            {
                throw new Exception("Unexpected characters at end of equation");
            }

            return result;
        }

        private List<Summand> ParseAddAndSubstract()
        {
            var leftSummands = ParseMultiply();

            while (true)
            {
                if (_tokenizer.Token != Token.Subtract
                    && _tokenizer.Token != Token.Add)
                {
                    return leftSummands;
                }

                var sign = 1;

                if (_tokenizer.Token == Token.Subtract)
                {
                    sign = -1;
                }

                _tokenizer.NextToken();

                var rightSummands = ParseMultiply();

                if (sign == -1)
                {
                    foreach (var s in rightSummands)
                    {
                        s.Multiply(-1);
                    }
                }

                leftSummands.AddRange(rightSummands);
            }
        }

        private List<Summand> ParseMultiply()
        {
            var leftSummands = ParseUnary();

            while (true)
            {
                if (_tokenizer.Token != Token.Multiply
                    && _tokenizer.Token != Token.Number
                    && _tokenizer.Token != Token.Variable
                    && _tokenizer.Token != Token.OpenParens)
                {
                    return leftSummands;
                }

                if (_tokenizer.Token == Token.Multiply)
                {
                    _tokenizer.NextToken();
                }

                var rightSummands = ParseUnary();

                Summand localSummand = null;

                if (rightSummands.Count == 1)
                {
                    localSummand = rightSummands.First();
                }
                else if (leftSummands.Count == 1)
                {
                    localSummand = leftSummands.First();
                }

                // (x + y)(z + y) - for example. 
                if (localSummand == null)
                {
                    throw new Exception("Multiplying two parent expressions it not supported");
                }

                if (leftSummands.Count == 1)
                {
                    leftSummands = rightSummands;
                }

                foreach(var summand in leftSummands)
                {
                    summand.Multiply(localSummand);
                }
            }
        }

        private List<Summand> ParseUnary()
        {
            while (true)
            {
                // Skip '+' - it don't matter in this case
                if (_tokenizer.Token == Token.Add)
                {
                    _tokenizer.NextToken();
                    continue;
                }

                if (_tokenizer.Token == Token.Subtract)
                {
                    _tokenizer.NextToken();

                    // Parse right side
                    // Recurses to support negative of a negative
                    var rightSummands = ParseUnary();

                    foreach (var s in rightSummands)
                    {
                        s.Multiply(-1);
                    }

                    return rightSummands;
                }

                // No positive/negative operator so parse a alone summand
                return ParseSummands();
            }
        }

        private List<Summand> ParseSummands()
        {
            if (_tokenizer.Token != Token.Number
                    && _tokenizer.Token != Token.Variable
                    && _tokenizer.Token != Token.OpenParens)
            {
                // Upon the village, cross the sky
                // Creepy gizmo’s flying by
                // There’s excess in those days
                // of such gizmos anyways
                // http://chastushki.net.ru/texts/22
                // P.S. Don't get me wrong...
                throw new Exception($"Unexpect token: {_tokenizer.Token}");
            }

            if (_tokenizer.Token == Token.OpenParens)
            {
                _tokenizer.NextToken();

                var summands = ParseAddAndSubstract();

                if (_tokenizer.Token != Token.CloseParens)
                {
                    throw new Exception("Invalid count of parens");
                }

                _tokenizer.NextToken();

                return summands;
            }

            var result = new List<Summand>();

            var summand = new Summand();
            summand.Multiplier = 1;

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
                    isValidSummand = false;

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

                if (isValidSummand)
                {
                    result.Add(summand);
                }
                else
                {
                    throw new Exception("Invalid multiplication");
                }                

                return result;
            }
        }
    }
}
