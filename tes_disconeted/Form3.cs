using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace tes_disconeted
{
    
    public partial class Form3 : Form
    {
        private string stringconnection = "Data Source=ACER;Initial Catalog=tes_disconeted;Integrated Security=True";
        private SqlConnection koneksi;
        private string nim, nama, alamat, jk, prodi;

        private void tbNIM_TextChanged(object sender, EventArgs e)
        {

        }

        private DateTime tgl;
        BindingSource custumersbindingSource = new BindingSource();

        private void cbProdi_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        public Form3()
        {
            InitializeComponent();
            koneksi = new SqlConnection(stringconnection);
            this.bindingNavigator1.BindingSource = this.custumersbindingSource;
            refreshform();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            tbNIM.Text = " ";
            tbNama.Text = " ";
            tbAlamat.Text = " ";
            dtTanggalLahir.Value = DateTime.Today;
            tbNIM.Enabled = true;
            tbNama.Enabled = true;
            cbJenisKelamin.Enabled = true;
            tbAlamat.Enabled = true;
            dtTanggalLahir.Enabled = true;
            cbProdi.Enabled = true;
            prodicbx();
            btnSave.Enabled = true;
            btnClear.Enabled = true;
            btnAdd.Enabled = true;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            refreshform();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            nim = tbNIM.Text;
            nama = tbNama.Text;
            jk = cbJenisKelamin.Text;
            alamat = tbAlamat.Text;
            tgl = dtTanggalLahir.Value;
            prodi = cbProdi.Text;
            int hs = 0;
            koneksi.Open();
            string strs = "select id_prodi from dbo.prodi1 where nama_prodi = @dd";
            SqlCommand cm = new SqlCommand(strs, koneksi);
            cm.CommandType = CommandType.Text;
            cm.Parameters.Add(new SqlParameter("@dd", prodi));
            SqlDataReader dr = cm.ExecuteReader();
            while (dr.Read())
            {
                hs = int.Parse(dr["id_prodi"].ToString());
            }
            dr.Close();
            string str = "insert into dbo.Data_Mahasiswa (nim, nama_mahasiswa, jenis_kelamin, alamat, tgl_lahir, id_prodi)" +
                "values(@NIM, @Nm, @Jk, @Al, @Tgl, @Idp)";
            SqlCommand cmd = new SqlCommand(str, koneksi);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add(new SqlParameter("NIM", nim));
            cmd.Parameters.Add(new SqlParameter("Nm", nama));
            cmd.Parameters.Add(new SqlParameter("Jk", jk));
            cmd.Parameters.Add(new SqlParameter("Al", alamat));
            cmd.Parameters.Add(new SqlParameter("Tgl", tgl));
            cmd.Parameters.Add(new SqlParameter("Idp", hs));
            cmd.ExecuteNonQuery();
            koneksi.Close();
            MessageBox.Show("Data Berhasill Disimpan", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
            refreshform();
        }

        private void FormDataMahasiswa_Load()
        {
            koneksi.Open();
            SqlDataAdapter DataAdapter1 = new SqlDataAdapter(new SqlCommand("Select m.nim, m.nama_mahasiswa, m.jenis_kelamin" +
                "m.alamat, m.tgl_lahir, p.id_prodi from dbo.mahasiswa m" +
                "join dbo.prodi1 p on m.id_prodi = p.id_prodi", koneksi));
            DataSet ds = new DataSet();
            DataAdapter1.Fill(ds);

            this.custumersbindingSource.DataSource = ds.Tables[0];
            this.tbNIM.DataBindings.Add(
                new Binding("Text", this.custumersbindingSource, "nim", true));
            this.tbNama.DataBindings.Add(
                new Binding("Text", this.custumersbindingSource, "nama_mahasiswa", true));
            this.tbAlamat.DataBindings.Add(
                new Binding("Text", this.custumersbindingSource, "alamat", true));
            this.cbJenisKelamin.DataBindings.Add(
                new Binding("Text", this.custumersbindingSource, "jenis_kelamin", true));
            this.dtTanggalLahir.DataBindings.Add(
                new Binding("Text", this.custumersbindingSource, "tgl_lahir", true));
            this.cbProdi.DataBindings.Add(
                new Binding("Text", this.custumersbindingSource, "nama_prodi", true));
            koneksi.Close();

        }
        private void clearBinding()
        {
            this.tbNIM.DataBindings.Clear();
            this.tbNama.DataBindings.Clear();
            this.tbAlamat.DataBindings.Clear();
            this.cbJenisKelamin.DataBindings.Clear();
            this.dtTanggalLahir.DataBindings.Clear();
            this.cbProdi.DataBindings.Clear();
        }
        private void refreshform()
        {
            tbNIM.Enabled = false;
            tbNama.Enabled = false;
            tbAlamat.Enabled = false;
            cbJenisKelamin.Enabled = false;
            dtTanggalLahir.Enabled = false;
            cbProdi.Enabled = false;
            btnAdd.Enabled = false;
            btnSave.Enabled = false;
            btnClear.Enabled = false;
            clearBinding();
            FormDataMahasiswa_Load();
        }
        private void prodicbx()
        {
            koneksi.Open();
            string str = "select nama_prodi from dbo.prodi1";
            SqlCommand cmd = new SqlCommand(str, koneksi);
            SqlDataAdapter da = new SqlDataAdapter(str, koneksi);
            DataSet ds = new DataSet();
            da.Fill(ds);
            cmd.ExecuteReader();
            koneksi.Close();
            cbProdi.DisplayMember = "nama_prodi";
            cbProdi.ValueMember = "id_prodi";
            cbProdi.DataSource = ds.Tables[0];
        }
        private void FormDataMahasiswa_FormClosed(object sender, FormClosedEventArgs e)
        {
            Form1 fm = new Form1();
            fm.Show();
            this.Hide();

        }
    }
}
