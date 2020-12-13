using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DGSappSem2.Models
{
    public class SystemUsers
    {

        public string SystemUserID { get; set; }
        [Display(Name = "First Name")]
    public string Name { get; set; }

  
    [Display(Name = "Last Name")]
    public string Surname { get; set; }

   
    public string Gender { get; set; }

 
    [Display(Name = "Date of Birth")]
    public string DateOfBirth { get; set; }

 
    [Display(Name = "Email Address")]
    public string Email { get; set; }

  
    [Phone]
    [Display(Name = "Phone Number")]
    public string PhoneNo { get; set; }

}
}