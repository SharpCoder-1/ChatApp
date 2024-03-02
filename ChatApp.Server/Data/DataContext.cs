using ChatApp.Server.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Server.Data
{
    public class DataContext:IdentityDbContext<User>
    {
        public DataContext(DbContextOptions<DataContext> options):base(options)
        {
            
        }
        public DbSet<Message> Messages { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<User>().HasMany<Message>(p=>p.SentMessages)
                .WithOne(p=>p.Sender).HasForeignKey(p=>p.SenderId).OnDelete(DeleteBehavior.Cascade);
            builder.Entity<User>().HasMany<Message>(p => p.ReceivedMessages)
                .WithOne(p => p.Receiver).HasForeignKey(p => p.ReceiverId).OnDelete(DeleteBehavior.Cascade);
            base.OnModelCreating(builder);
        }
    }
}
