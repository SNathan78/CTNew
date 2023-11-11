using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CTNew.Models
{
    public partial class CodeTestContext : DbContext
    {
        public CodeTestContext()
        {
        }

        public CodeTestContext(DbContextOptions<CodeTestContext> options)
            : base(options)
        {
        }

        public virtual DbSet<City> City { get; set; }
        public virtual DbSet<State> State { get; set; }
        public virtual DbSet<Users> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
           // if (!optionsBuilder.IsConfigured)
          //  {
//#warningTo protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=.\\SQLExpress;Database=CodeTest;Trusted_Connection=True;TrustServerCertificate=True;");
          //  }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<City>(entity =>
            {
                entity.Property(e => e.CityName)
                    .HasColumnName("CityName")
                    .HasMaxLength(200);

                entity.HasOne(d => d.State)
                    .WithMany(p => p.City)
                    .HasForeignKey(d => d.StateId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_State_StateId");
            });

            modelBuilder.Entity<State>(entity =>
            {
                entity.Property(e => e.StateName)
                    .HasColumnName("StateName")
                    .HasMaxLength(100);

                entity.Property(e => e.StateCode)
                    .HasColumnName("StateCode")
                    .HasMaxLength(5);
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.HasKey(e => e.UserId)
                    .HasName("PK_UserId");

                entity.Property(e => e.FirstName).HasMaxLength(100);

                entity.Property(e => e.Gender)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.LastName).HasMaxLength(100);

                entity.HasOne(d => d.City)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.CityId)
                    .HasConstraintName("FK_City_CityID");

            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
