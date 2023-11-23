using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using EquipmentPart_Mid_Month_Project.Models;

namespace EquipmentPart_Mid_Month_Project.ViewModels
{
    public class PartDetailEditModel
    {
        [Key]
        public int PartDetailId { get; set; }
        [Required, StringLength(50)]
        public string Description { get; set; }
        [Required, Column(TypeName = "date"), DataType(DataType.Date)]
        public DateTime Expiredate { get; set; }
        [Required, StringLength(50)]
        public string Material { get; set; }
        [Required, StringLength(50)]
        public string Company { get; set; }
        [Required, ForeignKey("Part")]
        public int PartId { get; set; }
        public virtual Part Part { get; set; }
    }
}