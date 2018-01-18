using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketOtomasyon.Entities
{
    [Table("Urunler")]
    public class Urun
    {
        [Key]
        public int UrunId { get; set; }
        public long BarkodId { get; set; } = 8691235371000;
        [Required]
        [StringLength(40, MinimumLength = 2, ErrorMessage = "Ürün adı 2-40 karakter aralığında olmalıdır")]
        [Index(IsUnique = true)]
        public string UrunAdi { get; set; }
        public decimal Fiyat { get; set; } = 0;
        public decimal Indirim { get; set; } = 0;
        public short Stok { get; set; } = 0;
        public DateTime EklenmeZamani { get; set; } = DateTime.Now;
        public int KategoriId { get; set; }
        public byte[] urunFoto { get; set; }

        [ForeignKey("KategoriId")]
        public virtual Kategori Kategori { get; set; }
        
        
    }
}
