using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace DGSappSem2.Models
{
    public class Book
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        [DisplayName("BookId")]

        public int BookId { get; set; }
        public int  CategoryId { get; set; }
        public virtual BookCategory BookCategory { get; set; }
        [DisplayName("Book Title")]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }

        [Display(Name = "Picture")]
        public byte[] Picture { get; set; }
        public string Author { get; set; }
        public int Numpages { get; set;}
        public string Genre { get; set; }
        public string Status { get; set; }
        public byte[] QrCodeImage { get; set; }
        [DisplayName("Book Quantity"),Required]

        public int BookQuantity { get; set; }
    }
}