using System;
namespace EquationTransformer
{
    public abstract class Node
    {
        public abstract double Eval();
    }

    class NodeNumber : Node
    {
        public NodeNumber(double number)
        {
            _number = number;
        }

        double _number;             // The number

        public override double Eval()
        {
            // Just return it.  Too easy.
            return _number;
        }
    }
}
