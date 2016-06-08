using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algo.Reco
{
    public struct SimilarMovies
    {
        public readonly Movie Movie;
        public readonly double Similarity;

        public SimilarMovies(Movie m, double s)
        {
            Movie = m;
            Similarity = s;
        }
    }
}
