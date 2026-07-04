namespace NaatsWebApp.Models
{
    public class Album
    {
        public string? nkid { get; set; }

        public int ano { get; set; }

        public string? title { get; set; }

        public int year { get; set; }

        public IFormFile? imgfile { get; set; }

        public string imgpath { get; set; }
    }
}
