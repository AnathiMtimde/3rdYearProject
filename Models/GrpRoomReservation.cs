using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DGSappSem2.Models
{
    public class GrpRoomReservation
    {
        [Key]
        public int GrpRoomReservationId { get; set; }
        [DisplayName("Student Email")]
        public string StudentEmail { get; set; }
        public string RoomNumber { get; set; }
        public int GroupRoomId { get; set; }
        //public virtual GroupRoom GroupRoom { get; set; }
       
        [DisplayName("Date Reserved"),DataType(DataType.DateTime)]
        public DateTime DateReserved { get; set; }
        [DisplayName("Date Reserving For"),DataType(DataType.Date)]
        public DateTime DateFor { get; set; }
        [DisplayName("Time In"), DataType(DataType.Time)]
        public DateTime TimeIn { get; set; }
        [DisplayName("Time Out"),DataType(DataType.Time)]
        public DateTime TimeOut { get; set; }
        public int capacity { get; set; }
        public string Status { get; set; }

    }
}