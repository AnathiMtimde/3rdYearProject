using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DGSappSem2.Models
{
    public class GroupRoom
    {
        [Key]
        public int GroupRoomId { get; set; }
        [DisplayName("Room Number"),Required]
        public string RoomNumber { get; set; }
        public string Status { get; set; }
        [Required]
        [Display(Name = "Capacity")]
        [Range(0, 15)]
        public int capacity { get; set; }
        public ICollection<GrpRoomReservation> grpRoomReservations { get; set; }
    }
}