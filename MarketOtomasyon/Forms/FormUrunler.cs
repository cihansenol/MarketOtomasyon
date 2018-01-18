
using MarketOtomasyon.DAL;
using MarketOtomasyon.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity.Validation;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MarketOtomasyon
{
    public partial class FormUrunler : Form
    {
        public FormUrunler()
        {
            InitializeComponent();
        }

        private void FormUrunler_Load(object sender, EventArgs e)
        {
            VerileriGetir();
        }
        void VerileriGetir()
        {
            try
            {
                MyContext db = new MyContext();
                cmbKategori.DataSource = db.Kategoriler.ToList();
                cmbKategori.DisplayMember = "KategoriAdi";
                cmbKategori.ValueMember = "KategoriId";

                lstUrun.DataSource = db.Urunler.OrderBy(x => x.UrunAdi).ToList();
                lstUrun.DisplayMember = "UrunAdi";
                lstUrun.ValueMember = "UrunId";

                this.Text = $" Toplam Ürün Sayısı: {db.Urunler.Count()} Toplam Ürün Maliyeti: {db.Urunler.Sum(x => x.Fiyat * x.Stok):c2} - {DateTime.Now:dd MMMM yy dddd}";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnKaydet_Click(object sender, EventArgs e)
        {
            Urun urun = new Urun()
            {
                Fiyat = nFiyat.Value,
                Stok = Convert.ToInt16(nStok.Value),
                KategoriId = (int)cmbKategori.SelectedValue,
                UrunAdi = txtUrunAdi.Text,
                urunFoto = resimDosyası
            };
            try
            {
                MyContext db = new MyContext();
                db.Urunler.Add(urun);
                db.SaveChanges();
                VerileriGetir();
                lstUrun.SelectedValue = urun.UrunId;
            }
            catch (DbEntityValidationException ex)
            {
                string mesajlar = string.Empty;
                foreach (var ev in ex.EntityValidationErrors)
                {
                    foreach (var ve in ev.ValidationErrors)
                    {
                        mesajlar += $"{ve.PropertyName} - {ve.ErrorMessage}\n";
                    }
                }
                MessageBox.Show(mesajlar);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnGuncelle_Click(object sender, EventArgs e)
        {
            if (lstUrun.SelectedItem == null) return;
            var seciliUrun = lstUrun.SelectedItem as Urun;
            try
            {
                MyContext db = new MyContext();
                seciliUrun = db.Urunler.Find(seciliUrun.UrunId);
                if (seciliUrun == null)
                {
                    MessageBox.Show("Ürün Bulunamadı");
                    VerileriGetir();
                    return;
                }
                seciliUrun.UrunAdi = txtUrunAdi.Text;
                seciliUrun.Fiyat = nFiyat.Value;
                seciliUrun.Stok = Convert.ToInt16(nStok.Value);
                seciliUrun.KategoriId = (int)cmbKategori.SelectedValue;
                seciliUrun.urunFoto = resimDosyası;
               
                db.SaveChanges();
                VerileriGetir();
                lstUrun.SelectedValue = seciliUrun.UrunId;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            if (lstUrun.SelectedItem == null) return;
            var seciliUrun = lstUrun.SelectedItem as Urun;
            var cevap = MessageBox.Show($"{seciliUrun.UrunAdi} isimli ürünü silmek istiyor musunuz?", "Ürün Silme", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (cevap != DialogResult.Yes) return;
            try
            {
                MyContext db = new MyContext();
                seciliUrun = db.Urunler.Find(seciliUrun.UrunId);
                if (seciliUrun == null)
                {
                    MessageBox.Show("Ürün Bulunamadı");
                    VerileriGetir();
                    return;
                }
                db.Urunler.Remove(seciliUrun);
                db.SaveChanges();
                VerileriGetir();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void txtAra_TextChanged(object sender, EventArgs e)
        {
            string ara = txtAra.Text.ToLower();
            try
            {
                MyContext db = new MyContext();
                lstUrun.DataSource = db.Urunler
                    .Where(x => x.UrunAdi.ToLower().Contains(ara) || x.Kategori.KategoriAdi.ToLower().Contains(ara))
                    .ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private byte[] resimDosyası;
        private void pbMarkaLogo_Click(object sender, EventArgs e)
        {
            OpenFileDialog dosya = new OpenFileDialog
            {

                Title = "Bir resim dosyası seçiniz",
                Multiselect = false,
                Filter = "JPG Formatı(*.jpg) | *.jpg;*jpeg;|PNG formatı |*.png",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
            };

            DialogResult result = dosya.ShowDialog();
            MemoryStream memorystream = new MemoryStream();
            var buffer = new byte[64];
            if (result == DialogResult.OK)
            {
                FileStream filestream = File.Open(dosya.FileName, FileMode.Open);
                while (filestream.Read(buffer, 0, 64) != 0)
                {

                    memorystream.Write(buffer, 0, 64);
                }
                resimDosyası = memorystream.ToArray();
                pbMarkaLogo.Image = new Bitmap(memorystream);
            }
        }

        private void lstUrun_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstUrun.SelectedItem == null) return;

            var seciliUrun = lstUrun.SelectedItem as Urun;//burada seçiliürünün değerleri toollar'a gönderilir.

            txtUrunAdi.Text = seciliUrun.UrunAdi;
            nFiyat.Value = seciliUrun.Fiyat;
            nStok.Value = seciliUrun.Stok; // değer ataması yapıldığında ??0 yapılmaz
            cmbKategori.SelectedValue = seciliUrun.KategoriId;
            if (seciliUrun.urunFoto != null)
            {
                try
                {
                    pbMarkaLogo.Image = new Bitmap(new MemoryStream(seciliUrun.urunFoto));

                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message);
                }
            }
            else
                pbMarkaLogo.Image = null;
        }
    }
}
