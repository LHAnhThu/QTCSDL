namespace MyApp
{
    partial class frmMain
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
            this.txtRole = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnKhachHang = new System.Windows.Forms.Button();
            this.btnNguoiBan = new System.Windows.Forms.Button();
            this.btn = new System.Windows.Forms.Button();
            this.btnHang = new System.Windows.Forms.Button();
            this.btnTaiKhoan = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtRole
            // 
            this.txtRole.BackColor = System.Drawing.SystemColors.Info;
            this.txtRole.Location = new System.Drawing.Point(661, 24);
            this.txtRole.Multiline = true;
            this.txtRole.Name = "txtRole";
            this.txtRole.Size = new System.Drawing.Size(111, 33);
            this.txtRole.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(506, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 20);
            this.label1.TabIndex = 1;
            // 
            // btnKhachHang
            // 
            this.btnKhachHang.Location = new System.Drawing.Point(35, 133);
            this.btnKhachHang.Name = "btnKhachHang";
            this.btnKhachHang.Size = new System.Drawing.Size(171, 31);
            this.btnKhachHang.TabIndex = 2;
            this.btnKhachHang.Text = "Quản lý Khách hàng";
            this.btnKhachHang.UseVisualStyleBackColor = true;
            this.btnKhachHang.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnNguoiBan
            // 
            this.btnNguoiBan.Location = new System.Drawing.Point(35, 186);
            this.btnNguoiBan.Name = "btnNguoiBan";
            this.btnNguoiBan.Size = new System.Drawing.Size(171, 31);
            this.btnNguoiBan.TabIndex = 6;
            this.btnNguoiBan.Text = "Quản lý Người bán";
            this.btnNguoiBan.UseVisualStyleBackColor = true;
            // 
            // btn
            // 
            this.btn.Location = new System.Drawing.Point(35, 292);
            this.btn.Name = "btn";
            this.btn.Size = new System.Drawing.Size(171, 31);
            this.btn.TabIndex = 7;
            this.btn.Text = "Quản lý ";
            this.btn.UseVisualStyleBackColor = true;
            this.btn.Click += new System.EventHandler(this.btn_Click);
            // 
            // btnHang
            // 
            this.btnHang.Location = new System.Drawing.Point(35, 240);
            this.btnHang.Name = "btnHang";
            this.btnHang.Size = new System.Drawing.Size(171, 31);
            this.btnHang.TabIndex = 4;
            this.btnHang.Text = "Quản lý Hàng hóa";
            this.btnHang.UseVisualStyleBackColor = true;
            // 
            // btnTaiKhoan
            // 
            this.btnTaiKhoan.Location = new System.Drawing.Point(35, 68);
            this.btnTaiKhoan.Name = "btnTaiKhoan";
            this.btnTaiKhoan.Size = new System.Drawing.Size(171, 31);
            this.btnTaiKhoan.TabIndex = 5;
            this.btnTaiKhoan.Text = "Quản lý Tài khoản";
            this.btnTaiKhoan.UseVisualStyleBackColor = true;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btn);
            this.Controls.Add(this.btnNguoiBan);
            this.Controls.Add(this.btnTaiKhoan);
            this.Controls.Add(this.btnHang);
            this.Controls.Add(this.btnKhachHang);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtRole);
            this.Name = "frmMain";
            this.Text = "Form7";
            this.Load += new System.EventHandler(this.TrangChu_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtRole;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnKhachHang;
        private System.Windows.Forms.Button btnNguoiBan;
        private System.Windows.Forms.Button btn;
        private System.Windows.Forms.Button btnHang;
        private System.Windows.Forms.Button btnTaiKhoan;
    }
}