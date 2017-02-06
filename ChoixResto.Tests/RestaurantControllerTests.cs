using ChoixResto.Controllers;
using ChoixResto.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ChoixResto.Tests
{
    [TestClass]
    public class RestaurantControllerTests
    {
        [TestMethod]
        public void RestaurantController_Index_LeControleurEstOk()
        {
            using(IDal dal = new DalEnDur())
            {
                RestaurantController ctrlr = new RestaurantController(dal);
                ViewResult result = (ViewResult)ctrlr.Index();

                List<Resto> model = (List<Resto>)result.Model;
                Assert.AreEqual("Resto pinambour", model[0].Name);
            }
        }

        [TestMethod]
        public void RestaurantController_ModifierRestaurantAvecRestoInvalide_RenvoiVueParDefaut()
        {
            using(IDal dal = new DalEnDur())
            {
                RestaurantController ctrlr = new RestaurantController(dal);
                ctrlr.ModelState.AddModelError("Name", "Le nom du restaurant doit être saisi");

                ViewResult result = (ViewResult)ctrlr.UpdateRestaurant(new Resto { Id = 1, Name = null, Phone = "0102030405" });

                Assert.AreEqual(string.Empty, result.ViewName);
                Assert.IsFalse(result.ViewData.ModelState.IsValid);
            }
        }

        [TestMethod]
        public void RestaurantController_ModifierRestaurantAvecRestoInvalideEtBindingDeModele_RenvoiVueParDefaut()
        {
            RestaurantController ctrlr = new RestaurantController(new DalEnDur());
            Resto r = new Resto { Id = 1, Name = null, Phone = "0102030405" };
            ctrlr.ValideLeModele(r);

            ViewResult result = (ViewResult)ctrlr.UpdateRestaurant(r);

            Assert.AreEqual(string.Empty, result.ViewName);
            Assert.IsFalse(result.ViewData.ModelState.IsValid);
        }

        [TestMethod]
        public void RestaurantController_ModifierRestaurantAvecRestoValide_CreerRestaurantEtRenvoiVueIndex()
        {
            using(IDal dal = new DalEnDur())
            {
                RestaurantController ctrlr = new RestaurantController(dal);
                Resto r = new Resto { Id = 1, Name = "Resto mate", Phone = "0102030405" };
                ctrlr.ValideLeModele(r);

                RedirectToRouteResult result = (RedirectToRouteResult)ctrlr.UpdateRestaurant(r);

                Assert.AreEqual("Index", result.RouteValues["action"]);
                Resto rt = dal.GetRestaurants().First();
                Assert.AreEqual("Resto mate", rt.Name);
            }
        }
    }
}
