using Web;
using Web.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;

namespace Web.Data
{
    public class InventoryContext:DbContext
    {
        public InventoryContext(DbContextOptions<InventoryContext> options)
        : base(options)
        {}

        public DbSet<ComponentType> ComponentTypes {get;set;}
        public DbSet<Component> Components {get;set;}

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<ComponentType>().HasKey(m => m.Id);
            builder.Entity<Component>().HasKey(m => m.Id);

            // shadow properties
            builder.Entity<ComponentType>().Property<DateTime>("UpdatedTimestamp");
            builder.Entity<Component>().Property<DateTime>("UpdatedTimestamp");

            base.OnModelCreating(builder);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            builder.UseNpgsql("User ID=ComponentShopWeb;Password=Wv00r7Cmv0O7;Host=localhost;Port=5432;Database=ComponentShop;Pooling=true;");
            base.OnConfiguring(builder);
        }

        public override int SaveChanges()
        {
            ChangeTracker.DetectChanges();

            updateUpdatedProperty<ComponentType>();
            updateUpdatedProperty<Component>();

            return base.SaveChanges();
        }

        private void updateUpdatedProperty<T>() where T : class
        {
            var modifiedSourceInfo =
                ChangeTracker.Entries<T>()
                    .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

            foreach (var entry in modifiedSourceInfo)
            {
                entry.Property("UpdatedTimestamp").CurrentValue = DateTime.UtcNow;
            }
        }
    }
}