using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI.Parser
{
    public class VariableNode : Node
    {
        public VariableNode( string name )
        {
            Name = name;
        }

        public string Name { get; }

        internal override Node Accept( NodeVisitor visitor ) => visitor.Visit( this );

        public override string ToString() => Name;
    }
}
