using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace DGSappSem2.Models
{
    public class BookCategory
    {
        [Key]

        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        
    }

    }
