using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace CodeFirstEF.Models
{
    public class StudentContext : DbContext
    {
        public StudentContext(): base()
        {
            Database.SetInitializer<StudentContext>(new CreateDatabaseIfNotExists<StudentContext>());
        }

        public DbSet<Student> Students { get; set; }
    }

    
}