using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using USF_Accom.Models;

namespace USF_Accom.Controllers
{
    public class RequestController : Controller
    {
        private USFAccomEntities db = new USFAccomEntities();
        // GET: Request
        public ActionResult Index()
        {
            if (Session["UserName"] == null)
            {
                Response.Redirect("/Account/Login");
            }
            var stuEmail = Convert.ToString(Session["UserName"]);
            var userID = db.StudentDetails.Where(x => x.Student_Email == stuEmail).Select(x => x.Student_ID).FirstOrDefault();
            var posted = (from a in db.Requests
                        join b in db.Communities
                        on a.CommunityID equals b.Community_ID
                        where a.RequestorID == userID
                        select new RequestsPosted()
                        {
                            RequesteeID = a.RequesteeID,
                            AptNo = a.AptNo,
                            CommunityName = b.Community_Name,
                            CommunityID = a.CommunityID
                        }).ToList();

            var received = (from a in db.Requests
                        join b in db.Communities on a.CommunityID equals b.Community_ID
                        join s in db.StudentDetails on a.RequestorID equals s.Student_ID
                        where a.RequesteeID == userID && s.Student_ID == a.RequestorID
                        select new RequestsReceived()
                        {
                            Student_Name = s.Student_FirstName + " " + s.Student_LastName,
                            Student_Phone = s.Student_Phone,
                            RequestorID = a.RequestorID,
                            AptNo = a.AptNo,
                            CommunityName = b.Community_Name,
                            CommunityID = a.CommunityID
                        }).ToList();

            RequestsViewModel reqObj = new RequestsViewModel();
            reqObj.RequestsPosted = posted;
            reqObj.RequestsReceived = received;

            return View(reqObj);
        }

        // GET: Request/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Request/Create
        public ActionResult Create(string requesteeId, int communityId, string aptNo)
        {
            Models.Request reqObj = new Models.Request();

            var stuEmail = Convert.ToString(Session["UserName"]);
            var userID = db.StudentDetails.Where(x => x.Student_Email == stuEmail).Select(x => x.Student_ID).FirstOrDefault();

            reqObj.RequestorID = userID;
            reqObj.RequesteeID = requesteeId;
            reqObj.CommunityID = communityId;
            reqObj.AptNo = aptNo;

            db.Requests.Add(reqObj);
            db.SaveChanges();

            return RedirectToAction("Index", "Request");
        }

        // POST: Request/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Request/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Request/Edit/5
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

        // GET: Request/Delete/5 //requestorId}/{communityId}/{aptNo
        public ActionResult Accept(string requestorId, int communityId, string aptNo)
        {
            

            var stuEmail = Convert.ToString(Session["UserName"]);
            var userID = db.StudentDetails.Where(x => x.Student_Email == stuEmail).Select(x => x.Student_ID).FirstOrDefault();

            var reqRec = db.Requests.Where(x => x.RequestorID == requestorId && x.RequesteeID == userID && x.CommunityID == communityId && x.AptNo == aptNo)
                                                   .Select(y => y).FirstOrDefault();

            db.Requests.Remove(reqRec);
            db.SaveChanges();

            var availabilityRec = db.Availabilities.Where(x => x.Student_ID == userID && x.Community_ID == communityId && x.Community_ID == communityId && x.AptNo == aptNo)
                                                   .Select(y => y).Single();
            availabilityRec.AvailableSlots = availabilityRec.AvailableSlots - 1;
            if(availabilityRec.AvailableSlots == 0)
            {
                availabilityRec.Status = false;
            }
            db.Availabilities.Attach(availabilityRec);
            db.Entry(availabilityRec).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();


            return RedirectToAction("Index", "Request");
        }
    }
}
