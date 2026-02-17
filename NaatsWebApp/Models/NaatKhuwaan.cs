using System.ComponentModel.DataAnnotations;

namespace NaatsWebApp.Models
{
    public class NaatKhuwaan
    {
        public String? nkid { get; set; }
        [Required]
        [Display(Name = "Name")]
        public string name { get; set; }
        [Display(Name = "City")]
        public string city { get; set; }
        [Display(Name = "Gender")]
        public char gender { get; set; }
        public bool isAlive { get; set; }
        [Display(Name = "Email")]
        public string email { get; set; }
        [StringLength(8)]
        [Display(Name = "Password")]
        public string password { get; set; }
    }
}
