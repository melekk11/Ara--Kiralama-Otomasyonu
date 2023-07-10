using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net.Configuration;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AracKiralama
{
    public partial class MusteriGiris : Form
    {
        SqlConnection cn; //bağlantı nesnesi
        SqlCommand cm;    //komut nesnesi
        SqlDataReader dr; //verileri sırayla okumak için kullanılan nesne
        private string setTcno; 
        public MusteriGiris()
        {
            InitializeComponent();
            //bağlantı nesnesi oluşturulur ve bağlantı metni tanımlanır 
            cn = new SqlConnection(); 
            cn.ConnectionString = "Data Source=localhost;Initial Catalog=AracKiralama;Integrated Security=True";
            
        }

        private void button1_Click(object sender, EventArgs e) //giriş yap butonu tetiklendiğinde çalışacak metod.
        {
            cn.Open(); //bağlantı açık hale getirilir
            //Şifre doğrulaması için girilen tc numarasına göre select işlemi yapılarak şifre bilgisi alınır.
            cm = new SqlCommand("select * from Üye where Tcno = @txtTcno ", cn); 
            cm.Parameters.AddWithValue("@txtTcno", txtTc.Text); //cm.Parameters.AddWithValue metodu komut metnine parametre eklenmesini sağlar 
            dr = cm.ExecuteReader();

            if (dr.Read()) //select işlemi veri döndürürse dr.read() = true değeri dönecektir.
            {
                var sifre =  dr["Parola"].ToString();//Veri tabanından şifre alınır 
                setTcno   =  dr["Tcno"].ToString();

                //veri tabanından çekilen şifre ile ekranda girilen şifre değerleri karşılaştırılır.
                if (txtParola.Text == sifre) //txtParola.Text = giriş sayfasında girilen şifre değeri 
                {   //girilen şifre ile veri tabanındaki şifre eşleşinde Müşteri aneline yönlendirme yapılır 
                    MüsteriPanel mpanel = new MüsteriPanel();
                    mpanel.setTc = setTcno; //mpanel.setTc komutu ile müşteri paneline veri gönderimi sağlanır
                    mpanel.ShowDialog();
                }
                else
                {
                    MessageBox.Show("Hatalı şifre!"); //girilen şifre ve ana şifre eşleşmediğine gösterilecek mesaj
                }
            }
            else
            {
                //Girilen tc numarası veri tabanında bulunamadığında aşağıdaki mesaj dönecektir
                MessageBox.Show("İlgili tc numarasına kayıtlı üye bulunmadı!");
            }
          cn.Close();
          this.Close(); //this.Close(); metodu yazıldığı panelin kapatılmasını sağlar.
        }

        // üye ol butonu tıklandığında tetiklenecek metod
        private void button2_Click(object sender, EventArgs e)
        {
            cn.Open();

            //and işlemi sayesinde üye olurken hiç bir parametre alanın boş geçilmemesi şartı oluşturulur 
            if ( txtTc.Text != null && 
                txtAdSoyad.Text != null &&
                txtTelefonNo.Text != null &&
                txtMail.Text != null &&
                txtAdres.Text != null &&
                txtParola.Text != null )
            {
                //koşul sağlandığında Üye tablosuna kayıt işlemi yapılır 
                cm = new SqlCommand("Insert Into Üye Values (@tcno,@AdSoyad,@Telefon,@Mail,@Adres,@Parola", cn);
                cm.Parameters.AddWithValue("@txtTcno", txtTc.Text);
                cm.Parameters.AddWithValue("@tcno",    txtTc.Text       );
                cm.Parameters.AddWithValue("@AdSoyad", txtAdSoyad.Text  );
                cm.Parameters.AddWithValue("@Telefon", txtTelefonNo.Text);
                cm.Parameters.AddWithValue("@Mail",    txtMail.Text     );
                cm.Parameters.AddWithValue("@Adres",   txtAdres.Text    );
                cm.Parameters.AddWithValue("@Parola",  txtParola.Text   );
                cm.ExecuteNonQuery();
                cn.Close();
                MessageBox.Show("Üye Kaydı Başarılı");
                checkBox1.Checked = false;
            }
            else
            {
                //kayıt ekranında herhangi alan boş geçildiğinde bu hata mesajı dönecektir.
                MessageBox.Show("Alanlar boş geçilemez!");
            }

        }

        //Üyelik oluştur adlı seçim kutusu değiştirildiğinde bu metod çalışacaktır.
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            txtAdSoyad.Visible =   checkBox1.Checked;
            txtAdres.Visible =     checkBox1.Checked;
            txtTelefonNo.Visible = checkBox1.Checked;
            txtMail.Visible =      checkBox1.Checked;
            txtParola2.Visible =   checkBox1.Checked;
            label4.Visible =       checkBox1.Checked;
            label5.Visible =       checkBox1.Checked;
            label6.Visible =       checkBox1.Checked;
            label3.Visible =       checkBox1.Checked;
            label7.Visible =       checkBox1.Checked;
            button2.Visible =      checkBox1.Checked;    //üye ol butonu 
            button1.Visible =      !(checkBox1.Checked); //giriş yap butonu 

        }
    }
}
