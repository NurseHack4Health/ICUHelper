using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace NurseHackLogin.ModelDB
{
    public partial class nursehackdbContext : DbContext
    {
        public nursehackdbContext()
        {
        }

        public nursehackdbContext(DbContextOptions<nursehackdbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Gender> Gender { get; set; }
        public virtual DbSet<IdentificationNumber> IdentificationNumber { get; set; }
        public virtual DbSet<Rol> Rol { get; set; }
        public virtual DbSet<RolType> RolType { get; set; }
        public virtual DbSet<TypeIdentification> TypeIdentification { get; set; }
        public virtual DbSet<UserAuth> UserAuth { get; set; }
        public virtual DbSet<Users> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=nursehack.database.windows.net;Database=nursehackdb;Integrated Security=False;User ID=isacalderon;Password=SuperSecret!;Connect Timeout=30;Encrypt=True;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Gender>(entity =>
            {
                entity.ToTable("gender");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Value)
                    .IsRequired()
                    .HasColumnName("value")
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<IdentificationNumber>(entity =>
            {
                entity.ToTable("identification_number");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.TypeIdentificationId).HasColumnName("type_identification_id");

                entity.Property(e => e.Value).HasColumnName("value");

                entity.HasOne(d => d.TypeIdentification)
                    .WithMany(p => p.IdentificationNumber)
                    .HasForeignKey(d => d.TypeIdentificationId)
                    .HasConstraintName("identification_number_fk");
            });

            modelBuilder.Entity<Rol>(entity =>
            {
                entity.ToTable("rol");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.RolTypeId).HasColumnName("rol_type_id");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.RolType)
                    .WithMany(p => p.Rol)
                    .HasForeignKey(d => d.RolTypeId)
                    .HasConstraintName("rol_FK");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Rol)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("rol_FK_1");
            });

            modelBuilder.Entity<RolType>(entity =>
            {
                entity.ToTable("rol_type");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Value)
                    .IsRequired()
                    .HasColumnName("value")
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TypeIdentification>(entity =>
            {
                entity.ToTable("type_identification");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Value)
                    .IsRequired()
                    .HasColumnName("value")
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<UserAuth>(entity =>
            {
                entity.ToTable("user_auth");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Email)
                    .HasColumnName("email")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasColumnName("password")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserAuth)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("user_auth_fk");
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.ToTable("users");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.DateOfBirth)
                    .HasColumnName("date_of_birth")
                    .HasColumnType("date");

                entity.Property(e => e.EmergencyContact)
                    .IsRequired()
                    .HasColumnName("emergency_contact")
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.FullName)
                    .HasColumnName("full_name")
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.GenderId).HasColumnName("gender_id");

                entity.Property(e => e.IdentificationNumberId).HasColumnName("identification_number_id");

                entity.Property(e => e.Phone)
                    .IsRequired()
                    .HasColumnName("phone")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.PhoneEmergencyContact)
                    .IsRequired()
                    .HasColumnName("phone_emergency_contact")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.HasOne(d => d.Gender)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.GenderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("user_gender_fk");

                entity.HasOne(d => d.IdentificationNumber)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.IdentificationNumberId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("user_fk");
            });

            modelBuilder.HasSequence<int>("genderseq").StartsAt(4);

            modelBuilder.HasSequence<int>("rol_seq");

            modelBuilder.HasSequence<int>("type_identification_seq");

            modelBuilder.HasSequence<int>("vitals_seq");

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
