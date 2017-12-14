using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using USF_Accom.Models;

namespace USF_Accom.Controllers
{
    public class AvailabilityController : Controller
    {
        private USFAccomEntities db = new USFAccomEntities();
        // GET: Availability
        public ActionResult Index()
        {
          if(Session["UserName"] == null)
            {
                Response.Redirect("/Account/Login");
            }
            var stuEmail = Convert.ToString(Session["UserName"]);
            var userID = db.StudentDetails.Where(x => x.Student_Email == stuEmail).Select(x => x.Student_ID).FirstOrDefault();
            var slots = from a in db.Availabilities
                        join b in db.Communities
                        on a.Community_ID equals b.Community_ID
                        where a.Status == true && a.Student_ID != userID
                        select new AvailabilityViewModel()
                        {
                            StudentId = a.Student_ID,
                            AptNo = a.AptNo,
                            CommunityName = b.Community_Name,
                            CommunityID = a.Community_ID,
                            Preference = a.Preference,
                            Slots = a.AvailableSlots
                        };

            return View(slots.ToList());
        }

        // GET: Availability/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Availability/Create
        public ActionResult Create()
        {
            if (Session["UserName"] == null)
            {
                Response.Redirect("/Account/Login");
            }
            IEnumerable<SelectListItem> items = db.Communities
                                                .Select(c => new SelectListItem
                                                {
                                                    Value = c.Community_ID.ToString(),
                                                    Text = c.Community_Name
                                                });
            ViewBag.Communities = items;
            var stuEmail = Convert.ToString(Session["UserName"]);
            ViewBag.StudentID = db.StudentDetails.Where(x => x.Student_Email == stuEmail).Select(x => x.Student_ID).FirstOrDefault();
            return View();
        }

        // POST: Availability/Create
        [HttpPost]
        public ActionResult Create(Availability collection)
        {
            try
            {
                db.Availabilities.Add(collection);
                db.SaveChanges();

                var slots = from a in db.Availabilities
                            join b in db.Communities
                            on a.Community_ID equals b.Community_ID
                            where a.Status == true && a.Student_ID == collection.Student_ID
                            select new AvailabilityViewModel()
                            {
                                StudentId = a.Student_ID,
                                AptNo = a.AptNo,
                                CommunityName = b.Community_Name,
                                Preference = a.Preference,
                                Slots = a.AvailableSlots
                            };
                return new JsonResult() { Data = slots, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                
            }
            catch
            {
                return View();
            }
        }

        public ActionResult GetSlots()
        {
            if (Session["UserName"] == null)
            {
                Response.Redirect("/Account/Login");
            }
            var stuEmail = Convert.ToString(Session["UserName"]);
            var StudentID = db.StudentDetails.Where(x => x.Student_Email == stuEmail).Select(x => x.Student_ID).FirstOrDefault();

            var slots = from a in db.Availabilities
                        join b in db.Communities
                        on a.Community_ID equals b.Community_ID
                        where a.Status == true && a.Student_ID == StudentID
                        select new AvailabilityViewModel()
                        {
                            StudentId = a.Student_ID,
                            AptNo = a.AptNo,
                            CommunityName = b.Community_Name,
                            Preference = a.Preference,
                            Slots = a.AvailableSlots
                        };
            return new JsonResult() { Data = slots, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        // GET: Availability/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Availability/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Availability/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Availability/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
