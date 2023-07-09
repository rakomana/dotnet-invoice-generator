global using Microsoft.EntityFrameworkCore;
using learnApi.Models;
using Microsoft.EntityFrameworkCore.SqlServer;

namespace learnApi.Data
{
    public class DataContextEF : DbContext
    {
        public DataContextEF(DbContextOptions<DataContextEF> options) : base(options)
        {
            
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            object value = optionsBuilder.UseSqlServer("Server=.\\SQLExpress;DataBase=laraMssqlApp;Trusted_Connection=true;TrustServerCertificate=true;");
        }

        public DbSet<User> users {get; set;}
        public DbSet<Item> items {get; set;}
        public DbSet<Quotation> quotations {get; set;}
    }
}