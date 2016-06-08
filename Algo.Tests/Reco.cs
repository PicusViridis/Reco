using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.IO;
using Algo.Reco;

namespace Algo.Tests
{
    [TestFixture]
    public class Reco
    {
        static string _badDataPath = @"C:\Intech\2016-1\S9-10\Repo\ThirdParty\MovieData\MovieLens\";
        static string _goodDataPath = @"C:\Intech\2016-1\S9-10\Repo\ThirdParty\MovieData\";

        [Test]
        public void CorrectData()
        {
            Dictionary<int, Movie> firstMovies;
            Dictionary<int, List<Movie>> duplicateMovies;
            Movie.ReadMovies( Path.Combine( _badDataPath, "movies.dat" ), out firstMovies, out duplicateMovies );
            int idMovieMin = firstMovies.Keys.Min();
            int idMovieMax = firstMovies.Keys.Max();
            Console.WriteLine( "{3} Movies from {0} to {1}, {2} duplicates.", idMovieMin, idMovieMax, duplicateMovies.Count, firstMovies.Count );

            Dictionary<int, User> firstUsers;
            Dictionary<int, List<User>> duplicateUsers;
            User.ReadUsers( Path.Combine( _badDataPath, "users.dat" ), out firstUsers, out duplicateUsers );
            int idUserMin = firstUsers.Keys.Min();
            int idUserMax = firstUsers.Keys.Max();
            Console.WriteLine( "{3} Users from {0} to {1}, {2} duplicates.", idUserMin, idUserMax, duplicateUsers.Count, firstUsers.Count );

            Dictionary<int, string> badLines;
            int nbRating = User.ReadRatings( Path.Combine( _badDataPath, "ratings.dat" ), firstUsers, firstMovies, out badLines );
            Console.WriteLine( "{0} Ratings: {1} bad lines.", nbRating, badLines.Count );

            Directory.CreateDirectory( _goodDataPath );
            // Saves Movies
            using( TextWriter w = File.CreateText( Path.Combine( _goodDataPath, "movies.dat" ) ) )
            {
                int idMovie = 0;
                foreach( Movie m in firstMovies.Values )
                {
                    m.MovieID = ++idMovie;
                    w.WriteLine( "{0}::{1}::{2}", m.MovieID, m.Title, String.Join( "|", m.Categories ) );
                }
            }

            // Saves Users
            string[] occupations = new string[]{
                "other",
                "academic/educator",
                "artist",
                "clerical/admin",
                "college/grad student",
                "customer service",
                "doctor/health care",
                "executive/managerial",
                "farmer",
                "homemaker",
                "K-12 student",
                "lawyer",
                "programmer",
                "retired",
                "sales/marketing",
                "scientist",
                "self-employed",
                "technician/engineer",
                "tradesman/craftsman",
                "unemployed",
                "writer" };
            using( TextWriter w = File.CreateText( Path.Combine( _goodDataPath, "users.dat" ) ) )
            {
                int idUser = 0;
                foreach( User u in firstUsers.Values )
                {
                    u.UserID = ++idUser;
                    string occupation;
                    int idOccupation;
                    if( int.TryParse( u.Occupation, out idOccupation )
                        && idOccupation >= 0
                        && idOccupation < occupations.Length )
                    {
                        occupation = occupations[idOccupation];
                    }
                    else occupation = occupations[0];
                    w.WriteLine( "{0}::{1}::{2}::{3}::{4}", u.UserID, u.Male ? 'M' : 'F', u.Age, occupation, "US-" + u.ZipCode );
                }
            }
            // Saves Rating
            using( TextWriter w = File.CreateText( Path.Combine( _goodDataPath, "ratings.dat" ) ) )
            {
                foreach( User u in firstUsers.Values )
                {
                    foreach( var r in u.Ratings )
                    {
                        w.WriteLine( "{0}::{1}::{2}", u.UserID, r.Key.MovieID, r.Value );
                    }
                }
            }
        }

        [Test]
        public void ReadMovieData()
        {
            RecoContext c = new RecoContext();
            c.LoadFrom( _goodDataPath );
            for( int i = 0; i < c.Users.Length; ++i )
                Assert.That( c.Users[i].UserID, Is.EqualTo( i + 1 ) );
            for( int i = 0; i < c.Movies.Length; ++i )
                Assert.That( c.Movies[i].MovieID, Is.EqualTo( i + 1 ) );
        }

        [Test]
        public void computing_distance_between_users()
        {
            RecoContext c = new RecoContext();
            c.LoadFrom( _goodDataPath );
            double d = c.DistanceNorm2( c.Users[13], c.Users[89] );

        }

        [Test]
        public void testing_best_keeper()
        {
            {
                var all = new[] { 10, 3, 5, 7, 9, 78, 12 };
                BestKeeper<int> k = new BestKeeper<int>( 1, ( a, b ) => b - a );
                foreach( var i in all ) k.AddCandidate( i );
                Assert.That( k.Count, Is.EqualTo( 1 ) );
                CollectionAssert.AreEqual( new[] { 78 }, k );
            }
            {
                var all = new[] { 10, 3, 5, 7, 9, 78, 12 };
                BestKeeper<int> k = new BestKeeper<int>( 4, ( a, b ) => b - a );
                foreach( var i in all ) k.AddCandidate( i );
                Assert.That( k.Count, Is.EqualTo( 4 ) );
                CollectionAssert.AreEqual( new[] { 78, 12, 10, 9 }, k );
            }
            {
                var all = new[] { 10, 3, 52 };
                BestKeeper<int> k = new BestKeeper<int>( 4, ( a, b ) => b - a );
                foreach( var i in all ) k.AddCandidate( i );
                Assert.That( k.Count, Is.EqualTo( 3 ) );
                CollectionAssert.AreEqual( new[] { 52, 10, 3 }, k );
            }
            {
                var all = new[] { 10, 3, 52, 100, 6, 1 };
                BestKeeper<int> k = new BestKeeper<int>( 4, ( a, b ) => b - a );
                foreach( var i in all ) k.AddCandidate( i );
                Assert.That( k.Count, Is.EqualTo( 4 ) );
                CollectionAssert.AreEqual( new[] { 100, 52, 10, 6 }, k );
            }
        }

        [Test]
        public void test_movies_similarity()
        {
            var data = new TestData();
            var context = new RecoContext();

            Assert.That(context.SimilarityPearson(data.Movies[0], data.Movies[0], data.Users), Is.EqualTo(1.000).Within(0.0005));
            Assert.That(context.SimilarityPearson(data.Movies[0], data.Movies[1], data.Users), Is.EqualTo(0.790).Within(0.0005));
            Assert.That(context.SimilarityPearson(data.Movies[0], data.Movies[2], data.Users), Is.EqualTo(0.436).Within(0.0005));
            Assert.That(context.SimilarityPearson(data.Movies[0], data.Movies[3], data.Users), Is.EqualTo(-0.511).Within(0.0005));
            Assert.That(context.SimilarityPearson(data.Movies[0], data.Movies[4], data.Users), Is.EqualTo(-0.406).Within(0.0005));
            Assert.That(context.SimilarityPearson(data.Movies[0], data.Movies[5], data.Users), Is.EqualTo(0.878).Within(0.0005));

            Assert.That(context.SimilarityPearson(data.Movies[1], data.Movies[1], data.Users), Is.EqualTo(1.000).Within(0.0005));
            Assert.That(context.SimilarityPearson(data.Movies[1], data.Movies[2], data.Users), Is.EqualTo(0.603).Within(0.0005));
            Assert.That(context.SimilarityPearson(data.Movies[1], data.Movies[3], data.Users), Is.EqualTo(-0.047).Within(0.0005));
            Assert.That(context.SimilarityPearson(data.Movies[1], data.Movies[4], data.Users), Is.EqualTo(-0.262).Within(0.0005));
            Assert.That(context.SimilarityPearson(data.Movies[1], data.Movies[5], data.Users), Is.EqualTo(0.472).Within(0.0005));

            Assert.That(context.SimilarityPearson(data.Movies[2], data.Movies[2], data.Users), Is.EqualTo(1.000).Within(0.0005));
            Assert.That(context.SimilarityPearson(data.Movies[2], data.Movies[3], data.Users), Is.EqualTo(0.156).Within(0.0005));
            Assert.That(context.SimilarityPearson(data.Movies[2], data.Movies[4], data.Users), Is.EqualTo(0.372).Within(0.0005));
            Assert.That(context.SimilarityPearson(data.Movies[2], data.Movies[5], data.Users), Is.EqualTo(0.224).Within(0.0005));

            Assert.That(context.SimilarityPearson(data.Movies[3], data.Movies[3], data.Users), Is.EqualTo(1.000).Within(0.0005));
            Assert.That(context.SimilarityPearson(data.Movies[3], data.Movies[4], data.Users), Is.EqualTo(0.562).Within(0.0005));
            Assert.That(context.SimilarityPearson(data.Movies[3], data.Movies[5], data.Users), Is.EqualTo(-0.489).Within(0.0005));

            Assert.That(context.SimilarityPearson(data.Movies[4], data.Movies[4], data.Users), Is.EqualTo(1.000).Within(0.0005));
            Assert.That(context.SimilarityPearson(data.Movies[4], data.Movies[5], data.Users), Is.EqualTo(-0.388).Within(0.0005));

            Assert.That(context.SimilarityPearson(data.Movies[5], data.Movies[5], data.Users), Is.EqualTo(1.000).Within(0.0005));
        }
    }
}