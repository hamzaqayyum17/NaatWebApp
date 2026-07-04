using Microsoft.AspNetCore.Mvc;
using NaatsWebApp.Models;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace NaatsWebApp.Controllers
{
    public class NaatKhuwanController : Controller
    {
        DBAccess db = new DBAccess();        
        private List<SelectListItem> GetCityList()
        {
            var cities = new List<SelectListItem>();
            db.OpenConnection();
            string q = "select distinct city from naatkhuwaan order by city";
            SqlDataReader sdr = db.GetData(q);
            while (sdr.Read())
            {
                cities.Add(new SelectListItem
                {
                    Text = sdr[0].ToString(),
                    Value = sdr[0].ToString()
                });
            }
            sdr.Close();
            db.CloseConnection();
            return cities;
        }

        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SignUp(NaatKhuwaan nk)
        {
            nk.nkid = nk.email.Split('@')[0];
            if (ModelState.IsValid)
            {
                db.OpenConnection();
                string q = "insert into NaatKhuwaan Values('" + nk.nkid + "','" + nk.name + "','" +
                           nk.city + "','" + nk.gender + "','" + nk.isAlive + "','" +
                           nk.email + "','" + nk.password + "')";
                db.IUD(q);
                db.CloseConnection();
            }
            return View(nk);
        }

        [HttpGet]
        public IActionResult AllNK()
        {
            List<NaatKhuwaan> nklist = new List<NaatKhuwaan>();
            db.OpenConnection();
            string q = "Select nkid,name,city from naatkhuwaan";
            SqlDataReader sdr = db.GetData(q);
            while (sdr.Read())
            {
                NaatKhuwaan nk = new NaatKhuwaan();
                nk.nkid = sdr["nkid"].ToString();
                nk.name = sdr["name"].ToString();
                nk.city = sdr["city"].ToString();
                nklist.Add(nk);
            }
            sdr.Close();
            db.CloseConnection();

            // ✅ FIX: Cities ViewBag mein pass karo
            ViewBag.Cities = GetCityList();
            return View(nklist);
        }

        [HttpPost]
        public IActionResult AllNK(string city)
        {
            List<NaatKhuwaan> nklist = new List<NaatKhuwaan>();
            db.OpenConnection();
            string q = "Select nkid,name,city from naatkhuwaan where city='" + city + "'";
            SqlDataReader sdr = db.GetData(q);
            while (sdr.Read())
            {
                NaatKhuwaan nk = new NaatKhuwaan();
                nk.nkid = sdr["nkid"].ToString();
                nk.name = sdr["name"].ToString();
                nk.city = sdr["city"].ToString();
                nklist.Add(nk);
            }
            sdr.Close();
            db.CloseConnection();

            // ✅ FIX: POST mein bhi Cities chahiye
            ViewBag.Cities = GetCityList();
            return View(nklist);
        }

        [HttpGet]
        public IActionResult Delete(string nkid)
        {
            db.OpenConnection();
            string q = "delete from naatKhuwaan where nkid='" + nkid + "'";
            db.IUD(q);
            db.CloseConnection();
            return RedirectToAction("AllNK");
        }

        [HttpGet]
        public IActionResult Detail(string nkid)
        {
            db.OpenConnection();
            string q = "Select nkid,name,city,gender,isAlive,email from naatkhuwaan where nkid='" + nkid + "'";
            SqlDataReader sdr = db.GetData(q);

            // ✅ FIX: null check — record na mile toh crash nahi hoga
            if (!sdr.Read())
            {
                sdr.Close();
                db.CloseConnection();
                return RedirectToAction("AllNK");
            }

            NaatKhuwaan nk = new NaatKhuwaan();
            nk.nkid    = sdr["nkid"].ToString();
            nk.name    = sdr["name"].ToString();
            nk.city    = sdr["city"].ToString();
            nk.gender  = char.Parse(sdr["gender"].ToString());
            nk.isAlive = bool.Parse(sdr["isAlive"].ToString());
            nk.email   = sdr["email"].ToString();
            sdr.Close();
            db.CloseConnection();
            return View(nk);
        }

        [HttpGet]
        public IActionResult Edit(string nkid)
        {
            db.OpenConnection();
            string q = "Select nkid,name,city,gender,isAlive,email from naatkhuwaan where nkid='" + nkid + "'";
            SqlDataReader sdr = db.GetData(q);

            // ✅ FIX: null check
            if (!sdr.Read())
            {
                sdr.Close();
                db.CloseConnection();
                return RedirectToAction("AllNK");
            }

            NaatKhuwaan nk = new NaatKhuwaan();
            nk.nkid    = sdr["nkid"].ToString();
            nk.name    = sdr["name"].ToString();
            nk.city    = sdr["city"].ToString();
            nk.gender  = char.Parse(sdr["gender"].ToString());
            nk.isAlive = bool.Parse(sdr["isAlive"].ToString());
            nk.email   = sdr["email"].ToString();
            sdr.Close();
            db.CloseConnection();
            return View(nk);
        }

        [HttpPost]
        public IActionResult Edit(NaatKhuwaan nk)
        {
            db.OpenConnection();
            string q = "update NaatKhuwaan set name='" + nk.name + "',city='" + nk.city +
                       "',gender='" + nk.gender + "',isAlive='" + nk.isAlive +
                       "',email='" + nk.email + "' where nkid='" + nk.nkid + "'";
            db.IUD(q);
            db.CloseConnection();
            // ✅ FIX: "AllNk" → "AllNK" (capital K — warna redirect fail hoga)
            return RedirectToAction("AllNK");
        }

        [HttpGet]
        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SignIn(string nkid, string password)
        {
            db.OpenConnection();
            string q = $"Select nkid,name from naatkhuwaan where nkid='{nkid}' and password='{password}'";
            SqlDataReader sdr = db.GetData(q);

            if (sdr.Read())
            {
                HttpContext.Session.SetString("nkid", sdr["nkid"].ToString());
                HttpContext.Session.SetString("name", sdr["name"].ToString());
                sdr.Close();
                db.CloseConnection();
                return RedirectToAction("Dashboard");
            }
            else
            {
                sdr.Close();
                db.CloseConnection();
                // ✅ FIX: ViewBag.Error — SignIn.cshtml ke saath match
                ViewBag.Error = "Invalid ID or Password. Please try again.";
                return View();
            }
        }

        [HttpGet]
        public IActionResult Dashboard()
        {
            string nkid = HttpContext.Session.GetString("nkid");
            if (nkid == null)
                return RedirectToAction("SignIn");
            return View();
        }
    }
}
