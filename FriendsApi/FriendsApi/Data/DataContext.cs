using FriendsApi.Models;
using Microsoft.EntityFrameworkCore;

namespace FriendsApi.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options):base(options)
        {
        }

        public DbSet<AppUser> Users { get; set; }
    }
}
