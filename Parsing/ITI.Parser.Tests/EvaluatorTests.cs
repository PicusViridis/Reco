using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace ITI.Parser.Tests
{
    [TestFixture]
    public class EvaluatorTests
    {

        [TestCase( "3", 3.0 )]
        [TestCase( "3+8", 3.0+8.0)]
        [TestCase( "3*7/2", 3.0*7.0/2.0 )]
        [TestCase( "3712/(45+98)*12*(58/12)", 3712.0 / (45.0 + 98.0) * 12.0 *( 58.0 / 12.0 ) )]
        [TestCase( "37*(12+4)/(45+98/(4+5+68-8-7+10))*1+(41/9*7+6-72)+2*(5+8/1-2)", 37.0 * (12.0 + 4.0) / (45.0 + 98.0 / (4.0 + 5.0 + 68.0 - 8.0 - 7.0 + 10.0)) * 1.0 + (41.0 / 9.0 * 7.0 + 6.0 - 72) + 2.0 * (5.0 + 8.0 / 1.0 - 2.0) )]
        [TestCase( "37 ? 8 : 5", 8.0 )]
        [TestCase( "3+7 ? 1+8 : 4+1", 9.0 )]
        [TestCase( "3-7 ? 1+8 : (-4+1 ? -8 : -24 )", -24.0 )]
        public void test_evaluation( string text, double expectedResult )
        {
            Node n = new Analyser().Analyse( new StringTokenizer( text ) );

            EvalVisitor visitor = new EvalVisitor();
            var c = (ConstantNode)visitor.VisitNode( n );
            Assert.That( c.Value, Is.EqualTo( expectedResult ) );
        }

        [TestCase( "3*X + 18", 3.0 * 5 + 18 )]
        [TestCase( "3*X + (Y*5/X+26540)", 3.0 * 5 + (7 * 5 / 5 + 26540) )]
        public void with_variables( string text, double expectedResult )
        {
            Dictionary<string, double> vars = new Dictionary<string, double>();
            vars.Add( "X", 5 );
            vars.Add( "Y", 7 );

            var visitor = new EvalVisitor( vars );

            Node n = new Analyser().Analyse( new StringTokenizer( text ) );
            var c = (ConstantNode)visitor.VisitNode( n );

            Assert.That( c.Value, Is.EqualTo( expectedResult ) );
        }


        [TestCase( "3*X + 18 + Toto", "(33 + Toto)")]
        [TestCase( "10*X / Toto", "(50 / Toto)" )]
        public void partial_evaluation_with_variables( string text, string simplified )
        {
            var visitor = new EvalVisitor( name => name == "X" ? 5.0 : (double?)null );

            Node n = new Analyser().Analyse( new StringTokenizer( text ) );
            var s = visitor.VisitNode( n );

            Assert.That( s.ToString(), Is.EqualTo( simplified ) );
        }
    }
}
