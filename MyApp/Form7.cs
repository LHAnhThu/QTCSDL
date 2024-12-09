using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyApp
{
    public partial class frmMain : Form
    {
        private string userRole;
        public frmMain(string role)
        {
            InitializeComponent();
            userRole = role;
        }

        private void TrangChu_Load(object sender, EventArgs e)
        {
            // Hiển thị vai trò trong TextBox
            txtRole.Text = userRole;

            if (userRole == "Inspection_staff")
            {
                btnKhachHang.Enabled = false;   
                btnTaiKhoan.Enabled = false;    
            }

            else if (userRole == "Sales_staff")
            {
                btnHang.Enabled = false;    
                btnNguoiBan.Enabled = false;    
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            new frmKhachHang(userRole).Show();
            this.Hide();
        }

        private void btn_Click(object sender, EventArgs e)
        {
            new Form8(userRole).Show();
            this.Hide();
        }
    }
}
