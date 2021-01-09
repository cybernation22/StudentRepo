using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentInfoEngine.Models
{
    public class StudentsDBContext : DbContext
    {
        public StudentsDBContext(DbContextOptions<StudentsDBContext> options)
         : base(options)
        {

        }
        public DbSet<Student> Students { get; set; }
        public DbSet<Gender> Genders { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=StudentsDB.db");
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Student>()
                .HasIndex(u => u.PrivateNumber)
                .IsUnique();
        }


    }
}
