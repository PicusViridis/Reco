using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI.Parser
{
    public class StringTokenizer : ITokenizer
    {
        string _toParse;
        int _pos;
        int _maxPos;
        TokenType _curToken;
        double _doubleValue;
        string _identifierValue;
        StringBuilder _buffer;

        public StringTokenizer( string s )
            : this( s, 0, s.Length )
        {
        }

        public StringTokenizer( string s, int startIndex )
            : this( s, startIndex, s.Length )
        {
        }

        public StringTokenizer( string s, int startIndex, int count )
        {
            _curToken = TokenType.None;
            _toParse = s;
            _pos = startIndex;
            _maxPos = startIndex + count;
            _buffer = new StringBuilder();
        }

        #region Input reader

        char Peek()
        {
            Debug.Assert( !IsEnd );
            return _toParse[_pos];
        }

        char Read()
        {
            Debug.Assert( !IsEnd );
            return _toParse[_pos++];
        }

        void Forward()
        {
            Debug.Assert( !IsEnd );
            ++_pos;
        }

        bool IsEnd
        {
            get { return _pos >= _maxPos; }
        }

        #endregion

        public TokenType CurrentToken
        {
            get { return _curToken; }
        }

        public bool Match( TokenType t )
        {
            if( _curToken == t )
            {
                GetNextToken();
                return true;
            }
            return false;
        }

        public bool MatchDouble( out double value )
        {
            value = _doubleValue;
            if( _curToken == TokenType.Number )
            {
                GetNextToken();
                return true;
            }
            return false;
        }

        public bool MatchInteger( int expectedValue )
        {
            if( _curToken == TokenType.Number
                && _doubleValue < Int32.MaxValue
                && (int)_doubleValue == expectedValue )
            {
                GetNextToken();
                return true;
            }
            return false;
        }

        public bool MatchInteger( out int value )
        {
            if( _curToken == TokenType.Number
                && _doubleValue < Int32.MaxValue )
            {
                value = (int)_doubleValue;
                GetNextToken();
                return true;
            }
            value = 0;
            return false;
        }

        public bool MatchIdentifier( string identifier )
        {
            if( _curToken == TokenType.Identifier && _identifierValue == identifier)
            {
                GetNextToken();
                return true;
            }
            return false;
        }

        public bool MatchIdentifier( out string identifier )
        {
            if( _curToken == TokenType.Identifier )
            {
                identifier = _identifierValue;
                GetNextToken();
                return true;
            }
            identifier = null;
            return false;
        }

        public TokenType GetNextToken()
        {
            if( IsEnd ) return _curToken = TokenType.EndOfInput;
            char c = Read();
            while( Char.IsWhiteSpace( c ) )
            {
                if( IsEnd ) return _curToken = TokenType.EndOfInput;
                c = Read();
            }
            switch( c )
            {
                case '+': _curToken = TokenType.Plus; break;
                case '-': _curToken = TokenType.Minus; break;
                case '?': _curToken = TokenType.QuestionMark; break;
                case ':': _curToken = TokenType.Colon; break;
                case '*': _curToken = TokenType.Mult; break;
                case '/': _curToken = TokenType.Div; break;
                case '(': _curToken = TokenType.OpenPar; break;
                case ')': _curToken = TokenType.ClosePar; break;
                default:
                    {
                        if( char.IsDigit( c ) )
                        {
                            _curToken = TokenType.Number;
                            double val = (int)(c - '0');
                            while( !IsEnd && Char.IsDigit( c = Peek() ) )
                            {
                                val = val * 10 + (int)(c - '0');
                                Forward();
                            }
                            _doubleValue = val;
                        }
                        else if( char.IsLetter( c ) || c == '_' )
                        {
                            _curToken = TokenType.Identifier;
                            _buffer.Clear();
                            _buffer.Append( c );
                            while( !IsEnd && (char.IsLetter( c = Peek() ) || c == '_') )
                            {
                                _buffer.Append( c );
                                Forward();
                            }
                            _identifierValue = _buffer.ToString();
                        }
                        else _curToken = TokenType.Error;
                        break;
                    }
            }
            return _curToken;
        }

    }
}
  
