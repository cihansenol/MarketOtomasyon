using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MarketOtomasyon.Entities
{
    [Table("Siparisler")]
    public class Siparis
    {
        [Key]
        public int SiparisId { get; set; }

        public DateTime SiparisTarihi { get; set; } = DateTime.Now;
        public DateTime? TeslimTarihi { get; set; }
        public decimal KargoFiyati { get; set; } = 0;



        public virtual List<SiparisDetay> SiparisDetaylar { get; set; } = new List<SiparisDetay>();
    }
}
