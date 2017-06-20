using Web.Models;
using Microsoft.EntityFrameworkCore;

namespace Web.Data
{
    public class InventoryContext:DbContext
    {
        public InventoryContext(DbContextOptions<InventoryContext> options)
        : base(options)
        {}

        public DbSet<ComponentType> ComponentTypes {get;set;}
        public DbSet<Component> Components {get;set;}
    }
}