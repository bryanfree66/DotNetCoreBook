using Web.Models;
using Microsoft.EntityFrameworkCore;

namespace Web
{
    public class ComponentShopContext:DbContext
    {
        public ComponentShopContext(DbContextOptions<ComponentShopContext> options)
        : base(options)
        {}

        public DbSet<ComponentType> ComponentTypes {get;set;}
        public DbSet<Component> Components {get;set;}
    }
}