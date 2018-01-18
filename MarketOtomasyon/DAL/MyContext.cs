using MarketOtomasyon.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketOtomasyon.DAL
{
    class MyContext : DbContext
    {
        public MyContext()
            : base("name=MyCon")
        {
        }
   
    public virtual DbSet<Kategori> Kategoriler { get; set; }
    public virtual DbSet<Urun> Urunler { get; set; }
        public virtual DbSet<Siparis> Siparisler { get; set; }
        public virtual DbSet<SiparisDetay> SiparisDetaylar { get; set; }

    }
}
