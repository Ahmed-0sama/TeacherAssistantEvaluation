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
                new Rating { RatingId = 5, RatingName = "ضعيف", ScoreValue = 1 }
            );
        }
    }
}
