using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Model;
using WpfApp1.Models;
using Microsoft.EntityFrameworkCore;

namespace WpfApp1.Data
{
    public class SyzyfContext : DbContext
    {
        public SyzyfContext(DbContextOptions<SyzyfContext> options) : base(options) { }

        public DbSet<Candidate> Candidates { get; set; }
        public DbSet<CandidateSelection> CandidateSelections { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectEmployee> ProjectEmployees { get; set; }
        public DbSet<ProjectVersion> ProjectVersions { get; set; }
        public DbSet<RecruitmentMeeting> RecruitmentMeetings { get; set; }
        public DbSet<Evaluation> Evaluations { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySql(
                    "server=localhost;port=3306;database=syzyf;user=root;password=;",
                    new MySqlServerVersion(new Version(8, 0, 29)));
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Candidate>().HasKey(c => c.Id);
            modelBuilder.Entity<CandidateSelection>().HasKey(cs => cs.Id);
            modelBuilder.Entity<Client>().HasKey(c => c.Id);
            modelBuilder.Entity<Employee>().HasKey(e => e.Id);
            modelBuilder.Entity<Position>().HasKey(p => p.Id);
            modelBuilder.Entity<Project>().HasKey(p => p.Id);
            modelBuilder.Entity<ProjectEmployee>().HasKey(pe => pe.Id);
            modelBuilder.Entity<ProjectVersion>().HasKey(pv => pv.Id);
            modelBuilder.Entity<RecruitmentMeeting>().HasKey(rm => rm.Id);
            modelBuilder.Entity<Evaluation>().HasKey(ev => ev.Id);
            modelBuilder.Entity<Invoice>().HasKey(i => i.Id);
            modelBuilder.Entity<User>().HasKey(u => u.Id);

            modelBuilder.Entity<Candidate>(entity =>
            {
                entity.Property(c => c.FullName).HasMaxLength(200);
                entity.Property(c => c.Email).HasMaxLength(100);
            });

            modelBuilder.Entity<Client>(entity =>
            {
                entity.Property(c => c.Company).HasMaxLength(200);
                entity.Property(c => c.Nip).HasMaxLength(20);
                entity.HasIndex(c => c.Nip).IsUnique(false);
            });

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.Property(e => e.FirstName).HasMaxLength(100);
                entity.Property(e => e.LastName).HasMaxLength(100);
                entity.Property(e => e.Email).HasMaxLength(100);
            });

            modelBuilder.Entity<Position>(entity =>
            {
                entity.Property(p => p.PositionName).HasMaxLength(100);
            });

            modelBuilder.Entity<Project>(entity =>
            {
                entity.Property(p => p.Name).HasMaxLength(200);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(u => u.Login).HasMaxLength(100);
                entity.Property(u => u.Email).HasMaxLength(100);
                entity.Property(u => u.Password).HasMaxLength(200);
            });

            modelBuilder.Entity<CandidateSelection>()
                .HasOne(cs => cs.Candidate)
                .WithMany(c => c.CandidateSelections)
                .HasForeignKey(cs => cs.CandidateId);

            modelBuilder.Entity<CandidateSelection>()
                .HasOne(cs => cs.Project)
                .WithMany(p => p.CandidateSelections)
                .HasForeignKey(cs => cs.ProjectId);

            modelBuilder.Entity<Employee>()
                .HasOne(e => e.Position)
                .WithMany(p => p.Employees)
                .HasForeignKey(e => e.PositionId);

            modelBuilder.Entity<Evaluation>()
                .HasOne(ev => ev.Candidate)
                .WithMany(c => c.Evaluations)
                .HasForeignKey(ev => ev.CandidateId);

            modelBuilder.Entity<Evaluation>()
                .HasOne(ev => ev.Client)
                .WithMany(c => c.Evaluations)
                .HasForeignKey(ev => ev.ClientId);

            modelBuilder.Entity<Invoice>()
                .HasOne(i => i.Client)
                .WithMany(c => c.Invoices)
                .HasForeignKey(i => i.ClientId);

            modelBuilder.Entity<Project>()
                .HasOne(p => p.Client)
                .WithMany(c => c.Projects)
                .HasForeignKey(p => p.ClientId);

            modelBuilder.Entity<ProjectEmployee>()
                .HasOne(pe => pe.Employee)
                .WithMany(e => e.ProjectEmployees)
                .HasForeignKey(pe => pe.EmployeeId);

            modelBuilder.Entity<ProjectEmployee>()
                .HasOne(pe => pe.Project)
                .WithMany(p => p.ProjectEmployees)
                .HasForeignKey(pe => pe.ProjectId);

            modelBuilder.Entity<ProjectVersion>()
                .HasOne(pv => pv.Employee)
                .WithMany(e => e.ProjectVersions)
                .HasForeignKey(pv => pv.EmployeeId);

            modelBuilder.Entity<ProjectVersion>()
                .HasOne(pv => pv.Project)
                .WithMany(p => p.ProjectVersions)
                .HasForeignKey(pv => pv.ProjectId);

            modelBuilder.Entity<RecruitmentMeeting>()
                .HasOne(rm => rm.Candidate)
                .WithMany(c => c.RecruitmentMeetings)
                .HasForeignKey(rm => rm.CandidateId);

            modelBuilder.Entity<RecruitmentMeeting>()
                .HasOne(rm => rm.Client)
                .WithMany(c => c.RecruitmentMeetings)
                .HasForeignKey(rm => rm.ClientId);

            modelBuilder.Entity<User>()
                .HasOne(u => u.Employee)
                .WithOne(e => e.User)
                .HasForeignKey<User>(u => u.EmployeeId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
