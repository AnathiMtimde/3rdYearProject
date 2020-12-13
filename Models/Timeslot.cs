using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DGSappSem2.Models
{
    public class Timeslot
    {
        [Key]
        public int TimeslotId { get; set; }
        [DisplayName("Time"),DataType(DataType.Time),Required]
        public DateTime Time_Slot { get; set; }
        [DisplayName("Time Type")]
        public string TimeTpe { get; set; }
    }
}