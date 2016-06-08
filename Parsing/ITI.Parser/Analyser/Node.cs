using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI.Parser
{
    public abstract class Node
    {

        internal abstract Node Accept( NodeVisitor visitor );
    }
}
