using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace HHTRQDChonTuong.Models
{
    public partial class HeHoTroRaQuyetDinhContext : DbContext
    {
        public HeHoTroRaQuyetDinhContext()
        {
        }

        public HeHoTroRaQuyetDinhContext(DbContextOptions<HeHoTroRaQuyetDinhContext> options)
            : base(options)
        {
        }

        public virtual DbSet<LsketQua> LsketQua { get; set; }
        public virtual DbSet<Nam> Nam { get; set; }
        public virtual DbSet<Nganh> Nganh { get; set; }
        public virtual DbSet<PhuThuoc> PhuThuoc { get; set; }
        public virtual DbSet<TieuChi> TieuChi { get; set; }
        public virtual DbSet<Truong> Truong { get; set; }
        public virtual DbSet<TruongAhp> TruongAhp { get; set; }
        public virtual DbSet<TruongNganh> TruongNganh { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source=DESKTOP-795K8U1\\TRUNGNGHIA;Initial Catalog=HeHoTroRaQuyetDinh;Integrated Security=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LsketQua>(entity =>
            {
                entity.ToTable("LSKetQua");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Ipmac)
                    .HasColumnName("IPMAC")
                    .HasMaxLength(50);

                entity.Property(e => e.MaTruong).HasMaxLength(50);

                entity.Property(e => e.Ngay).HasColumnType("datetime");
            });

            modelBuilder.Entity<Nam>(entity =>
            {
                entity.HasKey(e => e.MaNam);

                entity.ToTable("nam");

                entity.Property(e => e.MaNam)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Nam1).HasColumnName("Nam");
            });

            modelBuilder.Entity<Nganh>(entity =>
            {
                entity.HasKey(e => e.MaNganh);

                entity.ToTable("nganh");

                entity.Property(e => e.MaNganh)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TenNganh).HasMaxLength(150);
            });

            modelBuilder.Entity<PhuThuoc>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("phu_thuoc");

                entity.Property(e => e.MaNam)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.MaNganh)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.MaTruong)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.MaNamNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.MaNam)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_phu_thuoc_nam");

                entity.HasOne(d => d.MaNganhNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.MaNganh)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_phu_thuoc_nganh");

                entity.HasOne(d => d.MaTruongNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.MaTruong)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_phu_thuoc_truong");
            });

            modelBuilder.Entity<TieuChi>(entity =>
            {
                entity.HasKey(e => e.MaTieuChi)
                    .HasName("PK__TieuChi__41F85A35111E2539");

                entity.Property(e => e.MaTieuChi).HasMaxLength(50);

                entity.Property(e => e.TenTieuChi).HasMaxLength(100);
            });

            modelBuilder.Entity<Truong>(entity =>
            {
                entity.HasKey(e => e.MaTruong);

                entity.ToTable("truong");

                entity.Property(e => e.MaTruong)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DiaChi).HasMaxLength(250);

                entity.Property(e => e.Email)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.MoTa).HasMaxLength(1500);

                entity.Property(e => e.Sdt)
                    .HasColumnName("SDT")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TenTruong).HasMaxLength(250);

                entity.Property(e => e.Website)
                    .HasMaxLength(150)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TruongAhp>(entity =>
            {
                entity.ToTable("TruongAHP");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.MaTruong)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TruongNganh>(entity =>
            {
                entity.HasKey(e => new { e.MaTruong, e.MaNganh });

                entity.ToTable("truong_nganh");

                entity.Property(e => e.MaTruong)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.MaNganh)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.MaNganhNavigation)
                    .WithMany(p => p.TruongNganh)
                    .HasForeignKey(d => d.MaNganh)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_truong_nganh_nganh");

                entity.HasOne(d => d.MaTruongNavigation)
                    .WithMany(p => p.TruongNganh)
                    .HasForeignKey(d => d.MaTruong)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_truong_nganh_truong");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
