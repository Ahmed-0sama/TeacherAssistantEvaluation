using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.SeedData
{
    public class SeedEvaluationStatus
    {
        public static void SeedCriteria(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EvaluationStatus>().HasData(
            new EvaluationStatus
            {
                StatusId = 1,
                StatusName = "Draft",
                StatusDescription = "NUTA has not submitted their evaluation yet"
            },
                new EvaluationStatus
                {
                    StatusId = 2,
                    StatusName = "Submitted",
                    StatusDescription = "Evaluation submitted by TA and awaiting HOD review"
                },
                new EvaluationStatus
                {
                    StatusId = 3,
                    StatusName = "ReviewedByHOD",
                    StatusDescription = "HOD has reviewed and provided comments"
                },
                new EvaluationStatus
                {
                    StatusId = 4,
                    StatusName = "ReturnedByHOD",
                    StatusDescription = "HOD returned the evaluation to the TA for corrections"
                },
                new EvaluationStatus
                {
                    StatusId = 5,
                    StatusName = "ReviewedByDean",
                    StatusDescription = "Dean has reviewed the evaluation"
                },
                new EvaluationStatus
                {
                    StatusId = 6,
                    StatusName = "ReturnedByDean",
                    StatusDescription = "Dean returned the evaluation for corrections"
                },
                new EvaluationStatus
                {
                    StatusId = 7,
                    StatusName = "Approved",
                    StatusDescription = "Evaluation fully approved and completed"
                }
            ); 
        }
    }
}

