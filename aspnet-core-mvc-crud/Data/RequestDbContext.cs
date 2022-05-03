using aspnet_core_mvc_crud.Models;
using Microsoft.EntityFrameworkCore;

namespace aspnet_core_mvc_crud.Data
{
    public class RequestDbContext : DbContext
    {
        public DbSet<ServiceRequest> ServiceRequests { get; set; }
        public RequestDbContext(DbContextOptions<RequestDbContext> options): base(options)
        { 
        }

        /*protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=localhost\MSSQLSERVER01;Database=aspnetcore-webapp;Trusted_Connection=True");
        }*/
    }
}
