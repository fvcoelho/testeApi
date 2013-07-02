using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Wallmart.Database;

namespace MvcApplication1.Controllers
{
    public class CityController : Controller
    {
        CityDAL c = new CityDAL();

        //
        // GET: /City/
        public ActionResult Index()
        {
            var model = c.ListAll();
            return View(model);
        }

        //
        // GET: /City/Details/5
        public ActionResult Details(int id)
        {
            var model = c.ListAll().First(city => city.cityId == id);
            return View(model);
        }

        //
        // GET: /City/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /City/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                var s = new StateDAL();
                var newCity = new City();

                UpdateModel(newCity);
                newCity.state = s.ListAll().FirstOrDefault();
                c.Add(newCity);

                return RedirectToAction("Index");
            }
            catch
            {
                ModelState.AddModelError("cityId", "Error no create city");
                return View();
            }
        }

        //
        // GET: /City/Edit/5
        public ActionResult Edit(int id)
        {
            var model = c.ListAll().First(city => city.cityId == id);
            return View(model);
        }

        //
        // POST: /City/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            var model = new City();
            var s = new StateDAL();
            try
            {
                UpdateModel(model);
                model.state = s.ListAll().FirstOrDefault();
                c.UpdateById(model);

                return RedirectToAction("Index");
            }
            catch
            {
                ModelState.AddModelError("cityId", "Error no edit city");
                return View(model);
            }
        }

        //
        // POST/GET: /City/Delete/5
        public void Delete(int id)
        {
            c.RemoveById(id);
        }
    }
}
