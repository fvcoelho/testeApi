using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Wallmart.Database;

namespace MvcApplication1.Controllers
{
    public class StateController : Controller
    {
        StateDAL s = new StateDAL();

        //
        // GET: /State/
        public ActionResult Index()
        {
            var model = s.ListAll();
            return View(model);
        }

        //
        // GET: /State/Details/5
        public ActionResult Details(int id)
        {
            var model = s.ListAll().First(state => state.stateId == id);
            return View(model);
        }

        //
        // GET: /User/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /User/Create

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                var newState = new State();
                UpdateModel(newState);
                s.Add(newState);
                return RedirectToAction("Index");
            }
            catch
            {
                ModelState.AddModelError("stateId", "Error on create State !");
                return View();
            }
        }

        //
        // GET: /State/Edit/5
        public ActionResult Edit(int id)
        {
            var model = s.ListAll().First(state => state.stateId == id);
            return View(model);
        }

        //
        // POST: /State/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            var model = new State();
            try
            {
                UpdateModel(model);
                s.UpdateById(model);

                return RedirectToAction("Index");
            }
            catch
            {
                ModelState.AddModelError("stateId", "Error on update State !");
                return View(model);
            }
        }

        //
        // POST/GET: /State/Delete/5
        public void Delete(int id)
        {
            s.RemoveById(id);
        }

        //
        // POST/GET: /State/Search
        public ActionResult Search(string name)
        {
            var model = s.ListByName(name);
            return View("Index", model);
        }
    }

}
