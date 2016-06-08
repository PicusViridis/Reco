using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI.Parser
{
    public interface ITokenizer
    {
        TokenType CurrentToken { get; }

        TokenType GetNextToken();

        /// <summary>
        /// Checks that the <see cref="CurrentToken"/> is equal
        /// to <paramref name="t"/> and forwards the head on success.
        /// </summary>
        /// <param name="t">Type of the expected token.</param>
        /// <returns>True if token is of the given type.</returns>
        bool Match( TokenType t );

        bool MatchInteger( int expected );

        bool MatchInteger( out int value );

        bool MatchDouble( out double value );

        bool MatchIdentifier( out string identifier );

    }
}
