using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace AracKiralama
{
    public partial class ÜyeSatis : Form
    {
        
        SqlConnection cn;//bağlantı nesnesi
        SqlCommand cm;   //komut nesnesi
        SqlDataReader dr;//okuyucu nesnesi

        //kullanılacak değişlkenler tüm projeden erişilebilmek için public olarak tanımalnmıştır
        public String gv_tel;
        public String gv_adsoyad;
        public String cvv;
        public String sifre;
        private PictureBox aracGorsel;
        public String get_plaka;
        public DateTime alistar;
        public DateTime iadetar;
        public String tcno;
        public int guncel_bakiye;
        public int Tutar;
        public int width;
        public int height;

        public ÜyeSatis()
        {
            InitializeComponent();
            cn = new SqlConnection();
            cn.ConnectionString = "Data Source=localhost;Initial Catalog=AracKiralama;Integrated Security=True";
        }
        
        public void get_arac()
        {//Burada araca ait verilerin panele eklenmesi sağlanacaktır.
            
            //araç resminin ekleneceği panelin genişlik ve yükseklik değerleri alınır
            width  = panel4.Width;
            height = panel4.Height;

            //get_plaka değişkeni ile bir önceki panelden gönderilen plaka değerine göre select işlemi yapılır 
            cn.Open();
            cm = new SqlCommand("select * from araclar5 where Plaka = @plaka ", cn);
            cm.Parameters.AddWithValue("@plaka", get_plaka);
            dr = cm.ExecuteReader();

            while (dr.Read())
            {
                //alınan araç verisi panedeki boş alanlara atanır. 
                textBox1.Text = dr["plaka"].ToString();
                textBox2.Text = dr["Yakit"].ToString();
                textBox3.Text = dr["Km"].ToString();
                textBox4.Text = dr["vites"].ToString();
                txtMarka.Text = dr["Marka"].ToString();
                txtSeri.Text  =  dr["Seri"].ToString();
                txtModel.Text = dr["model"].ToString();
                textBox12.Text = dr["renk"].ToString();

                textBox10.Text = alistar.Date.ToShortDateString();
                textBox11.Text = iadetar.Date.ToShortDateString();
                txtKiraÜcreti.Text = dr["KiraUcret"].ToString();

                aracGorsel = new PictureBox();//arac resmi için picrutebox nesnesi oluşturuldu.
                aracGorsel.Width = width; //width = arac resminin eklendiği panelin genişliği
                aracGorsel.Height = height;// height = arac resminin eklendiği panelin yüksekliği
                aracGorsel.BackgroundImageLayout = ImageLayout.Stretch;
                aracGorsel.BorderStyle = BorderStyle.FixedSingle;
               
                //Veri tabanından alınan görsel BİTMAP tipine dönüştürülerek panele eklenmiştir 
                long len = dr.GetBytes(9, 0, null, 0, 0);
                byte[] array = new byte[System.Convert.ToInt32(len) + 1];
                dr.GetBytes(9, 0, array, 0, System.Convert.ToInt32(len));
                MemoryStream ms = new MemoryStream(array);
                Bitmap bitmap = new Bitmap(ms);
                aracGorsel.BackgroundImage = bitmap;
                panel4.Controls.Add(aracGorsel); //arac görseli panel4 öğesine atandı
           
            }
            cn.Close();
        }

        public void hespla()
        {   //Kiralanan toplam gün hesaplanmıştır. 
            TimeSpan gunfarki = iadetar - alistar;
            int gunhesap = gunfarki.Days;
            txtGün.Text = gunhesap.ToString();
            //Hesaplanan gün ile ücret alanı çarpılarak toplam tutar hesaplanmıştır.
            txtTutar.Text = (gunhesap * int.Parse(txtKiraÜcreti.Text)).ToString();
            Tutar = int.Parse(txtTutar.Text);
        }
      
        private void button3_Click(object sender, EventArgs e)
        {//rezrevasyon oluşturma işlemi
            cn.Open();
            cm = new SqlCommand("select * from kart where KartNo = @kno ", cn);
            cm.Parameters.AddWithValue("@kno", textBox7.Text);
            dr = cm.ExecuteReader();

            //kart numarasına göre ccv ve şifre değerleri veritabanından alınır 
            while (dr.Read())
            {
                cvv = dr["CCV"].ToString();
                sifre = dr["Sifre"].ToString();
            }
            cn.Close();
           

            if (textBox8.Text != "" && textBox9.Text != "")
            {//eliyet tarihi ve ehliyet tarihi alanları boş geçilmemişse 
                if (sifre == textBox6.Text )
                {
                    //Veritabanınadan alına şifre ve müşterinin girdiği şifre eşleşiyorsa 
                    if (cvv == textBox5.Text)
                    {
                        //Veritabanınadan alına cvv ve müşterinin girdiği cvv eşleşiyorsa 
                        
                        //karta ait tüm koşullar sağlandığında kiralama işlemi sağlanacaktır.
                        cn.Open();
                        cm = new SqlCommand("select * from Üye where Tcno = @tcno ", cn);
                        cm.Parameters.AddWithValue("@tcno", tcno);
                        dr = cm.ExecuteReader();
                        while (dr.Read())
                        {
                            //üyeye ait ad soyad ve telefon numarası bilgileri select işemi ile alını
                            //gv_adsoyad , gv_tel değişkenlerine atanmıştır.
                             gv_adsoyad = dr["AdSoyad"].ToString();
                             gv_tel =     dr["TelefonNo"].ToString();
                        }
                        cn.Close();

                        //Sözleşme tablosuna yeni kayıt oluşturulmuştur.
                        cn.Open();
                        cm = new SqlCommand("Insert Into Sözlesme Values (@tcno,@AdSoyad,@Telefon,@ehliyetno,@ehliyettarih,@plaka,@Marka,@Seri,@Model,@Renk,@kirasekli,@kiraücreti,@kiralanangünsayisi,@tutar,@cikistarih,@dönüstarih)", cn);
                        cm.Parameters.AddWithValue("@tcno", tcno);
                        cm.Parameters.AddWithValue("@AdSoyad", gv_adsoyad);
                        cm.Parameters.AddWithValue("@Telefon", gv_tel);
                        cm.Parameters.AddWithValue("@ehliyetno", textBox8.Text);
                        cm.Parameters.AddWithValue("@ehliyettarih", textBox9.Text);
                        cm.Parameters.AddWithValue("@plaka", get_plaka);
                        cm.Parameters.AddWithValue("@Marka", txtMarka.Text);
                        cm.Parameters.AddWithValue("@Seri", txtSeri.Text);
                        cm.Parameters.AddWithValue("@Model", txtModel.Text);
                        cm.Parameters.AddWithValue("@Renk", txtModel.Text);
                        cm.Parameters.AddWithValue("@kirasekli", 1);
                        cm.Parameters.AddWithValue("@kiraücreti", txtKiraÜcreti.Text);
                        cm.Parameters.AddWithValue("@kiralanangünsayisi", txtGün.Text);
                        cm.Parameters.AddWithValue("@tutar", Tutar);
                        cm.Parameters.AddWithValue("@cikistarih", alistar);
                        cm.Parameters.AddWithValue("@dönüstarih", iadetar);
                        cm.ExecuteNonQuery();
                        cn.Close();

                        
                        //REZREVASYON tablosuna girilen tarihler aralığında kayıt eklenmiştir.
                        cn.Open();
                        cm = new SqlCommand("Insert Into Rezervasyon1 Values(@plaka, @alis, @iade)", cn);
                        cm.Parameters.AddWithValue("@alis", alistar);
                        cm.Parameters.AddWithValue("@iade", iadetar);
                        cm.Parameters.AddWithValue("@plaka", get_plaka);
                        dr = cm.ExecuteReader();
                        cn.Close();

                        MessageBox.Show("Kiralama işlemi başarılı!");
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("cvv kart bilgileri ile uyuşmuyor ! ");
                    }
                }
                else
                {
                    MessageBox.Show("kart şifresi yanlış!");

                }
            }
            else
            {
                MessageBox.Show("Ehliyet no ve ya ehliyet tarihi alanları boş bırakılamaz!");
                
            }

        }
    }
}
