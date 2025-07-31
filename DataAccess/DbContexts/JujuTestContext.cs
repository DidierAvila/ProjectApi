using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.DbContexts
{
    public partial class JujuTestContext : DbContext
    {
        public JujuTestContext(DbContextOptions<JujuTestContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Customer> Customer { get; set; }
        public virtual DbSet<Logs> Logs { get; set; }
        public virtual DbSet<Post> Post { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Token> Tokens { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuración de User
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.Name).HasMaxLength(255).IsRequired();
                entity.Property(e => e.LastName).HasMaxLength(255).IsRequired();
                entity.Property(e => e.Email).HasMaxLength(100).IsRequired();
                entity.Property(e => e.Password).HasMaxLength(255).IsRequired();
                entity.Property(e => e.Role).HasMaxLength(255).IsRequired();
                entity.Property(e => e.Phone).HasMaxLength(20);
            });

            // Configuración de Token
            modelBuilder.Entity<Token>(entity =>
            {
                entity.ToTable("Token");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.TokenValue).HasMaxLength(500).IsRequired();
                entity.Property(e => e.Id).IsRequired();
                entity.Property(e => e.ExpirationDate).IsRequired();
                entity.Property(e => e.CreatedDate).IsRequired();
                entity.Property(e => e.Status).IsRequired();
            });

            // Configuración de Customer
            modelBuilder.Entity<Customer>(entity =>
            {
                entity.ToTable("Customer");
                entity.HasKey(e => e.CustomerId);
                entity.Property(e => e.CustomerId).ValueGeneratedOnAdd();
                entity.Property(e => e.Name).HasMaxLength(500).IsRequired();
                
                // Relación uno a muchos con Post
                entity.HasMany(e => e.Posts)
                      .WithOne(e => e.Customer)
                      .HasForeignKey(e => e.CustomerId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Configuración de Post
            modelBuilder.Entity<Post>(entity =>
            {
                entity.ToTable("Post");
                entity.HasKey(e => e.PostId);
                entity.Property(e => e.PostId).ValueGeneratedOnAdd();
                entity.Property(e => e.Title).HasMaxLength(500).IsRequired();
                entity.Property(e => e.Body).HasMaxLength(500).IsRequired();
                entity.Property(e => e.Category).HasMaxLength(500).IsRequired();
                entity.Property(e => e.Type).IsRequired();
                entity.Property(e => e.CustomerId).IsRequired();
            });

            // Configuración de Logs
            modelBuilder.Entity<Logs>(entity =>
            {
                entity.ToTable("Logs");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.Message).HasMaxLength(4000);
                entity.Property(e => e.MessageTemplate).HasMaxLength(4000);
                entity.Property(e => e.Level).HasMaxLength(50);
                entity.Property(e => e.Exception).HasMaxLength(4000);
                entity.Property(e => e.Properties).HasMaxLength(4000);
                entity.Property(e => e.TimeStamp).HasColumnType("datetime");
            });

            OnModelCreatingPartial(modelBuilder);
        }
        
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
