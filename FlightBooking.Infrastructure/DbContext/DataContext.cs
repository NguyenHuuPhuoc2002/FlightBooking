using FlightBooking.Entities.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightBooking.Infrastructure.DbContext
{
    public class DataContext : IdentityDbContext<ApplicationUser>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }
        public DbSet<ChiTietDichVu> ChiTietDichVus { get; set; }
        public DbSet<ChiTietLienHe> ChiTietLienHes { get; set; }
        public DbSet<ChuyenBay> ChuyenBays { get; set; }
        public DbSet<DichVu> DichVus{ get; set; }
        public DbSet<GiamGia> GiamGias { get; set; }
        public DbSet<LoaiGhe> LoaiGhe { get; set; }
        public DbSet<Ghe> Ghes{ get; set; }
        public DbSet<MayBay> MayBays{ get; set; }
        public DbSet<SanBay> SanBays{ get; set; }
        public DbSet<ThanhPho> ThanhPhos{ get; set; }
        public DbSet<TuyenBay> TuyenBays{ get; set; }
        public DbSet<Ve> Ves{ get; set; }
        public DbSet<RefreshToken> RefreshTokens{ get; set; }

       
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            #region SanBay
            modelBuilder.Entity<SanBay>(entity =>
            {
                entity.ToTable(nameof(SanBay));

                entity.Property(e => e.TenSanBay)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(e => e.DiaChi)
                      .IsRequired()
                      .HasMaxLength(200);

                entity.HasKey(sb => sb.MaSanBay);
                entity.HasOne(sb => sb.ThanhPho)
                      .WithMany(e => e.SanBays)
                      .HasForeignKey(e => e.MaThanhPho)
                      .OnDelete(DeleteBehavior.Restrict);
                
            });
            #endregion

            #region ThanhPho
            modelBuilder.Entity<ThanhPho>(entity =>
            {
                entity.ToTable(nameof(ThanhPho));
                entity.Property(e => e.TenThanhPho)
                      .IsRequired()
                      .HasMaxLength(50);

                entity.HasKey(e => e.MaThanhPho);
            });
            #endregion

            #region TuyenBay
            modelBuilder.Entity<TuyenBay>(entity =>
            {
                entity.ToTable(nameof(TuyenBay));

                entity.Property(e => e.KhoangCach).IsRequired();

                entity.HasKey(e => e.MaTuyenBay);

                entity.HasOne(tb => tb.ThanhPhoDi)
                      .WithMany()
                      .HasForeignKey(tb => tb.MaThanhPhoDi)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(tb => tb.ThanhPhoDen)
                      .WithMany()
                      .HasForeignKey(tb => tb.MaThanhPhoDen)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(tb => tb.SanBay)
                      .WithMany(sb => sb.TuyenBays)
                      .HasForeignKey(tb => tb.MaSanBay)
                      .OnDelete(DeleteBehavior.Restrict);
            });
            #endregion

            #region ChuyenBay
            modelBuilder.Entity<ChuyenBay>(entity =>
            {
                entity.ToTable(nameof(ChuyenBay));

                entity.Property(e => e.GioBay)
                      .IsRequired();
                entity.Property(e => e.NgayBay)
                      .IsRequired();
                entity.Property(e => e.GioDen)
                      .IsRequired();
                entity.Property(e => e.TrangThai)
                      .IsRequired();
                entity.Property(e => e.GiaVe)
                      .IsRequired();
                entity.Property(e => e.TrangThai)
                      .IsRequired();

                entity.HasKey(sb => sb.MaChuyenBay);

                entity.HasOne(sb => sb.TuyenBay)
                      .WithMany(e => e.ChuyenBays)
                      .HasForeignKey(e => e.MaTuyenBay)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(sb => sb.MayBay)
                      .WithMany(e => e.ChuyenBays)
                      .HasForeignKey(e => e.MaMayBay)
                      .OnDelete(DeleteBehavior.Restrict);
            });
            #endregion

            #region MayBay
            modelBuilder.Entity<MayBay>(entity =>
            {
                entity.ToTable(nameof(MayBay));

                entity.Property(e => e.TenMayBay)
                      .IsRequired()
                      .HasMaxLength(100);
                entity.Property(e => e.HangHangKhong)
                      .IsRequired()
                      .HasMaxLength(100);
                entity.Property(e => e.SoChoNgoi)
                      .IsRequired();
                entity.Property(e => e.SoChoNgoiPhoThong)
                      .IsRequired();
                entity.Property(e => e.SoChoNgoiThuongGia)
                      .IsRequired();

                entity.HasKey(sb => sb.MaMayBay);
                
            });
            #endregion

            #region Ve
            modelBuilder.Entity<Ve>(entity =>
            {
                entity.ToTable(nameof(Ve));

                entity.Property(e => e.NgayDatVe)
                     .IsRequired();
                     
                entity.Property(e => e.TrangThai)
                      .IsRequired();

                entity.Property(e => e.MaVe)
                      .ValueGeneratedNever();

                entity.HasKey(sb => sb.MaVe);
                entity.HasOne(sb => sb.GiamGia)
                      .WithMany(e => e.Ves)
                      .HasForeignKey(e => e.MaGiamGia);

                entity.HasOne(sb => sb.ChuyenBay)
                      .WithMany(e => e.ves)
                      .HasForeignKey(e => e.MaChuyenBay);

                entity.HasOne(sb => sb.Ghe)
                      .WithMany(e => e.Ves)
                      .HasForeignKey(e => e.MaGhe)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.Property(e => e.MaGhe)
                      .HasMaxLength(10);

                entity.HasOne(v => v.ChiTietLienHe)
                    .WithOne(ct => ct.Ve)
                    .HasForeignKey<ChiTietLienHe>(ct => ct.MaVe)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(v => v.ChiTietDichVus)
                      .WithOne(dv => dv.Ve) 
                      .HasForeignKey(dv => dv.MaVe)
                      .OnDelete(DeleteBehavior.Cascade);

            });
            #endregion

            #region GiamGia
            modelBuilder.Entity<GiamGia>(entity =>
            {
                entity.ToTable(nameof(GiamGia));

                entity.Property(e => e.PhanTram)
                      .IsRequired()
                      .HasMaxLength(11);

                entity.HasKey(sb => sb.MaGiamGia);
                
            });
            #endregion

            #region ChiTietDichVu
            modelBuilder.Entity<ChiTietDichVu>(entity =>
            {
                entity.ToTable(nameof(ChiTietDichVu));

                entity.HasKey(sb => sb.MaChiTietDV);
               
                entity.HasOne(sb => sb.DichVu)
                      .WithMany(e => e.ChiTietDichVus)
                      .HasForeignKey(e => e.MaDichVu);

            });
            #endregion

            #region DichVu
            modelBuilder.Entity<DichVu>(entity =>
            {
                entity.ToTable(nameof(DichVu));

                entity.Property(e => e.TenDichVu)
                      .IsRequired()
                      .HasMaxLength(100);
                entity.Property(e => e.GiaDichVu)
                     .IsRequired();

                entity.HasKey(sb => sb.MaDichVu);
            });
            #endregion

            #region Ghe
            modelBuilder.Entity<Ghe>(entity =>
            {
                entity.ToTable(nameof(Ghe));

                entity.HasOne(sb => sb.LoaiGhe)
                      .WithMany(e => e.Ghes)
                      .HasForeignKey(e => e.MaLoaiGhe);
                entity.HasKey(sb => sb.MaGhe);
                entity.Property(e => e.MaGhe)
                      .ValueGeneratedNever();
            });
            #endregion

            #region LoaiGhe
            modelBuilder.Entity<LoaiGhe>(entity =>
            {
                entity.ToTable(nameof(LoaiGhe));
                entity.HasKey(e => e.MaLoaiGhe);
                entity.Property(e => e.TenLoaiGhe)
                      .IsRequired()
                      .HasMaxLength(20);
                entity.Property(e => e.MaLoaiGhe)
                      .HasMaxLength(50)
                      .ValueGeneratedNever();
                entity.Property(e => e.HeSoGia)
                    .IsRequired();
            });
            #endregion

            #region ChiTietLienHe
            modelBuilder.Entity<ChiTietLienHe>(entity =>
            {
                entity.HasKey(e => e.MaChiTietLH);
                entity.ToTable(nameof(ChiTietLienHe));
                entity.Property(e => e.HoTen)
                      .IsRequired();
                entity.Property(e => e.Email)
                      .IsRequired();
                entity.Property(e => e.SDT)
                      .IsRequired()
                      .HasMaxLength(11);
               
            });

            #endregion
        }
    }
}
