using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI.Parser
{
    public class EvalVisitor : NodeVisitor
    {
        readonly Func<string, double?> _variables;

        public EvalVisitor( IDictionary<string,double> d )
        {
            _variables = name =>
            {
                double v;
                return d.TryGetValue( name, out v ) ? v : (double?)null;
            };
        }

        public EvalVisitor( Func<string,double?> variables = null )
        {
            //_variables = variables ?? (s => null);
            _variables = variables;
        }

        public override Node Visit( IfNode n )
        {
            var c = VisitNode( n.Condition );
            ConstantNode cN = c as ConstantNode;
            if( cN != null )
            {
                return cN.Value >= 0 ? VisitNode( n.WhenTrue ) : VisitNode( n.WhenFalse );
            }
            var t = VisitNode( n.WhenTrue );
            var f = VisitNode( n.WhenFalse );
            return c != n.Condition || t != n.WhenTrue || f != n.WhenFalse
                    ? new IfNode( c, t, f )
                    : n;
        }

        public override Node Visit( VariableNode n )
        {
            if( _variables != null )
            {
                double? v =_variables( n.Name );
                if( v.HasValue ) return new ConstantNode( v.Value );
            }
            return n;
        }

        public override Node Visit( BinaryNode n )
        {
            var left = VisitNode( n.Left );
            var right = VisitNode( n.Right );

            var cLeft = left as ConstantNode;
            var cRight = right as ConstantNode;
            if( cLeft != null && cRight != null )
            {
                switch( n.OperatorType )
                {
                    case TokenType.Mult: return new ConstantNode( cLeft.Value * cRight.Value ); 
                    case TokenType.Div: return new ConstantNode( cLeft.Value / cRight.Value );
                    case TokenType.Plus: return new ConstantNode( cLeft.Value + cRight.Value );
                    case TokenType.Minus: return new ConstantNode( cLeft.Value - cRight.Value );
                }
            }
            return left != n.Left || right != n.Right
                    ? new BinaryNode( n.OperatorType, left, right )
                    : n;
        }

        public override Node Visit( UnaryNode n )
        {
            var right = VisitNode( n.Right );
            var cRight = right as ConstantNode;
            if( cRight != null ) return new ConstantNode( -cRight.Value );
            return right != n.Right ? new UnaryNode( n.OperatorType, right ) : n;
        }


    }
}
