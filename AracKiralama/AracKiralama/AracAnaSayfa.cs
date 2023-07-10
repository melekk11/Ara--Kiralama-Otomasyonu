using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace AracKiralama
{
    public partial class AracAnaSayfa : Form

    {
        //veri tabanı işlemleri için bağlantı komut ve okuyucu nesneleri
        SqlConnection cn;
        SqlCommand    cm;
        SqlDataReader dr;
        SqlConnection cn1;
        SqlCommand    cm1;
        SqlDataReader dr1;

        //listelenecek araç bilgileri için gerekli nesneler 
        public DateTime aliş;
        public DateTime iade;
        public string tcno;
        public String plakha;
        private PictureBox arac;
        private Label marka;
        private Label yakit;
        private Label vites;
        private Label ucret;
        private Label konum;
        private Label Km;
        private Label seri;
        public String set_plaka;
        public String vites_tip;
        public String yakit_tip;
        private String p_konum;
        private int konum_len;
        private System.Windows.Forms.Button Kirala;

        public AracAnaSayfa()
        {
            InitializeComponent();
            //DateTimePickerFormat.Short metodu tarih bilgisini gün/ay/yıl formatına dönüştürür
            this.dateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTimePicker2.Format = System.Windows.Forms.DateTimePickerFormat.Short;

            cn = new SqlConnection();
            cn.ConnectionString = "Data Source=localhost;Initial Catalog=AracKiralama;Integrated Security=True";

            cn1 = new SqlConnection();
            cn1.ConnectionString = "Data Source=localhost;Initial Catalog=AracKiralama;Integrated Security=True";

            //araç filtreleem için comboBox nesneleri doldurulur. 
            // comboBox.Items.Add(""); "" işaretleri arasındaki değeri comboBox a eklemeyi sağlar 
            // comboBox.Items.Clear.("") "" işaretleri arasındaki değeri comboBox içinden silmeyi sağlar
            comboBox1.Items.Add("Dizel");
            comboBox1.Items.Add("Benzin");
            comboBox1.Items.Add("Hibrit");
            comboBox1.Items.Add("Elektrik");
            comboBox1.Items.Add("LPG");

            comboBox2.Items.Add("Hepsi");
            comboBox2.Items.Add("İzmir otogar");
            comboBox2.Items.Add("İzmir havalimanı");
            comboBox2.Items.Add("Ankara otogar");
            comboBox2.Items.Add("Ankara havalimanı");
            comboBox2.Items.Add("İstanbul otogar");
            comboBox2.Items.Add("İstanbul havalimanı");
           
            txtmax.Text = "10000"; //Fiyat filtresi için varsayılan değer tanımlaması yapıldı
            txtmin.Text = "0";

            vites_tip = "Otomatik";
            yakit_tip = "Hepsi";

        }

        //Müşteri_Load metodu ilgili panel yüklenirken yapılacak işlemleri tutan metoddur.
        private void Müşteri_Load(object sender, EventArgs e)
        {
           //Müşteri paneli yüklenirken tüm araçlar select komutu ile dr adlı okuyucuya alınır.
            cn.Open();
            cm = new SqlCommand("select * from araclar5  ", cn);
            dr = cm.ExecuteReader();
            GetData(); //Seçilen zaman aralığında KİRALANABİLECEK araçları filtreleyecek metod.
        }
        private void GetData()
        {
            /*SEÇİLEN ZAMAN ARALIĞINDAKİ ARAÇLARIN FİLTRELENMESİ:
             *1-DR NESNESİNE ALINAN ARAC VERİLERi WHİLE DÖNGÜSÜ İLE SIRASI İLE OKUNUR
             *2-HER OKUMA ADIMINDA PLAKA DEĞERİ PLAKHA ADLI PARAMETREYE ATANIR
             *3-Rezervasyon1 ADLI TABLOYA PLAKA DEĞERİNE GÖRE SELECT YAPILARAK İLGİLİ ARACIN 
             *KİRALANMIŞ TARİHLERİ ALIŞ VE İADE ADLI DEĞİŞKENLERE ALINIR
             *4-P_ALIŞ,_İADE ,ALIŞ,İADE TARİHLERİ KARŞILAŞTIRILIR
             *5-KARŞILAŞTIRMADA PLAKAYA AİT TÜM REZRVASYON TARİHLERİ WHİLE DÖNÜSÜNDE OKUNUR
             *EĞER GİRİLEN TARİHLER ARASINDA  REZREVASYON BİLGİSİ BULUNURSA 
             * DURUM DEĞİŞKENİ 0 A EŞİTLENİR VE O ARACIN LŞSTELENMEMESİ SAĞLANIR
             * KATIR BULUNMAZ İSE DURUM DEĞİŞKENİNİN DEFOULT DEĞERİ OLAN 1 DEĞİŞTİRİLMEZ VE
             * İLGİLİ ARAC LİSTELENİR.
             *6-KARŞILAŞTIRMA ADIMINDA BOŞTAKİ ARAÇLARI LİSTELEMEK İÇİN GETLİST() METODU ÇALIŞTIRILIR.
             */
            while (dr.Read())
            {
                int durum = 1;
                var plakha = dr["Plaka"].ToString(); //DR okuyucundaki plaka değeri alınır

                cn1.Open();
                //plaka değerine göre aracın kiralandığı tarihler rezervasyon1 tablosundan çekilir
                cm1 = new SqlCommand("select * from Rezervasyon1 where Plaka = @plakha ORDER BY AlışTar DESC", cn1);
                cm1.Parameters.AddWithValue("@plakha", plakha);
                dr1 = cm1.ExecuteReader();

                if (dr1.HasRows) // dr1.HasRows metodu okuyucuda veri varsa true , yoksa false değer dönen bir yapıdır.  
                {
                    //Bu blok ilgili araç için rezervasyon bilgisi olduğunda çalışacaktır.
                    //else bloğunda ise hiç kiralanma bilgisi bulunmayan araçlar için işlem yapılacaktır 
                    while (dr1.Read())
                    {
                        //Müşterinin seçtiği zaman dilimleri p_alış ve p_iade adlı değişkenlere atanır
                        DateTime p_aliş = dateTimePicker1.Value.Date;
                        DateTime p_iade = dateTimePicker2.Value.Date;
                        aliş = (DateTime)dr1["AlışTar"];
                        iade = (DateTime)dr1["IadeTar"];

                        p_aliş = p_aliş.Date; //'p_aliş.Date' yapısı sayesinde time bilgisinin atılması sağlanmıştır. 
                        p_iade = p_iade.Date;
                        aliş = aliş.Date;
                        iade = iade.Date;

                        if (p_aliş == aliş && p_iade == iade)
                        {   durum = 0;
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
                    if (durum == 1 )
                    {
                        Getlist();
                    }
                }
                else 
                {
                    //ilgili araç için hiç rezervasyon bilgisi yokken arac listelenmiştir
                    Getlist();
                }
                dr1.Close();
                cn1.Close();
            }
            dr.Close();
            cn.Close();
        }

        private void button1_Click(object sender, EventArgs e) //ARAC BUL butonu 
        {
            DateTime today = DateTime.Now;  
            if (dateTimePicker2.Value > dateTimePicker1.Value )//iade tarihi alış tarihinden büyük olmalıdır 
            {
               if (comboBox2.SelectedItem != null)//comboBox2(konum) seçeneği boş değil ise 
                {
                    p_konum = comboBox2.SelectedItem.ToString();//p_konum değişkenine seçilen konum atanır
                    konum_len = p_konum.Length;//.Length metodu string ifadenin uzunluğunu yakalar.
                }

                flowLayoutPanel1.Controls.Clear();//Controls.Clear paneldeki seçimleri sıfırlar 
                cn.Open();

                if (p_konum != "Hepsi" && p_konum != null) // KONUM SEÇİLDİYSE 
                {   //seçilen konum select ifadesine parametre olarak verilir.
                    cm = new SqlCommand("select * from araclar5 where Konum = @konum", cn);
                    cm.Parameters.AddWithValue("@konum", comboBox2.SelectedItem.ToString());
                }
                else // KONUM = HEPSİ 
                {    //konum kısmında hepsi ifadesi seçilmişse herhangi bir parametre verilmez 
                    cm = new SqlCommand("select * from araclar5  ", cn);
                }
                dr = cm.ExecuteReader();
                GetData();
            }
            else
            {
                MessageBox.Show("İade tarihi alış tarihinden küçük ve ya bu günden küçük tarih bilgisi girilemez!!");
            }
        }
        private void Getlist()
        {   // bu metod içinde veritabanından okunan araç verilerine göre dinamik arac kartları oluşturulacaktır
           
            //araca ait genel çerçevenin oluşturulması
            long len = dr.GetBytes(9, 0, null, 0, 0);
            byte[] array = new byte[System.Convert.ToInt32(len) + 1];
            dr.GetBytes(9, 0, array, 0, System.Convert.ToInt32(len));
            arac = new PictureBox(); 
            arac.Width = 280;
            arac.Height = 250;
            arac.BackgroundImageLayout = ImageLayout.Stretch;
            arac.BorderStyle = BorderStyle.FixedSingle;

            //Label Kullanıcıya form üzerinde bilgi vermek için kullanılır, kullanıcı label üzerinde değişim yapamaz 
   
            //new label metodu ile marka ve seri ismini tutan yapı oluşturulmuştur
            marka = new Label();
            marka.Text = dr["marka"].ToString() + dr["seri"].ToString();
            marka.BackColor = Color.FromArgb(250, 250, 252);//arkaplan rengi rgb kodları ile sağlanmıştır.
            marka.TextAlign = ContentAlignment.MiddleCenter;//TextAlign nesnenin konumlandırımasını sağlar, MiddleCenter ile ortalanmıştır.
            marka.Dock = DockStyle.Bottom;//DockStyle.Bottom ile nesneleri alt alta sıralanması sağlanmıştır
            marka.Height = 20;//marka nesnesinin yükseklik değeri verilmiştir.


            vites = new Label();
            vites.Text = "Vites:" + dr["vites"].ToString();
            vites.BackColor = Color.FromArgb(250, 250, 252);
            vites.TextAlign = ContentAlignment.MiddleCenter;
            vites.Dock = DockStyle.Bottom;
            vites.Height = 18;

            yakit = new Label();
            yakit.Text = "Yakit:" + dr["yakit"].ToString();
            yakit.BackColor = Color.FromArgb(250, 250, 252);
            yakit.TextAlign = ContentAlignment.MiddleCenter;
            yakit.Dock = DockStyle.Bottom;
            yakit.Height = 18;

            Km = new Label();
            Km.Text = dr["Km"].ToString() + "Km";
            Km.BackColor = Color.FromArgb(250, 250, 252);
            Km.TextAlign = ContentAlignment.MiddleCenter;
            Km.Dock = DockStyle.Bottom;
            Km.Height = 18;

            konum = new Label(); 
            konum.Text = "Konum:" + dr[11].ToString();
            konum.BackColor = Color.FromArgb(250, 250, 252);
            konum.TextAlign = ContentAlignment.MiddleCenter;
            konum.Dock = DockStyle.Bottom;
            konum.Height = 18;

            ucret = new Label();
            ucret.Text = "Kira Ücreti: " + dr["KiraUcret"].ToString() + "₺";
            ucret.BackColor = Color.FromArgb(250, 250, 252);
            ucret.TextAlign = ContentAlignment.MiddleCenter;
            ucret.Dock = DockStyle.Bottom;
            ucret.Height = 18;

            Kirala = new System.Windows.Forms.Button();//buton nesnesi eklenmiştir
            Kirala.Text = "Kirala"; //kirala butonunun adı tanımlanmıştır
            Kirala.Name = dr["Plaka"].ToString(); //butona id değeri tanımlanmıştır
            Kirala.Click += Kirala_Click1; //butona tıklama eventi eklenmiştir
            Kirala.BackColor = Color.CadetBlue;
            Kirala.TextAlign = ContentAlignment.MiddleCenter;
            Kirala.Dock = DockStyle.Bottom;
            Kirala.Height = 20;

            MemoryStream ms = new MemoryStream(array);
            Bitmap bitmap = new Bitmap(ms); 
            arac.BackgroundImage = bitmap;

            //arac nesnesine ait diğer verilerin eklenmesi 
            arac.Controls.Add(marka);
            arac.Controls.Add(vites);
            arac.Controls.Add(yakit);
            arac.Controls.Add(Km);
            arac.Controls.Add(ucret);
            arac.Controls.Add(konum);
            arac.Controls.Add(Kirala);
           
            flowLayoutPanel1.Controls.Add(arac);
        }
        private void Kirala_Click1(object sender, EventArgs e)
        {
        //Herhangi araç kartında Kirala butonuna tıklandığı zaman ÜyeSatiş paneline yönlendirme yaılacaktır
            if (dateTimePicker2.Value > dateTimePicker1.Value)//iade tarihi alış tarihinden büyükse 
            {
                //arac kartları oluşturulurken plaka alanları id(name) olarak ayarlanmıştı
                //((System.Windows.Forms.Button)sender).Name yapısı bize ilgili butonun id(name)değerini yani plakasını dönecektir.
                set_plaka = ((System.Windows.Forms.Button)sender).Name; 
                ÜyeSatis uyesatis = new ÜyeSatis();
                uyesatis.get_plaka = set_plaka; //üye satış sayfasına plaka verisi gönderirlir.
                uyesatis.alistar = dateTimePicker1.Value.Date;
                uyesatis.iadetar = dateTimePicker2.Value.Date;
                uyesatis.tcno = tcno;
                uyesatis.get_arac();
                uyesatis.hespla();
                uyesatis.ShowDialog();
                cn.Open();
                cm = new SqlCommand("select * from araclar5  ", cn);
                dr = cm.ExecuteReader();
                GetData();
                

            }
            else
            {
                MessageBox.Show("İade tarihi alış tarihinden büyük olmalıdır !!"); 
            }
        }
        private void button2_Click(object sender, EventArgs e) //yakıt,fiyat,vites filtresi
        {
            if (dateTimePicker2.Value > dateTimePicker1.Value)//iade tarihi > alış tarihi 
            {
                if (comboBox2.SelectedItem != null) //konum seçili ise 
                {
                    p_konum = comboBox2.SelectedItem.ToString();
                    konum_len = p_konum.Length;
                }
                else
                {
                    p_konum = "";
                    konum_len = p_konum.Length;
                }

                flowLayoutPanel1.Controls.Clear();//paneldeki seçimler sıfırlanır
                cn.Open();

                if (konum_len == 0 || p_konum == "Hepsi") //KONUM SEÇİLMEDİYSE VEYA HEPSİ 
                {
                    if (yakit_tip == "Hepsi")  //YAKIT SEÇİLMEDİĞİNDE 
                    {
                        //KONUM = BOŞ , YAKİT = BOŞ , VİTES DİNAMİK 
                        cm = new SqlCommand("select * from araclar5 where Vites = @vites and KiraUcret between @min and @max ", cn);
                        cm.Parameters.AddWithValue("@vites", vites_tip);
                     }
                    else //KONUM = BOŞ , YAKİT = DOLU, VİTES DİNAMİK 
                    {
                        cm = new SqlCommand("select * from araclar5 where Yakit = @yakit and Vites = @vites and KiraUcret between @min and @max  ", cn);
                        cm.Parameters.AddWithValue("@vites", vites_tip);
                        cm.Parameters.AddWithValue("@yakit", yakit_tip);
                    }
                    cm.Parameters.AddWithValue("@max", int.Parse(txtmax.Text));
                    cm.Parameters.AddWithValue("@min", int.Parse(txtmin.Text));
                    dr = cm.ExecuteReader();
                }
                else //KONUM SEÇİLDİ İSE 
                {
                    if (yakit_tip == "Hepsi")  //YAKIT SEÇİLMEDİĞİNDE 
                    {   //KONUM = DOLU , YAKİT = BOŞ , VİTES DİNAMİK 
                        cm = new SqlCommand("select * from araclar5 where  Vites = @vites and konum = @konum and KiraUcret between @min and @max", cn);
                        cm.Parameters.AddWithValue("@vites", vites_tip);
                        cm.Parameters.AddWithValue("@konum", p_konum);
                    }
                    else //KONUM = DOLU , YAKİT = DOLU, VİTES DİNAMİK 
                    {
                        cm = new SqlCommand("select * from araclar5 where  Yakit = @yakit and Vites = @vites and konum = @konum and KiraUcret between @min and @max ", cn);
                        cm.Parameters.AddWithValue("@vites", vites_tip);
                        cm.Parameters.AddWithValue("@yakit", yakit_tip);
                        cm.Parameters.AddWithValue("@konum", p_konum);
                    }
                    cm.Parameters.AddWithValue("@max", int.Parse(txtmax.Text));
                    cm.Parameters.AddWithValue("@min", int.Parse(txtmin.Text));
                    dr = cm.ExecuteReader();
                }
                GetData();
            }
            else
            {
                MessageBox.Show("İade tarihi alış tarihinden büyük olmalıdır !!");
            }
        }
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            vites_tip = "Otomatik"; //radioButton1 otomatik
        }
        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            vites_tip = "Manuel "; //radioButton1 manuel
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        { //comboBox1_SelectedIndexChanged combobox1 nesnesinin değeri değiştirildiği zaman otomatik çalışan metoddur
            yakit_tip = comboBox1.SelectedItem.ToString();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            //filtre temizle 
                flowLayoutPanel1.Controls.Clear();
                cn.Open();
                if (p_konum != "Hepsi" && p_konum != "" ) // KONUM SEÇİLDİYSE 
                {
                    cm = new SqlCommand("select * from araclar5 where  Konum = @konum", cn);
                    cm.Parameters.AddWithValue("@konum", comboBox2.SelectedItem.ToString());
                }
                else // KONUM = HEPSİ 
                {
                    cm = new SqlCommand("select * from araclar5 ", cn);
                }
                dr = cm.ExecuteReader();
                GetData();
            }
    }
}
