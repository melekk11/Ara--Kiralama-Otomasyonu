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

namespace AracKiralama
{
    public partial class Sozlesme : Form
    {
        SqlConnection cn;
        SqlConnection cn1;
        SqlCommand cm1;
        SqlCommand cm2;
        SqlCommand cm;
        SqlDataReader dr;
        SqlDataReader dr1;
        public String plaka1;
        SqlConnection cn3;
        SqlCommand cm3;
        SqlDataReader dr3;
        public DateTime aliş;
        public DateTime iade;

        public Sozlesme()
        {
            InitializeComponent();
            cn = new SqlConnection();
            cn.ConnectionString = "Data Source=localhost;Initial Catalog=AracKiralama;Integrated Security=True";

            cn1 = new SqlConnection();
            cn1.ConnectionString = "Data Source=localhost;Initial Catalog=AracKiralama;Integrated Security=True";

            cn3 = new SqlConnection();
            cn3.ConnectionString = "Data Source=localhost;Initial Catalog=AracKiralama;Integrated Security=True";

        }

        private void GetData()
        {
           cbxAraclar.Items.Clear();
           cn.Open();
           cm = new SqlCommand("select * from araclar5  ", cn);
           dr = cm.ExecuteReader();
          /*Burada müşteri panelinde araç listelemk için kullanılan yapı kullanılmıştır.
          Öncelikle tüm araçlar veri tabanından alınmıştır. Daha sonra seçilen tarih aralığı ile
          karşılaştırılarak boş araçlara ulaşışmıştır. Boş araçlara ait plaka değerleri checkbox a eklenmiştir. 
           */
            while (dr.Read())
            {
              
                DateTime p_aliş = dateTimePicker2.Value.Date;
                DateTime p_iade = dateTimePicker1.Value.Date;
                plaka1 = dr["Plaka"].ToString();

                cn1.Open();
                cm1 = new SqlCommand("select * from Rezervasyon1 where Plaka = @plakha", cn1);
                cm1.Parameters.AddWithValue("@plakha", plaka1);
                dr1 = cm1.ExecuteReader();
                int durum = 1;
                if (dr1.HasRows)
                {
                    while (dr1.Read())
                    {
                        //Müşterinin seçtiği zaman dilimleri p_alış ve p_iade adlı değişkenlere atanır
                   
                        aliş = (DateTime)dr1["AlışTar"];
                        iade = (DateTime)dr1["IadeTar"];

                        p_aliş = p_aliş.Date; //'p_aliş.Date' yapısı sayesinde time bilgisinin atılması sağlanmıştır. 
                        p_iade = p_iade.Date;
                        aliş = aliş.Date;
                        iade = iade.Date;

                        if (p_aliş == aliş && p_iade == iade)
                        {
                            durum = 0;
                            /*Müşterinin girdiği tarihlerde rezervasyon işlemi bulunduğunda 
                            DURUM DEĞİŞKENİ 1 e eşitlenecektir
                             */
                            break;
                        }
                        else if (p_aliş < aliş && p_iade > aliş)
                        {
                            durum = 0;
                            /*Müşterinin girdiği tarihlerde rezervasyon işlemi bulunduğunda 
                            DURUM DEĞİŞKENİ 1 e eşitlenecektir
                             */
                            break;
                        }
                        else if (p_aliş < iade && p_iade > iade)
                        {
                            durum = 0;
                            /*Müşterinin girdiği tarihlerde rezervasyon işlemi bulunduğunda 
                             DURUM DEĞİŞKENİ 1 e eşitlenecektir
                              */
                            break;
                        }
                        else if (p_aliş >= aliş && p_iade <= iade)
                        {
                            durum = 0;
                            /*Müşterinin girdiği tarihlerde rezervasyon işlemi bulunduğunda 
                             DURUM DEĞİŞKENİ 1 e eşitlenecektir
                              */
                            break;
                        }

                    }
                    //durum değerine göre getlist metodu çağrılarak aracın listelenmesi sağlanmıştır. 
                    if (durum == 1)
                    {
                        cbxAraclar.Items.Add(dr["Plaka"]); 
                    }
                   
                }
                else
                {
                    cbxAraclar.Items.Add(dr["Plaka"]);
                }

                dr1.Close();
                cn1.Close();

            }
            dr.Close();
            cn.Close();

        }


        public void Sozlesme_Listele()
        {//tüm sözleşmeler veritabanından alınarak dataGridView1 nesnesine eklenmiştir.
            cn.Open();
            cm = new SqlCommand("Select * From Sözlesme", cn);
            SqlDataAdapter da = new SqlDataAdapter(cm);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            cn.Close();

        }
       
        private void Sozlesme_Load(object sender, EventArgs e)
        {
            Sozlesme_Listele();
        }

        private void cbxAraclar_SelectedIndexChanged(object sender, EventArgs e)
        {/*cbxAraclar adlı checkBox üzerinde herhangi bir plaka seçilinde, seçilen plakaya
          * göre select işlemi yapılarak araca ait diğer veriler alınarak paneldeki alanlara atsnmıştır.
         */
            cn.Open();
            cm = new SqlCommand("Select * From Araclar5 where Plaka like '" + cbxAraclar.SelectedItem + "'", cn);
            SqlDataReader dr = cm.ExecuteReader();

            while(dr.Read())
            {
                txtMarka.Text = dr["Marka"].ToString();
                txtSeri.Text =  dr["Seri"].ToString();
                txtModel.Text = dr["Model"].ToString();
                txtRenk.Text =  dr["Renk"].ToString();
            }
            cn.Close() ;
        }


        private void btnEkle_Click(object sender, EventArgs e)
        {
            sozlesmeolustur();
        }

        public void sozlesmeolustur()
        {
            cn.Open();
            cm = new SqlCommand("Insert Into Sözlesme Values (@tcno,@AdSoyad,@Telefon,@ehliyetno,@ehliyettarih,@plaka,@Marka,@Seri,@Model,@Renk,@kirasekli,@kiraücreti,@kiralanangünsayisi,@tutar,@cikistarih,@dönüstarih)", cn);
            cm.Parameters.AddWithValue("@tcno", txtTc.Text);
            cm.Parameters.AddWithValue("@AdSoyad", txtAdSoyad.Text);
            cm.Parameters.AddWithValue("@Telefon", txtTel.Text);
            cm.Parameters.AddWithValue("@ehliyetno", txtEhliyetNo.Text);
            cm.Parameters.AddWithValue("@ehliyettarih", txtEhliyetTarih.Text);
            cm.Parameters.AddWithValue("@plaka", cbxAraclar.Text);
            cm.Parameters.AddWithValue("@Marka", txtMarka.Text);
            cm.Parameters.AddWithValue("@Seri", txtSeri.Text);
            cm.Parameters.AddWithValue("@Model", txtModel.Text);
            cm.Parameters.AddWithValue("@Renk", txtRenk.Text);
            cm.Parameters.AddWithValue("@kirasekli", comboBox1.SelectedItem);
            cm.Parameters.AddWithValue("@kiraücreti", textBox1.Text);
            cm.Parameters.AddWithValue("@kiralanangünsayisi", txtGün.Text);
            cm.Parameters.AddWithValue("@tutar", txtTutar.Text);
            cm.Parameters.AddWithValue("@cikistarih", dateTimePicker2.Value);
            cm.Parameters.AddWithValue("@dönüstarih", dateTimePicker1.Value);
            cm.ExecuteNonQuery();
            cn.Close();

            cn.Open();
            cm = new SqlCommand("Insert Into Rezervasyon1 Values(@plaka, @alis, @iade)", cn);
            cm.Parameters.AddWithValue("@alis", dateTimePicker2.Value);
            cm.Parameters.AddWithValue("@iade", dateTimePicker1.Value);
            cm.Parameters.AddWithValue("@plaka", cbxAraclar.Text);
            dr = cm.ExecuteReader();
            cn.Close();

            Sozlesme_Listele();
            MessageBox.Show("Kayıt Başarılı");

            cbxAraclar.Text = null;
            cbxAraclar.Items.Clear();
          
        }

        private void button1_Click(object sender, EventArgs e)
        {//Seçilen kira şekline göre tutar hesalaması yapılmıştır.
            //Kiralanan toplam gün hesaplanıp gunfarkı değişkenine gönderilmiştir
            TimeSpan gunfarki = DateTime.Parse(dateTimePicker1.Text) - DateTime.Parse(dateTimePicker2.Text);
            int gunhesap = gunfarki.Days;
            txtGün.Text = gunhesap.ToString();
            //int.Parse(textBox1.Text)).ToString() = string formatındaki veri int formatına dönüştürülmüştür.
            txtTutar.Text = (gunhesap * int.Parse(textBox1.Text)).ToString();
        }

        private void comboBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            cn.Open();
            cm = new SqlCommand("Select KiraUcret From Araclar5 where Plaka like '" + cbxAraclar.SelectedItem + "'", cn);
            SqlDataReader dr = cm.ExecuteReader();

            while (dr.Read())
            {
                if (comboBox1.SelectedIndex == 0)
                {
                    textBox1.Text = (int.Parse(dr["KiraUcret"].ToString()) * 1).ToString();
                }
                else if (comboBox1.SelectedIndex == 1)
                {
                    textBox1.Text = (int.Parse(dr["KiraUcret"].ToString()) * 0.80).ToString();
                }
                else if (comboBox1.SelectedIndex == 2)
                {
                    textBox1.Text = (int.Parse(dr["KiraUcret"].ToString()) * 0.70).ToString();
                }

            }
            cn.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DataGridViewRow satir = dataGridView1.CurrentRow;
            DateTime bugün = DateTime.Now;
            int ucret = int.Parse(satir.Cells["KiraUcret"].Value.ToString());
            int tutar = int.Parse(satir.Cells["Tutar"].Value.ToString());
            DateTime cikis = DateTime.Parse(satir.Cells["CikisTarihi"].Value.ToString());
            
            /*gecikme ihtimaline karşın tutr hesaplaması iade edilen tarih üzerinden yapılır.
            yani iade tarihi bu gün alınır ve alış tarihinden çıkarılarak kiaralanan toplam gün hesaplanır.
            toplamtutar değeri ise bulunan gün üzerinden hesaplanır.
            */
            TimeSpan gun = bugün - cikis;
            int gunu = gun.Days;
            int toplamtutar = gunu * ucret;

            cn.Open();
            cm = new SqlCommand("Insert Into Satis Values (@tc_no,@AdSoyad,@plaka,@kirasekli,@kiraücreti,@tutar,@cikistarih,@dönüstarih)", cn);
            cm.Parameters.AddWithValue("@tc_no",   satir.Cells["Tcno"].Value.ToString());
            cm.Parameters.AddWithValue("@AdSoyad", satir.Cells["AdSoyad"].Value.ToString());
            cm.Parameters.AddWithValue("@plaka",   satir.Cells["Plaka"].Value.ToString());
            cm.Parameters.AddWithValue("@kirasekli", satir.Cells["KiraTipi"].Value.ToString());
            cm.Parameters.AddWithValue("@kiraücreti", tutar); // ücret
            cm.Parameters.AddWithValue("@cikistarih", satir.Cells["CikisTarihi"].Value.ToString());
            cm.Parameters.AddWithValue("@dönüstarih", satir.Cells["DönüsTarihi"].Value.ToString());
            cm.Parameters.AddWithValue("@tutar", toplamtutar);
            cm.ExecuteNonQuery();
            cn.Close();

            cn.Open() ;
            cm1 = new SqlCommand("Delete from Sözlesme where Plaka = @plaka and CikisTarihi = @cikistarih and DönüsTarihi = @dönüstarih", cn);
            cm1.Parameters.AddWithValue("@cikistarih", satir.Cells["CikisTarihi"].Value);
            cm1.Parameters.AddWithValue("@dönüstarih", satir.Cells["DönüsTarihi"].Value);
            cm1.Parameters.AddWithValue("@plaka", satir.Cells["Plaka"].Value.ToString());
            cm1.ExecuteNonQuery();
            cn.Close();

            cn.Open();
            cm1 = new SqlCommand("Delete from Rezervasyon1 where Plaka = @plaka and AlışTar = @cikistarih and IadeTar = @dönüstarih ", cn);
            cm1.Parameters.AddWithValue("@cikistarih", satir.Cells["CikisTarihi"].Value);
            cm1.Parameters.AddWithValue("@dönüstarih", satir.Cells["DönüsTarihi"].Value);
            cm1.Parameters.AddWithValue("@plaka",      satir.Cells["Plaka"].Value.ToString());
            cm1.ExecuteNonQuery();
            cn.Close();

            MessageBox.Show("Araç Teslim Edildi");
            dataGridView1.Controls.Clear();
           
            Sozlesme_Listele();
            cn.Close();
        }

        private void txtTc_TextChanged(object sender, EventArgs e)
        {/* Burada sisteme kayıtlı müşterilere ait bilgilerin otomatik doldurulması sağlanır.
           txtTc alanından alınan tc değerine göre veritabanına select işlemi yapılır
           eğer tc değerine ait kayıt varsa paneldeki diğer alanlar alınan veriler ile doldurulur.
          */
            cn.Open();
            cm = new SqlCommand("Select * From Musteriler where Tcno = @p_tc ", cn);
            cm.Parameters.AddWithValue("@p_tc", txtTc.Text);
            SqlDataReader dr = cm.ExecuteReader();

            while (dr.Read())
            {
                txtAdSoyad.Text = dr["AdSoyad"].ToString();
                txtTel.Text = dr["TelefonNo"].ToString();
                txtEhliyetNo.Text = dr["EhliyetNo"].ToString();
                txtEhliyetTarih.Text = dr["EhiyetTar"].ToString();
            }
            cn.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {//Seçilen tarih verilerine göre uygun araçları listeleme 
            GetData(); 
        }
    }
}
