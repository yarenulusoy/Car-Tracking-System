using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using yazlab2.Models;
using Newtonsoft.Json;
using System.Net;
using System.IO;

namespace yazlab2.Controllers
{

    public class CaptchaResponse
    {
        [JsonProperty("success")]
        public bool Success { get; set; }
        [JsonProperty("error-codes")]
        public List<string> ErrorCodes { get; set; }
    }

    public class LoginController : Controller
    {
        SqlConnection con = new SqlConnection();
        SqlCommand com = new SqlCommand();
        SqlDataReader dr;
        SqlConnection con2 = new SqlConnection();
        SqlCommand com2 = new SqlCommand();
        SqlDataReader dr2;
        int id, id2;
        List<int> aracliste = new List<int>();

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        void connectionString()
        {
            con.ConnectionString = "connectionstring";

        }
        void connectionString2()
        {
            con2.ConnectionString = "connectionstring";

        }
        [HttpPost]
        public IActionResult Index(LoginModel login)
        {

            connectionString();
            con.Open();
            com.Connection = con;
            com.CommandText = "select Arac_id from Araclar where Araclar.Kullanici_id in (select Kullanicilar.Id from Kullanicilar where KullaniciAdi='" + login.username + "' and Sifre='" + login.password + "')";
            dr = com.ExecuteReader();
            connectionString2();
            con2.Open();
            com2.Connection = con2;
            com2.CommandText = "select Id from Kullanicilar where KullaniciAdi='" + login.username + "' and Sifre='" + login.password + "'";
            dr2 = com2.ExecuteReader();


            if (dr.HasRows && dr2.HasRows)
            {

                while (dr.Read())
                {
                    aracliste.Add((int)dr["Arac_id"]);
                }

                HttpContext.Session.SetString("arabaid", string.Join(",", aracliste));
                con.Close();

                while (dr2.Read())
                {

                    id = (int)dr2["Id"];
                    string connection ="connectionstring";
                    using (SqlConnection sqlcon = new SqlConnection(connection))
                    {
                        string ekle = "insert into Giris_zamanlari(Kullanici_id,Giris_tarihi,Giris_saati) values('" + id + "','" + DateTime.Now + "','" + DateTime.Now.ToLongTimeString() + "')";
                        using (SqlCommand comm = new SqlCommand(ekle, sqlcon))
                        {
                            sqlcon.Open();
                            comm.ExecuteNonQuery();
                        }
                        sqlcon.Close();
                    }
                }


            return RedirectToAction("Index", "Welcome");
            }
            else
            {

                ViewBag.Message = "1";
                con.Close();
                //HttpContext.Session.SetString("yanlis", string.Join(",", say));
                return View();

            }


        }

        public IActionResult Error(int id)
        {
            HttpContext.Session.SetString("sayac", (id + 1).ToString());

            return PartialView("_MessageLogin");
        }
        public IActionResult Error2(int id)
        {
            HttpContext.Session.SetString("sayac", (id).ToString());


            return RedirectToAction("Index", "Login");
        }
        public IActionResult Error3(int id)
        {
            var response = Request.Form["g-recaptcha-response"];
            const string secret = "secretkey";
            //Kendi Secret keyinizle değiştirin.
            HttpContext.Session.SetString("sayac", "0");
            var client = new WebClient();
            var reply =
                client.DownloadString(
                    string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}", secret, response));

            var captchaResponse = JsonConvert.DeserializeObject<CaptchaResponse>(reply);

            if (!captchaResponse.Success)
            {
                TempData["Message"] = "Lütfen güvenliği doğrulayınız.";
                TempData["yanlis"] = "1";
            }
            else
                TempData["Message"] = "Güvenlik başarıyla doğrulanmıştır.";

            return RedirectToAction("Index", "Login");


        }
        public IActionResult Cikis(int id)
        {
            connectionString();
            con.Open();
            com.Connection = con;
            com.CommandText = "select Kullanici_id from Araclar where Arac_id='" + id + "'";
            dr = com.ExecuteReader();
            if (dr.Read())
            {

                id2 = (int)dr["Kullanici_id"];

                con.Close();



                string connection = "connectionstring";
                using (SqlConnection sqlcon = new SqlConnection(connection))
                {
                    string ekle = "insert into Cikis_zamanlari(Kullanici_id,Cikis_tarihi,Cikis_saati) values('" + id2 + "','" + DateTime.Now + "','" + DateTime.Now.ToLongTimeString() + "')";
                    using (SqlCommand comm = new SqlCommand(ekle, sqlcon))
                    {
                        sqlcon.Open();
                        comm.ExecuteNonQuery();
                    }
                    sqlcon.Close();
                }
            }

            return RedirectToAction("Index", "Login");
        }
    }
}
