using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using BikeManagementSystem.Models;

namespace BikeManagementSystem.Controllers
{
    
    public class OrderController : Controller
    {
        BIKEMGMTSYSTEMEntities2 db = new BIKEMGMTSYSTEMEntities2();
        // GET: Order
        [HttpGet]
        public ActionResult NewOrder()
        {
            ViewBag.GetBikes = GetBike();
            return View();
        }
        [HttpPost]
        public JsonResult PlaceOrder(int bikeId, double totalHours, double totalAmount, int paymentMethod)
        {
            Bike bike = db.Bikes.Find(bikeId);
            if (bike != null)
            {
                bike.isActive = false;
                db.SaveChanges();
                var invoice = new Invoice
                {
                    idBike = bikeId,
                    totalHours = totalHours,
                    totalAmount = totalAmount,
                    idPayment = paymentMethod,
                    invoiceTime = DateTime.Now.TimeOfDay,
                    idUser = Convert.ToInt32(Session["userId"])
                };

                db.Invoices.Add(invoice);
                db.SaveChanges();

                
                return Json(new { success = true });
            }
            else
            {
                return Json(new {success=false });
            }
        }

        List<Bike> GetBike()
        {
            return db.Bikes.ToList();
        }
    }
}