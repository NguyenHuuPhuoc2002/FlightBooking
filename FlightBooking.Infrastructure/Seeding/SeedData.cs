using FlightBooking.Entities.Entities;
using FlightBooking.Infrastructure.DbContext;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightBooking.Infrastructure.Seeding
{
    public class SeedData
    {
        public static async Task Seed(DataContext context, RoleManager<IdentityRole> roleManager)
        {
            try
            {
                 if (!await roleManager.RoleExistsAsync("Admin"))
                 {
                    var adminRole = new IdentityRole("Admin");
                    var result = await roleManager.CreateAsync(adminRole);
                 }

                // Kiểm tra nếu vai trò "Customer" chưa tồn tại
                 if (!await roleManager.RoleExistsAsync("Customer"))
                 {
                    var customerRole = new IdentityRole("Customer");
                    var result = await roleManager.CreateAsync(customerRole);

                 }

                // Seed Data
           
                // Seed ThanhPho and SanBay
                if (!context.ThanhPhos.Any())
                {
                    var thanhPhoHCM = new ThanhPho { TenThanhPho = "Ho Chi Minh City" };
                    var thanhPhoHN = new ThanhPho { TenThanhPho = "Hanoi" };

                    await context.ThanhPhos.AddRangeAsync(thanhPhoHCM, thanhPhoHN);
                    await context.SaveChangesAsync(); // Lưu để lấy ID

                    var sanBayTanSonNhat = new SanBay
                    {
                        TenSanBay = "Tan Son Nhat",
                        DiaChi = "Ho Chi Minh City",
                        MaThanhPho = thanhPhoHCM.MaThanhPho
                    };
                    var sanBayNoiBai = new SanBay
                    {
                        TenSanBay = "Noi Bai",
                        DiaChi = "Hanoi",
                        MaThanhPho = thanhPhoHN.MaThanhPho
                    };

                    await context.SanBays.AddRangeAsync(sanBayTanSonNhat, sanBayNoiBai);
                }
                
                // Seed DichVu
                if (!context.DichVus.Any())
                {
                    var dichVu1 = new DichVu { TenDichVu = "Wifi", GiaDichVu = 20000 };
                    var dichVu2 = new DichVu { TenDichVu = "Bữa ăn", GiaDichVu = 50000 };

                    await context.DichVus.AddRangeAsync(dichVu1, dichVu2);
                }

                // Seed GiamGia
                if (!context.GiamGias.Any())
                {
                    var giamGia1 = new GiamGia { PhanTram = 10 };
                    var giamGia2 = new GiamGia { PhanTram = 20 };

                    await context.GiamGias.AddRangeAsync(giamGia1, giamGia2);
                }

                // Seed Ghe
                if (!context.Ghes.Any())
                {
                    var ghe1 = new Ghe { LoaiGhe = "Phổ thông", HeSoGia = 1 };
                    var ghe2 = new Ghe { LoaiGhe = "Thương gia", HeSoGia = 2 };

                    await context.Ghes.AddRangeAsync(ghe1, ghe2);
                }
                //Maybay
                if (!context.MayBays.Any())
                {
                    var mayBay1 = new MayBay
                    {
                        TenMayBay = "Boeing 737",
                        HangHangKhong = "Boeing",
                        SoChoNgoi = 180,
                        SoChoNgoiPhoThong = 150,
                        SoChoNgoiThuongGia = 30
                    };
                    var mayBay2 = new MayBay
                    {
                        TenMayBay = "Airbus A320",
                        HangHangKhong = "Airbus",
                        SoChoNgoi = 180,
                        SoChoNgoiPhoThong = 150,
                        SoChoNgoiThuongGia = 30
                    };

                    await context.MayBays.AddRangeAsync(mayBay1, mayBay2);
                }
                await context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi seed dữ liệu: {ex.Message}");
                throw; // Ném lại lỗi để bạn biết
            }
        }
    }
}
