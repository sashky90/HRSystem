namespace HRSystem.Data
{
    using HRSystem.Data.Migrations;
    using HRSystem.Models;
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class EmployeeDbContext : DbContext
    {
        public EmployeeDbContext()
            : base("EmployeeDbContext")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<EmployeeDbContext, Configuration>());
        }

        public static EmployeeDbContext Create()
        {
            return new EmployeeDbContext();
        }

        public IDbSet<Employee> Employees { get; set; }

        public IDbSet<Team> Teams { get; set; }

        public IDbSet<Project> Projects { get; set; }

        public new void SaveChanges()
        {
            base.SaveChanges();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Team>()
                    .HasRequired(e => e.Leader)
                    .WithMany()
                    .HasForeignKey(l => l.LeaderId)
                    .WillCascadeOnDelete(false);
        }
    }

}