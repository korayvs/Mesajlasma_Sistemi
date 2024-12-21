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

namespace Mesaj_Test
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        public string numara, adsoyad;

        SqlConnection baglanti = new SqlConnection(@"Data Source=DESKTOP-F1A12T8\KORAY;Initial Catalog=Test2;Integrated Security=True");

        void gelenkutusu()
        {
            SqlDataAdapter da1 = new SqlDataAdapter("Select AD + ' ' + SOYAD As 'GÖNDEREN', BASLIK As 'BAŞLIK', ICERIK As 'İÇERİK' From TBLMESAJLAR Inner Join TBLKISILER On TBLMESAJLAR.GONDEREN = TBLKISILER.NUMARA Where ALICI = " + numara, baglanti);
            DataTable dt1 = new DataTable();
            da1.Fill(dt1);
            dataGridView1.DataSource = dt1;
        }

        void gidenkutusu()
        {
            SqlDataAdapter da2 = new SqlDataAdapter("Select AD + ' ' + SOYAD As 'ALICI', BASLIK As 'BAŞLIK', ICERIK As 'İÇERİK' From TBLMESAJLAR Inner Join TBLKISILER On TBLMESAJLAR.ALICI = TBLKISILER.NUMARA Where GONDEREN = " + numara, baglanti);
            DataTable dt2 = new DataTable();
            da2.Fill(dt2);
            dataGridView2.DataSource = dt2;
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            LblNo.Text = numara;
            LblAdSoyad.Text = adsoyad;

            gelenkutusu();
            
            gidenkutusu();

            //comboboxa ad soyad çekme
            SqlCommand komut2 = new SqlCommand("Select AD, SOYAD, NUMARA From TBLKISILER Where ID NOT IN (@p1)", baglanti);
            komut2.Parameters.AddWithValue("@p1", numara);
            SqlDataAdapter da = new SqlDataAdapter(komut2);
            DataTable dt = new DataTable();
            da.Fill(dt);
            CmbAlici.DisplayMember = "AD";
            CmbAlici.ValueMember = "NUMARA";
            CmbAlici.DataSource = dt;

            //Ad Soyadı Çekme
            baglanti.Open();
            SqlCommand komut = new SqlCommand("Select AD, SOYAD From TBLKISILER Where NUMARA = " + numara, baglanti);
            SqlDataReader dr = komut.ExecuteReader();
            while (dr.Read())
            {
                LblAdSoyad.Text = dr[0] + " " + dr[1];
            }
            baglanti.Close();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            SqlCommand komut = new SqlCommand("Insert Into TBLMESAJLAR (GONDEREN, ALICI, BASLIK, ICERIK) Values (@p1, @p2, @p3, @p4)", baglanti);
            komut.Parameters.AddWithValue("@p1", numara);
            komut.Parameters.AddWithValue("@p2", CmbAlici.SelectedValue);
            komut.Parameters.AddWithValue("@p3", TxtBaslik.Text);
            komut.Parameters.AddWithValue("@p4", richTextBox1.Text);
            komut.ExecuteNonQuery();
            baglanti.Close();
            MessageBox.Show("Mesajınız İletildi");
            gidenkutusu();
        }
    }
}
