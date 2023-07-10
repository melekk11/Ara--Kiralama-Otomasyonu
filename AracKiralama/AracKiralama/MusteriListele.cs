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

namespace AracKiralama
{
    public partial class MusteriListele : Form
    {
        SqlConnection cn;
        SqlCommand cm;
        SqlDataReader dr;

        public MusteriListele()
        {
            InitializeComponent();
            cn = new SqlConnection();
            cn.ConnectionString = "Data Source=localhost;Initial Catalog=AracKiralama;Integrated Security=True";
            Musteri_Listele();
        }

        private void btnGüncelle_Click(object sender, EventArgs e)
        {//Müşteri bilgileri i çin güncelleme işlemleri update komutu sayesinde gerçekleştirilmiştir.
            cn.Open();
            cm = new SqlCommand("Update Musteriler set AdSoyad=@adsoyad,TelefonNo=@telefon,Mail=@mail,Adres=@adres where Tcno=@tc", cn);
            cm.Parameters.AddWithValue("@tc", txtTc.Text);
            cm.Parameters.AddWithValue("@adsoyad", txtAdSoyad.Text);
            cm.Parameters.AddWithValue("@telefon", txtTel.Text);
            cm.Parameters.AddWithValue("@mail", txtMail.Text);
            cm.Parameters.AddWithValue("@adres", txtAdres.Text);
            cm.ExecuteNonQuery();
            cn.Close();
            Musteri_Listele();//güncel verileri tekrardan çekilmesi sağlanmıştır.
        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            //müşteri verileri üzerinde silme işlemi delete komusu sayesinde gerçekleştirilmiştir.
            cn.Open();
            cm = new SqlCommand("Delete from Musteriler where Tcno=@tc", cn);
            cm.Parameters.AddWithValue("@tc", txtTc.Text);
            cm.ExecuteNonQuery();
            cn.Close();
            Musteri_Listele();
        }

        public void Musteri_Listele()
        {
            //sisteme kayıtlı tüm müşteriler select komutu kullanılarak alınmıştır.
            cn.Open();
            cm = new SqlCommand("Select Tcno,AdSoyad,TelefonNo,Mail,Adres from Musteriler", cn);
            SqlDataAdapter da = new SqlDataAdapter(cm);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            cn.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close(); //ilgili panelin kapatılması sağlanmıştır.
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //dataGridView1.CurrentRow komutu kullanılarak seçilen satır verileri alınmıştır.
            txtTc.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
            txtAdSoyad.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            txtMail.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
            txtTel.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();
            txtAdres.Text = dataGridView1.CurrentRow.Cells[4].Value.ToString();
        }
      

    }

}
