using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI.Parser
{
    public abstract class NodeVisitor
    {
        public Node VisitNode( Node n )
        {
            return n.Accept( this );
        }

        public virtual Node Visit( BinaryNode n )
        {
            var left = VisitNode( n.Left );
            var right = VisitNode( n.Right );
            return left != n.Left || right != n.Right
                    ? new BinaryNode( n.OperatorType, left, right )
                    : n;
        }

        public virtual Node Visit( ConstantNode n ) => n;

        public virtual Node Visit( VariableNode n ) => n;

        public virtual Node Visit( ErrorNode n ) => n;

        public virtual Node Visit( IfNode n )
        {
            var c = VisitNode( n.Condition );
            var t = VisitNode( n.WhenTrue );
            var f = VisitNode( n.WhenFalse );
            return c != n.Condition || t != n.WhenTrue || f != n.WhenFalse
                    ? new IfNode( c, t, f )
                    : n;
        }

        public virtual Node Visit( UnaryNode n )
        {
            var r = VisitNode( n.Right );
            return r != n.Right ? new UnaryNode( n.OperatorType, r ) : n;
        }


    }
}
