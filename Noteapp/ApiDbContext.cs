using NoteApp.Models;
using Microsoft.EntityFrameworkCore;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace NoteApp
{
    /// <summary>
    /// Represents the database context for the API.
    /// </summary>
    public class ApiDbContext : IdentityDbContext<User>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApiDbContext"/> class.
        /// </summary>
        /// <param name="options">The options for this context.</param>
        public ApiDbContext(DbContextOptions<ApiDbContext> options)
            : base(options)
        {
        }

        /// <summary>
        /// Gets or sets the set of notes.
        /// </summary>
        public DbSet<Note> Notes { get; set; }

        /// <summary>
        /// Configures relationships and initializes roles.
        /// </summary>
        /// <param name="builder">The model builder instance.</param>
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Define the relationship between Note and User
            builder.Entity<Note>()
                .HasOne(n => n.User) // A note belongs to one user
                .WithMany(u => u.Notes) // A user can have multiple notes
                .HasForeignKey(n => n.UserId) // Foreign key from note to user
                .OnDelete(DeleteBehavior.Cascade); // If user is deleted, delete all their notes

            // Seed roles data
            List<IdentityRole> roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                },
                new IdentityRole
                {
                    Name = "User",
                    NormalizedName = "USER"
                },
            };
            builder.Entity<IdentityRole>().HasData(roles);
        }
    }
}