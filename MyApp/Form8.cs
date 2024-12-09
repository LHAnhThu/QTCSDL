using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace MyApp
{
    public partial class Form8 : Form
    {

        string sCon = "Data Source=DESKTOP-71TIUA9\\MSSQLSERVER02;Initial Catalog=NhatNamFood;Integrated Security=True;TrustServerCertificate=True";

        private string role;
        public Form8(string userRole)
        {
                InitializeComponent();
                role = userRole;
        }

        private void Form8_Load(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(sCon);
            try
            {
                con.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show("lỗi");
            }
            string sQuery = "select * from Nhap";
            SqlDataAdapter adapter = new SqlDataAdapter(sQuery, con);
            DataSet ds = new DataSet();
            adapter.Fill(ds, "Nhap");
            dataGridView1.DataSource = ds.Tables["Nhap"];
            con.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form9 form9 = new Form9
            {
                IsNew = true
            };
            form9.Show();
            this.Hide();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Lấy mã hóa đơn từ hàng được chọn
            if (e.RowIndex >= 0) // Kiểm tra dòng hợp lệ
            {
                string maHD = dataGridView1.Rows[e.RowIndex].Cells["MaHD"].Value.ToString();
                Form9 form9 = new Form9
                {
                    IsNew = false, // Chế độ xem chi tiết hóa đơn
                    MaHD = maHD
                };
                form9.Show();
                this.Hide();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            new frmMain(role).Show();
            this.Hide();
        }
    }
}
