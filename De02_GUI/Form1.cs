using De02_BUS;
using De02_DAL.QLSanPham;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace De02_GUI
{
    public partial class Form1 : Form
    {
        public readonly SanPhamService sanPhamService = new SanPhamService();
        public readonly LoaiSPService loaiSPService = new LoaiSPService();
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                setGridViewStyle(dgvSanpham);
                var loaiSPs = loaiSPService.GetAllLoaiSP();  // Lấy tất cả loại sản phẩm
                var sanphams = sanPhamService.GetAllSanpham();  // Lấy tất cả sản phẩm
                FillLoaiSPCombobox(loaiSPs);  // Đổ dữ liệu vào ComboBox loại sản phẩm
                BindGrid(sanphams);  // Đổ dữ liệu vào DataGridView sản phẩm
                //dgvSanpham.SelectionChanged += dgvSanpham_SelectionChanged;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        
        private void FillLoaiSPCombobox(List<LoaiSP> loaiSPs)
        {
            loaiSPs.Insert(0, new LoaiSP());  // Thêm một mục rỗng
            this.cboLoaiSP.DataSource = loaiSPs;
            this.cboLoaiSP.DisplayMember = "TenLoai";
            this.cboLoaiSP.ValueMember = "MaLoai";
        }
        private void BindGrid(List<Sanpham> sanphams)
        {
            dgvSanpham.Rows.Clear();
            foreach (var sanpham in sanphams)
            {
                int index = dgvSanpham.Rows.Add();
                dgvSanpham.Rows[index].Cells[0].Value = sanpham.MaSP;
                dgvSanpham.Rows[index].Cells[1].Value = sanpham.TenSP;
                dgvSanpham.Rows[index].Cells[2].Value = sanpham.Ngaynhap.ToString();
                if (sanpham.LoaiSP != null)
                    dgvSanpham.Rows[index].Cells[3].Value = sanpham.LoaiSP.TenLoai;
            }
        }
        public void setGridViewStyle(DataGridView dgview)
        {
            dgview.BorderStyle = BorderStyle.None;
            dgview.DefaultCellStyle.SelectionBackColor = Color.DarkTurquoise;
            dgview.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgview.BackgroundColor = Color.White;
            dgview.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }

        private void btThem_Click(object sender, EventArgs e)
        {
            try
            {
                // Tạo đối tượng sản phẩm mới
                var sanpham = new Sanpham
                {
                    MaSP = txtMaSP.Text,
                    TenSP = txtTenSP.Text,
                    Ngaynhap = dtNgaynhap.Value,
                    MaLoai = cboLoaiSP.SelectedValue?.ToString()
                };

                // Thêm sản phẩm
                sanPhamService.InsertUpdate(sanpham);

                // Cập nhật DataGridView
                BindGrid(sanPhamService.GetAllSanpham());

                // Thông báo thành công
                MessageBox.Show("Thêm sản phẩm thành công!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void btSua_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvSanpham.SelectedRows.Count > 0)
                {
                    // Lấy mã sản phẩm từ dòng được chọn
                    var maSP = dgvSanpham.SelectedRows[0].Cells[0].Value.ToString();

                    // Tìm sản phẩm theo mã
                    var sanpham = sanPhamService.FindById(maSP);

                    if (sanpham != null)
                    {
                        // Kiểm tra lại giá trị có thể null hoặc không đúng định dạng
                        if (string.IsNullOrEmpty(txtTenSP.Text))
                        {
                            MessageBox.Show("Vui lòng nhập tên sản phẩm.");
                            return;
                        }

                        // Cập nhật thông tin sản phẩm
                        sanpham.TenSP = txtTenSP.Text;

                        // Ngày nhập cần được kiểm tra kỹ
                        if (dtNgaynhap.Value != null)
                        {
                            sanpham.Ngaynhap = dtNgaynhap.Value;
                        }
                        else
                        {
                            MessageBox.Show("Vui lòng chọn ngày nhập.");
                            return;
                        }

                        // Kiểm tra và cập nhật mã loại sản phẩm
                        if (cboLoaiSP.SelectedValue != null)
                        {
                            sanpham.MaLoai = cboLoaiSP.SelectedValue.ToString();
                        }
                        else
                        {
                            MessageBox.Show("Vui lòng chọn loại sản phẩm.");
                            return;
                        }

                        // Cập nhật sản phẩm
                        sanPhamService.InsertUpdate(sanpham);

                        // Cập nhật lại DataGridView
                        BindGrid(sanPhamService.GetAllSanpham());

                        // Thông báo thành công
                        MessageBox.Show("Cập nhật sản phẩm thành công!");
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy sản phẩm để sửa.");
                    }
                }
                else
                {
                    MessageBox.Show("Vui lòng chọn sản phẩm để sửa.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void btXoa_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvSanpham.SelectedRows.Count > 0)
                {
                    // Lấy mã sản phẩm từ dòng được chọn
                    var maSP = dgvSanpham.SelectedRows[0].Cells[0].Value.ToString();

                    // Xác nhận xóa
                    var result = MessageBox.Show("Bạn có chắc chắn muốn xóa sản phẩm này?", "Xác nhận xóa", MessageBoxButtons.YesNo);
                    if (result == DialogResult.Yes)
                    {
                        // Xóa sản phẩm
                        sanPhamService.DeleteSanpham(maSP);

                        // Cập nhật DataGridView
                        BindGrid(sanPhamService.GetAllSanpham());

                        // Thông báo thành công
                        MessageBox.Show("Xóa sản phẩm thành công!");
                    }
                }
                else
                {
                    MessageBox.Show("Vui lòng chọn sản phẩm để xóa.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void btThoat_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Bạn có chắc chắn muốn thoát?", "Xác nhận thoát", MessageBoxButtons.YesNo);

            // Nếu người dùng chọn Yes thì đóng ứng dụng
            if (dialogResult == DialogResult.Yes)
            {
                this.Close();  // Đóng form hiện tại
            }
        }

        private void btTim_Click(object sender, EventArgs e)
        {
            try
            {
                // Lấy mã sản phẩm hoặc tên sản phẩm từ TextBox
                string input = txtTim.Text;

                // Kiểm tra nếu input không rỗng
                if (!string.IsNullOrEmpty(input))
                {
                    // Tìm sản phẩm theo mã hoặc tên
                    var sanpham = sanPhamService.FindById(input); // Tìm theo mã

                    // Nếu không tìm thấy sản phẩm theo mã, thì tìm theo tên
                    if (sanpham == null)
                    {
                        // Tìm theo tên, có thể thêm hàm tìm theo tên sản phẩm
                        sanpham = sanPhamService.FindByName(input);
                    }

                    // Kiểm tra nếu tìm thấy sản phẩm
                    if (sanpham != null)
                    {
                        // Hiển thị thông tin sản phẩm trên DataGridView
                        dgvSanpham.Rows.Clear();
                        int index = dgvSanpham.Rows.Add();
                        dgvSanpham.Rows[index].Cells[0].Value = sanpham.MaSP;
                        dgvSanpham.Rows[index].Cells[1].Value = sanpham.TenSP;
                        dgvSanpham.Rows[index].Cells[2].Value = sanpham.Ngaynhap?.ToString("dd/MM/yyyy");
                        dgvSanpham.Rows[index].Cells[3].Value = sanpham.LoaiSP?.TenLoai;

                        // Thông báo thành công
                        MessageBox.Show("Tìm thấy sản phẩm!");
                    }
                    else
                    {
                        // Thông báo không tìm thấy sản phẩm
                        MessageBox.Show("Không tìm thấy sản phẩm có mã hoặc tên: " + input);
                    }
                }
                else
                {
                    MessageBox.Show("Vui lòng nhập mã hoặc tên sản phẩm.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }

        }

        private void btLuu_Click(object sender, EventArgs e)
        {
            try
            {
                // Giống với logic bạn đã làm trong sự kiện thêm hoặc sửa
                var sanpham = new Sanpham
                {
                    MaSP = txtMaSP.Text,
                    TenSP = txtTenSP.Text,
                    Ngaynhap = dtNgaynhap.Value,
                    MaLoai = cboLoaiSP.SelectedValue?.ToString()
                };

                // Thực hiện lưu dữ liệu
                sanPhamService.InsertUpdate(sanpham);

                // Cập nhật DataGridView
                BindGrid(sanPhamService.GetAllSanpham());

                // Thông báo thành công
                MessageBox.Show("Lưu sản phẩm thành công!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lưu: " + ex.Message);
            }
        }

        private void btKLuu_Click(object sender, EventArgs e)
        {
            // Hủy bỏ các thay đổi, có thể xóa các giá trị đã nhập vào TextBox, ComboBox, v.v.
            txtMaSP.Clear();
            txtTenSP.Clear();
            cboLoaiSP.SelectedIndex = -1; // Hoặc chọn mục mặc định
            dtNgaynhap.Value = DateTime.Now;

            // Thông báo người dùng
            MessageBox.Show("Các thay đổi đã được hủy.");
        }

       

        private void dgvsp_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                if (dgvSanpham.Rows[e.RowIndex].Cells[0].Value != null)
                {
                    txtMaSP.Text = dgvSanpham.Rows[e.RowIndex].Cells[0].Value.ToString();
                    txtTenSP.Text = dgvSanpham.Rows[e.RowIndex].Cells[1].Value.ToString();
                    dtNgaynhap.Text = dgvSanpham.Rows[e.RowIndex].Cells[2].Value.ToString();
                    cboLoaiSP.Text = dgvSanpham.Rows[e.RowIndex].Cells[3].Value.ToString();
                }
            }
        }
    }
}
