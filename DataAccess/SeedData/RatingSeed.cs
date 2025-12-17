using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.SeedData
{
    public static class RatingSeed
    {
        public static void SeedRatings(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Rating>().HasData(

                // ================================
                // 1. Teaching Activities (1–5)
                // 0, 0.5, 1, 1.5, 2
                // ================================
                new Rating { RatingId = 1, RatingName = "TA_ضعيف", ScoreValue = 0 },
                new Rating { RatingId = 2, RatingName = "TA_مقبول", ScoreValue = 1 }, // 0.5
                new Rating { RatingId = 3, RatingName = "TA_جيد", ScoreValue = 2 }, // 1.0
                new Rating { RatingId = 4, RatingName = "TA_جيد جداً", ScoreValue = 3 }, // 1.5
                new Rating { RatingId = 5, RatingName = "TA_ممتاز", ScoreValue = 4 }, // 2.0

                // ================================
                // 2. Student Activities (6–10)
                // 0, 1, 2, 3, 3.33 (mapped via code)
                // ================================
                new Rating { RatingId = 6, RatingName = "SA_ضعيف", ScoreValue = 0 },
                new Rating { RatingId = 7, RatingName = "SA_مقبول", ScoreValue = 1 },
                new Rating { RatingId = 8, RatingName = "SA_جيد", ScoreValue = 2 },
                new Rating { RatingId = 9, RatingName = "SA_جيد جداً", ScoreValue = 3 },
                new Rating { RatingId = 10, RatingName = "SA_ممتاز", ScoreValue = 4 }, // mapped to 3.33

                // ================================
                // 3. Personal Traits (11–15)
                // 0, 0.5, 1, 1.5, 2
                // ================================
                new Rating { RatingId = 11, RatingName = "PT_ضعيف", ScoreValue = 0 },
                new Rating { RatingId = 12, RatingName = "PT_مقبول", ScoreValue = 1 },
                new Rating { RatingId = 13, RatingName = "PT_جيد", ScoreValue = 2 },
                new Rating { RatingId = 14, RatingName = "PT_جيد جداً", ScoreValue = 3 },
                new Rating { RatingId = 15, RatingName = "PT_ممتاز", ScoreValue = 4 },

                // ================================
                // 4. Administrative Committee (Criterion 20)
                // 0–10 direct score
                // ================================
                new Rating { RatingId = 16, RatingName = "Admin_0", ScoreValue = 0 },
                new Rating { RatingId = 17, RatingName = "Admin_1", ScoreValue = 1 },
                new Rating { RatingId = 18, RatingName = "Admin_2", ScoreValue = 2 },
                new Rating { RatingId = 19, RatingName = "Admin_3", ScoreValue = 3 },
                new Rating { RatingId = 20, RatingName = "Admin_4", ScoreValue = 4 },
                new Rating { RatingId = 21, RatingName = "Admin_5", ScoreValue = 5 },
                new Rating { RatingId = 22, RatingName = "Admin_6", ScoreValue = 6 },
                new Rating { RatingId = 23, RatingName = "Admin_7", ScoreValue = 7 },
                new Rating { RatingId = 24, RatingName = "Admin_8", ScoreValue = 8 },
                new Rating { RatingId = 25, RatingName = "Admin_9", ScoreValue = 9 },
                new Rating { RatingId = 26, RatingName = "Admin_10", ScoreValue = 10 }
            );
        }
    }
}