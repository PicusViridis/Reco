using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace ITI.Parser.Tests
{
    [TestFixture]
    public class TokenizerTests
    {
        [TestCase( "3" )]
        [TestCase( "3 8" )]
        [TestCase( "3 8 12 8787 3712" )]
        public void list_of_numbers( string toParse )
        {
            var t = new StringTokenizer( toParse );
            List<double> values = new List<double>();
            t.GetNextToken();
            while( t.CurrentToken != TokenType.EndOfInput )
            {
                Assert.That( t.CurrentToken == TokenType.Number );
                double v;
                Assert.That( t.MatchDouble( out v ) );
                values.Add( v );
            }
            CollectionAssert.AreEqual( toParse.Split( ' ' ).Select( s => Double.Parse( s ) ).ToList(), values );
        }


    }
}
