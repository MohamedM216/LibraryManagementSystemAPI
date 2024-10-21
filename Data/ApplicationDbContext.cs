using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystemAPI.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Member>()
            .HasMany(m => m.Borrowings)
            .WithOne(b => b.Member)
            .HasForeignKey(b => b.MemberId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Book>()
            .HasMany(b => b.Borrowings)
            .WithOne(br => br.Book)
            .HasForeignKey(br => br.BookId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<Author>()
            .HasMany(a => a.Books)
            .WithOne(b => b.Author)
            .HasForeignKey(b => b.AuthorId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<User>()
            .HasKey(u => new { u.UserId, u.UserType, u.LoginTime });

        modelBuilder.Entity<Book>()
            .HasIndex(b => b.ISBN)
            .IsUnique();


        modelBuilder.Entity<Transaction>()
            .Property(t => t.DeliveryCost)
            .HasColumnType("decimal(18, 2)");

        modelBuilder.Entity<Transaction>()
            .Property(t => t.AuthorPayout)
            .HasColumnType("decimal(18, 2)");

        modelBuilder.Entity<Member>()
            .Property(t => t.LateFee)
            .HasColumnType("decimal(18, 2)");

        modelBuilder.Entity<Member>()
            .HasIndex(m => m.Email)
            .IsUnique();

        modelBuilder.Entity<Author>()
            .HasIndex(a => a.Email)
            .IsUnique();

        modelBuilder.Entity<AuthorsDue>()
            .Property(a => a.DueAuthorPayout)
            .HasColumnType("decimal(10, 3)");

        modelBuilder.Entity<AdminRefreshToken>()
            .ToTable("AdminRefreshTokens");

        modelBuilder.Entity<AuthorRefreshToken>()
            .ToTable("AuthorRefreshTokens");

        modelBuilder.Entity<MemberRefreshToken>()
            .ToTable("MemberRefreshTokens");
    }

    public DbSet<Member> Members { get; set; }
    public DbSet<Book> Books { get; set; }
    public DbSet<Borrowing> Borrowings { get; set; }
    public DbSet<Author> Authors { get; set; }
    public DbSet<Admin> Admins { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<AuthorsDue> AuthorsDues { get; set; }
    public DbSet<MemberRefreshToken> MemberRefreshTokens { get; set; }
    public DbSet<AdminRefreshToken> AdminRefreshTokens { get; set; }
    public DbSet<AuthorRefreshToken> AuthorRefreshTokens { get; set; }
}