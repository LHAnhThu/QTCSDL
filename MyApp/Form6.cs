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
using System.Xml;

namespace MyApp
{
    public partial class frmKhachHang : Form
    {
        string sCon = "Data Source=DESKTOP-71TIUA9\\MSSQLSERVER02;Initial Catalog=NhatNamFood;Integrated Security=True;TrustServerCertificate=True";

        private string role;
        public frmKhachHang(string userRole)
        {
            InitializeComponent();
            role = userRole;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(sCon);
            try
            {
                con.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi mở kết nối: " + ex.Message);
                return;
            }

            // Lấy thông tin từ các textbox
            string sTenKH = txtTenKH.Text.Trim();
            string sDiaChi = txtDiaChi.Text.Trim();

            // Kiểm tra trùng tên khách hàng và địa chỉ
            try
            {
                string checkQuery = "SELECT COUNT(*) FROM KhachHang WHERE TenKH = @tenKH AND DiaChi = @diaChi";
                SqlCommand checkCmd = new SqlCommand(checkQuery, con);
                checkCmd.Parameters.AddWithValue("@tenKH", sTenKH);
                checkCmd.Parameters.AddWithValue("@diaChi", sDiaChi);

                int count = (int)checkCmd.ExecuteScalar();
                if (count > 0)
                {
                    MessageBox.Show("Khách hàng đã tồn tại!");
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi kiểm tra khách hàng: " + ex.Message);
                return;
            }

            // Lấy mã khách hàng mới từ stored procedure
            string newMaKH = "";
            try
            {
                SqlCommand getMaKHCmd = new SqlCommand("spNew_MaKH", con);
                getMaKHCmd.CommandType = CommandType.StoredProcedure;

                SqlParameter outputParam = new SqlParameter("@new_MaKH", SqlDbType.VarChar, 9)
                {
                    Direction = ParameterDirection.Output
                };
                getMaKHCmd.Parameters.Add(outputParam);

                getMaKHCmd.ExecuteNonQuery();
                newMaKH = outputParam.Value.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lấy mã khách hàng: " + ex.Message);
                return;
            }

            // Chèn khách hàng mới vào cơ sở dữ liệu
            string sQuery = "INSERT INTO KhachHang (MaKH, TenKH, DiaChi) VALUES (@maKH, @tenKH, @diaChi)";
            SqlCommand cmd = new SqlCommand(sQuery, con);
            cmd.Parameters.AddWithValue("@maKH", newMaKH);
            cmd.Parameters.AddWithValue("@tenKH", sTenKH);
            cmd.Parameters.AddWithValue("@diaChi", sDiaChi);

            try
            {
                cmd.ExecuteNonQuery();
                MessageBox.Show("Thêm khách hàng thành công");
                RefreshCustomerList(); // Cập nhật danh sách trên form
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thêm khách hàng: " + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }

        // Hàm làm mới danh sách khách hàng trên form
        private void RefreshCustomerList()
        {
            SqlConnection con = new SqlConnection(sCon);
            try
            {
                con.Open();
                string sQuery = "select MaKH, TenKH, DiaChi from KhachHang";
                SqlDataAdapter da = new SqlDataAdapter(sQuery, con);
                DataTable dt = new DataTable();
                da.Fill(dt);

                // Giả sử bạn đang dùng DataGridView để hiển thị danh sách khách hàng
                dataGridView1.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải danh sách khách hàng: " + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }



        private void Form6_Load(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(sCon);
            try
            {
                con.Open();
            }
            catch(Exception ex) 
            {
                MessageBox.Show("lỗi"); 
            }
            string sQuery = "select * from KhachHang";
            SqlDataAdapter adapter = new SqlDataAdapter(sQuery, con );
            DataSet ds = new DataSet();
            adapter.Fill(ds, "KhachHang");
            dataGridView1.DataSource = ds.Tables["KhachHang"];
            con.Close();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtTenKH.Text = dataGridView1.Rows[e.RowIndex].Cells["TenKH"].Value.ToString();
            txtDiaChi.Text = dataGridView1.Rows[e.RowIndex].Cells["DiaChi"].Value.ToString();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection(sCon);
            try
            {
                conn.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi");
            }

            string sTenKH = txtTenKH.Text;
            string sDiaChi = txtDiaChi.Text;
            string selectedMaKH = dataGridView1.CurrentRow.Cells["MaKH"].Value.ToString();

            string sQuery = "update KhachHang set TenKH = @tenKH, DiaChi = @diachi where MaKH = @maKH";

            SqlCommand cmd = new SqlCommand(sQuery, conn);
            cmd.Parameters.AddWithValue("@tenKH", sTenKH);
            cmd.Parameters.AddWithValue("@diaChi", sDiaChi);
            cmd.Parameters.AddWithValue("@maKH", selectedMaKH);
            try
            {
                cmd.ExecuteNonQuery();
                MessageBox.Show("Cập nhật thành công");
                RefreshCustomerList();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi");
            }
            conn.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult ret = MessageBox.Show("Có chắc chắn xóa không?", "Thông báo", MessageBoxButtons.OKCancel);
            SqlConnection conn = new SqlConnection(sCon);
            try
            {
                conn.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi");
            }

            string selectedMaKH = dataGridView1.CurrentRow.Cells["MaKH"].Value.ToString();

            string sQuery = "delete KhachHang where MaKH = @maKH";

            SqlCommand cmd = new SqlCommand(sQuery, conn);
            cmd.Parameters.AddWithValue("@maKH", selectedMaKH);
            try
            {
                cmd.ExecuteNonQuery();
                MessageBox.Show("Xóa thành công");
                RefreshCustomerList();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi");
            }
            conn.Close();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection(sCon);
            try
            {
                conn.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi");
            }

            string tenKH = txtTenKH.Text;

            string sQuery = "select * from KhachHang where TenKH like @tenKH";
            SqlCommand cmd = new SqlCommand(sQuery, conn);
            cmd.Parameters.AddWithValue("@tenKH", "%" + tenKH + "%"); // Tìm kiếm một phần tên

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            try
            {
                da.Fill(dt); // Thực hiện lấy dữ liệu từ cơ sở dữ liệu

                if (dt.Rows.Count > 0)
                {
                    // Hiển thị kết quả trong DataGridView
                    dataGridView1.DataSource = dt;
                }
                else
                {
                    MessageBox.Show("Không tìm thấy khách hàng.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tìm kiếm: " + ex.Message);
            }

            conn.Close();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            new frmMain(role).Show();
            this.Hide();
        }
    }
}
