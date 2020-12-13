using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DGSappSem2.Models
{
    public class LibraryTimes
    {
        [Key]
        public int LibraryTimesId { get; set; }
        [DisplayName("Opening Time"), DataType(DataType.Time)]
        public DateTime openingTime { get; set; }
        [DisplayName("Closing Time"), DataType(DataType.Time)]
        public DateTime closingTime { get; set; }
    }
}