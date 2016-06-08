using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace ITI.Parser.Tests
{
    [TestFixture]
    public class AnalyserTests
    {

        [Test]
        public void simple_factors()
        {
            var a = new Analyser();
            var node = a.Analyse( new StringTokenizer( "8*12/5*12" ) );
            Assert.That( node.ToString(), Is.EqualTo( "(((8 * 12) / 5) * 12)" ) );
        }

        [Test]
        public void factors_with_addition()
        {
            var a = new Analyser();
            var node = a.Analyse( new StringTokenizer( "8+12*12" ) );
            Assert.That( node.ToString(), Is.EqualTo( "(8 + (12 * 12))" ) );
        }
        
        [Test]
        public void factors_with_minus()
        {
            var a = new Analyser();
            var node = a.Analyse( new StringTokenizer( "8-12*12+5" ) );
            Assert.That( node.ToString(), Is.EqualTo( "((8 - (12 * 12)) + 5)" ) );
        }
        
        [Test]
        public void addition_and_factors()
        {
            var a = new Analyser();
            var node = a.Analyse( new StringTokenizer( "8*12+12" ) );
            Assert.That( node.ToString(), Is.EqualTo( "((8 * 12) + 12)" ) );
        }

        [Test]
        public void unary_minus_at_work()
        {
            var a = new Analyser();
            var node = a.Analyse( new StringTokenizer( "12*-7" ) );
            Assert.That( node.ToString(), Is.EqualTo( "(12 * -(7))" ) );
        }

        [Test]
        public void with_variables()
        {
            var a = new Analyser();
            var node = a.Analyse( new StringTokenizer( "12*-X+9" ) );
            Assert.That( node.ToString(), Is.EqualTo( "((12 * -(X)) + 9)" ) );
        }

    }
}
