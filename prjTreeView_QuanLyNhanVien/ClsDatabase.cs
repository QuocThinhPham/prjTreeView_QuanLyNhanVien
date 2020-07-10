using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.IO;

namespace prjTreeView_QuanLyNhanVien
{
    class ClsDatabase
    {
        public String ConnString = @"Data Source=.\SQLEXPRESS;Initial Catalog=QLyNHANVIEN;Integrated Security=True";
        public SqlConnection Conn = new SqlConnection();

        public Boolean ConnectToDatabase()
        {
            try
            {
                if (Conn.State == ConnectionState.Broken || Conn.State == ConnectionState.Closed)
                {
                    Conn.ConnectionString = ConnString;
                    Conn.Open();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public DataTable GetData(string chuoiSQL)
        {
            DataTable tbl = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(chuoiSQL, Conn);
            da.Fill(tbl);
            return tbl;
        }

        public void HienThiCay(TreeView tw, DataTable bangCha, DataTable bangCon, string txtCha, string tagCha, string txtCon, string tagCon)
        {
            tw.Nodes.Clear();
            TreeNode nutCha, nutCon;
            foreach (DataRow dongCha in bangCha.Rows)
            {
                nutCha = new TreeNode();
                nutCha.Text = dongCha[txtCha].ToString();
                nutCha.Tag = dongCha[tagCha].ToString();
                foreach (DataRow dongCon in bangCon.Rows)
                    if (nutCha.Tag.ToString().ToUpper() == dongCon[tagCha].ToString().ToUpper())
                    {
                        nutCon = new TreeNode();
                        nutCon.Text = dongCon[txtCon].ToString() + " (" + dongCon[tagCon] + ")";
                        nutCon.Tag = dongCon[tagCon].ToString();

                        nutCha.Nodes.Add(nutCon);
                    }
                tw.Nodes.Add(nutCha);
            }
        }

        public DataTable LayDLCoDK(string TenBang, string DieuKien = "")
        {
            DataTable tbl = new DataTable();
            string ChuoiSQL = "SELECT * FROM " + TenBang;
            if (DieuKien != "") ChuoiSQL += " WHERE " + DieuKien;
            SqlDataAdapter da = new SqlDataAdapter(ChuoiSQL, Conn);
            da.Fill(tbl);
            return tbl;
        }

        public bool CapNhatBang(string tenBang, DataTable tbl)
        {
            try
            {
                string ChuoiSQL = "SELECT * FROM " + tenBang;
                SqlDataAdapter da = new SqlDataAdapter(ChuoiSQL, Conn);
                SqlCommandBuilder cmdBD = new SqlCommandBuilder(da);
                da.Update(tbl);
                return true;
            }
            catch
            {
                MessageBox.Show("Lỗi cập nhật");
                return false;
            }
        }

        public DataTable LayDLIn(string MaSo, string DuongDanHinh)
        {
            string chuoiSQL = "SELECT MaNV, HoTenNV, Nam, NgaySinh, DiaChi, QueQuan, TenPB, Hinh FROM NhanVien n, PhongBan p WHERE MaNV = '" + MaSo + "' AND n.MaPB = p.MaPB";
            DataTable tblGoc = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(chuoiSQL, Conn);
            da.Fill(tblGoc);
            int CotCuoi = tblGoc.Columns.Count - 1;

            DataTable tblBC = new DataTable();
            DataRow dongBC = tblBC.NewRow();
            FileStream fs;
            BinaryReader br;
            string LinkHinh;
            for (int i = 0; i <= CotCuoi; i++)
            {
                tblBC.Columns.Add(tblGoc.Columns[i].ColumnName, tblGoc.Columns[i].DataType);
                dongBC[i] = tblGoc.Rows[0][i].ToString();
            }
            if (tblGoc.Rows[0][CotCuoi].ToString() != "")
            {
                tblBC.Columns.Add("Anh", System.Type.GetType("System.Byte[]"));
                LinkHinh = DuongDanHinh + tblGoc.Rows[0][CotCuoi].ToString();
                fs = new FileStream(LinkHinh, FileMode.Open);
                br = new BinaryReader(fs);
                byte[] mb = new byte[fs.Length];
                mb = br.ReadBytes(Convert.ToInt32(fs.Length));
                dongBC[CotCuoi + 1] = mb;
                br.Close();
                fs.Close();
            }
            tblBC.Rows.Add(dongBC);
            return tblBC;
        }

        public DataTable LayDLPB_NV(string MaPhong)
        {
            string chuoiSQL = "SELECT p.MaPB, TenPB, SoDT, MaNV, HoTenNV, Nam, NgaySinh, QueQuan, DiaChi FROM PhongBan p, NhanVien n WHERE p.MaPB = '" +
                                MaPhong + "' AND p.MaPB = n.MaPB";
            DataTable tbl = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(chuoiSQL, Conn);
            da.Fill(tbl);
            return tbl;
        }

        public bool ThemNhanVien(string MaNV, string HoTen, DateTime NgaySinh, bool Nam, string DiaChi, string QueQuan, string MaPB, string Hinh)
        {
            try
            {
                string sqlQuery = string.Format("EXEC PROC_InsertNV N'{0}', N'{1}', '{2}', {3}, N'{4}', N'{5}', N'{6}', N'{7}'", 
                                        MaNV, HoTen, NgaySinh.ToString("yyyy-MM-dd"), (Nam == true ? 1 : 0), DiaChi, QueQuan, MaPB, Hinh);
                SqlCommand cmd = new SqlCommand(sqlQuery, Conn);
                cmd.ExecuteNonQuery();
                return true;
            }
            catch
            {
                MessageBox.Show("Lỗi thêm mới");
                return false;
            }
        }

        public bool SuaNhanVien(string MaNV, string HoTen, DateTime NgaySinh, bool Nam, string DiaChi, string QueQuan, string MaPB, string Hinh)
        {
            try
            {
                string sqlQuery = string.Format("EXEC PROC_UpdateNV N'{0}', N'{1}', '{2}', {3}, N'{4}', N'{5}', N'{6}', N'{7}'",
                                        MaNV, HoTen, NgaySinh.ToString("yyyy-MM-dd"), (Nam == true ? 1 : 0), DiaChi, QueQuan, MaPB, Hinh);
                SqlCommand cmd = new SqlCommand(sqlQuery, Conn);
                cmd.ExecuteNonQuery();
                return true;
            }
            catch
            {
                MessageBox.Show("Lỗi sửa nhân viên");
                return false;
            }
        }

        public bool XoaNhanVien(string MaNV)
        {
            try
            {
                string sqlQuery = string.Format("EXEC PROC_DeleteNV N'{0}'", MaNV);
                SqlCommand cmd = new SqlCommand(sqlQuery, Conn);
                cmd.ExecuteNonQuery();
                return true;
            }
            catch
            {
                MessageBox.Show("Lỗi xóa nhân viên");
                return false;
            }
        }
    }
}
