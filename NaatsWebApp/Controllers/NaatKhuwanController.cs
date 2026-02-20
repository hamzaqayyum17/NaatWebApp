using Microsoft.AspNetCore.Mvc;
using NaatsWebApp.Models;
using System.Data.SqlClient;

namespace NaatsWebApp.Controllers
{
    public class NaatKhuwanController : Controller
    {
        DBAccess db=new DBAccess();
        //public IActionResult Index()
        //{
        //    return View();
        //}
        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        public IActionResult SignUp(NaatKhuwaan nk)
        {
            nk.nkid = nk.email.Split('@')[0];
            DBAccess Db = new DBAccess();
            if (ModelState.IsValid)
            {
                Db.OpenConnection();
                string q = "insert into NaatKhuwaan Values('" + nk.nkid + "','" + nk.name + "','" + nk.city + "','" + nk.gender + "','" + nk.isAlive + "','" + nk.email + "','" + nk.password + "')";
                Db.IUD(q);
                Db.CloseConnection();
                return View();
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
            return View(nklist);
        }

        [HttpPost]
        public IActionResult AllNK(string city)
        {
            List<NaatKhuwaan> nklist = new List<NaatKhuwaan>();
            db.OpenConnection();
            string q = "Select nkid,name,city from naatkhuwaan where city='"+city+"'";
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
            return View(nklist);
        }
        [HttpGet]
        public IActionResult Delete(string nkid)
        {            
            db.OpenConnection();          
            string q = "delete from naatKhuwaan where nkid='"+nkid+"'";
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
            sdr.Read();       
            NaatKhuwaan nk = new NaatKhuwaan();
            nk.nkid = sdr["nkid"].ToString();
            nk.name = sdr["name"].ToString();
            nk.city = sdr["city"].ToString();
            nk.gender = char.Parse(sdr["gender"].ToString());
            nk.isAlive = bool.Parse(sdr["isAlive"].ToString());
            nk.email = sdr["email"].ToString();            
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
            sdr.Read();
            NaatKhuwaan nk = new NaatKhuwaan();
            nk.nkid = sdr["nkid"].ToString();
            nk.name = sdr["name"].ToString();
            nk.city = sdr["city"].ToString();
            nk.gender = char.Parse(sdr["gender"].ToString());
            nk.isAlive = bool.Parse(sdr["isAlive"].ToString());
            nk.email = sdr["email"].ToString();
            sdr.Close();
            db.CloseConnection();
            return View(nk);

        }
        [HttpPost]
        public IActionResult Edit(NaatKhuwaan nk)
        {            
            DBAccess Db = new DBAccess();                        
            Db.OpenConnection();
            string q = "update NaatKhuwaan set name='" + nk.name + "',city='" + nk.city + "',gender='" + nk.gender + "',isAlive='" + nk.isAlive + "',email='" + nk.email + "' where nkid='" + nk.nkid + "'";
            Db.IUD(q);
            Db.CloseConnection();
            return RedirectToAction("AllNk");
        }
    }
}
