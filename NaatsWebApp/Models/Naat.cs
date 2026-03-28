namespace NaatsWebApp.Models
{
    public class Naat
    {
        public string? nkid { get; set; }
        public int ano { get; set; }
        public int nno { get; set; }
        public string? naattitle { get; set; }
        public IFormFile? naatfile { get; set; }
        public string naatpath { get; set; }
    }
}
