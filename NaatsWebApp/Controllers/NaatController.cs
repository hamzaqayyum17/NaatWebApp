using Microsoft.AspNetCore.Mvc;
using NaatsWebApp.Models;
using System.Data.SqlClient;


namespace NaatsWebApp.Controllers
{
    public class NaatController : Controller
    {
        private readonly IWebHostEnvironment _env;
        public NaatController(IWebHostEnvironment env)
        {
            _env = env;
        }
        DBAccess dB = new DBAccess();
        public IActionResult AddNaat()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddNaat(Naat n, int ano)

        {

            n.nkid = HttpContext.Session.GetString("nkid");
            n.ano = ano;
            string fn = n.naatfile.FileName;//4.mpeg
            string ext = Path.GetExtension(fn).ToLowerInvariant();
            var allowExt = new[] { ".mp3", ".mp4", ".mpeg" };
            if (allowExt.Contains(ext))
            {
                string myfn = n.nkid + "" + n.ano + "" + n.nno + ext;//ali123_1_1.mpeg
                var path = Path.Combine(_env.WebRootPath, "naats", myfn);
                var stream = new FileStream(path, FileMode.Create);
                n.naatfile.CopyTo(stream);

                n.naatpath = "/naats/" + myfn;
                string q = "insert into Naat Values('" + n.nkid + "','" + n.ano + "','" + n.nno + "','" + n.naattitle + "','" + n.naatpath + "')";
                dB.OpenConnection();
                dB.IUD(q);
                dB.CloseConnection();

            }
            return View(n);
        }        
        [HttpGet]
        public IActionResult ViewNaats(string nkid, int ano)
        {

            List<Naat> nklist = new List<Naat>();
            dB.OpenConnection();
            string q = "select naatpath,nno,naattitle from naat where nkid='" + nkid + "' and ano='" + ano + "'";
            SqlDataReader sdr = dB.GetData(q);            

            while (sdr.Read())
            {
                Naat a = new Naat();
                a.naatpath = sdr["naatpath"].ToString();
                a.nno = int.Parse(sdr["nno"].ToString());
                a.naattitle = sdr["naattitle"].ToString();
                nklist.Add(a);
            }
            sdr.Close();
            dB.CloseConnection();
            return View(nklist);
        }
    }
}