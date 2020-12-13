using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DGSappSem2.Models
{
    public class Guardian
    {
        [Key]
        public int GuardianID { get; set; }

        [Display(Name = "Parents' Name")]
        public string StudentParentsName { get; set; }


        [Display(Name = "Parents Id")]
        public string StudentParentsID { get; set; }


        [Display(Name = "Parents Contact")]
        public string StudentParentsContact { get; set; }

        [Display(Name = "Parents Email")]
        public string StudentParentsEmail { get; set; }


    }
}