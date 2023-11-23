using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace EquipmentPart_Mid_Month_Project.Models
{
    public class Equipment
    {
        public int EquipmentId { get; set; }
        [Required, StringLength(50)]
        public string EquipmentName { get; set; }
        [Required, Column(TypeName = "Date"), DataType(DataType.Date), DisplayFormat(DataFormatString ="{0:yyyy-MM-dd}", ApplyFormatInEditMode =true)]
        public DateTime DeliveryDate { get; set; }
        [Required, Column(TypeName = "money"), DataType(DataType.Currency)]
        public decimal Price { get; set; }
        public bool Available { get; set; }
        [Required, StringLength(30)]
        public string Picture { get; set; }
        public virtual List<Part> Parts { get; set; } = new List<Part>();
    }
    public class Part
    {
        public int PartId { get; set; }
        [Required, StringLength(50)]
        public string PartName { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required, ForeignKey("Equipment")]
        public int EquipmentId { get; set; }
        public virtual  Equipment Equipment { get; set; }
    }
   
    public class EquipmentDbContext : DbContext
    {
        public DbSet<Equipment> Equipments { get; set; }
        public DbSet<Part> Parts { get; set; }
    }
}