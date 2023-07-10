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
using System.IO;


namespace AracKiralama
{
    public partial class AracEkle : Form
    {
        SqlConnection cn;
        SqlCommand cm;
       
        public AracEkle()
        {
            cn = new SqlConnection();
            cn.ConnectionString = "Data Source=localhost;Initial Catalog=AracKiralama;Integrated Security=True";

            InitializeComponent();
            //combobox4 nesnesine konum bilgileri eklenir
            comboBox4.Items.Add("İzmir otogar");
            comboBox4.Items.Add("İzmir havalimanı");
            comboBox4.Items.Add("Ankara otogar");
            comboBox4.Items.Add("Ankara havalimanı");
            comboBox4.Items.Add("İstanbul otogar");
            comboBox4.Items.Add("İstanbul havalimanı");
        }

        //marka seçimine göre combobox2 verileride dinamik olarak eklenir
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox2.Items.Clear();
            if (comboBox1.SelectedIndex == 0)
            {
                comboBox2.Items.Add("Corsa");
                comboBox2.Items.Add("Astra");
                comboBox2.Items.Add("İnsignia");

            }
            else if (comboBox1.SelectedIndex == 1)
            {
                comboBox2.Items.Add("A-200");
                comboBox2.Items.Add("C63");
                comboBox2.Items.Add("S-400");
            }
            else if (comboBox1.SelectedIndex == 2)
            {
                comboBox2.Items.Add("M 520");
                comboBox2.Items.Add("M4 competition");
                comboBox2.Items.Add("M 63");
            }
            else if (comboBox1.SelectedIndex == 3)
            {
                comboBox2.Items.Add("Megane 6");
                comboBox2.Items.Add("Clıo");
                comboBox2.Items.Add("Fluence");
            }
            else if (comboBox1.SelectedIndex == 4)
            {
                comboBox2.Items.Add("courier");
                comboBox2.Items.Add("GT-500");
                comboBox2.Items.Add("Mustang");
            }
        }


        private void getimage_Click(object sender, EventArgs e)
        {
            //.Filter yapısı dosya uzantısı için filtre verilmesini sağlar.
            openFileDialog1.Filter = "Image files (*.png) |*.png|(*.jpg)|*.jpg|(*.gif)|*.gif|(*.jfif)|*.jfif";
            openFileDialog1.ShowDialog();
            //masaüstünden alınan görsel picturebox nesnesine ayarlanır.
            pictureBox1.BackgroundImage = Image.FromFile(openFileDialog1.FileName);
        }
        private void button2_Click(object sender, EventArgs e)
        {
            MemoryStream ms = new MemoryStream();
            pictureBox1.BackgroundImage.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            byte[] arrImage = ms.GetBuffer();

            DateTime bugun = DateTime.Now;

            cn.Open();
            cm = new SqlCommand("Insert Into araclar5 Values (@plaka,@Marka,@Seri,@Model,@Renk,@Km,@Yakit,@Ücret,@image,@Vites,@Konumm)", cn);
          
            cm.Parameters.AddWithValue("@plaka", textBox1.Text);
            cm.Parameters.AddWithValue("@Marka", comboBox1.SelectedItem);
            cm.Parameters.AddWithValue("@Seri", comboBox2.SelectedItem);
            cm.Parameters.AddWithValue("@Model", textBox2.Text);
            cm.Parameters.AddWithValue("@Renk", textBox3.Text);
            cm.Parameters.AddWithValue("@Km", textBox4.Text);
            cm.Parameters.AddWithValue("@Yakit", comboBox3.SelectedItem);
            cm.Parameters.AddWithValue("@Ücret", textBox5.Text);
            cm.Parameters.AddWithValue("@image", arrImage);
            cm.Parameters.AddWithValue("@Vites", textBox6.Text);
            cm.Parameters.AddWithValue("@Konumm", comboBox4.SelectedItem.ToString());
            cm.ExecuteNonQuery();
            cn.Close();
            MessageBox.Show("Kayıt Başarılı");
        }

       

    }
}
