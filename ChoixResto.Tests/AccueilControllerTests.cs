using ChoixResto.Controllers;
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
    public class AccueilControllerTests
    {
        [TestMethod]
        public void AccueilController_Index_RenvoiVueParDefaut()
        {
            AccueilController ctrlr = new AccueilController();
            ViewResult result = (ViewResult)ctrlr.Index();

            Assert.AreEqual(string.Empty, result.ViewName);
        }
    }
}
