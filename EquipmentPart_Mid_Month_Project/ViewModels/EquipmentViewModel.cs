using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EquipmentPart_Mid_Month_Project.ViewModels
{
    public class EquipmentViewModel
    {
        public int EquipmentId { get; set; }
       
        public string EquipmentName { get; set; }
       
        public DateTime DeliveryDate { get; set; }
       
        public decimal Price { get; set; }
        public bool Available { get; set; }
       
        public string Picture { get; set; }
        public byte[] Image { get; set; }
    }
}