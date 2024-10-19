using De02_DAL.QLSanPham;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace De02_BUS
{
    public class LoaiSPService
    {
        private Model1 dbContext = new Model1();

        // Lấy tất cả các loại sản phẩm
        public List<LoaiSP> GetAllLoaiSP()
        {
            return dbContext.LoaiSPs.ToList();
        }

        // Lấy tất cả các loại sản phẩm (phiên bản đơn giản hơn)
        public List<LoaiSP> GetAll()
        {
            return dbContext.LoaiSPs.ToList();
        }

        // Thêm mới loại sản phẩm
        public void AddLoaiSP(LoaiSP loaiSP)
        {
            dbContext.LoaiSPs.Add(loaiSP);
            dbContext.SaveChanges();
        }

        // Cập nhật loại sản phẩm
        public void UpdateLoaiSP(LoaiSP loaiSP)
        {
            dbContext.Entry(loaiSP).State = EntityState.Modified;
            dbContext.SaveChanges();
        }

        // Xóa loại sản phẩm theo mã loại
        public void DeleteLoaiSP(string maLoai)
        {
            var loaiSP = dbContext.LoaiSPs.Find(maLoai);
            if (loaiSP != null)
            {
                dbContext.LoaiSPs.Remove(loaiSP);
                dbContext.SaveChanges();
            }
        }

    }
}
