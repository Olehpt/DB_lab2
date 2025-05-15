using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using LabDomain.Model;

namespace LabInfrastructure;

public partial class DbLab2Context : DbContext
{
    public DbLab2Context()
    {
    }

    public DbLab2Context(DbContextOptions<DbLab2Context> options)
        : base(options)
    {
    }

    public virtual DbSet<Author> Authors { get; set; }

    public virtual DbSet<Comment> Comments { get; set; }

    public virtual DbSet<Organization> Organizations { get; set; }

    public virtual DbSet<Publication> Publications { get; set; }

    public virtual DbSet<PublicationType> PublicationTypes { get; set; }

    public virtual DbSet<Subject> Subjects { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=USER-PK\\SQLEXPRESS; Database=DB_lab2; Trusted_Connection=True; TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Author>(entity =>
        {
            entity.ToTable("Author");

            entity.Property(e => e.AuthorId)
                .ValueGeneratedNever()
                .HasColumnName("AuthorID");
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Password).HasMaxLength(50);

            entity.HasOne(d => d.OrganizationNavigation).WithMany(p => p.Authors)
                .HasForeignKey(d => d.Organization)
                .HasConstraintName("FK_Author_Organization");
        });

        modelBuilder.Entity<Comment>(entity =>
        {
            entity.ToTable("Comment");

            entity.Property(e => e.CommentId)
                .ValueGeneratedNever()
                .HasColumnName("CommentID");

            entity.HasOne(d => d.AuthorNavigation).WithMany(p => p.Comments)
                .HasForeignKey(d => d.Author)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Comment_Author");

            entity.HasOne(d => d.PublicationNavigation).WithMany(p => p.Comments)
                .HasForeignKey(d => d.Publication)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Comment_Publication");
        });

        modelBuilder.Entity<Organization>(entity =>
        {
            entity.ToTable("Organization");

            entity.Property(e => e.OrganizationId)
                .ValueGeneratedNever()
                .HasColumnName("OrganizationID");
            entity.Property(e => e.Location).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Publication>(entity =>
        {
            entity.ToTable("Publication");

            entity.Property(e => e.PublicationId)
                .ValueGeneratedNever()
                .HasColumnName("PublicationID");
            entity.Property(e => e.Title).HasMaxLength(50);

            entity.HasOne(d => d.AuthorNavigation).WithMany(p => p.Publications)
                .HasForeignKey(d => d.Author)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Publication_Author");

            entity.HasOne(d => d.PublicationTypeNavigation).WithMany(p => p.Publications)
                .HasForeignKey(d => d.PublicationType)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Publication_PublicationType");

            entity.HasOne(d => d.SubjectNavigation).WithMany(p => p.Publications)
                .HasForeignKey(d => d.Subject)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Publication_Subject");
        });

        modelBuilder.Entity<PublicationType>(entity =>
        {
            entity.ToTable("PublicationType");

            entity.Property(e => e.PublicationTypeId)
                .ValueGeneratedNever()
                .HasColumnName("PublicationTypeID");
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Subject>(entity =>
        {
            entity.ToTable("Subject");

            entity.Property(e => e.SubjectId)
                .ValueGeneratedNever()
                .HasColumnName("SubjectID");
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
