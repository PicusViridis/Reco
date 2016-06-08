using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI.Parser
{
    public class IfNode : Node
    {
        public IfNode( Node condition, Node whenTrue, Node whenFalse  )
        {
            Condition = condition;
            WhenTrue = whenTrue;
            WhenFalse = whenFalse;
        }

        public Node Condition { get; private set; }
        public Node WhenTrue { get; private set; }
        public Node WhenFalse { get; private set; }

        [DebuggerStepThrough]
        internal override Node Accept( NodeVisitor visitor ) => visitor.Visit( this );

    }
}
