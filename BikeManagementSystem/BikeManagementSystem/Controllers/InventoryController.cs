using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BikeManagementSystem.Models;
using System.Data.Entity;

namespace BikeManagementSystem.Controllers
{
    public class InventoryController : Controller
    {
        BIKEMGMTSYSTEMEntities2 db = new BIKEMGMTSYSTEMEntities2();
        static int bikeId = 0;
        // GET: Inventory
        [HttpGet]
        public ActionResult Bikes()
        {
            ViewBag.Btn = "Save";
            ViewBag.BikeGrid = GetBikes();
            if(bikeId != 0)
            {
                Bike bike = db.Bikes.Find(bikeId);
                bikeId = 0;
                ViewBag.Btn = "Update";
                return View(bike);
            }
            return View();
        }

        [HttpPost]
        public ActionResult Bikes(Bike bike,HttpPostedFileBase imgFile)
        {
            if(bike.idBike != 0)
            {
                if (imgFile != null)
                {
                    bike.photo = ConvertImage(imgFile);
                }
                db.Bikes.Attach(bike);
                db.Entry(bike).State = EntityState.Modified;
            }
            else
            {
                if(imgFile != null)
                {
                    bike.photo = ConvertImage(imgFile);
                }
                db.Bikes.Add(bike);
            }
            db.SaveChanges();
            return RedirectToAction("Bikes");
        }
        List<Bike> GetBikes()
        {
            return db.Bikes.ToList();
        }
        public ActionResult EditBike(int id)
        {
            bikeId = id;
            return RedirectToAction("Bikes");
        }
        public ActionResult DeleteBike(int id)
        {
            Bike bike = db.Bikes.Find(id);
            db.Bikes.Remove(bike);
            db.SaveChanges();
            return RedirectToAction("Bikes");
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
    }
}