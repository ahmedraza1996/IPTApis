using System.Configuration;
using IptApis.Models.Admission;
using Microsoft.EntityFrameworkCore;

namespace IptApis.Data
{
    public partial class AdmissionDBContext : DbContext
    {
        public AdmissionDBContext()
        {
        }

        public AdmissionDBContext(DbContextOptions<AdmissionDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AdmissionTest> AdmissionTest { get; set; }
        public virtual DbSet<Batch> Batch { get; set; }
        public virtual DbSet<CandidateStudent> CandidateStudent { get; set; }
        public virtual DbSet<CandidateTestDetails> CandidateTestDetails { get; set; }
        public virtual DbSet<Department> Department { get; set; }
        public virtual DbSet<Programme> Programme { get; set; }
        public virtual DbSet<ScandidateApplication> ScandidateApplication { get; set; }
        public virtual DbSet<Student> Student { get; set; }
        public virtual DbSet<StudentOpening> StudentOpening { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
                optionsBuilder.UseSqlServer(ConfigurationManager.AppSettings["SqlDBConn"].ToString());
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AdmissionTest>(entity =>
            {
                entity.HasKey(e => e.TestId)
                    .HasName("PK__Admissio__8CC33100B076C6A1");

                entity.Property(e => e.TestId).HasColumnName("TestID");

                entity.Property(e => e.TestOpeningId).HasColumnName("TestOpeningID");
            });

            modelBuilder.Entity<Batch>(entity =>
            {
                entity.Property(e => e.BatchId).HasColumnName("BatchID");

                entity.Property(e => e.BatchName)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<CandidateStudent>(entity =>
            {
                entity.HasKey(e => e.CandidateId)
                    .HasName("PK__SCandida__DF539BFCC1AD9B59");

                entity.Property(e => e.CandidateId).HasColumnName("CandidateID");

                entity.Property(e => e.Caddress)
                    .HasColumnName("CAddress")
                    .HasMaxLength(100);

                entity.Property(e => e.Cname)
                    .IsRequired()
                    .HasColumnName("CName")
                    .HasMaxLength(100);

                entity.Property(e => e.ContactInfo).HasMaxLength(100);

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Hscresult)
                    .HasColumnName("HSCResult")
                    .HasMaxLength(100);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.Sscresult)
                    .HasColumnName("SSCResult")
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<CandidateTestDetails>(entity =>
            {
                entity.HasKey(e => e.DetailsId)
                    .HasName("PK__Candidat__BAC862ACCDE64C78");

                entity.HasIndex(e => new { e.CandidateId, e.AssignedTestId })
                    .HasName("UQ_CandidateTest")
                    .IsUnique();

                entity.Property(e => e.DetailsId).HasColumnName("DetailsID");

                entity.Property(e => e.AssignedTestId).HasColumnName("AssignedTestID");

                entity.Property(e => e.CandidateId).HasColumnName("CandidateID");

                entity.Property(e => e.TestScore).HasMaxLength(5);
                });

            modelBuilder.Entity<Department>(entity =>
            {
                entity.Property(e => e.DepartmentId).HasColumnName("DepartmentID");

                entity.Property(e => e.DepartmentName)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<Programme>(entity =>
            {
                entity.Property(e => e.ProgrammeId).HasColumnName("ProgrammeID");

                entity.Property(e => e.DepartmentId).HasColumnName("DepartmentID");

                entity.Property(e => e.ProgrammeName)
                    .IsRequired()
                    .HasMaxLength(100);
                });

            modelBuilder.Entity<ScandidateApplication>(entity =>
            {
                entity.HasKey(e => e.RefId)
                    .HasName("PK__SCandida__2D2A2CD1BD8B5255");

                entity.ToTable("SCandidateApplication");

                entity.Property(e => e.RefId).HasColumnName("RefID");

                entity.Property(e => e.CandidateId).HasColumnName("CandidateID");

                entity.Property(e => e.QualifiedStatus)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.SopeningId).HasColumnName("SOpeningID");
                });

            modelBuilder.Entity<Student>(entity =>
            {
                entity.Property(e => e.StudentId).HasColumnName("StudentID");

                entity.Property(e => e.BatchId).HasColumnName("BatchID");

                entity.Property(e => e.CandidateId).HasColumnName("CandidateID");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.MobileNumber)
                    .IsRequired()
                    .HasMaxLength(11);

                entity.Property(e => e.RollNumber)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.Sname)
                    .IsRequired()
                    .HasColumnName("SName")
                    .HasMaxLength(100);

                entity.Property(e => e.Spassword)
                    .IsRequired()
                    .HasColumnName("SPassword")
                    .HasMaxLength(255);
                });

            modelBuilder.Entity<StudentOpening>(entity =>
            {
                entity.HasKey(e => e.SopeningId)
                    .HasName("PK__StudentO__D773456921102EB5");

                entity.Property(e => e.SopeningId).HasColumnName("SOpeningID");

                entity.Property(e => e.ProgrammeId).HasColumnName("ProgrammeID");

                entity.Property(e => e.Syear).HasColumnName("SYear");
                });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
