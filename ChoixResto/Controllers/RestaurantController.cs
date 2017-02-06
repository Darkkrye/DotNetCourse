using ChoixResto.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ChoixResto.Controllers
{
    public class RestaurantController : Controller
    {
        private IDal dal;

        public RestaurantController() : this(new Dal()) { }

        public RestaurantController(IDal dalIoc)
        {
            dal = dalIoc;
        }

        // GET: Restaurant
        public ActionResult Index()
        {
            List<Resto> lr = dal.GetRestaurants();

            return View(lr);
        }

        public ActionResult UpdateRestaurant(int? id)
        {
            // string id = Request.Url.AbsolutePath.Split('/').Last();
            // string id = Request.QueryString["id"];

            if (id.HasValue)
            {
                Resto r = dal.GetRestaurants().FirstOrDefault(re => re.Id == id.Value);
                if (r == null)
                    return View("Error");

                ViewBag.Id = id.Value;
                return View(r);
            }
            else
                return HttpNotFound();
        }

        /*[HttpPost]
        public ActionResult UpdateRestaurant(int? id, string name, string phone)
        {
            if (id.HasValue)
            {
                using (IDal dal = new Dal())
                {
                    dal.UpdateRestaurant(id.Value, name, phone);
                    return RedirectToAction("Index");
                }
            }
            else
                return View("Error");
        }*/

        [HttpPost]
        public ActionResult UpdateRestaurant(Resto r)
        {
            if (!ModelState.IsValid)
                return View(r);

            dal.UpdateRestaurant(r.Id, r.Name, r.Phone);
            return RedirectToAction("Index");
        }

        public ActionResult CreateRestaurant()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateRestaurant(Resto r)
        {
            if (dal.IsRestaurantExisting(r.Name))
            {
                ModelState.AddModelError("Name", "Ce nom de restaurant existe déjà.");
                return View(r);
            }
            if (!ModelState.IsValid)
            {
                return View(r);
            }

            dal.CreateRestaurant(r.Name, r.Phone);

            return RedirectToAction("Index");
        }
    }
}