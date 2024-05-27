using BikeManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace BikeManagementSystem.Controllers
{
    public class AdminController : Controller
    {
        BIKEMGMTSYSTEMEntities2 db = new BIKEMGMTSYSTEMEntities2();
        InvoiceService _invoiceServices;        
        static int userId = 0;
        public AdminController()
        {
            _invoiceServices = new InvoiceService(db);
        }

        

        // GET: Admin
        #region Dashboard
        [HttpGet]
        public ActionResult DashBoard()
        {
            return View();
        }

        public JsonResult GetChartData()
        {
            var data = db.Invoices
                .Include(i => i.User)
                .GroupBy(i => new { i.User.name })
                .Select(g => new
                {
                    PersonName = g.Key.name,
                    TotalAmount = g.Sum(i => i.totalAmount)
                })
                .ToList();

            return Json(data, JsonRequestBehavior.AllowGet);
        }
        #endregion


        #region Users
        [HttpGet]
        public ActionResult Users()
        {
            ViewBag.UserGrid = GetUsers();
            ViewBag.Btn = "Save";
            if(userId != 0)
            {
                User u = db.Users.Find(userId);
                userId = 0;
                ViewBag.Btn = "Update";
                return View(u);
            }
            return View();
        }
        [HttpPost]
        public ActionResult Users(User user,HttpPostedFileBase frontCnic,HttpPostedFileBase backCnic, HttpPostedFileBase imgFile)
        {
            if(user.idUsers != 0)
            {
                if(frontCnic != null && frontCnic.ContentLength> 0)
                {
                    user.cnicFront = ConvertImage(frontCnic);
                }
                if (backCnic != null && backCnic.ContentLength > 0)
                {
                    user.cnicBack = ConvertImage(backCnic);
                }
                if (imgFile != null && imgFile.ContentLength > 0)
                {
                    user.photo = ConvertImage(imgFile);
                }
                user.isActive = true;
                db.Users.Attach(user);
                db.Entry(user).State = EntityState.Modified;
            }
            else
            {
                user.idRol = 2;
                user.isActive = true;
                db.Users.Add(user);
            }
            db.SaveChanges();
            return RedirectToAction("Users");
        }
        List<User> GetUsers()
        {
            return db.Users.ToList();
        }
        
        public ActionResult EditUser(int id)
        {
            userId = id;
            return RedirectToAction("Users");
        }
        public ActionResult DeleteUser(int id)
        {
            User user = db.Users.Find(id);
            db.Users.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Users");
        }
        #endregion

        #region AccountHistory
        [HttpGet]
        public ActionResult AccountHistory()
        {
            ViewBag.InvoiceGrid = GetInvoice();
            return View();
        }
        List<Invoice> GetInvoice()
        {
            return db.Invoices.ToList();
        }
        public ActionResult GetInvoiceDetails(int id)
        {
            var invoice = _invoiceServices.GetInvoicesWithDetails().FirstOrDefault(i => i.InvoiceId == id);
            if (invoice == null)
            {
                return HttpNotFound();
            }
            return PartialView("_InvoiceDetails", invoice);
        }

        #endregion
        byte[] ConvertImage(HttpPostedFileBase imgFile)
        {
            byte[] imageData = new byte[0];
            using (var binaryReader = new BinaryReader(imgFile.InputStream))
            {
                imageData = binaryReader.ReadBytes(imgFile.ContentLength);
            }
            return imageData;
        }



    }

    public class InvoiceModel
    {
        public int InvoiceId { get; set; }
        public double TotalHours { get; set; }
        public double TotalAmount { get; set; }
        public int PaymentMethodId { get; set; }
        public string PaymentMethodName { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerPhone { get; set; }
        public int BikeId { get; set; }
        public string BikeName { get; set; }
        public string BikeBrand { get; set; }
        public decimal? BikePricePerHour { get; set; }
    }

    public class InvoiceService
    {
        private readonly BIKEMGMTSYSTEMEntities2 db;

        public InvoiceService(BIKEMGMTSYSTEMEntities2 context)
        {
            db = context;
        }

        public IQueryable<InvoiceModel> GetInvoicesWithDetails()
        {
            var query = from invoice in db.Invoices
                        join user in db.Users on invoice.idUser equals user.idUsers
                        join payment in db.Payments on invoice.idPayment equals payment.idPayment
                        join bike in db.Bikes on invoice.idBike equals bike.idBike
                        select new InvoiceModel
                        {
                            InvoiceId = invoice.idInvoice,
                            TotalHours = (double)invoice.totalHours,
                            TotalAmount = (double)invoice.totalAmount,
                            PaymentMethodId = payment.idPayment,
                            PaymentMethodName = payment.name,
                            CustomerId = user.idUsers,
                            CustomerName = user.name,
                            CustomerEmail = user.email,
                            CustomerPhone = user.phone,
                            BikeId = bike.idBike,
                            BikeName = bike.name,
                            BikeBrand = bike.brand,
                            BikePricePerHour = bike.price_hour
                        };

            return query;
        }
    }


}