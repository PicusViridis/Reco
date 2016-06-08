using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Algo.Reco;

namespace Algo
{
    public class RecoContext
    {
        public User[] Users { get; private set; }
        public Movie[] Movies { get; private set; }

        public RecoContext()
        {
        }

        public void LoadFrom(string folder)
        {
            Users = User.ReadUsers(Path.Combine(folder, "users.dat"));
            Movies = Movie.ReadMovies(Path.Combine(folder, "movies.dat"));
            User.ReadRatings(Users, Movies, Path.Combine(folder, "ratings.dat"));
        }

        public double DistanceNorm2(User u1, User u2)
        {
            if (u1 == u2) return 0;

            var common = from ra in u1.Ratings
                         join rb in u2.Ratings on ra.Key.MovieID equals rb.Key.MovieID
                         select new
                         {
                             Movie = ra.Key,
                             R1 = ra.Value,
                             R2 = rb.Value
                         };

            if (common.Any())
            {
                return Math.Sqrt(common.Sum(r => Math.Pow(r.R1 - r.R2, 2)));
            }

            return double.MaxValue;
        }

        public double DistanceNorm2(User u1, User u2, IEnumerable<Movie> common)
        {
            if (u1 == u2) return 0;

            if (common.Any())
            {
                return Math.Sqrt(common.Sum(m => Math.Pow(u1.Ratings[m] - u2.Ratings[m], 2)));
            }

            return double.MaxValue;
        }

        public double SimilarityNorm2(User u1, User u2, IEnumerable<Movie> common)
        {
            return 1 / (1 + DistanceNorm2(u1, u2, common));
        }

        public double SimilarityPearson(User u1, User u2)
        {
            double sumProd = 0;
            double sumSquare1 = 0;
            double sumSquare2 = 0;
            double sum1 = 0;
            double sum2 = 0;
            int count = 0;

            IEnumerable<Movie> common = u1.Ratings.Keys.Intersect(u2.Ratings.Keys);
            foreach (Movie m in common)
            {
                count++;
                int r1 = u1.Ratings[m];
                int r2 = u2.Ratings[m];
                sum1 += r1;
                sum2 += r2;
                sumSquare1 += r1 * r1;
                sumSquare2 += r2 * r2;
                sumProd += r1 * r2;
            }

            if (count == 0) return 0;
            if (count == 1) return SimilarityNorm2(u1, u2, common);

            double numerator = sumProd - ((sum1 * sum2) / count);
            double denominator1 = sumSquare1 - ((sum1 * sum1) / count);
            double denominator2 = sumSquare2 - ((sum2 * sum2) / count);
            double denominator = Math.Sqrt(denominator1 * denominator2);

            if (denominator <= 2 * double.Epsilon) return 1;
            return numerator / denominator;
        }


        public IReadOnlyCollection<SimilarUser> GetSimilarUsers(User u, int count = 100)
        {
            BestKeeper<SimilarUser> best = new BestKeeper<SimilarUser>(count,
                (u1, u2) => Math.Sign(u1.Similarity - u2.Similarity));

            foreach (var other in Users)
            {
                if (other != u)
                {
                    best.AddCandidate(new SimilarUser(other, SimilarityPearson(u, other)));
                }
            }
            return best;
        }

        public IEnumerable<Movie> GetUserBasedRecommendationsFor(User u)
        {
            var similars = GetSimilarUsers(u);
            return similars.SelectMany(
                        su => su.User.Ratings
                                        .Where(mr => !u.Ratings.ContainsKey(mr.Key))
                                        .Select(mr =>
                                                   new
                                                   {
                                                       Movie = mr.Key,
                                                       S = su.Similarity,
                                                       R = mr.Value
                                                   }))
                                             .Select(r => new { Movie = r.Movie, W = r.S * r.R })
                                             .GroupBy(r => r.Movie)
                                             .Select(g => new { Movie = g.Key, W = g.Sum(x => x.W) })
                                             .OrderByDescending(mR => mR.W)
                                             .Select(mR => mR.Movie);
        }

        public IEnumerable<Movie> GetItemBasedRecommendationsFor(User u, User[] users)
        {
            return u.Ratings.Where(kvp => kvp.Value > 3).SelectMany(kvp => GetSimilarMovies(kvp.Key, users).Where(sm => !u.Ratings.ContainsKey(sm.Movie)).Select(sm => sm.Movie));
        }

        public IReadOnlyCollection<SimilarMovies> GetSimilarMovies(Movie m, User[] users, int count = 100)
        {
            BestKeeper<SimilarMovies> best = new BestKeeper<SimilarMovies>(count,
                (m1, m2) => Math.Sign(m1.Similarity - m2.Similarity));

            foreach (var movie in Movies)
            {
                if (movie != m)
                {
                    best.AddCandidate(new SimilarMovies(movie, SimilarityPearson(m, movie, users)));
                }
            }
            return best;
        }

        public double SimilarityPearson(Movie m1, Movie m2, User[] users)
        {
            double sumProd = 0;
            double sumSquare1 = 0;
            double sumSquare2 = 0;
            double sum1 = 0;
            double sum2 = 0;
            int count = 0;

            List<User> common = new List<User>();

            foreach (User u in users)
            {
                if (u.Ratings.ContainsKey(m1) && u.Ratings.ContainsKey(m2))
                {
                    count++;
                    int r1 = u.Ratings[m1];
                    int r2 = u.Ratings[m2];
                    sum1 += r1;
                    sum2 += r2;
                    sumSquare1 += r1 * r1;
                    sumSquare2 += r2 * r2;
                    sumProd += r1 * r2;
                    common.Add(u);
                }
            }

            if (count == 0) return 0;
            if (count == 1) return SimilarityNorm2(m1, m2, common);

            double numerator = sumProd - ((sum1 * sum2) / count);
            double denominator1 = sumSquare1 - ((sum1 * sum1) / count);
            double denominator2 = sumSquare2 - ((sum2 * sum2) / count);
            double denominator = Math.Sqrt(denominator1 * denominator2);

            if (denominator <= 2 * double.Epsilon) return 1;
            return numerator / denominator;
        }

        private double SimilarityNorm2(Movie m1, Movie m2, List<User> common)
        {
            return 1 / (1 + DistanceNorm2(m1, m2, common));
        }

        private double DistanceNorm2(Movie m1, Movie m2, List<User> common)
        {
            if (m1 == m2) return 0;
            if (common.Any())
            {
                return Math.Sqrt(common.Sum(u => Math.Pow(u.Ratings[m1] - u.Ratings[m2], 2)));
            }
            return double.MaxValue;
        }
    }
}
