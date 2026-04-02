using Microsoft.EntityFrameworkCore;
using Пример_практика.Models;

namespace Пример_практика
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Inventory> Inventories { get; set; }
        public DbSet<WarehouseTask> Tasks { get; set; }
        public DbSet<OperationsLog> OperationsLogs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=ROSIE;Database=DarsSklad;Trusted_Connection=True;TrustServerCertificate=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Явное указание имен таблиц
            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<Product>().ToTable("Products");
            modelBuilder.Entity<Location>().ToTable("Locations");
            modelBuilder.Entity<Inventory>().ToTable("Inventory");
            modelBuilder.Entity<WarehouseTask>().ToTable("Tasks");
            modelBuilder.Entity<OperationsLog>().ToTable("OperationsLog");

            // Настройка связей для Inventory
            modelBuilder.Entity<Inventory>()
                .HasOne(i => i.Product)
                .WithMany(p => p.Inventories)
                .HasForeignKey(i => i.ProductID);

            modelBuilder.Entity<Inventory>()
                .HasOne(i => i.Location)
                .WithMany(l => l.Inventories)
                .HasForeignKey(i => i.LocationID);

            // Настройка связей для WarehouseTask
            modelBuilder.Entity<WarehouseTask>()
                .HasOne(t => t.AssignedUser)
                .WithMany(u => u.Tasks)
                .HasForeignKey(t => t.AssignedTo);

            // Настройка связей для OperationsLog
            modelBuilder.Entity<OperationsLog>()
                .HasOne(o => o.User)
                .WithMany(u => u.OperationsLogs)
                .HasForeignKey(o => o.UserID);
        }
    }
}