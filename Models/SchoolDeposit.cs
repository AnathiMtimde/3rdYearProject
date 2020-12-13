using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DGSappSem2.Models
{
    public class SchoolDeposit
    {
        [Key]
        public int SchoolDepositId { get; set; }
        public decimal Amount { get; set; }
    }
}