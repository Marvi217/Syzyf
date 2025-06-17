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
        public DbSet<ProjectCard> ProjectCards { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<ProjectAcceptance> ProjectAcceptances { get; set; }
        public DbSet<Evaluation> Evaluations { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Meeting> Meetings { get; set; }
        public DbSet<MeetingParticipant> MeetingParticipants { get; set; }
        public DbSet<Order> Orders { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Candidate>().ToTable("candidates");
            modelBuilder.Entity<CandidateSelection>().ToTable("candidateselection");
            modelBuilder.Entity<Client>().ToTable("clients");
            modelBuilder.Entity<Employee>().ToTable("employees");
            modelBuilder.Entity<Position>().ToTable("positions");
            modelBuilder.Entity<Project>().ToTable("projects");
            modelBuilder.Entity<ProjectCard>().ToTable("project_cards");
            modelBuilder.Entity<Notification>().ToTable("notification");
            modelBuilder.Entity<Evaluation>().ToTable("evaluation");
            modelBuilder.Entity<Invoice>().ToTable("invoices");
            modelBuilder.Entity<User>().ToTable("users");
            modelBuilder.Entity<Meeting>().ToTable("meetings");
            modelBuilder.Entity<MeetingParticipant>().ToTable("meeting_participants");
            modelBuilder.Entity<Order>().ToTable("orders");

            modelBuilder.Entity<Candidate>().HasKey(c => c.Id);
            modelBuilder.Entity<CandidateSelection>().HasKey(cs => cs.Id);
            modelBuilder.Entity<Client>().HasKey(c => c.Id);
            modelBuilder.Entity<Employee>().HasKey(e => e.Id);
            modelBuilder.Entity<Position>().HasKey(p => p.Id);
            modelBuilder.Entity<Project>().HasKey(p => p.Id);
            modelBuilder.Entity<ProjectCard>().HasKey(pc => pc.Id);
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
                entity.Property(p => p.ProjectCardId).HasColumnName("project_card_id");
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
                entity.Property(p => p.RecruiterId).HasColumnName("recruiter_id");

                entity.Property(p => p.Status)
                  .HasColumnName("status")
                  .HasConversion(
                    v => v.ToString(),
                    v => (ProjectStatus)Enum.Parse(typeof(ProjectStatus), v));



                entity.HasOne(p => p.Client)
                      .WithMany(c => c.Projects)
                      .HasForeignKey(p => p.ClientId)
                      .HasConstraintName("projects_ibfk_1");
            });

            modelBuilder.Entity<ProjectCard>(entity =>
            {
                entity.ToTable("project_cards");

                entity.Property(pc => pc.Id).HasColumnName("id");
                entity.Property(pc => pc.ClientId).HasColumnName("client_id");
                entity.Property(pc => pc.NumberOfPeople).HasColumnName("number_of_people");
                entity.Property(pc => pc.IsSalaryVisible).HasColumnName("is_salary_visible");
                entity.Property(pc => pc.JobTitle).HasColumnName("job_title").HasMaxLength(255);
                entity.Property(pc => pc.JobLevels).HasColumnName("job_levels");
                entity.Property(pc => pc.Department).HasColumnName("department").HasMaxLength(255);
                entity.Property(pc => pc.MainDuties).HasColumnName("main_duties");
                entity.Property(pc => pc.AdditionalDuties).HasColumnName("additional_duties").IsRequired(false);
                entity.Property(pc => pc.DevelopmentOpportunities).HasColumnName("development_opportunities").IsRequired(false);
                entity.Property(pc => pc.PlannedHiringDate).HasColumnName("planned_hiring_date").IsRequired(false);
                entity.Property(pc => pc.Education).HasColumnName("education").HasMaxLength(255);
                entity.Property(pc => pc.PreferredStudyFields).HasColumnName("preferred_study_fields").HasMaxLength(255).IsRequired(false);
                entity.Property(pc => pc.AdditionalCertifications).HasColumnName("additional_certifications").HasMaxLength(255).IsRequired(false);
                entity.Property(pc => pc.RequiredExperience).HasColumnName("required_experience");
                entity.Property(pc => pc.PreferredExperience).HasColumnName("preferred_experience").IsRequired(false);
                entity.Property(pc => pc.RequiredSkills).HasColumnName("required_skills");
                entity.Property(pc => pc.PreferredSkills).HasColumnName("preferred_skills").IsRequired(false);
                entity.Property(pc => pc.RequiredLanguages).HasColumnName("required_languages");
                entity.Property(pc => pc.PreferredLanguages).HasColumnName("preferred_languages").IsRequired(false);
                entity.Property(pc => pc.EmploymentsForms).HasColumnName("employment_forms").HasMaxLength(255).IsRequired(false);
                entity.Property(pc => pc.GrossSalary).HasColumnName("gross_salary").HasMaxLength(255);
                entity.Property(pc => pc.BonusSystem).HasColumnName("bonus_system");
                entity.Property(pc => pc.AdditionalBenefits).HasColumnName("additional_benefits").IsRequired(false);
                entity.Property(pc => pc.WorkTools).HasColumnName("work_tools").IsRequired(false);
                entity.Property(pc => pc.WorkPlace).HasColumnName("work_place").HasMaxLength(255);
                entity.Property(pc => pc.WorkModes).HasColumnName("work_modes").HasMaxLength(255).IsRequired(false);
                entity.Property(pc => pc.WorkingHours).HasColumnName("working_hours").HasMaxLength(255).IsRequired(false);
                entity.Property(pc => pc.OtherRemarks).HasColumnName("other_remarks").IsRequired(false);
                entity.Property(pc => pc.RecruiterId).HasColumnName("recruiter_id");
                entity.Property(pc => pc.IsAcceptedDb).HasColumnName("is_accepted");

                modelBuilder.Entity<ProjectCard>()
                .Property(e => e.Status)
                .HasConversion<string>();


                entity.HasOne(pc => pc.Client)
                      .WithMany(c => c.ProjectCards)
                      .HasForeignKey(pc => pc.ClientId)
                      .HasConstraintName("project_cards_ibfk_1")
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(pc => pc.Recruiter)
                      .WithMany()
                      .HasForeignKey(pc => pc.RecruiterId)
                      .HasConstraintName("project_cards_ibfk_2")
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(pc => pc.ProjectAcceptance)
                      .WithOne(pa => pa.ProjectCard)
                      .HasForeignKey<ProjectAcceptance>(pa => pa.ProjectCardId)
                      .HasConstraintName("FK_project_acceptances_project_card")
                      .OnDelete(DeleteBehavior.Cascade);
            });


            modelBuilder.Entity<Meeting>(entity =>
            {
                entity.Property(m => m.Id).HasColumnName("id");
                entity.Property(m => m.Title).HasColumnName("title").HasMaxLength(255);
                entity.Property(m => m.StartTime).HasColumnName("start_time");
                entity.Property(m => m.EndTime).HasColumnName("end_time");
            });

            modelBuilder.Entity<MeetingParticipant>(entity =>
            {
                entity.ToTable("meeting_participants");

                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.MeetingId).HasColumnName("meeting_id");
                entity.Property(e => e.EmployeeId).HasColumnName("employee_id");
                entity.Property(e => e.CandidateId).HasColumnName("candidate_id");
                entity.Property(e => e.ClientId).HasColumnName("client_id");

                entity.HasOne(e => e.Meeting)
                    .WithMany(m => m.Participants)
                    .HasForeignKey(e => e.MeetingId);
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
                entity.Property(n => n.ProjectCardId).HasColumnName("project_card_id");
                entity.Property(n => n.ProjectId).HasColumnName("project_id");
                entity.Property(n => n.Title).HasColumnName("title").HasMaxLength(255);
                entity.Property(n => n.Tag).HasColumnName("tag").HasMaxLength(50);
                entity.Property(n => n.Message).HasColumnName("message");
                entity.Property(n => n.FromId).HasColumnName("msg_from");
                entity.Property(n => n.ToId).HasColumnName("msg_to");
                entity.Property(n => n.IsRead).HasColumnName("is_read");
                entity.Property(n => n.OrderId).HasColumnName("order_id");
                entity.Property(n => n.CreatedAt).HasColumnName("created_at");
            });

            modelBuilder.Entity<ProjectAcceptance>(entity =>
            {
                entity.ToTable("project_acceptance");

                entity.HasKey(pa => pa.Id);

                entity.Property(pa => pa.Id).HasColumnName("id");
                entity.Property(pa => pa.ProjectCardId).HasColumnName("project_card_id");
                entity.Property(pa => pa.AcceptedByRecruiter).HasColumnName("accepted_by_recruiter");
                entity.Property(pa => pa.RecruiterAcceptedAt).HasColumnName("recruiter_accepted_at");
                entity.Property(pa => pa.SupportId).HasColumnName("support_id");
                entity.Property(pa => pa.AcceptedBySupport).HasColumnName("accepted_by_support");
                entity.Property(pa => pa.SupportAcceptedAt).HasColumnName("support_accepted_at");
                entity.Property(pa => pa.AcceptedByClient).HasColumnName("accepted_by_client");
                entity.Property(pa => pa.ClientAcceptedAt).HasColumnName("client_accepted_at");

                entity.HasOne(pa => pa.ProjectCard)
                      .WithOne(pc => pc.ProjectAcceptance)
                      .HasForeignKey<ProjectAcceptance>(pa => pa.ProjectCardId)
                      .HasConstraintName("FK_project_acceptances_project_card")
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(o => o.Id);
                entity.Property(o => o.Id).HasColumnName("id");

                entity.Property(o => o.ClientId).HasColumnName("client_id").IsRequired();
                entity.Property(o => o.SalesId).HasColumnName("sales_id");
                entity.Property(o => o.OrderContent).HasColumnName("order_content").IsRequired();
                entity.Property(o => o.IsSignedByClient).HasColumnName("is_signed_by_client");
                entity.Property(o => o.SignedDate).HasColumnName("signed_date");
                entity.Property(o => o.CreatedDate).HasColumnName("created_date");
                entity.Property(o => o.Status).HasColumnName("status");
                entity.Property(o => o.ProjectCardId).HasColumnName("project_card_id");

                entity.HasOne(o => o.Client)
                    .WithMany()
                    .HasForeignKey(o => o.ClientId);

                entity.HasOne(o => o.Sales)
                    .WithMany()
                    .HasForeignKey(o => o.SalesId);

                entity.HasOne(o => o.ProjectCard)
                    .WithMany()
                    .HasForeignKey(o => o.ProjectCardId);
            });



            modelBuilder.Entity<Notification>()
                .HasOne(n => n.ProjectCard)
                .WithMany(pc => pc.Notifications)
                .HasForeignKey(n => n.ProjectCardId)
                .HasConstraintName("notification_ibfk_1");


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

            modelBuilder.Entity<User>()
                .HasOne(u => u.Employee)
                .WithOne(e => e.User)
                .HasForeignKey<User>(u => u.EmployeeId)
                .HasConstraintName("FK6p2ib82uai0pj9yk1iassppgq");

            modelBuilder.Entity<User>()
                .HasOne(u => u.Client)
                .WithOne(c => c.User)
                .HasForeignKey<User>(u => u.ClientId)
                .HasConstraintName("users_ibfk_1");

            modelBuilder.Entity<Order>()
               .HasOne(o => o.Client)
               .WithMany()
               .HasForeignKey(o => o.ClientId)
               .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.Sales)
                .WithMany()
                .HasForeignKey(o => o.SalesId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.ProjectCard)
                .WithMany()
                .HasForeignKey(o => o.ProjectCardId)
                .OnDelete(DeleteBehavior.SetNull);




            base.OnModelCreating(modelBuilder);
        }
    }
}