using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace AracKiralama
{
    public partial class MüsteriPanel : Form
    {
        SqlConnection cn; //bağlantı nesnesi
        SqlCommand cm;    //komut nesnesi 
        SqlDataReader dr; //okuyucu nesnesi
        public string setTc; //public tanımlanan nesneler diğer classlar üzerinden dahi kullanılabilir
        public string isim; 
        public MüsteriPanel()
        {
            InitializeComponent();
            cn = new SqlConnection();
            cn.ConnectionString = "Data Source=localhost;Initial Catalog=AracKiralama;Integrated Security=True";
        }

        private void MüsteriPanel_Load_1(object sender, EventArgs e)//müşteri aneli yüklenirken bu metod tetiklenecektir
        {
            Get_data();
        }

        public void Get_data() //müşteri verilerinin alındığı metod
        {   
            cn.Open();
            cm = new SqlCommand("Select * from Üye where Tcno=@tc", cn);
            cm.Parameters.AddWithValue("@tc", setTc);
            dr = cm.ExecuteReader();
            while (dr.Read())
            { //veri tabanından alınan veriler panel üzerindeki textbox nesnelerine verilrir.
                txtTc.Text = dr["Tcno"].ToString();
                txtAdSoyad.Text = dr["AdSoyad"].ToString();
                txtAdres.Text = dr["Adres"].ToString();
                txtMail.Text = dr["Mail"].ToString();
                txtParola.Text = dr["Parola"].ToString();
                txtTelefon.Text = dr["TelefonNo"].ToString();
                isim = dr["AdSoyad"].ToString();
            }
            cn.Close();
        }

        //Bilgilerimi güncelle adlı seçim kutusu değiştirildiğinde tetiklenecek metod
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            //ReadOnly özelliği true ayarlandığında nesne üzerinde sadece okuma işlemi yapılır,
            //ReadOnly özelliği false ayarlandığında nesne üzerinde değişim işlemi de yapılabilmektedir.

            /* seçim kutusu seçili iken yani değeri true iken ReadOnly alanı
             * !(checkBox1.Checked) komutu sayesinde false dönecek ve değişime açık hale gelecektir.
            */

            //Burada sadece güncellenebilecek alanlar değişime açık hale gelecektir.
            txtAdres.ReadOnly = !(checkBox1.Checked);
            txtMail.ReadOnly = !(checkBox1.Checked);
            txtParola.ReadOnly = !(checkBox1.Checked);
            txtTelefon.ReadOnly = !(checkBox1.Checked);
            button6.Visible = checkBox1.Checked; //güncelleß butonu 
        }

        //güncelle butonu tıklandığında tetiklenecek metod
        private void button6_Click(object sender, EventArgs e)
        {
            cn.Open();
            //tc numarası değerine göre güncelleme işlemi yapılır
            cm = new SqlCommand("Update Üye set TelefonNo=@telefon,Mail=@mail,Adres=@adres,Parola=@parola where Tcno=@tc", cn);
            cm.Parameters.AddWithValue("@tc",      txtTc.Text);
            cm.Parameters.AddWithValue("@telefon", txtTelefon.Text);
            cm.Parameters.AddWithValue("@mail",    txtMail.Text);
            cm.Parameters.AddWithValue("@adres",   txtAdres.Text);
            cm.Parameters.AddWithValue("@parola",  txtParola.Text);
            cm.ExecuteNonQuery();
            cn.Close();

            checkBox1.Checked = false;//güncelleme işleminden sonra seçim kutusu false ayarlanır.
            Get_data(); //güncel müşteri verileri için yeniden get_data() metodu çalıştırılır.
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //araçların sergilendiği anasayfa paneli 
            AracAnaSayfa anasayfa = new AracAnaSayfa();
            anasayfa.tcno = setTc;
            anasayfa.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //rezervasyonların listelendiği panel
            Rezervasyon rez = new Rezervasyon();
            rez.getTc = setTc;
            rez.rezerv();
            rez.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //kartların sergilendiği panel
            kartlarım kart = new kartlarım();
            kart.getTc = setTc;
            kart.getİsim = isim;
            kart.Get_kart();
            kart.ShowDialog();
        }        

        private void button3_Click(object sender, EventArgs e)
        {
            //geçmiş işlemlerin tutulduğu panel
            ÜyeGeçmiş uyegecmiş = new ÜyeGeçmiş();
            uyegecmiş.getTc = setTc;
            uyegecmiş.gecmiş();
            uyegecmiş.ShowDialog();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            DialogResult result; //DialogResult yapısı programın önüne (ShowDialog açar gibi) bir uyarı mesajından sonucu verir. 
            result = MessageBox.Show("Çıkmak İstediğinizden Emin misiniz ?", "Çıkış", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            
            if (result == DialogResult.Yes)
            {
                this.Close(); // bulunduğun sekmeyi kapatır
                //Application.Exit(); tüm uygulamayı kapatır
            }

        }

        /*button1_MouseEnter metodu mouse ilgili buton üzerine gelince butonun arkaplan renginin
         *değişmesini sağlar. Bu metod tüm butonlar için uygulanır.
        */
        private void button1_MouseEnter(object sender, EventArgs e)
        {
            button1.BackColor = Color.CadetBlue;
        }

        private void button1_MouseLeave(object sender, EventArgs e)
        { 
            button1.BackColor = Color.White;
        }

        private void button2_MouseEnter(object sender, EventArgs e)
        {
            button2.BackColor = Color.CadetBlue;
        }

        private void button2_MouseLeave(object sender, EventArgs e)
        {
            button2.BackColor = Color.White;
        }

        private void button4_MouseEnter(object sender, EventArgs e)
        {
            button4.BackColor = Color.CadetBlue;
        }

        private void button4_MouseLeave(object sender, EventArgs e)
        {
            button4.BackColor = Color.White;
        }

        private void button3_MouseEnter(object sender, EventArgs e)
        {
            button3.BackColor = Color.CadetBlue;
        }

        private void button3_MouseLeave(object sender, EventArgs e)
        {
            button3.BackColor = Color.White;
        }

        private void button5_MouseEnter(object sender, EventArgs e)
        {
            button5.BackColor = Color.CadetBlue;
        }

        private void button5_MouseLeave(object sender, EventArgs e)
        {
            button5.BackColor = Color.White;
        }
    }
}
