using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DGSappSem2.Models
{
    public class PenaltyCost
    {
        [Key]
        public int PenaltyCostId { get; set; }
        [DisplayName("No. Days"),Required]
        public int NumberOfDays { get; set; }
        [DisplayName("Cost Price"),DataType(DataType.Currency),Required]
        public decimal CostPrice { get; set; }
    }
}