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
    public partial class MüşteriEkle : Form
    {
        SqlConnection cn; //bağlantı nesnesi
        SqlCommand cm;    //komut nesnesi 
        public MüşteriEkle() 
        {
            InitializeComponent();
            cn = new SqlConnection();
            cn.ConnectionString = "Data Source=localhost;Initial Catalog=AracKiralama;Integrated Security=True";

        }

        private void btnkaydet_Click(object sender, EventArgs e)
        {//müşteri ekleme işlemi insert komutu kullanılarak sağanmıştır.
            cn.Open();
            cm = new SqlCommand("Insert Into Musteriler Values (@tcno,@AdSoyad,@Telefon,@Mail,@Adres,@eno,@etar)", cn);
            cm.Parameters.AddWithValue("@tcno", txtTc.Text);
            cm.Parameters.AddWithValue("@AdSoyad", txtAdSoyad.Text);
            cm.Parameters.AddWithValue("@Telefon", maskedtxtTel.Text);
            cm.Parameters.AddWithValue("@Mail", txtMail.Text);
            cm.Parameters.AddWithValue("@eno", textBox1.Text);
            cm.Parameters.AddWithValue("@etar", textBox2.Text);
            cm.Parameters.AddWithValue("@Adres", txtAdres.Text);
            cm.ExecuteNonQuery();
            cn.Close();
            MessageBox.Show("Kayıt Başarılı");
        }
    }
}
