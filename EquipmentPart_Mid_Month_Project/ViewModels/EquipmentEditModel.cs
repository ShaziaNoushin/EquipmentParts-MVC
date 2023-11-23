using EquipmentPart_Mid_Month_Project.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EquipmentPart_Mid_Month_Project.ViewModels
{
    public class EquipmentEditModel
    {
        [Key]
        public int EquipmentId { get; set; }
        [Required, StringLength(50)]
        public string EquipmentName { get; set; }
        [Required, Column(TypeName = "Date"), DataType(DataType.Date)]
        public DateTime DeliveryDate { get; set; }
        [Required, Column(TypeName = "money"), DataType(DataType.Currency)]
        public decimal Price { get; set; }
        public bool Available { get; set; }
      
        public HttpPostedFileBase Picture { get; set; }
        public virtual List<Part> Parts { get; set; } = new List<Part>();
    }
}