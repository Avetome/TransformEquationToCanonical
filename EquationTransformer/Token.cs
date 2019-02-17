using System;
namespace EquationTransformer
{
    public enum Token
    {
        EOF,
        Add,
        Subtract,
        Multiply,
        Divide,
        OpenParens,
        CloseParens,
        Number,
        Variable,
        Equals
    }
}
