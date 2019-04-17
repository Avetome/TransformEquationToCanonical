using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;

namespace EquationTransformer
{
    [DebuggerDisplay("{ToString(false)}")]
    public class Summand
    {
        private Dictionary<char, int> _variables = new Dictionary<char, int>();

        private int _maxPower = 0;

        public double Multiplier { get; set; } = 1;

        public int Power => _maxPower;

        /// <summary>
        /// TODO: think about zero power and negative power; Exclude if zero power.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="power"></param>
        public void AddVariable(char name, int power)
        {
            name = char.ToLowerInvariant(name);

            // handle y * y * x^2 situation
            if (_variables.ContainsKey(name))
            {
                power += _variables[name];
            }

            if (power > _maxPower)
            {
                _maxPower = power;
            }

            _variables[name] = power;
        }

        public void Multiply(Summand summand)
        {
            Multiplier *= summand.Multiplier;

            foreach (var variable in summand.Variables.Keys)
            {
                AddVariable(variable, summand.Variables[variable]);
            }
        }

        public void Multiply(int number)
        {
            Multiplier *= number;
        }

        public Dictionary<char, int> Variables => _variables;

        public bool IsEqualent(Summand summand)
        {
            if (summand.Variables.Count != _variables.Count)
            {
                return false;
            }
            
            foreach(var variable in summand.Variables.Keys)
            {
                if (!_variables.ContainsKey(variable))
                {
                    return false;
                }

                if (_variables[variable] != summand.Variables[variable])
                {
                    return false;
                }
            }

            return true;
        }

        public string ToString(bool isInEquation = false)
        {
            if (Math.Abs(Multiplier) < double.Epsilon && _variables.Any())
            {
                return string.Empty;
            }

            if (_variables.Count() == 1 && _variables.First().Value == 0)
            {
                return $"{Math.Abs(Multiplier).ToString(CultureInfo.InvariantCulture)}";
            }

            var variables = _variables
                .Select(v => (Variable: v.Key, Power: v.Value))
                .OrderBy(v => v.Power)
                .ThenBy(v => v.Variable)
                .Select(v => v.Power != 1 ? $"{v.Variable}^{v.Power}" : $"{v.Variable}")
                .ToList();

            // Multiplier == 1
            if (Math.Abs(Multiplier - 1) < double.Epsilon && variables.Any())
            {
                return string.Join(string.Empty, variables);
            }

            if (isInEquation)
            {
                // Multiplier == -1
                if (Math.Abs(Multiplier + 1) < double.Epsilon && variables.Any())
                {
                    return string.Join(string.Empty, variables);
                }

                return $"{Math.Abs(Multiplier).ToString(CultureInfo.InvariantCulture)}{string.Join(string.Empty, variables)}";
            }

            // Multiplier == -1
            if (Math.Abs(Multiplier + 1) < double.Epsilon && variables.Any())
            {
                return $"-{string.Join(string.Empty, variables)}";
            }

            return $"{Multiplier.ToString(CultureInfo.InvariantCulture)}{string.Join(string.Empty, variables)}";
        }
    }
}
