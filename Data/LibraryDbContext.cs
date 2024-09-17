using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.Data;

public partial class LibraryDbContext : DbContext
{
    public LibraryDbContext()
    {
    }

    public LibraryDbContext(DbContextOptions<LibraryDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Author> Authors { get; set; }

    public virtual DbSet<AuthorBook> AuthorBooks { get; set; }

    public virtual DbSet<Book> Books { get; set; }

    public virtual DbSet<BookCategory> BookCategories { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Copy> Copies { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<Publisher> Publishers { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySQL("Server=localhost; Database=librarydb; user=root");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Author>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("authors");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.FirstName).HasMaxLength(20);
            entity.Property(e => e.LastName).HasMaxLength(20);
            entity.Property(e => e.Patronymic)
                .HasMaxLength(20)
                .HasDefaultValueSql("'NULL'");
            entity.HasMany<AuthorBook>(d => d.AuthorBooks).WithOne(p => p.Author).HasForeignKey(d => d.AuthorId);
        });

        modelBuilder.Entity<AuthorBook>(entity =>
        {
            entity.ToTable("author_book");

            entity
                .HasKey(it => new { it.AuthorId, it.Isbn });

            entity.HasIndex(e => e.AuthorId, "AuthorId");

            entity.HasIndex(e => e.Isbn, "ISBN");

            entity.Property(e => e.AuthorId).HasColumnType("int(11)").HasColumnName("AuthorId");
            entity.Property(e => e.Isbn)
                .HasMaxLength(17)
                .HasColumnName("ISBN");

            entity.HasOne(d => d.Author).WithMany(d => d.AuthorBooks)
                .HasForeignKey(d => d.AuthorId)
                .HasConstraintName("author_book_ibfk_1");

            entity.HasOne(d => d.IsbnNavigation).WithMany(d => d.AuthorBooks)
                .HasForeignKey(d => d.Isbn)
                .HasConstraintName("author_book_ibfk_2");
        });

        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasKey(e => e.Isbn).HasName("PRIMARY");

            entity.ToTable("books");

            entity.HasIndex(e => e.PublisherId, "PublisherId");

            entity.Property(e => e.Isbn)
                .HasMaxLength(17)
                .HasColumnName("ISBN");
            entity.Property(e => e.Pages).HasColumnType("int(11)");
            entity.Property(e => e.Price).HasPrecision(9);
            entity.Property(e => e.PublisherId).HasColumnType("int(11)");
            entity.Property(e => e.Title).HasMaxLength(200);
            entity.Property(e => e.Year).HasColumnType("year(4)");
            entity.HasMany<AuthorBook>(d => d.AuthorBooks).WithOne(p => p.IsbnNavigation).HasForeignKey(d => d.Isbn);
            entity.HasMany<BookCategory>(d => d.BookCategory).WithOne(p => p.IsbnNavigation).HasForeignKey(d => d.Isbn);

            entity.HasOne(d => d.Publisher).WithMany(p => p.Books)
                .HasForeignKey(d => d.PublisherId)
                .HasConstraintName("books_ibfk_1");
        });

        modelBuilder.Entity<BookCategory>(entity =>
        {
            entity.ToTable("book_category");

            entity
                .HasKey(it => new { it.CategoryId, it.Isbn });

            entity.HasIndex(e => e.CategoryId, "CategoryId");

            entity.HasIndex(e => e.Isbn, "ISBN");

            entity.Property(e => e.CategoryId).HasColumnType("int(11)");
            entity.Property(e => e.Isbn)
                .HasMaxLength(17)
                .HasColumnName("ISBN");

            entity.HasOne(d => d.Category).WithMany(d => d.BookCategory)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("book_category_ibfk_2");

            entity.HasOne(d => d.IsbnNavigation).WithMany(d => d.BookCategory)
                .HasForeignKey(d => d.Isbn)
                .HasConstraintName("book_category_ibfk_1");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("categories");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.Name).HasColumnType("varchar(255)");

            entity.HasMany<BookCategory>(d => d.BookCategory).WithOne(p => p.Category).HasForeignKey(d => d.CategoryId);
        });

        modelBuilder.Entity<Copy>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("copies");

            entity.HasIndex(e => e.Isbn, "ISBN");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.Isbn)
                .HasMaxLength(17)
                .HasColumnName("ISBN");

            entity.HasOne(d => d.IsbnNavigation).WithMany(p => p.Copies)
                .HasForeignKey(d => d.Isbn)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("copies_ibfk_1");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("customers");

            entity.HasIndex(e => e.UserId, "UserId").IsUnique();

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.Address).HasMaxLength(50);
            entity.Property(e => e.AltPhone)
                .HasMaxLength(20)
                .HasDefaultValueSql("'NULL'");
            entity.Property(e => e.FirstName).HasMaxLength(20);
            entity.Property(e => e.LastName).HasMaxLength(20);
            entity.Property(e => e.Patronymic)
                .HasMaxLength(20)
                .HasDefaultValueSql("'NULL'");
            entity.Property(e => e.Phone).HasMaxLength(20);
            entity.Property(e => e.UserId).HasColumnType("int(11)");

            entity.HasOne(d => d.User).WithOne(p => p.Customer)
                .HasForeignKey<Customer>(d => d.UserId)
                .HasConstraintName("customers_ibfk_1");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("orders");

            entity.HasIndex(e => e.CopyId, "CopyId");

            entity.HasIndex(e => e.CustomerId, "CustomerId");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.Borrowed)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("datetime");
            entity.Property(e => e.CopyId).HasColumnType("int(11)");
            entity.Property(e => e.Created)
                .HasDefaultValueSql("'current_timestamp()'")
                .HasColumnType("datetime");
            entity.Property(e => e.CustomerId).HasColumnType("int(11)");
            entity.Property(e => e.Returned)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Copy).WithMany(p => p.Orders)
                .HasForeignKey(d => d.CopyId)
                .HasConstraintName("orders_ibfk_1");

            entity.HasOne(d => d.Customer).WithMany(p => p.Orders)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("orders_ibfk_2");
        });

        modelBuilder.Entity<Publisher>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("publishers");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.Address).HasMaxLength(50);
            entity.Property(e => e.City).HasMaxLength(20);
            entity.Property(e => e.Country).HasMaxLength(20);
            entity.Property(e => e.Name).HasMaxLength(20);
            entity.Property(e => e.Phone).HasMaxLength(20);
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("roles");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.Name).HasMaxLength(20);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("users");

            entity.HasIndex(e => e.RoleId, "users_ibfk_1");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.Login).HasMaxLength(20);
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(60)
                .IsFixedLength();
            entity.Property(e => e.RoleId).HasColumnType("int(11)");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("users_ibfk_1");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
