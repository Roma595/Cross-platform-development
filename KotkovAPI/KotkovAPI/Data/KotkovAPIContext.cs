namespace KotkovAPI.Data
{
    using Microsoft.EntityFrameworkCore;
    using KotkovAPI.Models;
    public class KotkovAPIContext : DbContext
    {
        public KotkovAPIContext(DbContextOptions<KotkovAPIContext> options)
            : base(options)
        {
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Test> Tests { get; set; }
        public DbSet<Status> Statuses { get; set; }
        public DbSet<Progress> Progresses { get; set; }
        public DbSet<Attendence> Attendences { get; set; }
        public DbSet<Person> People { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Teacher>(entity =>
            {
                entity.ToTable("teachers");

                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id)
                    .HasColumnType("INT")
                    .ValueGeneratedOnAdd();
                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasColumnType("VARCHAR(50)");
                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasColumnType("VARCHAR(50)");
                entity.Property(e => e.PhoneNumber)
                    .HasColumnType("VARCHAR(20)");

                entity.HasMany<Course>(e => e.Courses)
                    .WithOne(c => c.Teacher)
                    .HasForeignKey(c => c.TeacherId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Student>(entity =>
            {
                entity.ToTable("students");

                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id)
                    .HasColumnType("INT")
                    .ValueGeneratedOnAdd();
                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasColumnType("VARCHAR(50)");
                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasColumnType("VARCHAR(50)");
                entity.Property(e => e.PhoneNumber)
                    .HasColumnType("VARCHAR(20)");

                entity.HasMany(s => s.Courses)
                    .WithMany(c => c.Students)
                    .UsingEntity<Attendence>();

                entity.HasMany(s => s.Tests)
                    .WithMany(t => t.Students)
                    .UsingEntity<Progress>();
            });

            modelBuilder.Entity<Course>(entity =>
            {
                entity.ToTable("courses");

                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id)
                    .HasColumnType("INT")
                    .ValueGeneratedOnAdd();
                entity.Property(e => e.TeacherId)
                    .IsRequired()
                    .HasColumnType("INT");
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("VARCHAR(45)");
                entity.Property(e => e.TotalPlaces)
                    .IsRequired()
                    .HasColumnType("INT");
                entity.Property(e => e.StartDate)
                    .IsRequired()
                    .HasColumnType("DATE");
                entity.Property(e => e.EndDate)
                    .IsRequired()
                    .HasColumnType("DATE");

                entity.HasOne<Teacher>(e => e.Teacher)
                    .WithMany(t => t.Courses)
                    .HasForeignKey(e => e.TeacherId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany<Test>(e => e.Tests)
                    .WithOne(a => a.Course)
                    .HasForeignKey(a => a.CourseId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Status>(entity =>
            {
                entity.ToTable("statuses");

                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id)
                    .HasColumnType("INT")
                    .ValueGeneratedOnAdd();
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("VARCHAR(45)");

                entity.HasMany<Attendence>(e => e.Attendences)
                    .WithOne(a => a.Status)
                    .HasForeignKey(a => a.StatusId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Restrict);
            });


            modelBuilder.Entity<Attendence>(entity =>
            {
                entity.ToTable("attendences");

                entity.HasKey(e => new { e.StudentId, e.CourseId });
                
                entity.Property(e => e.StudentId)
                    .IsRequired()
                    .HasColumnType("INT");
                entity.Property(e => e.CourseId)
                    .IsRequired()
                    .HasColumnType("INT");
                entity.Property(e => e.StatusId)
                    .IsRequired()
                    .HasColumnType("INT");

                // entity.HasOne<Student>(e => e.Student)
                //     .WithMany()
                //     .HasForeignKey(e => e.StudentId)
                //     .IsRequired()
                //     .OnDelete(DeleteBehavior.Restrict);

                // entity.HasOne<Course>(e => e.Course)
                //     .WithMany()
                //     .HasForeignKey(e => e.CourseId)
                //     .IsRequired()
                //     .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne<Status>(e => e.Status)
                    .WithMany(s => s.Attendences)
                    .HasForeignKey(e => e.StatusId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Test>(entity =>
            {
                entity.ToTable("tests");

                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id)
                    .HasColumnType("INT")
                    .ValueGeneratedOnAdd();
                entity.Property(e => e.CourseId)
                    .IsRequired()
                    .HasColumnType("INT");
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("VARCHAR(45)");
                entity.Property(e => e.HighestMark)
                    .IsRequired()
                    .HasColumnType("INT");

                entity.HasOne<Course>(e => e.Course)
                    .WithMany(c => c.Tests)
                    .HasForeignKey(e => e.CourseId)
                    .OnDelete(DeleteBehavior.Restrict);

            });

            modelBuilder.Entity<Progress>(entity =>
            {
                entity.ToTable("progresses");
                entity.HasKey(e => new { e.TestId, e.StudentId });

                entity.Property(e => e.TestId)
                    .IsRequired()
                    .HasColumnType("INT");
                entity.Property(e => e.StudentId)
                    .IsRequired()
                    .HasColumnType("INT");
                entity.Property(e => e.Mark)
                    .IsRequired()
                    .HasColumnType("INT");

                // entity.HasOne<Test>(e => e.Test)
                //     .WithMany()
                //     .HasForeignKey(e => e.TestId)
                //     .IsRequired()
                //     .OnDelete(DeleteBehavior.Restrict);

                // entity.HasOne<Student>(e => e.Student)
                //     .WithMany()
                //     .HasForeignKey(e => e.StudentId)
                //     .IsRequired()
                //     .OnDelete(DeleteBehavior.Restrict);
            });
            
            modelBuilder.Entity<Person>(entity =>
            {
                entity.ToTable("people");
                entity.HasKey(e => e.Login);
                
                entity.Property(e => e.Login)
                    .IsRequired()
                    .HasColumnType("VARCHAR(50)");
                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasColumnType("VARCHAR(1024)");
                entity.Property(e => e.Role)
                    .IsRequired()
                    .HasColumnType("VARCHAR(50)");
            });

        }
    }
}