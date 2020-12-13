using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DGSappSem2.Models
{
    public class StudentDebt
    {
        [Key]
        public int StudentDebtId { get; set; }
        [DisplayName("Student Email")]
        public string StudentEmail { get; set; }
        [DataType(DataType.Currency)]
        public decimal Amount { get; set; }
        public string Status { get; set; }
    }
}