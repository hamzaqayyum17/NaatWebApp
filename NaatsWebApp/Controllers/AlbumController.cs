using Microsoft.AspNetCore.Mvc;
using NaatsWebApp.Models;
using System.Data.SqlClient;

namespace NaatsWebApp.Controllers
{
    public class AlbumController : Controller
    {
        private readonly IWebHostEnvironment _env;

        public AlbumController(IWebHostEnvironment env)
        {
            _env = env;
        }
        DBAccess db=new DBAccess();
        [HttpGet]
        public IActionResult CreateAlbum()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CreateAlbum(Album a)
        {
            a.nkid = HttpContext.Session.GetString("nkid");
            string fn = a.imgfile.FileName; //124.jpgn
            var allowedExts = new[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp" };
            var ext = Path.GetExtension(fn).ToLowerInvariant();// .jpg
            if (allowedExts.Contains(ext))
            {
                var myfn = a.nkid + "_" + a.ano + ext;// ali123_4.jpg
                var path = Path.Combine(_env.WebRootPath, "images", myfn);
                FileStream stream = new FileStream(path, FileMode.Create);
                a.imgfile.CopyTo(stream);

                a.imgpath = "/images/" + myfn;
                db.OpenConnection();
                string q = $"insert into album values('{a.nkid}','{a.ano}','{a.title}','{a.year}','{a.imgpath}')";
                db.IUD(q);
                db.CloseConnection();
            }
            return View(a);
        }
        [HttpGet]
        public IActionResult ViewAlbums()
        {
            string nkid = HttpContext.Session.GetString("nkid");
            List<Album> nklist = new List<Album>();
            db.OpenConnection();
            string q = "select imgpath,ano,title from album where nkid='" + nkid + "'";
            SqlDataReader sdr = db.GetData(q);

            while (sdr.Read())
            {
                Album a = new Album();
                a.imgpath = sdr["imgpath"].ToString();
                a.ano = int.Parse(sdr["ano"].ToString());
                a.title = sdr["title"].ToString();
                nklist.Add(a);
            }
            sdr.Close();
            db.CloseConnection();
            return View(nklist);
        }
    }

}
