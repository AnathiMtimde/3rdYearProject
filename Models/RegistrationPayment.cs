using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DGSappSem2.Models
{
    public class RegistrationPayment
    {
        [Key]
        public int RegistrationPaymentID { get; set; }
        public string item_name { get; set; }
        public double amount { get; set; }
    }
}