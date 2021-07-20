using Microsoft.EntityFrameworkCore;
using TribeTussleGameDataEditor.API.DAL.Models;

namespace TribeTussleGameDataEditor.API.DAL
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<VerificationToken> VerificationTokens { get; set; }
        public DbSet<PasswordResetToken> PasswordResetTokens { get; set; }
    }
}
