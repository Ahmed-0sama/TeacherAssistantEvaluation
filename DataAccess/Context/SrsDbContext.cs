using DataAccess.Entities;
using DataAccess.SeedData;
using Microsoft.EntityFrameworkCore;


public partial class SrsDbContext : DbContext
{
    public SrsDbContext()
    {
    }

    public SrsDbContext(DbContextOptions<SrsDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AutoCalculatedScore> AutoCalculatedScores { get; set; }

    public virtual DbSet<Evaluation> Evaluations { get; set; }

    public virtual DbSet<EvaluationGradeRange> EvaluationGradeRanges { get; set; }

    public virtual DbSet<EvaluationPeriod> EvaluationPeriods { get; set; }

    public virtual DbSet<EvaluationStatus> EvaluationStatuses { get; set; }

    public virtual DbSet<GsdeanEvaluation> GsdeanEvaluations { get; set; }

    public virtual DbSet<Hodevaluation> Hodevaluations { get; set; }

    public virtual DbSet<HodevaluationCriterion> HodevaluationCriteria { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<ProfessorCourseEvaluation> ProfessorCourseEvaluations { get; set; }

    public virtual DbSet<Rating> Ratings { get; set; }

    public virtual DbSet<ReminderLog> ReminderLogs { get; set; }

    public virtual DbSet<ResearchActivity> ResearchActivities { get; set; }

    public virtual DbSet<ResearchStatus> ResearchStatuses { get; set; }

    public virtual DbSet<Tasubmission> Tasubmissions { get; set; }

    public virtual DbSet<VpgsEvaluation> VpgsEvaluations { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AutoCalculatedScore>(entity =>
        {
            entity.HasKey(e => e.ScoreId).HasName("PK_SystemScores");

            entity.ToTable("AutoCalculatedScores", "taEvaluation");

            entity.HasIndex(e => e.EvaluationId, "UQ_AutoCalc_EvaluationID").IsUnique();

            entity.Property(e => e.ScoreId).HasColumnName("ScoreID");
            entity.Property(e => e.EvaluationId).HasColumnName("EvaluationID");

            entity.HasOne(d => d.Evaluation).WithOne(p => p.AutoCalculatedScore)
                .HasForeignKey<AutoCalculatedScore>(d => d.EvaluationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AutoCalculatedScores_Evaluations");
        });

        modelBuilder.Entity<Evaluation>(entity =>
        {
            entity.HasKey(e => e.EvaluationId).HasName("PK__Evaluati__36AE68D38FC7B59C");

            entity.ToTable("Evaluations", "taEvaluation");

            entity.Property(e => e.EvaluationId)
                .ValueGeneratedOnAdd()
                .HasColumnName("EvaluationID");
            entity.Property(e => e.DeanReturnComment).HasColumnName("Dean_ReturnComment");
            entity.Property(e => e.FinalGrade).HasMaxLength(50);
            entity.Property(e => e.HodReturnComment).HasColumnName("HOD_ReturnComment");
            entity.Property(e => e.HodStrengths).HasColumnName("HOD_Strengths");
            entity.Property(e => e.HodWeaknesses).HasColumnName("HOD_Weaknesses");
            entity.Property(e => e.PeriodId).HasColumnName("PeriodID");
            entity.Property(e => e.StatusId).HasColumnName("StatusID");
            entity.Property(e => e.TaEmployeeId).HasColumnName("TA_EmployeeID");
            entity.Property(e => e.TotalScore).HasColumnType("decimal(6, 2)");

            entity.HasOne(d => d.Period).WithMany(p => p.Evaluations)
                .HasForeignKey(d => d.PeriodId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Evaluations_PeriodID");

            entity.HasOne(d => d.Status).WithMany(p => p.Evaluations)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Evaluations_StatusID");
        });

        modelBuilder.Entity<EvaluationGradeRange>(entity =>
        {
            entity.HasKey(e => e.RangeId).HasName("PK__Evaluati__6899CAF40A26F490");

            entity.ToTable("EvaluationGradeRange", "taEvaluation");

            entity.Property(e => e.RangeId).HasColumnName("RangeID");
            entity.Property(e => e.Description).HasMaxLength(20);
            entity.Property(e => e.MaxGrade).HasColumnType("decimal(6, 2)");
            entity.Property(e => e.MinGrade).HasColumnType("decimal(6, 2)");
        });

        modelBuilder.Entity<EvaluationPeriod>(entity =>
        {
            entity.HasKey(e => e.PeriodId).HasName("PK__Evaluati__E521BB36B76F7D95");

            entity.ToTable("EvaluationPeriods", "taEvaluation");

            entity.HasIndex(e => e.PeriodName, "UQ_EvaluationPeriods").IsUnique();

            entity.Property(e => e.PeriodId).HasColumnName("PeriodID");
            entity.Property(e => e.PeriodName).HasMaxLength(100);
        });

        modelBuilder.Entity<EvaluationStatus>(entity =>
        {
            entity.HasKey(e => e.StatusId).HasName("PK__Evaluati__C8EE2043B4D1D470");

            entity.ToTable("EvaluationStatuses", "taEvaluation");

            entity.HasIndex(e => e.StatusName, "UQ_StatusName").IsUnique();

            entity.Property(e => e.StatusId).HasColumnName("StatusID");
            entity.Property(e => e.StatusDescription).HasMaxLength(255);
            entity.Property(e => e.StatusName).HasMaxLength(100);
        });
        DataAccess.SeedData.SeedEvaluationStatus.SeedCriteria(modelBuilder);

        modelBuilder.Entity<GsdeanEvaluation>(entity =>
        {
            entity.HasKey(e => e.GsevalId);

            entity.ToTable("GSDean_Evaluations", "taEvaluation");

            // Composite unique index to allow one TA per period
            entity.HasIndex(e => new { e.EvaluationPeriodId, e.TaEmployeeId })
                  .IsUnique()
                  .HasDatabaseName("UQ_GSDean_Period_TA");

            entity.Property(e => e.GsevalId).HasColumnName("GSEvalID");
            entity.Property(e => e.EvaluationPeriodId).HasColumnName("EvaluationPeriodID");
            entity.Property(e => e.TaEmployeeId).HasColumnName("TA_EmployeeID");
            entity.Property(e => e.StatusId).HasColumnName("StatusID");

            entity.HasOne(d => d.EvaluationPeriod)
                  .WithMany(p => p.GsdeanEvaluations) 
                  .HasForeignKey(d => d.EvaluationPeriodId)
                  .OnDelete(DeleteBehavior.ClientSetNull)
                  .HasConstraintName("FK_GSDean_Evaluations_EvaluationPeriodID");

            entity.HasOne(d => d.Status)
                  .WithMany(p => p.GsdeanEvaluations)
                  .HasForeignKey(d => d.StatusId)
                  .OnDelete(DeleteBehavior.ClientSetNull)
                  .HasConstraintName("FK_GSDean_Evaluations_StatusID");
        });

        modelBuilder.Entity<Hodevaluation>(entity =>
        {
            entity.HasKey(e => e.HodevalId).HasName("PK__HODEvalu__6A4AF4FA7D98075F");

            entity.ToTable("HODEvaluations", "taEvaluation");

            // ✅ REMOVED: The old unique index will be replaced by a filtered unique index in migration
            // entity.HasIndex(e => new { e.EvaluationId, e.CriterionId }, "UQ_HODEval").IsUnique();

            entity.Property(e => e.HodevalId).HasColumnName("HODEvalID");
            entity.Property(e => e.CriterionId).HasColumnName("CriterionID");
            entity.Property(e => e.EvaluationId).HasColumnName("EvaluationID");
            entity.Property(e => e.RatingId).HasColumnName("RatingID");

            // ✅ ADD: Default values for audit tracking columns
            entity.Property(e => e.SourceRole)
                .HasMaxLength(50)
                .HasDefaultValue("HOD");

            entity.Property(e => e.IsActive)
                .HasDefaultValue(true);

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");

            entity.HasOne(d => d.Criterion).WithMany(p => p.Hodevaluations)
                .HasForeignKey(d => d.CriterionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_HODEvaluations_CriterionID");

            entity.HasOne(d => d.Evaluation).WithMany(p => p.Hodevaluations)
                .HasForeignKey(d => d.EvaluationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_HODEvaluations_EvaluationID");

            entity.HasOne(d => d.Rating).WithMany(p => p.Hodevaluations)
                .HasForeignKey(d => d.RatingId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_HODEvaluations_RatingID");
        });
        DataAccess.SeedData.HodEvaluationCriterionSeed.SeedCriteria(modelBuilder);

        modelBuilder.Entity<HodevaluationCriterion>(entity =>
        {
            entity.HasKey(e => e.CriterionId).HasName("PK__HODEvalu__647C3BD120762426");

            entity.ToTable("HODEvaluation_Criteria", "taEvaluation");

            entity.Property(e => e.CriterionId).HasColumnName("CriterionID");
            entity.Property(e => e.CriterionName).HasMaxLength(255);
            entity.Property(e => e.CriterionType).HasMaxLength(50);
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.NotificationId).HasName("PK__Notifica__20CF2E32770FC71F");

            entity.ToTable("Notifications", "taEvaluation");

            entity.Property(e => e.NotificationId).HasColumnName("NotificationID");
            entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");
            entity.Property(e => e.Message).HasMaxLength(1000);
            entity.Property(e => e.Timestamp).HasDefaultValueSql("(getdate())");
        });

        modelBuilder.Entity<ProfessorCourseEvaluation>(entity =>
        {
            entity.HasKey(e => e.ProfEvalId).HasName("PK__Professo__DACBBA357239488F");

            entity.ToTable("ProfessorCourseEvaluations", "taEvaluation");

            entity.Property(e => e.ProfEvalId).HasColumnName("ProfEvalID");
            entity.Property(e => e.CourseCode).HasMaxLength(20);
            entity.Property(e => e.CourseName).HasMaxLength(255);
            entity.Property(e => e.EvaluationPeriodId).HasColumnName("EvaluationPeriodID");
            entity.Property(e => e.HodReturnComment).HasColumnName("HOD_ReturnComment");
            entity.Property(e => e.ProfessorEmployeeId).HasColumnName("Professor_EmployeeID");
            entity.Property(e => e.StatusId).HasColumnName("StatusID");
            entity.Property(e => e.TaEmployeeId)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("TA_EmployeeID");
            entity.Property(e => e.TotalScore).HasComputedColumnSql("(([OfficeHoursScore]+[AttendanceScore])+[PerformanceScore])", false);

            entity.HasOne(d => d.EvaluationPeriod).WithMany(p => p.ProfessorCourseEvaluations)
                .HasForeignKey(d => d.EvaluationPeriodId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProfessorCourseEvaluations_EvaluationPeriods");

            entity.HasOne(d => d.Status).WithMany(p => p.ProfessorCourseEvaluations)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProfessorCourseEvaluations_EvaluationStatuses");
        });

        modelBuilder.Entity<Rating>(entity =>
        {
            entity.HasKey(e => e.RatingId).HasName("PK__Ratings__FCCDF85CD6813305");

            entity.ToTable("Ratings", "taEvaluation");

            entity.HasIndex(e => e.RatingName, "UQ_RatingName").IsUnique();

            entity.Property(e => e.RatingId).HasColumnName("RatingID");
            entity.Property(e => e.RatingName).HasMaxLength(50);
        });
        DataAccess.SeedData.RatingSeed.SeedRatings(modelBuilder);

        modelBuilder.Entity<ReminderLog>(entity =>
        {
            entity.HasKey(e => e.LogId).HasName("PK__Reminder__5E5499A89B61C1AD");

            entity.ToTable("ReminderLogs", "taEvaluation");

            entity.Property(e => e.LogId).HasColumnName("LogID");
            entity.Property(e => e.EvaluationId).HasColumnName("EvaluationID");
            entity.Property(e => e.RecievedByEmployeeId).HasColumnName("RecievedBy_EmployeeID");
            entity.Property(e => e.RecipientDescription).HasMaxLength(255);
            entity.Property(e => e.SentByEmployeeId).HasColumnName("SentBy_EmployeeID");
            entity.Property(e => e.Timestamp).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Evaluation).WithMany(p => p.ReminderLogs)
                .HasForeignKey(d => d.EvaluationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ReminderLogs_EvaluationID");
        });

        modelBuilder.Entity<ResearchActivity>(entity =>
        {
            entity.HasKey(e => e.ActivityId).HasName("PK__Research__45F4A7F1FF725F51");

            entity.ToTable("ResearchActivities", "taEvaluation");

            entity.Property(e => e.ActivityId).HasColumnName("ActivityID");
            entity.Property(e => e.Journal).HasMaxLength(500);
            entity.Property(e => e.Location).HasMaxLength(255);
            entity.Property(e => e.StatusId).HasColumnName("StatusID");
            entity.Property(e => e.SubmissionId).HasColumnName("SubmissionID");
            entity.Property(e => e.Title).HasMaxLength(500);
            entity.Property(e => e.Url)
                .HasMaxLength(2048)
                .HasColumnName("URL");

            entity.HasOne(d => d.Status).WithMany(p => p.ResearchActivities)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ResearchActivities_StatusID");

            entity.HasOne(d => d.Submission).WithMany(p => p.ResearchActivities)
                .HasForeignKey(d => d.SubmissionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ResearchActivities_SubmissionID");
        });

        modelBuilder.Entity<ResearchStatus>(entity =>
        {
            entity.HasKey(e => e.StatusId).HasName("PK__Research__C8EE204312F7CC6C");

            entity.ToTable("ResearchStatuses", "taEvaluation");

            entity.HasIndex(e => e.StatusKey, "UQ__Research__096C98C29B241A88").IsUnique();

            entity.Property(e => e.StatusId).HasColumnName("StatusID");
            entity.Property(e => e.StatusKey)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.StatusName).HasMaxLength(100);
        });
        ResearchStatusSeed.Seed(modelBuilder);

        modelBuilder.Entity<Tasubmission>(entity =>
        {
            entity.HasKey(e => e.SubmissionId).HasName("PK__TASubmis__449EE1055BF179B8");

            entity.ToTable("TASubmissions", "taEvaluation");

            entity.HasIndex(e => e.EvaluationId, "UQ__TASubmis__36AE68D2799DBF4B").IsUnique();

            entity.Property(e => e.SubmissionId).HasColumnName("SubmissionID");
            entity.Property(e => e.EvaluationId).HasColumnName("EvaluationID");

            entity.HasOne(d => d.Evaluation).WithOne(p => p.Tasubmission)
                .HasForeignKey<Tasubmission>(d => d.EvaluationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TA_Submissions_EvaluationID");
        });

        modelBuilder.Entity<VpgsEvaluation>(entity =>
        {
            entity.HasKey(e => e.VpgsevalId).HasName("PK__VPGS_Eva__57C1D3BEE96AAFED");

            entity.ToTable("VPGS_Evaluations", "taEvaluation");

            entity.HasIndex(e => e.EvaluationId, "UQ__VPGS_Eva__36AE68D2910EDF18").IsUnique();

            entity.Property(e => e.VpgsevalId).HasColumnName("VPGSEvalID");
            entity.Property(e => e.EvaluationId).HasColumnName("EvaluationID");
            entity.Property(e => e.ScientificScore).HasColumnType("decimal(3, 1)");
            entity.Property(e => e.StatusId).HasColumnName("StatusID");

            entity.HasOne(d => d.Status).WithMany(p => p.VpgsEvaluations)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_VPGS_Evaluations_EvaluationStatuses");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}


