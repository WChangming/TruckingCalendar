using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using TruckingCalendar.Models;

namespace TruckingCalendar.Controllers
{
    public class TruckingsController : Controller
    {
        private TruckingEntities db = new TruckingEntities();

        // GET: Calendar
        public ActionResult Calendar(DateTime? truckdate)
        {
            ViewBag.TruckDate = truckdate ?? DateTime.Today;
            return View();
        }

        // GET: Truckings/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Trucking trucking = db.Truckings.Find(id);
            if (trucking == null)
            {
                return HttpNotFound();
            }
            return View(trucking);
        }

        // GET: Truckings/Create
        public ActionResult Create(DateTime truckDate)
        {
            ViewBag.TruckDate = truckDate;
            return View();
        }

        // POST: Truckings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TruckID,TruckDate,LoadMinutes,TruckStatus")] Trucking trucking)
        {
            if (ModelState.IsValid)
            {
                db.Truckings.Add(trucking);
                db.SaveChanges();
                return RedirectToAction("Calendar", new { truckdate = trucking.TruckDate });
            }

            return View(trucking);
        }

        // GET: Truckings/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Trucking trucking = db.Truckings.Find(id);
            if (trucking == null)
            {
                return HttpNotFound();
            }
            return View(trucking);
        }

        // POST: Truckings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TruckID,TruckDate,LoadMinutes,TruckStatus")] Trucking trucking)
        {
            if (ModelState.IsValid)
            {
                db.Entry(trucking).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Calendar", new { truckdate = trucking.TruckDate });
            }
            return View(trucking);
        }

        // GET: Truckings/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Trucking trucking = db.Truckings.Find(id);
            if (trucking == null)
            {
                return HttpNotFound();
            }
            return View(trucking);
        }

        // POST: Truckings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Trucking trucking = db.Truckings.Find(id);
            db.Truckings.Remove(trucking);
            db.SaveChanges();
            return RedirectToAction("Calendar", new { truckdate = trucking.TruckDate });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public JsonResult GetEvents(DateTime start, DateTime end)
        {
            var truckings = (from t in db.Truckings
                          where t.TruckDate >= start && DbFunctions.AddMinutes(t.TruckDate, t.LoadMinutes) <= end
                          select new { t.TruckID, t.TruckDate, t.LoadMinutes, t.TruckStatus }).ToList()
            .Select(x => new Events()
            {
                id = x.TruckID,
                title = "Trucking " + x.TruckID + ", " + x.LoadMinutes + "min ",
                start = x.TruckDate,
                end = x.TruckDate.AddMinutes(x.LoadMinutes),
                color = Enum.GetName(typeof(TruckStatus), x.TruckStatus)
            }).ToArray();
            return Json(truckings, JsonRequestBehavior.AllowGet);
        }

        public void UpdateEvent(int id, DateTime start, DateTime end)
        {
            Trucking trucking = db.Truckings.Find(id);
            trucking.TruckDate = start;
            trucking.LoadMinutes = (short)(end - start).TotalMinutes;
            db.SaveChanges();
        }
    }
}
