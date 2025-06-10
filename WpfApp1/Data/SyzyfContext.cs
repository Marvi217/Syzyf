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
        public DbSet<Notification> Notifications { get; set; }

        public DbSet<Evaluation> Evaluations { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Meeting> Meetings { get; set; }
        public DbSet<MeetingParticipant> MeetingParticipants { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Candidate>().ToTable("candidates");
            modelBuilder.Entity<CandidateSelection>().ToTable("candidateselection");
            modelBuilder.Entity<Client>().ToTable("clients");
            modelBuilder.Entity<Employee>().ToTable("employees");
            modelBuilder.Entity<Position>().ToTable("positions");
            modelBuilder.Entity<Project>().ToTable("projects");
            modelBuilder.Entity<ProjectEmployee>().ToTable("projects-employees");
            modelBuilder.Entity<ProjectVersion>().ToTable("projectversions");
            modelBuilder.Entity<RecruitmentMeeting>().ToTable("recruitmentmeetings");
            modelBuilder.Entity<Notification>().ToTable("notification");
            modelBuilder.Entity<Evaluation>().ToTable("evaluation");
            modelBuilder.Entity<Invoice>().ToTable("invoices");
            modelBuilder.Entity<User>().ToTable("users");
            modelBuilder.Entity<Meeting>().ToTable("meetings");
            modelBuilder.Entity<MeetingParticipant>().ToTable("meeting_participants");

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
                entity.Property(c => c.Id).HasColumnName("id");
                entity.Property(c => c.FirstName).HasColumnName("first_name").HasMaxLength(255);
                entity.Property(c => c.LastName).HasColumnName("last_name").HasMaxLength(255);
                entity.Property(c => c.Email).HasColumnName("email").HasMaxLength(255);
                entity.Property(c => c.Address).HasColumnName("address");
                entity.Property(c => c.PhoneNumber).HasColumnName("phone_number");
                entity.Property(c => c.Positions).HasColumnName("positions");
                entity.Property(c => c.Cv).HasColumnName("cv");
            });

            modelBuilder.Entity<CandidateSelection>(entity =>
            {
                entity.Property(cs => cs.Id).HasColumnName("id");
                entity.Property(cs => cs.SelectionDate).HasColumnName("selection_date");
                entity.Property(cs => cs.Status).HasColumnName("status")
                    .HasConversion<string>();
                entity.Property(cs => cs.CandidateId).HasColumnName("candidate_id");
                entity.Property(cs => cs.ProjectId).HasColumnName("project_id");
            });

            modelBuilder.Entity<Client>(entity =>
            {
                entity.Property(c => c.Id).HasColumnName("id");
                entity.Property(c => c.Company).HasColumnName("company").HasMaxLength(255);
                entity.Property(c => c.Nip).HasColumnName("nip").HasMaxLength(255);
                entity.Property(c => c.Address).HasColumnName("address");
                entity.Property(c => c.ContactEmail).HasColumnName("contact_email");
                entity.Property(c => c.ContactNumber).HasColumnName("contact_number");
                entity.Property(c => c.ContactPersonEmail).HasColumnName("contact_person_email");
                entity.Property(c => c.ContactPersonNumber).HasColumnName("contact_person_number");
            });

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.FirstName).HasColumnName("first_name").HasMaxLength(255);
                entity.Property(e => e.LastName).HasColumnName("last_name").HasMaxLength(255);
                entity.Property(e => e.Email).HasColumnName("email").HasMaxLength(255);
                entity.Property(e => e.PhoneNumber).HasColumnName("phone_number");
                entity.Property(e => e.WorkSince).HasColumnName("work_since");
                entity.Property(e => e.PositionId).HasColumnName("position");
            });

            modelBuilder.Entity<Position>(entity =>
            {
                entity.Property(p => p.Id).HasColumnName("id");
                entity.Property(p => p.PositionName).HasColumnName("position_name").HasMaxLength(255);
            });

            modelBuilder.Entity<Project>(entity =>
            {

                entity.HasKey(p => p.Id);
                entity.Property(p => p.Id).HasColumnName("id").ValueGeneratedOnAdd();
                entity.Property(p => p.ClientId).HasColumnName("client_id");
                entity.Property(p => p.NumberOfPeople).HasColumnName("number_of_people");
                entity.Property(p => p.IsSalaryVisible).HasColumnName("is_salary_visible");
                entity.Property(p => p.JobTitle).HasColumnName("job_title").HasMaxLength(255);
                entity.Property(p => p.JobLevels).HasColumnName("job_levels");
                entity.Property(p => p.Department).HasColumnName("department").HasMaxLength(255);
                entity.Property(p => p.MainDuties).HasColumnName("main_duties");
                entity.Property(p => p.AdditionalDuties).HasColumnName("additional_duties").IsRequired(false);
                entity.Property(p => p.DevelopmentOpportunities).HasColumnName("development_opportunities").IsRequired(false);
                entity.Property(p => p.PlannedHiringDate).HasColumnName("planned_hiring_date");
                entity.Property(p => p.Education).HasColumnName("education").HasMaxLength(255);
                entity.Property(p => p.PreferredStudyFields).HasColumnName("preferred_study_fields").HasMaxLength(255).IsRequired(false);
                entity.Property(p => p.AdditionalCertifications).HasColumnName("additional_certifications").HasMaxLength(255).IsRequired(false);
                entity.Property(p => p.RequiredExperience).HasColumnName("required_experience");
                entity.Property(p => p.PreferredExperience).HasColumnName("preferred_experience").IsRequired(false);
                entity.Property(p => p.RequiredSkills).HasColumnName("required_skills");
                entity.Property(p => p.PreferredSkills).HasColumnName("preferred_skills").IsRequired(false);
                entity.Property(p => p.RequiredLanguages).HasColumnName("required_languages");
                entity.Property(p => p.PreferredLanguages).HasColumnName("preferred_languages").IsRequired(false);
                entity.Property(p => p.GrossSalary).HasColumnName("gross_salary").HasMaxLength(255);
                entity.Property(p => p.EmploymentsForms).HasColumnName("employment_forms").HasMaxLength(255).IsRequired(false);
                entity.Property(p => p.BonusSystem).HasColumnName("bonus_system");
                entity.Property(p => p.AdditionalBenefits).HasColumnName("additional_benefits").IsRequired(false);
                entity.Property(p => p.WorkTools).HasColumnName("work_tools").IsRequired(false);
                entity.Property(p => p.WorkPlace).HasColumnName("work_place").HasMaxLength(255);
                entity.Property(p => p.WorkModes).HasColumnName("work_modes").HasMaxLength(255).IsRequired(false);
                entity.Property(p => p.WorkingHours).HasColumnName("working_hours").HasMaxLength(255).IsRequired(false);
                entity.Property(p => p.OtherRemarks).HasColumnName("other_remarks").IsRequired(false);

                entity.HasOne(p => p.Client)
                      .WithMany(c => c.Projects)
                      .HasForeignKey(p => p.ClientId)
                      .HasConstraintName("projects_ibfk_1");
            });


            modelBuilder.Entity<ProjectEmployee>(entity =>
            {
                entity.Property(pe => pe.Id).HasColumnName("id");
                entity.Property(pe => pe.EmployeeId).HasColumnName("employee_id");
                entity.Property(pe => pe.ProjectId).HasColumnName("project_id");
            });

            modelBuilder.Entity<ProjectVersion>(entity =>
            {
                entity.Property(pv => pv.Id).HasColumnName("id");
                entity.Property(pv => pv.Details).HasColumnName("details");
                entity.Property(pv => pv.ChangedTime).HasColumnName("changed_time");
                entity.Property(pv => pv.EmployeeId).HasColumnName("employee_id");
                entity.Property(pv => pv.ProjectId).HasColumnName("project_id");
            });

            modelBuilder.Entity<Meeting>(entity =>
            {
                entity.Property(m => m.Id).HasColumnName("id");
                entity.Property(m => m.Title).HasColumnName("title").HasMaxLength(255);
                entity.Property(m => m.StartTime).HasColumnName("start_time");
                entity.Property(m => m.EndTime).HasColumnName("end_time");
            });


            modelBuilder.Entity<RecruitmentMeeting>(entity =>
            {
                entity.Property(rm => rm.Id).HasColumnName("id");
                entity.Property(e => e.Title).HasColumnName("title");
                entity.Property(rm => rm.MeetingDate).HasColumnName("meeting_date");
                entity.Property(rm => rm.Location).HasColumnName("location");
                entity.Property(rm => rm.MeetingType).HasColumnName("meeting_type");
                entity.Property(rm => rm.CandidateId).HasColumnName("candidate_id");
                entity.Property(rm => rm.ClientId).HasColumnName("client_id");
            });

            modelBuilder.Entity<Evaluation>(entity =>
            {
                entity.Property(ev => ev.Id).HasColumnName("id");
                entity.Property(ev => ev.Score).HasColumnName("score");
                entity.Property(ev => ev.Comment).HasColumnName("comment");
                entity.Property(ev => ev.EvaluationDate).HasColumnName("evaluation_date");
                entity.Property(ev => ev.CandidateId).HasColumnName("candidate_id");
                entity.Property(ev => ev.ClientId).HasColumnName("client_id");
            });

            modelBuilder.Entity<Invoice>(entity =>
            {
                entity.Property(i => i.Id).HasColumnName("id");
                entity.Property(i => i.InvoiceNumber).HasColumnName("invoice_number").HasMaxLength(50);
                entity.Property(i => i.Amount).HasColumnName("amount").HasMaxLength(50);
                entity.Property(i => i.Price).HasColumnName("price").HasMaxLength(50);
                entity.Property(i => i.Company).HasColumnName("company").HasMaxLength(50);
                entity.Property(i => i.ServiceRange).HasColumnName("service_range").HasMaxLength(50);
                entity.Property(i => i.RangeValue).HasColumnName("range_value").HasMaxLength(50);
                entity.Property(i => i.ClientId).HasColumnName("client_id");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(u => u.Id).HasColumnName("id");
                entity.Property(u => u.Login).HasColumnName("login").HasMaxLength(255);
                entity.Property(u => u.Password).HasColumnName("password").HasMaxLength(255);
                entity.Property(u => u.EmployeeId).HasColumnName("employee_id");
                entity.Property(u => u.ClientId).HasColumnName("client_id");
                entity.Property(u => u.CandidateId).HasColumnName("candidate_id");
            });

            modelBuilder.Entity<Notification>(entity =>
            {

                entity.HasKey(n => n.Id);

                entity.Property(n => n.Id).HasColumnName("id");
                entity.Property(n => n.Title).HasColumnName("title").HasMaxLength(255);
                entity.Property(n => n.Tag).HasColumnName("tag").HasMaxLength(50);
                entity.Property(n => n.Message).HasColumnName("message");
                entity.Property(n => n.FromId).HasColumnName("msg_from");
                entity.Property(n => n.ToId).HasColumnName("msg_to");
                entity.Property(n => n.IsRead).HasColumnName("is_read");
            });


            modelBuilder.Entity<MeetingParticipant>()
                .HasOne(mp => mp.Meeting)
                .WithMany(m => m.Participants)
                .HasForeignKey(mp => mp.MeetingId);

            modelBuilder.Entity<MeetingParticipant>()
                .HasOne(mp => mp.Employee)
                .WithMany()
                .HasForeignKey(mp => mp.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<MeetingParticipant>()
                .HasOne(mp => mp.Candidate)
                .WithMany()
                .HasForeignKey(mp => mp.CandidateId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<MeetingParticipant>()
                .HasOne(mp => mp.Client)
                .WithMany()
                .HasForeignKey(mp => mp.ClientId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relacje
            modelBuilder.Entity<CandidateSelection>()
                .HasOne(cs => cs.Candidate)
                .WithMany(c => c.CandidateSelections)
                .HasForeignKey(cs => cs.CandidateId)
                .HasConstraintName("FKrlgx5rhxm6s9wqke0vu84dgpy");

            modelBuilder.Entity<CandidateSelection>()
                .HasOne(cs => cs.Project)
                .WithMany(p => p.CandidateSelections)
                .HasForeignKey(cs => cs.ProjectId)
                .HasConstraintName("FK74p3ugtaapuefwuo8qyjllg8q");

            modelBuilder.Entity<Employee>()
                .HasOne(e => e.Position)
                .WithMany(p => p.Employees)
                .HasForeignKey(e => e.PositionId)
                .HasConstraintName("FK37pcuift0oxh29rxdeertk11n");

            modelBuilder.Entity<Evaluation>()
                .HasOne(ev => ev.Candidate)
                .WithMany(c => c.Evaluations)
                .HasForeignKey(ev => ev.CandidateId)
                .HasConstraintName("FKtlqle7jgjhu3p5oq1h8a7x577");

            modelBuilder.Entity<Evaluation>()
                .HasOne(ev => ev.Client)
                .WithMany(c => c.Evaluations)
                .HasForeignKey(ev => ev.ClientId)
                .HasConstraintName("FK4tv23wmonelk56hav5ey8f59a");

            modelBuilder.Entity<Invoice>()
                .HasOne(i => i.Client)
                .WithMany(c => c.Invoices)
                .HasForeignKey(i => i.ClientId)
                .HasConstraintName("FK9ioqm804urbgy986pdtwqtl0x");

            modelBuilder.Entity<Project>()
                .HasOne(p => p.Client)
                .WithMany(c => c.Projects)
                .HasForeignKey(p => p.ClientId)
                .HasConstraintName("FKksdiyuily2f4ca2y53k07pmq");

            modelBuilder.Entity<ProjectEmployee>()
                .HasOne(pe => pe.Employee)
                .WithMany(e => e.ProjectEmployees)
                .HasForeignKey(pe => pe.EmployeeId)
                .HasConstraintName("FKdid31jlghyg4721cyyulaue8h");

            modelBuilder.Entity<ProjectEmployee>()
                .HasOne(pe => pe.Project)
                .WithMany(p => p.ProjectEmployees)
                .HasForeignKey(pe => pe.ProjectId)
                .HasConstraintName("FKiwf9jobo18083tqqyb7g6xe2l");

            modelBuilder.Entity<ProjectVersion>()
                .HasOne(pv => pv.Employee)
                .WithMany(e => e.ProjectVersions)
                .HasForeignKey(pv => pv.EmployeeId)
                .HasConstraintName("FKkrw0lri0bpmqit2px7eul163l");

            modelBuilder.Entity<ProjectVersion>()
                .HasOne(pv => pv.Project)
                .WithMany(p => p.ProjectVersions)
                .HasForeignKey(pv => pv.ProjectId)
                .HasConstraintName("FKecgv0u8rxetowkes6f3d6e538");

            modelBuilder.Entity<RecruitmentMeeting>()
                .HasOne(rm => rm.Candidate)
                .WithMany(c => c.RecruitmentMeetings)
                .HasForeignKey(rm => rm.CandidateId)
                .HasConstraintName("FKqocug7k7x65ifk7852af4mbm0");

            modelBuilder.Entity<RecruitmentMeeting>()
                .HasOne(rm => rm.Client)
                .WithMany(c => c.RecruitmentMeetings)
                .HasForeignKey(rm => rm.ClientId)
                .HasConstraintName("FKtoxclyjc589lc4peirv8i24fp");

            modelBuilder.Entity<User>()
                .HasOne(u => u.Employee)
                .WithOne(e => e.User)
                .HasForeignKey<User>(u => u.EmployeeId)
                .HasConstraintName("FK6p2ib82uai0pj9yk1iassppgq");

            base.OnModelCreating(modelBuilder);
        }
    }
}