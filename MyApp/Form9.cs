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
using static MyApp.Form9;

namespace MyApp
{
    public partial class Form9 : Form
    {
        string sCon = "Data Source=DESKTOP-71TIUA9\\MSSQLSERVER02;Initial Catalog=NhatNamFood;Integrated Security=True;TrustServerCertificate=True";

        public string MaHD { get; set; }
        public bool IsNew { get; set; } // Xác định mở ở chế độ Thêm hoặc Xem
        public Form9()
        {
            InitializeComponent();
        }

        private void Form9_Load(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(sCon);
            try
            {
                con.Open();

                if (IsNew)
                {
                    // Nếu là hóa đơn mới, hiển thị ngày hôm nay
                    MaHD = GetNewMaHD(con); // Tạo mã hóa đơn mới
                    txtMaHD.Text = MaHD;   // Hiển thị mã hóa đơn mới
                    txtNgayGio.Text = DateTime.Now.ToString("dd/MM/yyyy"); // Hiển thị ngày hôm nay
                    LoadNguoiBan();
                    LoadHang();

                    comboBox1.Enabled = true;
                    txtNgayGio.Enabled = true;
                    txtMaHD.Enabled = true;
                }
                else
                {
                    // Nếu là hóa đơn sửa
                    txtMaHD.Text = MaHD;
                    LoadChiTietNhap(MaHD);
                    LoadNguoiBan();
                    LoadHang();

                    // Lấy các giá trị Công Thành, Thuế Suất GTGT, Tổng Tiền từ bảng Nhap
                    string query = @"SELECT NgayNhap, CongTH, ThueSuatGTGT, TongTT FROM Nhap WHERE MaHD = @MaHD";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@MaHD", MaHD);

                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        // Lấy ngày nhập
                        if (reader["NgayNhap"] != DBNull.Value)
                        {
                            DateTime ngayNhap = Convert.ToDateTime(reader["NgayNhap"]);
                            txtNgayGio.Text = ngayNhap.ToString("dd/MM/yyyy");
                        }

                        // Hiển thị Công Thành, Thuế Suất GTGT, Tổng Tiền
                        txtCongTH.Text = Convert.ToDecimal(reader["CongTH"]).ToString("N0"); // Format tiền tệ
                        txtThueSuatGTGT.Text = Convert.ToDecimal(reader["ThueSuatGTGT"]).ToString("N2"); // Format tỷ lệ thuế
                        txtTongTien.Text = Convert.ToDecimal(reader["TongTT"]).ToString("N0"); // Format tiền tệ
                    }
                    reader.Close();

                    comboBox1.Enabled = false;
                    txtNgayGio.Enabled = false;
                    txtMaHD.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xử lý dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                con.Close();
            }
        }
        private void LoadChiTietNhap(string maHD)
        {
            SqlConnection con = new SqlConnection(sCon);

            try
            {
                con.Open();

                string query = @"SELECT TenH, DonGiaNhap, SoLuong, (DonGiaNhap * SoLuong) AS ThanhTien 
                                 FROM NhapChiTiet 
                                 JOIN Hang ON NhapChiTiet.MaH = Hang.MaH
                                 WHERE MaHD = @MaHD";

                SqlDataAdapter adapter = new SqlDataAdapter(query, con);
                adapter.SelectCommand.Parameters.AddWithValue("@MaHD", maHD);

                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                dataGridView1.DataSource = dataTable;

                // Tùy chỉnh tiêu đề cột
                dataGridView1.Columns["TenH"].HeaderText = "TenH";
                dataGridView1.Columns["DonGiaNhap"].HeaderText = "Đơn giá nhập";
                dataGridView1.Columns["SoLuong"].HeaderText = "Số lượng";
                dataGridView1.Columns["ThanhTien"].HeaderText = "ThanhTien";
                dataGridView1.Columns["ThanhTien"].DefaultCellStyle.Format = "N0";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải chi tiết nhập: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            con.Close();
        }

        private void LoadNguoiBan()
        {
            SqlConnection con = new SqlConnection(sCon);

            try
            {
                con.Open();

                // Truy vấn lấy danh sách người bán
                string queryNguoiBan = "SELECT MaNB, TenNB FROM NguoiBan";
                SqlCommand cmdNguoiBan = new SqlCommand(queryNguoiBan, con);
                SqlDataReader reader = cmdNguoiBan.ExecuteReader();

                // Xóa các mục cũ trong ComboBox nếu có
                comboBox1.Items.Clear();

                // Thêm danh sách người bán vào ComboBox
                while (reader.Read())
                {
                    // Sử dụng ComboBoxItem để lưu cả Tên và Mã (chỉ lưu Tên, không hiển thị Mã)
                    comboBox1.Items.Add(new ComboBoxItem
                    {
                        Text = reader["TenNB"].ToString(), // Hiển thị tên người bán
                        Value = reader["MaNB"].ToString()  // Lưu mã người bán nhưng không hiển thị
                    });
                }

                reader.Close();

                // Nếu là hóa đơn sửa, tìm và chọn người bán tương ứng
                if (!IsNew)
                {
                    // Lấy MaNB từ bảng Nhap (có MaHD)
                    string queryMaNB = "SELECT MaNB FROM Nhap WHERE MaHD = @MaHD";
                    SqlCommand cmdMaNB = new SqlCommand(queryMaNB, con);
                    cmdMaNB.Parameters.AddWithValue("@MaHD", MaHD);
                    var maNB = cmdMaNB.ExecuteScalar();

                    // Kiểm tra MaNB có hợp lệ không và chọn trong ComboBox
                    if (maNB != null)
                    {
                        foreach (ComboBoxItem item in comboBox1.Items)
                        {
                            if (item.Value == maNB.ToString())
                            {
                                comboBox1.SelectedItem = item;
                                break;
                            }
                        }
                    }
                }
                else
                {
                    // Đặt mục mặc định là rỗng khi mới vào (trong trường hợp tạo hóa đơn mới)
                    comboBox1.SelectedIndex = -1; // Không chọn gì khi mới vào
                }

                // Kích hoạt tính năng tìm kiếm trong ComboBox
                comboBox1.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                comboBox1.AutoCompleteSource = AutoCompleteSource.ListItems;

            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải danh sách người bán: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            con.Close();
        }

        private void LoadHang()
        {
            SqlConnection con = new SqlConnection(sCon);

            try
            {
                con.Open();

                // Lấy danh sách Hàng từ cơ sở dữ liệu
                string queryHang = "SELECT MaH, TenH FROM Hang";
                SqlCommand cmd = new SqlCommand(queryHang, con);
                SqlDataReader reader = cmd.ExecuteReader();

                comboBox2.Items.Clear(); // Xóa danh sách cũ (nếu có)

                while (reader.Read())
                {
                    // Thêm dữ liệu Hàng vào ComboBox
                    comboBox2.Items.Add(new ComboBoxItem
                    {
                        Text = reader["TenH"].ToString(), // Hiển thị tên Hàng
                        Value = reader["MaH"].ToString()  // Lưu mã Hàng (ẩn)
                    });
                }
                reader.Close();

                if (!IsNew)
                {
                    // Tìm và chọn Hàng theo mã Hóa đơn (nếu sửa)
                    string queryMaH = "SELECT MaH FROM NhapChiTiet WHERE MaHD = @MaHD";
                    SqlCommand cmdMaH = new SqlCommand(queryMaH, con);
                    cmdMaH.Parameters.AddWithValue("@MaHD", MaHD);
                    var maH = cmdMaH.ExecuteScalar();

                    if (maH != null)
                    {
                        foreach (ComboBoxItem item in comboBox2.Items)
                        {
                            if (item.Value == maH.ToString())
                            {
                                comboBox2.SelectedItem = item;
                                break;
                            }
                        }
                    }
                }
                else
                {
                    comboBox2.SelectedIndex = -1; // Không chọn gì khi mới vào
                }

                // Bật tính năng tìm kiếm
                comboBox2.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                comboBox2.AutoCompleteSource = AutoCompleteSource.ListItems;

            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải danh sách Hàng: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
            con.Close();
        }

        // Lớp ComboBoxItem để lưu Tên và Mã người bán
        public class ComboBoxItem
        {
            public string Text { get; set; }  // Tên người bán
            public string Value { get; set; } // Mã người bán (chỉ lưu, không hiển thị)

            public override string ToString()
            {
                return Text; // Hiển thị Tên trong ComboBox
            }
        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                // Lấy thông tin từ dòng được chọn
                string tenH = row.Cells["TenH"].Value.ToString(); // Tên hàng
                string soLuong = row.Cells["SoLuong"].Value.ToString(); // Số lượng
                string donGia = row.Cells["DonGiaNhap"].Value.ToString(); // Đơn giá

                // Hiển thị thông tin lên ComboBox và các TextBox
                foreach (ComboBoxItem item in comboBox2.Items)
                {
                    if (item.Text == tenH)
                    {
                        comboBox2.SelectedItem = item;
                        break;
                    }
                }
                txtQuantity.Text = soLuong;
                txtPrice.Text = donGia;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                foreach (DataGridViewRow row in dataGridView1.SelectedRows)
                {
                    // Xóa dòng khỏi DataGridView
                    if (!row.IsNewRow)
                    {
                        dataGridView1.Rows.Remove(row);
                    }
                }
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(sCon);
            try
            {
                con.Open();

                // Xử lý thông tin bảng Nhap
                DateTime ngayNhap;
                if (!DateTime.TryParseExact(txtNgayGio.Text, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out ngayNhap))
                {
                    MessageBox.Show("Ngày nhập không hợp lệ.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string updateNhapQuery = @"UPDATE Nhap 
                                   SET NgayNhap = @NgayNhap
                                   WHERE MaHD = @MaHD";
                SqlCommand cmdUpdateNhap = new SqlCommand(updateNhapQuery, con);
                cmdUpdateNhap.Parameters.AddWithValue("@MaHD", MaHD);
                cmdUpdateNhap.Parameters.AddWithValue("@NgayNhap", ngayNhap);
                cmdUpdateNhap.ExecuteNonQuery();

                // Cập nhật bảng NhapChiTiet
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (!row.IsNewRow)
                    {
                        string tenH = row.Cells["TenH"].Value.ToString();
                        string maH = GetMaHByTenH(tenH, con);

                        string updateChiTietQuery = @"UPDATE NhapChiTiet
                                              SET DonGiaNhap = @DonGiaNhap, SoLuong = @SoLuong
                                              WHERE MaHD = @MaHD AND MaH = @MaH";
                        SqlCommand cmdUpdateChiTiet = new SqlCommand(updateChiTietQuery, con);
                        cmdUpdateChiTiet.Parameters.AddWithValue("@MaHD", MaHD);
                        cmdUpdateChiTiet.Parameters.AddWithValue("@MaH", maH);
                        cmdUpdateChiTiet.Parameters.AddWithValue("@DonGiaNhap", Convert.ToDecimal(row.Cells["DonGiaNhap"].Value));
                        cmdUpdateChiTiet.Parameters.AddWithValue("@SoLuong", Convert.ToInt32(row.Cells["SoLuong"].Value));
                        cmdUpdateChiTiet.ExecuteNonQuery();
                    }
                }

                // Tải lại dữ liệu
                LoadChiTietNhap(MaHD);

                MessageBox.Show("Hóa đơn đã được cập nhật!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi sửa hóa đơn: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            con.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(sCon);
            try
            {
                con.Open();

                // Tạo mã hóa đơn mới
                string newMaHD = GetNewMaHD(con);
                if (string.IsNullOrEmpty(newMaHD))
                    throw new Exception("Không thể tạo mã hóa đơn mới.");

                // Thêm vào bảng Nhap
                string insertNhapQuery = "INSERT INTO Nhap (MaHD, NgayNhap) VALUES (@MaHD, @NgayNhap)";
                SqlCommand cmdNhap = new SqlCommand(insertNhapQuery, con);
                cmdNhap.Parameters.AddWithValue("@MaHD", newMaHD);
                cmdNhap.Parameters.AddWithValue("@NgayNhap", DateTime.Now); // Ngày nhập hiện tại
                cmdNhap.ExecuteNonQuery();

                // Thêm chi tiết nhập
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (!row.IsNewRow)
                    {
                        string tenH = row.Cells["TenH"].Value.ToString();
                        string maH = GetMaHByTenH(tenH, con);

                        string insertChiTietQuery = @"INSERT INTO NhapChiTiet (MaHD, MaH, DonGiaNhap, SoLuong) 
                                              VALUES (@MaHD, @MaH, @DonGiaNhap, @SoLuong)";
                        SqlCommand cmdChiTiet = new SqlCommand(insertChiTietQuery, con);
                        cmdChiTiet.Parameters.AddWithValue("@MaHD", newMaHD);
                        cmdChiTiet.Parameters.AddWithValue("@MaH", maH);
                        cmdChiTiet.Parameters.AddWithValue("@DonGiaNhap", row.Cells["DonGiaNhap"].Value);
                        cmdChiTiet.Parameters.AddWithValue("@SoLuong", row.Cells["SoLuong"].Value);
                        cmdChiTiet.ExecuteNonQuery();
                    }
                }

                // Lấy Công Thành, Thuế GTGT, Tổng Tiền sau khi thêm
                string queryGetValues = "SELECT CongTH, ThueSuatGTGT, TongTT FROM Nhap WHERE MaHD = @MaHD";
                SqlCommand cmdGetValues = new SqlCommand(queryGetValues, con);
                cmdGetValues.Parameters.AddWithValue("@MaHD", newMaHD);

                SqlDataReader reader = cmdGetValues.ExecuteReader();
                if (reader.Read())
                {
                    txtCongTH.Text = Convert.ToDecimal(reader["CongTH"]).ToString("N0");
                    txtThueSuatGTGT.Text = Convert.ToDecimal(reader["ThueSuatGTGT"]).ToString("P2");
                    txtTongTien.Text = Convert.ToDecimal(reader["TongTT"]).ToString("N0");
                }
                reader.Close();

                MessageBox.Show("Hóa đơn mới đã được thêm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thêm hóa đơn: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
                con.Close();
        }

        private string GetNewMaHD(SqlConnection con)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("spNew_MaHD", con)
                {
                    CommandType = CommandType.StoredProcedure
                };

                SqlParameter outputParam = new SqlParameter("@new_MaHD", SqlDbType.VarChar, 9)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(outputParam);

                cmd.ExecuteNonQuery();

                return outputParam.Value.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lấy mã hóa đơn mới: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        private string GetMaHByTenH(string tenH, SqlConnection con)
        {
            try
            {
                string query = "SELECT MaH FROM Hang WHERE TenH = @TenH";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@TenH", tenH);
                return cmd.ExecuteScalar()?.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lấy mã hàng: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
         
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            new Form8(MaHD).Show();
            this.Hide();
        }
    }
}
