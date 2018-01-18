using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketOtomasyon.Entities
{
    [Table("Kategoriler")]
    public class Kategori
    {
        [Key]
        public int KategoriId { get; set; }
        [Required]
        [StringLength(25)]
        [Column("Kategori Adı", TypeName = "varchar")]
        public string KategoriAdi { get; set; }
        public string Aciklama { get; set; }
        [Range(0,18)]
        public decimal Kdv { get; set; } 

        public virtual List<Urun> Urunler { get; set; } = new List<Urun>();
    }
}
