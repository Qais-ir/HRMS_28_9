using HRMS.Models;
using Microsoft.EntityFrameworkCore;

namespace HRMS.DbContexts
{
    public class HRMSContext : DbContext
    {

        public HRMSContext(DbContextOptions<HRMSContext> options) : base(options)
        {

            // options
            // sql provider : sql server, oracle, mysql....
            // connection string
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //modelBuilder.Entity<Employee>()
            //    .HasOne(x => x.Department) // Each employee has one department
            //    .WithMany() // Each department has many employees
            //    .HasForeignKey(x => x.DepartmentId);


            // Seeding
            modelBuilder.Entity<Lookup>().HasData(
                
                // Employee Positions (Major Code = 0)
                new Lookup { Id = 1, MajorCode = 0, MinorCode = 0, Name = "Employee Positions" },
                new Lookup { Id = 2, MajorCode = 0, MinorCode = 1, Name = "HR"},
                new Lookup { Id = 3, MajorCode = 0, MinorCode = 2, Name = "Manager"},
                new Lookup { Id = 4, MajorCode = 0, MinorCode = 3, Name = "Developer"},

                // Department Types (Major Code = 1)
                new Lookup { Id = 5, MajorCode = 1, MinorCode = 0, Name = "Department Types" },
                new Lookup { Id = 6, MajorCode = 1, MinorCode = 1, Name = "Finance" },
                new Lookup { Id = 7, MajorCode = 1, MinorCode = 2, Name = "Adminstrative" },
                new Lookup { Id = 8, MajorCode = 1, MinorCode = 3, Name = "Technical" }
                );

            // BCrypt.Net.BCrypt.HashPassword("Admin@123") = "$2a$11$MhHLoubmXdikbYbxiZf.LuhXK4HBduXLNYafEOZNLslcwdMQ1gpyC"
            modelBuilder.Entity<User>().HasData(
                new User { Id = 1, Username = "Admin", HashedPassword = "$2a$11$MhHLoubmXdikbYbxiZf.LuhXK4HBduXLNYafEOZNLslcwdMQ1gpyC", IsAdmin = true}
                );
        }

        // Tables <--> DbSet
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Lookup> Lookups { get; set; }
        public DbSet<User> Users { get; set; }

    }
}
