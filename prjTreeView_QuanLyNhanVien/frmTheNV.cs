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
    public partial class frmTheNV : Form
    {
        ClsDatabase dl = new ClsDatabase();
        string DuongDanHinh = Application.StartupPath;
        string sMa;

        public string Ma
        {
            get { return sMa; }
            set { sMa = value; }
        }

        public frmTheNV()
        {
            InitializeComponent();
        }

        private void vwTheNV_Load(object sender, EventArgs e)
        {
            //if (!dl.ConnectToDatabase())
            //{
            //    Close();
            //    return;
            //}
            //DuongDanHinh = DuongDanHinh.Substring(0, DuongDanHinh.LastIndexOf("Bin", StringComparison.OrdinalIgnoreCase)) + @"\Hinh\";
            //DataTable tbl = dl.LayDLIn(Ma, DuongDanHinh);

            //rptTheNV rpt = new rptTheNV();
            //rpt.SetDataSource(tbl);
            //vwTheNV.ReportSource = rpt;
        }

        private void frmTheNV_Load(object sender, EventArgs e)
        {
            if (!dl.ConnectToDatabase())
            {
                Close();
                return;
            }
            DuongDanHinh = DuongDanHinh.Substring(0, DuongDanHinh.LastIndexOf("Bin", StringComparison.OrdinalIgnoreCase)) + @"\Hinh\";
            DataTable tbl = dl.LayDLIn(Ma, DuongDanHinh);

            rptTheNV rpt = new rptTheNV();
            rpt.SetDataSource(tbl);
            vwTheNV.ReportSource = rpt;
        }
    }
}
