using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyApp
{
    public partial class frmDangNhap : Form
    {
        string sCon = "Data Source=DESKTOP-71TIUA9\\MSSQLSERVER02;Initial Catalog=NhatNamFood;Integrated Security=True;TrustServerCertificate=True";
        public frmDangNhap() 
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {
          
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(sCon);
            try
            {
                con.Open();
                string tk = txtTaiKhoan.Text;
                string mk = txtMatKhau.Text;
                string selectedRole = cmbRole.SelectedItem.ToString(); // Lấy vai trò từ ComboBox

                // Gọi thủ tục spSelectTaiKhoan
                SqlCommand cmd = new SqlCommand("spSelectTaiKhoan", con);
                cmd.CommandType = CommandType.StoredProcedure;

                // Thêm tham số
                cmd.Parameters.AddWithValue("@LGName", tk);
                cmd.Parameters.AddWithValue("@Pass", mk);

                // Tham số đầu ra
                SqlParameter retParam = new SqlParameter
                {
                    ParameterName = "@ret",
                    SqlDbType = SqlDbType.Bit,
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(retParam);

                // Thực thi thủ tục
                cmd.ExecuteNonQuery();

                // Lấy giá trị trả về
                if (Convert.ToBoolean(retParam.Value)) // Nếu đăng nhập thành công
                {
                    // Truy vấn vai trò người dùng
                    string role = GetRoleByUsername(tk, con);
                    // So sánh vai trò từ ComboBox với vai trò trong bảng TaiKhoan
                    if (role.Equals(selectedRole, StringComparison.OrdinalIgnoreCase))
                    {
                        if (role.Equals(selectedRole, StringComparison.OrdinalIgnoreCase))
                        {

                            new frmMain(role).Show();

                            this.Hide(); // Ẩn form đăng nhập sau khi thành công
                        }
                    }
                    else
                    {
                        // Thông báo lỗi nếu vai trò không khớp
                        MessageBox.Show("Vai trò bạn chọn không khớp với vai trò trong hệ thống!", "Lỗi vai trò", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Đăng nhập thất bại. Kiểm tra lại tài khoản và mật khẩu.", "Lỗi đăng nhập", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kết nối: " + ex.Message, "Lỗi kết nối", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Truy vấn vai trò người dùng từ bảng TaiKhoan
        private string GetRoleByUsername(string username, SqlConnection con)
        {
            string role = string.Empty;
            try
            {
                // Truy vấn để lấy vai trò người dùng từ bảng TaiKhoan
                SqlCommand cmd = new SqlCommand("SELECT Role_user FROM TaiKhoan WHERE LGName = @LGName", con);
                cmd.Parameters.AddWithValue("@LGName", username);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    role = reader["Role_user"].ToString(); // Lấy vai trò
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi truy vấn vai trò: " + ex.Message);
            }
            return role;
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void txtTaiKhoan_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void txtMatKhau_TextChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
