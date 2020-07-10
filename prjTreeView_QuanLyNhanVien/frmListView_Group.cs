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
    public partial class frmListView_Group : Form
    {
        public frmListView_Group()
        {
            InitializeComponent();
        }

        ClsDatabase db = new ClsDatabase();
        DataTable tblNhanVien, tblPhongBan;

        private void frmListView_Group_Load(object sender, EventArgs e)
        {
            if (!db.ConnectToDatabase())
            {
                MessageBox.Show("Kết nối Database thất bại", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            tblNhanVien = db.GetData("SELECT * FROM NhanVien");
            tblPhongBan = db.GetData("SELECT * FROM PhongBan");
            foreach(DataRow row in tblPhongBan.Rows)
            {
                cboPhongBan.Items.Add(row[1]);
            }
            ListViewItem item;
            foreach(DataRow row in tblNhanVien.Rows)
            {
                item = new ListViewItem(row[0].ToString());
                item.SubItems.Add(row[1].ToString());
                item.SubItems.Add(row[2].ToString());
                item.SubItems.Add((row[3].ToString() == "True") ? "Nam" : "Nữ");
                item.SubItems.Add(row[4].ToString());
                item.SubItems.Add(row[5].ToString());
                item.SubItems.Add(row[6].ToString());
                item.SubItems.Add(row[7].ToString());

                lwNhanVien.Items.Add(item);
            }
        }

        private void lwNhanVien_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(lwNhanVien.SelectedItems.Count == 1)
            {
                ListViewItem item = lwNhanVien.SelectedItems[0];
                txtMaNV.Text = item.SubItems[0].Text;
                txtHoTen.Text = item.SubItems[1].Text;
                radNam.Checked = (item.SubItems[3].Text == "Nam") ? true : false;
                radNu.Checked = !radNam.Checked;
                txtDiaChi.Text = item.SubItems[4].Text;
                txtQueQuan.Text = item.SubItems[5].Text;
            }
        }
    }
}

