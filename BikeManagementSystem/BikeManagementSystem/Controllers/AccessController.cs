using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using BikeManagementSystem.Models;
namespace BikeManagementSystem.Controllers
{
    public class AccessController : Controller
    {
        BIKEMGMTSYSTEMEntities2 db = new BIKEMGMTSYSTEMEntities2();
        // GET: Access
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(User User)
        {
            var user = db.Users.SingleOrDefault(x => x.email == User.email && x.password == User.password);
            if (user != null)
            {
                Session["userId"] = user.idUsers;
                Session["name"] = user.name;
                Session["email"] = user.email;
                Session["image"] = Convert.ToBase64String(user.photo);
                string roleName =db.Rols
                   .Where(r => r.idRol == user.idRol)
                   .Select(r => r.description)
                   .FirstOrDefault();
                Session["varRoleName"] = roleName;
                if(roleName == "Admin")
                {
                    return RedirectToAction("DashBoard", "Admin");
                }
                else
                {
                    return RedirectToAction("NewOrder", "Order");
                }
            }
            ViewBag.InvalidCredential = "Invalid Email or Password";
            return View();
        }

        [HttpGet]
        public ActionResult NewRegistration()
        {
            return View();
        }

        [HttpPost]
        public ActionResult NewRegistration(User user, HttpPostedFileBase frontCnic, HttpPostedFileBase backCnic, HttpPostedFileBase imgFile)
        {
            try
            {
                user.idRol = 2;
                user.isActive = true;
                if (frontCnic != null) 
                {
                    user.cnicFront = ConvertImage(frontCnic);
                }
                if (backCnic != null)
                {
                    user.cnicBack = ConvertImage(backCnic);
                }
                if (frontCnic != null)
                {
                    user.photo = ConvertImage(imgFile);
                }
                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Login");
            }
            catch (Exception)
            {
                ViewBag.Error("Error in registring user!!");
                throw;
            }
        }
        byte[] ConvertImage(HttpPostedFileBase imgFile)
        {
            byte[] imageData = new byte[0];
            using (var binaryReader = new BinaryReader(imgFile.InputStream))
            {
                imageData = binaryReader.ReadBytes(imgFile.ContentLength);
            }
            return imageData;
        }

        public ActionResult Logout()
        {
            Session.Abandon();
            Session.RemoveAll();
            return RedirectToAction("Login");
        }
    }
}