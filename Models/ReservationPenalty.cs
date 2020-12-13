using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DGSappSem2.Models
{
    public class ReservationPenalty
    {
        [Key]
        public int ReservationPenaltyId { get; set; }

        public int ReservationId { get; set; }
        [DisplayName("No. Days")]
        public int NumberOfDays { get; set; }
        [DisplayName("Penalty Cost"),DataType(DataType.Currency)]
        public decimal PenaltyCost { get; set; }
    }
}