using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algo.Tests
{
    internal class TestData
    {
        public User[] Users { get; set; }

        public Movie[] Movies { get; set; }

        public TestData()
        {
            Users = new User[6];
            Movies = new Movie[6];

            var user1 = new User(1, "Olivier");
            var user2 = new User(2, "Paul");
            var user3 = new User(3, "Anthony");
            var user4 = new User(4, "Thibault");
            var user5 = new User(5, "Elizabeth");
            var user6 = new User(6, "Johnny");

            var movie1 = new Movie(1, "Star wars");
            var movie2 = new Movie(2, "L'infirmière…");
            var movie3 = new Movie(3, "Matrix");
            var movie4 = new Movie(4, "L'aile ou la Cuisse");
            var movie5 = new Movie(5, "OSS 117");
            var movie6 = new Movie(6, "La classe américaine");

            user1.Ratings.Add(movie1, 4);
            user1.Ratings.Add(movie2, 3);
            user1.Ratings.Add(movie3, 5);
            user1.Ratings.Add(movie4, 2);
            user1.Ratings.Add(movie5, 4);
            user1.Ratings.Add(movie6, 4);

            user2.Ratings.Add(movie1, 5);
            user2.Ratings.Add(movie2, 4);
            user2.Ratings.Add(movie3, 3);
            user2.Ratings.Add(movie4, 2);
            user2.Ratings.Add(movie5, 1);
            user2.Ratings.Add(movie6, 5);

            user3.Ratings.Add(movie1, 1);
            user3.Ratings.Add(movie2, 2);
            user3.Ratings.Add(movie3, 3);
            user3.Ratings.Add(movie4, 3);
            user3.Ratings.Add(movie5, 2);
            user3.Ratings.Add(movie6, 1);

            user4.Ratings.Add(movie1, 3);
            user4.Ratings.Add(movie2, 4);
            user4.Ratings.Add(movie3, 5);
            user4.Ratings.Add(movie4, 5);
            user4.Ratings.Add(movie5, 4);
            user4.Ratings.Add(movie6, 3);

            user5.Ratings.Add(movie1, 4);
            user5.Ratings.Add(movie2, 5);
            user5.Ratings.Add(movie3, 5);
            user5.Ratings.Add(movie4, 3);
            user5.Ratings.Add(movie5, 3);
            user5.Ratings.Add(movie6, 2);

            user6.Ratings.Add(movie1, 1);
            user6.Ratings.Add(movie2, 2);
            user6.Ratings.Add(movie3, 3);
            user6.Ratings.Add(movie4, 4);
            user6.Ratings.Add(movie5, 5);
            user6.Ratings.Add(movie6, 1);

            Users[0] = user1;
            Users[1] = user2;
            Users[2] = user3;
            Users[3] = user4;
            Users[4] = user5;
            Users[5] = user6;

            Movies[0] = movie1;
            Movies[1] = movie2;
            Movies[2] = movie3;
            Movies[3] = movie4;
            Movies[4] = movie5;
            Movies[5] = movie6;
        }
    }
}
