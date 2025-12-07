using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.SeedData
{
    public class RatingSeed
    {
        public static void SeedRatings(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Rating>().HasData(
                new Rating { RatingId = 1, RatingName = "ممتاز", ScoreValue = 5 },
                new Rating { RatingId = 2, RatingName = "جيد جداً", ScoreValue = 4 },
                new Rating { RatingId = 3, RatingName = "جيد", ScoreValue = 3 },
                new Rating { RatingId = 4, RatingName = "مقبول", ScoreValue = 2 },
                new Rating { RatingId = 5, RatingName = "ضعيف", ScoreValue = 1 },
            // Ratings for 0-10 scale (for criterion 20)
            new Rating { RatingId = 6, RatingName = "0", ScoreValue = 0 },
            new Rating { RatingId = 7, RatingName = "1", ScoreValue = 1 },
            new Rating { RatingId = 8, RatingName = "2", ScoreValue = 2 },
            new Rating { RatingId = 9, RatingName = "3", ScoreValue = 3 },
            new Rating { RatingId = 10, RatingName = "4", ScoreValue = 4 },
            new Rating { RatingId = 11, RatingName = "5", ScoreValue = 5 },
            new Rating { RatingId = 12, RatingName = "6", ScoreValue = 6 },
            new Rating { RatingId = 13, RatingName = "7", ScoreValue = 7 },
            new Rating { RatingId = 14, RatingName = "8", ScoreValue = 8 },
            new Rating { RatingId = 15, RatingName = "9", ScoreValue = 9 },
            new Rating { RatingId = 16, RatingName = "10", ScoreValue = 10 }
            );
        }
    }
}
