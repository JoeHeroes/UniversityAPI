using Microsoft.EntityFrameworkCore;
using UniAPI.Authorization;

namespace UniAPI.Entites
{
    public class UniversityDbContext: DbContext
    {
        public UniversityDbContext(DbContextOptions<UniversityDbContext> options):base(options)
        {

        }

        public DbSet<University> Universities { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {



            modelBuilder.Entity<User>()
                .Property(u => u.Email)
                .IsRequired();

            modelBuilder.Entity<Role>()
                .Property(r => r.Name)
                .IsRequired()
                .HasMaxLength(25);



            modelBuilder.Entity<University>()
                .Property(u => u.Name)
                .IsRequired()
                .HasMaxLength(25);

            //modelBuilder.Entity<University>().HasData();

            /*
            modelBuilder.Entity<Student>()
                .Property(s => s.IndexNumber)
                .IsRequired()
                .HasMaxLength(6);

            modelBuilder.Entity<Teacher>()
                .Property(t => t.SecondName)
                .IsRequired();
            */
            modelBuilder.Entity<Department>()
             .Property(d => d.Name)
             .IsRequired()
             .HasMaxLength(25);
        }

    }
}
