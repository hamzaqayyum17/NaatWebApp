using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace NaatsWebApp.Models
{
    public class NaatKhuwaan
    {
        [Display(Name = "Nk-ID")]
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
        [NotMapped]
        [Compare("password", ErrorMessage = "Passwords do not match!")]
        public string ConfirmPassword { get; set; }
    }
}
