using System.Globalization;
using System.IO;
using System.Text;

namespace EquationTransformer
{
    /// <summary>
    /// TODO: implement IDisposable
    /// </summary>
    public class Tokenizer
    {
        private TextReader _reader;
        private char _currentChar;
        private Token _currentToken;
        private Token _prevToken;
        private double _number;
        private (char name, int power) _variable;

        public Tokenizer(TextReader reader)
        {
            _reader = reader;
            NextChar();
            NextToken();
        }

        public Tokenizer(string str) : this(new StringReader(str))
        {
        }

        public Token Token => _currentToken;

        public Token PrevToken => _prevToken;

        public double Number => _number;

        public (char Name, int Power) Variable => _variable;

        void NextChar()
        {
            int ch = _reader.Read();
            _currentChar = ch < 0 ? '\0' : (char)ch;
        }

        public void NextToken()
        {
            _prevToken = _currentToken;
            _number = 0;

            SkipWhitespaces();

            switch (_currentChar)
            {
                case '\0':
                    _currentToken = Token.EOF;
                    return;

                case '+':
                    NextChar();
                    _currentToken = Token.Add;
                    return;

                case '-':
                    NextChar();
                    _currentToken = Token.Subtract;
                    return;

                case '*':
                    NextChar();
                    _currentToken = Token.Multiply;
                    return;

                case '(':
                    NextChar();
                    _currentToken = Token.OpenParens;
                    return;

                case ')':
                    NextChar();
                    _currentToken = Token.CloseParens;
                    return;

                case '=':
                    NextChar();
                    _currentToken = Token.Equals;
                    return;
            }

            if (char.IsDigit(_currentChar) || _currentChar == '.')
            {
                _number = ReadDouble();
                _currentToken = Token.Number;

                return;
            }

            if (char.IsLetter(_currentChar) || char.IsDigit(_currentChar))
            {
                _variable.name = _currentChar;
                _variable.power = 1;

                NextChar();
                SkipWhitespaces();

                if (_currentChar == '^')
                {
                    NextChar();
                    SkipWhitespaces();

                    var sign = 1;

                    if (_currentChar == '-')
                    {
                        sign = -1;

                        NextChar();
                        SkipWhitespaces();
                    }

                    _variable.power = sign * ReadInt();
                }

                _currentToken = Token.Variable;

                return;
            }

            throw new InvalidDataException($"Unexpected character: {_currentChar}");
        }

        private void SkipWhitespaces()
        {
            while (char.IsWhiteSpace(_currentChar))
            {
                NextChar();
            }
        }

        private int ReadInt()
        {
            var sb = new StringBuilder();
            while (char.IsDigit(_currentChar))
            {
                sb.Append(_currentChar);
                NextChar();
            }

            return int.Parse(sb.ToString(), CultureInfo.InvariantCulture);
        }

        private double ReadDouble()
        {
            var sb = new StringBuilder();
            bool haveDecimalPoint = false;
            while (char.IsDigit(_currentChar) || (!haveDecimalPoint && _currentChar == '.'))
            {
                sb.Append(_currentChar);
                haveDecimalPoint = _currentChar == '.';
                NextChar();
            }

            return double.Parse(sb.ToString(), CultureInfo.InvariantCulture);
        }
    }
}
