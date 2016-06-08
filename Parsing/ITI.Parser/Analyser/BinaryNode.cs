using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI.Parser
{
    public class BinaryNode : Node
    {
        public BinaryNode( TokenType operatorType, Node left, Node right )
        {
            OperatorType = operatorType;
            Left = left;
            Right = right;
        }

        public TokenType OperatorType { get; }
        public Node Left { get; }
        public Node Right { get; }

        [DebuggerStepThrough]
        internal override Node Accept( NodeVisitor visitor ) => visitor.Visit( this );

        public override string ToString()
        {
            string op = null;
            switch( OperatorType )
            {
                case TokenType.Div: op = " / "; break;
                case TokenType.Mult: op = " * "; break;
                case TokenType.Plus: op = " + "; break;
                case TokenType.Minus: op = " - "; break;
            }
            return "(" + Left.ToString() + op + Right.ToString() + ")";
        }

    }
}
