using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DotNetCoreApp.Models
{
    public partial class libraryContext : DbContext
    {
        public virtual DbSet<Authors> Authors { get; set; }
        public virtual DbSet<Book> Book { get; set; }
        public virtual DbSet<BookAuthors> BookAuthors { get; set; }
        public virtual DbSet<BookLoans> BookLoans { get; set; }
        public virtual DbSet<Borrower> Borrower { get; set; }
        public virtual DbSet<Fines> Fines { get; set; }
        public virtual DbSet<Publisher> Publisher { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer(@"Server=hitesh-pc\sqlexpress;Database=library;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Authors>(entity =>
            {
                entity.HasKey(e => e.AuthorId);

                entity.Property(e => e.AuthorId).HasColumnName("Author_id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Book>(entity =>
            {
                entity.HasKey(e => e.Isbn);

                entity.Property(e => e.Isbn)
                    .HasColumnType("char(10)")
                    .ValueGeneratedNever();

                entity.Property(e => e.Cover)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.HasOne(d => d.Publisher)
                    .WithMany(p => p.Book)
                    .HasForeignKey(d => d.PublisherId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_BOOK_PUBLISHERS");
            });

            modelBuilder.Entity<BookAuthors>(entity =>
            {
                entity.ToTable("Book_Authors");

                entity.Property(e => e.AuthorId).HasColumnName("Author_id");

                entity.Property(e => e.Isbn)
                    .IsRequired()
                    .HasColumnType("char(10)");

                entity.HasOne(d => d.Author)
                    .WithMany(p => p.BookAuthors)
                    .HasForeignKey(d => d.AuthorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_BOOK_AUTHORS_AUTHOR");

                entity.HasOne(d => d.IsbnNavigation)
                    .WithMany(p => p.BookAuthors)
                    .HasForeignKey(d => d.Isbn)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_BOOK_AUTHORS_BOOK");
            });

            modelBuilder.Entity<BookLoans>(entity =>
            {
                entity.HasKey(e => e.LoanId);

                entity.ToTable("Book_Loans");

                entity.Property(e => e.LoanId).HasColumnName("Loan_id");

                entity.Property(e => e.CardId)
                    .IsRequired()
                    .HasColumnName("Card_id")
                    .HasColumnType("char(8)");

                entity.Property(e => e.DateIn)
                    .HasColumnName("Date_in")
                    .HasColumnType("date");

                entity.Property(e => e.DateOut)
                    .HasColumnName("Date_out")
                    .HasColumnType("date");

                entity.Property(e => e.DueDate)
                    .HasColumnName("Due_DATE")
                    .HasColumnType("date");

                entity.Property(e => e.Isbn)
                    .IsRequired()
                    .HasColumnType("char(10)");

                entity.HasOne(d => d.Card)
                    .WithMany(p => p.BookLoans)
                    .HasPrincipalKey(p => p.CardId)
                    .HasForeignKey(d => d.CardId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_BOOK_LOANS_BORROWER");

                entity.HasOne(d => d.IsbnNavigation)
                    .WithMany(p => p.BookLoans)
                    .HasForeignKey(d => d.Isbn)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_BOOK_LOANS_BOOK");
            });

            modelBuilder.Entity<Borrower>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                    .HasColumnType("int")
                    .ValueGeneratedNever();

                entity.HasIndex(e => e.CardId)
                    .HasName("ck_BORROWER")
                    .IsUnique();

                entity.Property(e => e.Address)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Bname)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.CardId)
                    .HasColumnName("Card_id")
                    .HasColumnType("char(8)")
                    .ValueGeneratedOnAdd()
                    .HasComputedColumnSql("(CONVERT([char](8),'ID'+right('000000'+CONVERT([varchar](8),[ID]),(6))))");

                entity.Property(e => e.Email)
                    .HasMaxLength(60)
                    .IsUnicode(false);

                entity.Property(e => e.Phone)
                    .IsRequired()
                    .HasMaxLength(13)
                    .IsUnicode(false);

                entity.Property(e => e.Ssn)
                    .IsRequired()
                    .HasColumnType("char(10)");
            });

            modelBuilder.Entity<Fines>(entity =>
            {
                entity.HasKey(e => e.LoanId);

                entity.Property(e => e.LoanId)
                    .HasColumnName("Loan_id");

                entity.Property(e => e.FineAmt)
                    .HasColumnName("Fine_amt")
                    .HasColumnType("decimal(8, 2)");

                entity.HasOne(d => d.Loan)
                    .WithOne(p => p.Fines)
                    .HasForeignKey<Fines>(d => d.LoanId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_FINES_BOOK_LOANS");
            });

            modelBuilder.Entity<Publisher>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });
        }
    }
}
