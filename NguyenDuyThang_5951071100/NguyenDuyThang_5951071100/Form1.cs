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

namespace NguyenDuyThang_5951071100
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        
        public int StudentID;

        private void Form1_Load(object sender, EventArgs e)
        {
            GetStudentsRecord();
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }

        private SqlConnection getConnection() { 
            return new SqlConnection(@"Data Source=DESKTOP-4A00V1R\SQLEXPRESS;Initial Catalog=DemoCRUD;Integrated Security=True");
        }

        private void GetStudentsRecord()
        {
            SqlConnection con = getConnection();

            SqlCommand cmd = new SqlCommand("SELECT * FROM StudentsTb", con);
            DataTable dt = new DataTable();
            con.Open();
            SqlDataReader sdr = cmd.ExecuteReader();
            dt.Load(sdr);
            con.Close();
            dgv.DataSource = dt;
        }

        private bool IsValidData() {
            if(txtHo.Text == string.Empty
                || txtTenSinhVien.Text == string.Empty
                || txtDiaChi.Text == string.Empty
                || string.IsNullOrEmpty(txtSDT.Text)
                || string.IsNullOrEmpty(txtSBD.Text))
            {
                MessageBox.Show("Có lỗi chưa nhập dữ liệu!!!", "Lỗi dữ liệu ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }   
            return true;
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (IsValidData()) {
                SqlConnection con = getConnection();

                SqlCommand cmd = new SqlCommand("insert into StudentsTb values" +
                    "(@Name, @FatherName, @RollNumber, @Address, @Mobile)", con);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@Name", txtTenSinhVien.Text);
                cmd.Parameters.AddWithValue("@FatherName", txtHo.Text);
                cmd.Parameters.AddWithValue("@RollNumber", txtSBD.Text);
                cmd.Parameters.AddWithValue("@Address", txtDiaChi.Text);
                cmd.Parameters.AddWithValue("@Mobile", txtSDT.Text);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                GetStudentsRecord();
            }
        }

        private void dgv_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            StudentID = Convert.ToInt32(dgv.SelectedRows[0].Cells[5].Value);
            txtTenSinhVien.Text = dgv.SelectedRows[0].Cells[0].Value.ToString();
            txtHo.Text = dgv.SelectedRows[0].Cells[1].Value.ToString();
            txtSBD.Text = dgv.SelectedRows[0].Cells[2].Value.ToString();
            txtDiaChi.Text = dgv.SelectedRows[0].Cells[3].Value.ToString();
            txtSDT.Text = dgv.SelectedRows[0].Cells[4].Value.ToString();
        }

        private void btnCapNhat_Click(object sender, EventArgs e)
        {
            if (StudentID > 0) {
                SqlConnection con = getConnection();
                SqlCommand cmd = new SqlCommand("update StudentsTb SET " +
                    "Name = @Name, FatherName = @FatherName," +
                    "RollNumber = @RollNumber, Address = @Address," +
                    "Mobile = @Mobile where StudentID = @ID", con);

                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@Name", txtTenSinhVien.Text);
                cmd.Parameters.AddWithValue("@FatherName", txtHo.Text);
                cmd.Parameters.AddWithValue("@RollNumber", txtSBD.Text);
                cmd.Parameters.AddWithValue("@Address", txtDiaChi.Text);
                cmd.Parameters.AddWithValue("@Mobile", txtSDT.Text);
                cmd.Parameters.AddWithValue("@ID", StudentID);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();

                GetStudentsRecord();
                ResetData();
            }
            else
            {
                MessageBox.Show("Cập nhật bị lỗi !!!", "Lỗi !", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void ResetData()
        {
            StudentID = 0;
            txtTenSinhVien.Text = string.Empty;
            txtHo.Text = string.Empty;
            txtSBD.Text = string.Empty;
            txtDiaChi.Text = string.Empty;
            txtSDT.Text = string.Empty;
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (StudentID > 0)
            {
                SqlConnection con = getConnection();
                SqlCommand cmd = new SqlCommand("delete from StudentsTb where StudentID = @ID", con);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@ID", StudentID);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                GetStudentsRecord();
                ResetData();
            }
            else
            {
                MessageBox.Show("Xoá bị lỗi !!!", "Lỗi !", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
