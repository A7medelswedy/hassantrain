using Microsoft.EntityFrameworkCore;
using assignmentt.ViewModel;
namespace assignmentt.Models
{
    public class AbbDbContext : DbContext
    {

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    base.OnModelCreating(modelBuilder);
        //    modelBuilder.Entity<OrderDetailsViewModel>().HasNoKey();
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrderDetailsViewModel>()
                .HasNoKey();
            modelBuilder.Entity<OrderViewModel>().HasNoKey();
        }
        public AbbDbContext(DbContextOptions<AbbDbContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<assignmentt.ViewModel.OrderDetailsViewModel> OrderDetailsViewModel { get; set; } = default!;
        public DbSet<assignmentt.ViewModel.OrderViewModel> OrderViewModel { get; set; } = default!;


    }

}
