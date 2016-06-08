using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI.Parser
{
    [Flags]
    public enum TokenType
    {
        None = 0,
        IsAddOperator = 1,
        Plus = IsAddOperator,
        Minus = IsAddOperator + 2,
        IsMultOperator = 4,
        Mult = IsMultOperator,
        Div = IsMultOperator + 2,
        Number = 8,
        OpenPar = 16,
        ClosePar = 32,
        EndOfInput = 64,
        QuestionMark = 128,
        Colon = 256,
        Identifier = 512,
        Error = 1024
    }
}
