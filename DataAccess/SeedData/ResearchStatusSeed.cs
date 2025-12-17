using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.SeedData
{
        public static class ResearchStatusSeed
        {
            public static void Seed(ModelBuilder modelBuilder)
            {
                modelBuilder.Entity<ResearchStatus>().HasData(
                    new ResearchStatus
                    {
                        StatusId = 1,
                        StatusKey = "P",
                        StatusName = "Published"
                    },
            new ResearchStatus
            {
                StatusId = 2,
                StatusKey = "S",
                StatusName = "Submitted"
            },
            new ResearchStatus
            {
                StatusId = 3,
                StatusKey = "R",
                StatusName = "Rejected"
            }
                );
            }
        }
    }
