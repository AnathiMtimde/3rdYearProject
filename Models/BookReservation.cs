using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace DGSappSem2s.Models
{
    public class BookReservation
    {
        [Key]
        public int ReservationId { get; set; }
        public int BookId { get; set; }
        public DateTime ReservationDate { get; set; }
        [DisplayName("Student Email")]

        public string StudentEmail { get; set; }
        [DisplayName("Collection Date"),DataType(DataType.Date)]
        public DateTime CollectionDate { get; set; }
        [DisplayName("Return Date")]

        public DateTime ReturnDate { get; set; }

        public string Status { get; set; }

    }
}