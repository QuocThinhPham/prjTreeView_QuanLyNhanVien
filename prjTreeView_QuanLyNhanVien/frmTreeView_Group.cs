using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace prjTreeView_QuanLyNhanVien
{
    public partial class frmTreeView_Group : Form
    {
        public frmTreeView_Group()
        {
            InitializeComponent();
        }

        ClsDatabase db = new ClsDatabase();
        DataTable tblPhongBan, tblNhanVien;
        string ImagePath = Application.StartupPath;
        string ImageName = "", sMaPB = "", sMaNV = "";
        int IndexPB = -1, IndexNV = -1;
        bool New = false;

        private void frmMain_Load(object sender, EventArgs e)
        {
            if (!db.ConnectToDatabase())
            {
                MessageBox.Show("Kết Nối Database thất bại", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            ImagePath = ImagePath.Substring(0, ImagePath.LastIndexOf("bin", StringComparison.OrdinalIgnoreCase)) + @"\Hinh\";
            tblPhongBan = db.GetData("SELECT * FROM PhongBan");
            tblNhanVien = db.GetData("SELECT * FROM NhanVien");
            db.HienThiCay(twPhongBan, tblPhongBan, tblNhanVien, "TenPB", "MaPB", "HoTenNV", "MaNV");
            // if (twPhongBan.Nodes.Count > 0) twPhongBan.ExpandAll();
            MoDKh(false);
        }

        private void MoDKh(bool Mo)
        {
            txtMaNV.ReadOnly = !Mo;
            txtHoTen.ReadOnly = !Mo;
            txtQueQuan.ReadOnly = !Mo;
            txtDiaChi.ReadOnly = !Mo;
            grpPhai.Enabled = Mo;
            dtpNgaySinh.Enabled = Mo;
            grpHinh.Enabled = Mo;
            grpTSX.Enabled = !Mo;
            twPhongBan.Enabled = !Mo;
        }

        public void BuocDL()
        {
            txtMaNV.DataBindings.Clear();
            txtHoTen.DataBindings.Clear();
            txtQueQuan.DataBindings.Clear();
            txtDiaChi.DataBindings.Clear();
            radNam.DataBindings.Clear();
            dtpNgaySinh.DataBindings.Clear();
            txtHinh.DataBindings.Clear();
            txtMaPB.DataBindings.Clear();

            txtMaNV.DataBindings.Add("Text", tblNhanVien, "MaNV", true);
            txtHoTen.DataBindings.Add("Text", tblNhanVien, "HoTenNV", true);
            txtQueQuan.DataBindings.Add("Text", tblNhanVien, "QueQuan", true);
            txtDiaChi.DataBindings.Add("Text", tblNhanVien, "DiaChi", true);
            radNam.DataBindings.Add("Checked", tblNhanVien, "Nam", true);
            dtpNgaySinh.DataBindings.Add("Value", tblNhanVien, "NgaySinh", true);
            txtHinh.DataBindings.Add("Text", tblNhanVien, "Hinh", true);
            txtMaPB.DataBindings.Add("Text", tblNhanVien, "MaPB", true);
        }

        private void XoaND()
        {
            txtMaNV.Clear();
            txtHoTen.Clear();
            txtQueQuan.Clear();
            txtDiaChi.Clear();
            radNam.Checked = true;
            dtpNgaySinh.Value = DateTime.Now;
            txtHinh.Clear();
            txtMaPB.Clear();
        }

        private void HienThiThem()
        {
            radNu.Checked = !radNam.Checked;
            ImageName = txtHinh.Text;
            if (ImageName != "")
                ptbHinh.Image = Image.FromFile(ImagePath + ImageName);
            else
                ptbHinh.Image = null;
        }

        private void twPhongBan_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (twPhongBan.SelectedNode != null)
            {
                TreeNode NutChon = twPhongBan.SelectedNode;
                if (NutChon.Parent == null)
                {
                    IndexPB = NutChon.Index;
                    IndexNV = -1;
                    sMaPB = NutChon.Tag.ToString();
                    sMaNV = "";
                    BuocDL();
                    XoaND();
                    btnSua.Enabled = false;
                    btnXoa.Enabled = false;
                    btnIn.Enabled = false;
                }
                else
                {
                    IndexPB = NutChon.Parent.Index;
                    IndexNV = NutChon.Index;
                    sMaPB = NutChon.Parent.Tag.ToString();
                    sMaNV = NutChon.Tag.ToString();
                    tblNhanVien = db.LayDLCoDK("NhanVien", "MaNV ='" + sMaNV + "'");
                    BuocDL();
                    HienThiThem();
                    btnSua.Enabled = true;
                    btnXoa.Enabled = true;
                    btnIn.Enabled = true;
                }
            }
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            DialogResult tb = MessageBox.Show("Bạn chắc chắn muốn thoát ?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (tb == DialogResult.Yes)
                Application.Exit();
        }

        private void XoaThem()
        {
            radNam.Checked = false;
            dtpNgaySinh.ResetText();
            dtpNgaySinh.Value = DateTime.Today.AddYears(-18);
            txtMaPB.Text = sMaPB;
            txtHinh.Text = "";
            HienThiThem();
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            New = true;
            XoaND();
            this.BindingContext[tblNhanVien].AddNew();
            XoaThem();
            MoDKh(true);
            
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            MoDKh(true);
            txtMaNV.ReadOnly = true;
            txtHoTen.Focus();
        }

        private void btnKhong_Click(object sender, EventArgs e)
        {
            if (IndexNV != -1)
            {
                twPhongBan.SelectedNode = twPhongBan.Nodes[IndexPB].Nodes[IndexNV];
            }
            else
            {
                twPhongBan.SelectedNode = twPhongBan.Nodes[IndexPB];
            }
            twPhongBan_AfterSelect(sender, null);
            twPhongBan.Focus();
            New = false;
            MoDKh(false);
        }

        private void TBao(string chuoiTB, string TieuDe = "Kiểm Tra Thông Tin")
        {
            MessageBox.Show(chuoiTB, TieuDe);
        }

        private bool KTTT()
        {
            if (txtMaNV.Text.Trim() == "")
            {
                TBao("Xin cho biết mã nhân viên");
                txtMaNV.SelectAll();
                txtMaNV.Focus();
                return false;
            }
            if (txtHoTen.Text.Trim() == "")
            {
                TBao("Xin cho biết họ tên nhân viên");
                txtHoTen.SelectAll();
                txtHoTen.Focus();
                return false;
            }
            if (txtQueQuan.Text.Trim() == "")
            {
                TBao("Xin cho biết quê quán nhân viên");
                txtQueQuan.SelectAll();
                txtQueQuan.Focus();
                return false;
            }
            if (txtDiaChi.Text.Trim() == "")
            {
                TBao("Xin cho biết địa chỉ nhân viên");
                txtDiaChi.SelectAll();
                txtDiaChi.Focus();
                return false;
            }
            return true;
        }

        private void TimNV(string Ma)
        {
            TreeNode NutCha = twPhongBan.Nodes[IndexPB];
            foreach (TreeNode nCon in NutCha.Nodes)
            {
                if (nCon.Tag.ToString().ToUpper() == Ma.ToUpper())
                {
                    IndexNV = nCon.Index;
                    twPhongBan.SelectedNode = twPhongBan.Nodes[IndexPB].Nodes[IndexNV];
                    twPhongBan.Focus();
                    return;
                }
            }
        }

        private void btnIn_Click(object sender, EventArgs e)
        {
            if(ptbHinh.Image == null)
            {
                TBao("Nhân viên chưa có hình nên không thể in thẻ", "In thẻ nhân viên");
            }
            else
            {
                ptbHinh.Image.Dispose();    //đóng file hình đang mở
                frmTheNV fr = new frmTheNV();
                fr.Ma = txtMaNV.Text;
                fr.ShowDialog();
                ImageName = txtHinh.Text;
                if(ImageName != "")
                    ptbHinh.Image = Image.FromFile(ImagePath + ImageName);
                else
                    ptbHinh.Image = null;
            }
        }

        private void HienThiLaiCay()
        {
            tblNhanVien = db.GetData("SELECT * FROM NhanVien");
            db.HienThiCay(twPhongBan, tblPhongBan, tblNhanVien, "TenPB", "MaPB", "HoTenNV", "MaNV");
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            if (!KTTT()) return;
            if (New)
            {
                DataTable tbl = new DataTable();
                tbl = db.LayDLCoDK("NhanVien", "MaNV = '" + txtMaNV.Text + "'");
                if (tbl.Rows.Count > 0)
                {
                    TBao("Mã số này đã có, hãy cho mã số khác", "Kiểm Tra Mã Số");
                    txtMaNV.SelectAll();
                    txtMaNV.Focus();
                    return;
                }
            }
            this.BindingContext[tblNhanVien].EndCurrentEdit();
            if (!db.CapNhatBang("NhanVien", tblNhanVien))
            {
                return;
            }
            string MaSoTim = txtMaNV.Text;
            MoDKh(false);
            New = false;
            HienThiLaiCay();
            MoDKh(false);
            TimNV(MaSoTim);
            New = false;
        }

        private void TimPhong()
        {
            twPhongBan.CollapseAll();
            twPhongBan.Nodes[IndexPB].Expand();
            twPhongBan.SelectedNode = twPhongBan.Nodes[IndexPB];
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            DialogResult tb = MessageBox.Show("Bạn thật sự muốn xóa nhân viên này không?", "Cảnh báo, xóa dữ liệu", MessageBoxButtons.YesNo);
            if (tb == DialogResult.Yes)
            {
                this.BindingContext[tblNhanVien].RemoveAt(0);
                if (!db.CapNhatBang("NhanVien",tblNhanVien)) return;
                HienThiLaiCay();
                HienThiThem();
                TimPhong();
                MoDKh(false);
                // Moi = false;
            }
        }
    }
}
