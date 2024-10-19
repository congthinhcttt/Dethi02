using De02_DAL.QLSanPham;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace De02_BUS
{
    public class SanPhamService
    {
        private Model1 dbContext = new Model1();

        // Lấy tất cả sản phẩm, bao gồm loại sản phẩm (LoaiSP)
        public List<Sanpham> GetAllSanpham()
        {
            return dbContext.Sanphams.Include(s => s.LoaiSP).ToList();
        }

        // Lấy tất cả sản phẩm không có loại sản phẩm (LoaiSP)
        public List<Sanpham> GetAllSanphamHasNoLoai()
        {
            return dbContext.Sanphams.Where(s => s.LoaiSP == null).ToList();
        }

        // Lấy tất cả sản phẩm không có loại sản phẩm nhưng thuộc về một nhóm sản phẩm cụ thể (theo FacultyID giống như bên StudentService)
        public List<Sanpham> GetAllSanphamHasNoLoai(string maLoaiSP)
        {
            return dbContext.Sanphams.Where(s => s.LoaiSP == null && s.MaLoai == maLoaiSP).ToList();
        }

        // Tìm sản phẩm theo mã sản phẩm
        public Sanpham FindById(string maSP)
        {
            return dbContext.Sanphams.FirstOrDefault(s => s.MaSP == maSP);
        }
        public Sanpham FindByName(string tenSP)
        {
            return dbContext.Sanphams.FirstOrDefault(sp => sp.TenSP.Equals(tenSP, StringComparison.OrdinalIgnoreCase));
        }

        // Thêm hoặc cập nhật sản phẩm
        public void InsertUpdate(Sanpham sanpham)
        {
            dbContext.Sanphams.AddOrUpdate(sanpham);
            dbContext.SaveChanges();
        }


        // Xóa sản phẩm theo mã sản phẩm
        public void DeleteSanpham(string maSP)
        {
            var sanpham = dbContext.Sanphams.FirstOrDefault(s => s.MaSP == maSP);
            if (sanpham != null)
            {
                dbContext.Sanphams.Remove(sanpham);
                dbContext.SaveChanges();
            }
        }

    }
}
