namespace prjTreeView_QuanLyNhanVien
{
    partial class frmTheNV
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.vwTheNV = new CrystalDecisions.Windows.Forms.CrystalReportViewer();
            this.SuspendLayout();
            // 
            // vwTheNV
            // 
            this.vwTheNV.ActiveViewIndex = -1;
            this.vwTheNV.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.vwTheNV.Cursor = System.Windows.Forms.Cursors.Default;
            this.vwTheNV.Dock = System.Windows.Forms.DockStyle.Fill;
            this.vwTheNV.Location = new System.Drawing.Point(0, 0);
            this.vwTheNV.Name = "vwTheNV";
            this.vwTheNV.Size = new System.Drawing.Size(963, 569);
            this.vwTheNV.TabIndex = 0;
            this.vwTheNV.Load += new System.EventHandler(this.vwTheNV_Load);
            // 
            // frmTheNV
            // 
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(963, 569);
            this.Controls.Add(this.vwTheNV);
            this.Name = "frmTheNV";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmTheNV_Load);
            this.ResumeLayout(false);

        }

        private CrystalDecisions.Windows.Forms.CrystalReportViewer vwTheNV;
        #endregion
    }
}