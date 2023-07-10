using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace AracKiralama
{
    public partial class KurumsalPanel : Form
    {
        SqlConnection cn;
        SqlCommand cm;
        SqlDataReader dr;

        private PictureBox pic;
        private Label marka;
        private Label plaka;
        private Label ucret;
        public KurumsalPanel()
        {
            InitializeComponent();
            cn = new SqlConnection();
            cn.ConnectionString = "Data Source=localhost;Initial Catalog=AracKiralama;Integrated Security=True";
        }

        //butonlara ait panel yönlendirmeleri tanımlanmıştır.
        private void btnaraclist_Click(object sender, EventArgs e)
        {
            AracListele araclistele = new AracListele();
            araclistele.ShowDialog();
        }
        private void btnAracEkle_Click_1(object sender, EventArgs e)
        {
            Aracdüzenle aracdüzenle = new Aracdüzenle();
            aracdüzenle.ShowDialog();
        }
        private void btnSatislar_Click(object sender, EventArgs e){
            Satis satislar = new Satis();
            satislar.ShowDialog();
        }
        private void btnSozlesme_Click(object sender, EventArgs e){
            Sozlesme sozlesme = new Sozlesme();
            sozlesme.ShowDialog();
        }
        private void btnmekle_Click(object sender, EventArgs e){
            MüşteriEkle musteriekle = new MüşteriEkle();
            musteriekle.ShowDialog();
        }
        private void btnMusteriListele_Click_1(object sender, EventArgs e){
            MusteriListele musteriListelefrm = new MusteriListele();
            musteriListelefrm.ShowDialog();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            AracEkle aracekle = new AracEkle();
            aracekle.ShowDialog();
        }
    }
}
