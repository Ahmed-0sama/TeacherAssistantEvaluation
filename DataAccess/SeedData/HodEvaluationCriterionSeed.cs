using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.SeedData
{
    public class HodEvaluationCriterionSeed
    {
        public static void SeedCriteria(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<HodevaluationCriterion>().HasData(
                // Direct Teaching Activities (10%)
                new HodevaluationCriterion { CriterionId = 1, CriterionName = "إعداد مذكرات للجزء العملي/تدريبات", CriterionType = "DirectTeaching" },
                new HodevaluationCriterion { CriterionId = 2, CriterionName = "إعداد مساعدات تعليمية وتدريسية جديدة", CriterionType = "DirectTeaching" },
                new HodevaluationCriterion { CriterionId = 3, CriterionName = "المساعدة في إعداد التجارب العملية/التمارين", CriterionType = "DirectTeaching" },
                new HodevaluationCriterion { CriterionId = 4, CriterionName = "المشاركة في تنظيم وإدارة دورات تدريسية/مؤتمرات", CriterionType = "DirectTeaching" },
                new HodevaluationCriterion { CriterionId = 5, CriterionName = "أي نشاط تعليمي آخر مكلف به", CriterionType = "DirectTeaching" },

                // Administrative Activities
                new HodevaluationCriterion { CriterionId = 6, CriterionName = "لجنة الإرشاد الأكاديمي", CriterionType = "Administrative" },
                new HodevaluationCriterion { CriterionId = 7, CriterionName = "لجنة الجدولة", CriterionType = "Administrative" },
                new HodevaluationCriterion { CriterionId = 8, CriterionName = "لجنة أعمال الجودة", CriterionType = "Administrative" },
                new HodevaluationCriterion { CriterionId = 9, CriterionName = "لجنة التجهيزات المعملية", CriterionType = "Administrative" },
                new HodevaluationCriterion { CriterionId = 10, CriterionName = "لجنة تنظيم امتحانات", CriterionType = "Administrative" },
                new HodevaluationCriterion { CriterionId = 11, CriterionName = "لجان النشاط الاجتماعي أو الرياضي", CriterionType = "Administrative" },

                // Student Activities
                new HodevaluationCriterion { CriterionId = 12, CriterionName = "نشاط رياضي", CriterionType = "StudentActivities" },
                new HodevaluationCriterion { CriterionId = 13, CriterionName = "نشاط اجتماعي", CriterionType = "StudentActivities" },
                new HodevaluationCriterion { CriterionId = 14, CriterionName = "نشاط ثقافي", CriterionType = "StudentActivities" },

                // Personal Attributes
                new HodevaluationCriterion { CriterionId = 15, CriterionName = "التعاون والعمل الجماعي", CriterionType = "PersonalTraits" },
                new HodevaluationCriterion { CriterionId = 16, CriterionName = "الالتزام بالمواعيد", CriterionType = "PersonalTraits" },
                new HodevaluationCriterion { CriterionId = 17, CriterionName = "المظهر العام", CriterionType = "PersonalTraits" },
                new HodevaluationCriterion { CriterionId = 18, CriterionName = "المبادرة وتحمل المسؤولية", CriterionType = "PersonalTraits" },
                new HodevaluationCriterion { CriterionId = 19, CriterionName = "إدارة الوقت", CriterionType = "PersonalTraits" }
            );
        }
    }
}
