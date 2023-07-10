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

namespace AracKiralama
{
    public partial class AracListele : Form
    {
        SqlConnection cn;
        SqlCommand cm;
        SqlDataReader dr;
        public DateTime aliş;
        public DateTime iade;
        public string tcno;
        private PictureBox pic;
        private PictureBox picCAR;
        private Label marka;
        private Label yakit;
        private Label yer;
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
        StringBuilder komutCumlesi = new StringBuilder();

        public AracListele()
        {
            InitializeComponent();

            cn = new SqlConnection();
            cn.ConnectionString = "Data Source=localhost;Initial Catalog=AracKiralama;Integrated Security=True";
           
            //komut cümlesini dinamikleştirmek için select ifadeninin bir kısmı başlangıçta tanımlanmıştır
            komutCumlesi.Append("select * from araclar5 where kiraucret between 0 and 30000");

            comboBox1.Items.Add("Dizel");
            comboBox1.Items.Add("Benzin");
            comboBox1.Items.Add("Hibrit");
            comboBox1.Items.Add("Elektrik");
            comboBox1.Items.Add("LPG");

            comboBox2.Items.Add("İzmir otogar");
            comboBox2.Items.Add("İzmir havalimanı");
            comboBox2.Items.Add("Ankara otogar");
            comboBox2.Items.Add("Ankara havalimanı");
            comboBox2.Items.Add("İstanbul otogar");
            comboBox2.Items.Add("İstanbul havalimanı");

            comboBox3.Items.Add("Otomatik");
            comboBox3.Items.Add("Manuel");

            vites_tip = "Otomatik";
            yakit_tip = "Hepsi";
 }

        private void AracListele_Load(object sender, EventArgs e)
        {
            cn.Open();
            cm = new SqlCommand("select * from araclar5 ", cn);
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                Getlist();
            }
            cn.Close();
        }

        private void Getlist()
        {
            long len = dr.GetBytes(9, 0, null, 0, 0);
            byte[] array = new byte[System.Convert.ToInt32(len) + 1];
            dr.GetBytes(9, 0, array, 0, System.Convert.ToInt32(len));

            pic = new PictureBox();
            pic.Width = 280;
            pic.Height = 250;
            pic.BackgroundImageLayout = ImageLayout.Stretch;
            pic.BorderStyle = BorderStyle.FixedSingle;

            picCAR = new PictureBox();
            picCAR.Width = 280;
            picCAR.Height = 135;
            picCAR.BackgroundImageLayout = ImageLayout.Stretch;
            picCAR.BorderStyle = BorderStyle.FixedSingle;

            marka = new Label();
            marka.Text = dr["marka"].ToString() + dr["seri"].ToString();
            marka.BackColor = Color.AliceBlue;
            marka.TextAlign = ContentAlignment.MiddleCenter;
            marka.Dock = DockStyle.Bottom;
            marka.Height = 20;

            yer = new Label();
            yer.Text = dr[11].ToString() ;
            marka.BackColor = Color.AliceBlue;
            yer.TextAlign = ContentAlignment.MiddleCenter;
            yer.Dock = DockStyle.Bottom;
            yer.Height = 20;

            vites = new Label();
            vites.Text = "Vites:" + dr["vites"].ToString();
            vites.BackColor = Color.AliceBlue;
            vites.TextAlign = ContentAlignment.MiddleCenter;
            vites.Dock = DockStyle.Bottom;
            vites.Height = 18;

            yakit = new Label();
            yakit.Text = "Yakit:" + dr["yakit"].ToString();
            yakit.BackColor = Color.AliceBlue;
            yakit.TextAlign = ContentAlignment.MiddleCenter;
            yakit.Dock = DockStyle.Bottom;
            yakit.Height = 18;

            Km = new Label();
            Km.Text = dr["Km"].ToString() + "Km";
            Km.BackColor = Color.AliceBlue;
            Km.TextAlign = ContentAlignment.MiddleCenter;
            Km.Dock = DockStyle.Bottom;
            Km.Height = 18;

            ucret = new Label();
            ucret.Text = "Kira Ücreti: " + dr["KiraUcret"].ToString() + "₺";
            ucret.BackColor = Color.AliceBlue;
            ucret.TextAlign = ContentAlignment.MiddleCenter;
            ucret.Dock = DockStyle.Bottom;
            ucret.Height = 18;

            MemoryStream ms = new MemoryStream(array);
            Bitmap bitmap = new Bitmap(ms);
            picCAR.BackgroundImage = bitmap;
            pic.Controls.Add(picCAR);
            pic.Controls.Add(marka);
            pic.Controls.Add(vites);
            pic.Controls.Add(yakit);
            pic.Controls.Add(Km);
            pic.Controls.Add(ucret);
            pic.Controls.Add(yer);
            flowLayoutPanel1.Controls.Add(pic);

        }

        /*burada combobox yapılarında seçilen parametrelere göre dinamik komut cümlesi 
        oluşturan bir yapı kurulmuştur. 
          line adlı değişkene istenen parametre atanmıştır
          komutCumlesi.Append(line); komutu sayesinde filtre değeri komut cümlesine eklenmiştir.
        */
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            string line = " and Konum = '" + comboBox2.SelectedItem.ToString() + "'";
            komutCumlesi.Append(line);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string line = " and Yakit = '" + comboBox1.SelectedItem.ToString() + "'";
            komutCumlesi.Append(line);
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            string line = " and Vites = '" + comboBox3.SelectedItem.ToString() + "'";
            komutCumlesi.Append(line);

        }
        private void button2_Click(object sender, EventArgs e) //filtre uygula
        {

            flowLayoutPanel1.Controls.Clear();
            cn.Open();
            String strkomut = komutCumlesi.ToString();
            SqlCommand komut = new SqlCommand(strkomut, cn);

            dr = komut.ExecuteReader();
            while (dr.Read())
            {
                Getlist();
            }
            cn.Close();
            
            komutCumlesi.Clear(); //filtreleme işelminden sonra seçim parametreleri sıfırlanır 
           //dinamik komut cümlesi için yapı eski haline getirilir. 
            komutCumlesi.Append("select * from araclar5 where kiraucret between 0 and 30000");
            comboBox1.Text = "";
            comboBox2.Text = "";
            comboBox3.Text = "";
        } 
        private void button3_Click(object sender, EventArgs e) //filtre temizle
        {
            flowLayoutPanel1.Controls.Clear() ;
            cn.Open();
            cm = new SqlCommand("select * from araclar5 ", cn);
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                Getlist();
            }
            cn.Close();
        }
    }
}
