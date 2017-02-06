using ChoixResto.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChoixResto.Tests
{
    [TestClass]
    public class DalTests
    {
        private IDal dal;

        [TestInitialize]
        public void Init_AvantChaqueTest()
        {
            IDatabaseInitializer<BDDContext> init = new DropCreateDatabaseAlways<BDDContext>();
            Database.SetInitializer(init);
            init.InitializeDatabase(new BDDContext());

            dal = new Dal();
        }

        [TestMethod]
        public void CreateRestaurant_AvecUnNouveauRestaurant_ObtientTousLesRestaurantsRenvoitBienLeRestaurant()
        {
            dal.CreateRestaurant("La Bonne Fourchette", "01 02 03 04 05");
            List<Resto> restos = dal.GetRestaurants();

            Assert.IsNotNull(restos);
            Assert.AreEqual(1, restos.Count);
            Assert.AreEqual("La Bonne Fourchette", restos[0].Name);
            Assert.AreEqual("01 02 03 04 05", restos[0].Phone);
        }

        [TestMethod]
        public void UpdateRestaurant_CreationDUnNouveauRestaurantEtChangementNomEtTelephone_LaModificationEstCorrecteApresRechargement()
        {
            dal.CreateRestaurant("La Bonne Chourfette", "01 02 03 04 05");
            dal.UpdateRestaurant(1, "La Bonne Cuiller", null);

            List<Resto> restos = dal.GetRestaurants();

            Assert.IsNotNull(restos);
            Assert.AreEqual(1, restos.Count);
            Assert.AreEqual("La Bonne Cuiller", restos[0].Name);
            Assert.IsNull(restos[0].Phone);
        }

        [TestMethod]
        public void IsRestaurantExisting_AvecCreationDunRestaurant_RenvoiQuilExiste()
        {
            dal.CreateRestaurant("La Bonne Fourchette", "0102030405");
            bool existe = dal.IsRestaurantExisting("La Bonne Fourchette");

            Assert.IsTrue(existe);
        }

        [TestMethod]
        public void IsRestaurantExisting_AvecRestaurantInexistant_RenvoiQuilNExistePas()
        {
            bool existe = dal.IsRestaurantExisting("La Bonne Fourchette");

            Assert.IsFalse(existe);
        }

        [TestMethod]
        public void GetUser_UtilisateurInexistant_RetourneNull()
        {
            User u = dal.GetUser(1);
            Assert.IsNull(u);
        }

        [TestMethod]
        public void GetUser_IDNonNumerique_RetourneNull()
        {
            User u = dal.GetUser("abc");
            Assert.IsNull(u);
        }

        [TestMethod]
        public void AddUser_NouvelUtilisateurEtRecuperation_UtilisateurBienRecupere()
        {
            dal.AddUser("Nouvel Utilisateur", "12345");
            User u = dal.GetUser(1);

            Assert.IsNotNull(u);
            Assert.AreEqual("Nouvel Utilisateur", u.Name);

            u = dal.GetUser("1");

            Assert.IsNotNull(u);
            Assert.AreEqual("Nouvel Utilisateur", u.Name);
        }

        [TestMethod]
        public void Authentificate_LoginMdpOk_AuthentificationOK()
        {
            dal.AddUser("Nouvel Utilisateur", "12345");

            User u = dal.Authentificate("Nouvel Utilisateur", "12345");

            Assert.IsNotNull(u);
            Assert.AreEqual("Nouvel Utilisateur", u.Name);
        }

        [TestMethod]
        public void Authentificate_LoginOkMdpKo_AuthentificationKo()
        {
            dal.AddUser("Nouvel Utilisateur", "12345");

            User u = dal.Authentificate("Nouvel Utilisateur", "0");

            Assert.IsNull(u);
        }

        [TestMethod]
        public void Authentificate_LoginKoMdpOk_AuthentificationKo()
        {
            dal.AddUser("Nouvel Utilisateur", "12345");

            User u = dal.Authentificate("Nouvel", "12345");

            Assert.IsNull(u);
        }

        [TestMethod]
        public void Authentificate_LoginMdpKo_AuthentificationKo()
        {
            User u = dal.Authentificate("Nouvel Utilisateur", "12345");

            Assert.IsNull(u);
        }

        [TestMethod]
        public void HasAlreadyVoted_AvecIdNumerique_RetourneFalse()
        {
            bool pasVote = dal.HasAlreadyVoted(1, "abc");
            Assert.IsFalse(pasVote);
        }

        [TestMethod]
        public void HasAlreadyVoted_UtilisateurNaPasVote_RetourneFalse()
        {
            int idSondage = dal.CreateSondage();
            int idUser = dal.AddUser("Nouvel Utilisateur", "12345");

            bool pasVote = dal.HasAlreadyVoted(idSondage, idUser.ToString());

            Assert.IsFalse(pasVote);
        }

        [TestMethod]
        public void HasAlreadyVoted_UtilisateurAsVote_RetourneTrue()
        {
            int idSondage = dal.CreateSondage();
            int idUser = dal.AddUser("Nouvel Utilisateur", "12345");
            dal.CreateRestaurant("La Bonne Fourchette", "0102030405");
            dal.AddVote(idSondage, 1, idUser);

            bool aVote = dal.HasAlreadyVoted(idSondage, idUser.ToString());

            Assert.IsTrue(aVote);
        }

        [TestMethod]
        public void GetResults_AvecQuelquesChoix_RetourneBienLesResultats()
        {
            int idSondage = dal.CreateSondage();
            int idUser1 = dal.AddUser("Utilisateur1", "12345");
            int idUser2 = dal.AddUser("Utilisateur2", "12345");
            int idUser3 = dal.AddUser("Utilisateur3", "12345");

            dal.CreateRestaurant("Resto pinière", "0102030405");
            dal.CreateRestaurant("Resto pinambour", "0102030405");
            dal.CreateRestaurant("Resto mate", "0102030405");
            dal.CreateRestaurant("Resto ride", "0102030405");

            dal.AddVote(idSondage, 1, idUser1);
            dal.AddVote(idSondage, 3, idUser1);
            dal.AddVote(idSondage, 4, idUser1);
            dal.AddVote(idSondage, 1, idUser2);
            dal.AddVote(idSondage, 1, idUser3);
            dal.AddVote(idSondage, 3, idUser3);

            List<Results> resultats = dal.GetResults(idSondage);

            Assert.AreEqual(3, resultats[0].NbVotes);
            Assert.AreEqual("Resto pinière", resultats[0].Name);
            Assert.AreEqual("0102030405", resultats[0].Phone);
            Assert.AreEqual(2, resultats[1].NbVotes);
            Assert.AreEqual("Resto mate", resultats[1].Name);
            Assert.AreEqual("0102030405", resultats[1].Phone);
            Assert.AreEqual(1, resultats[2].NbVotes);
            Assert.AreEqual("Resto ride", resultats[2].Name);
            Assert.AreEqual("0102030405", resultats[2].Phone);
        }

        [TestMethod]
        public void GetResults_AvecDeuxSondages_RetourneBienLesResultats()
        {
            int idSondage = dal.CreateSondage();
            int idUser1 = dal.AddUser("Utilisateur1", "12345");
            int idUser2 = dal.AddUser("Utilisateur2", "12345");
            int idUser3 = dal.AddUser("Utilisateur3", "12345");

            dal.CreateRestaurant("Resto pinière", "0102030405");
            dal.CreateRestaurant("Resto pinambour", "0102030405");
            dal.CreateRestaurant("Resto mate", "0102030405");
            dal.CreateRestaurant("Resto ride", "0102030405");

            dal.AddVote(idSondage, 1, idUser1);
            dal.AddVote(idSondage, 3, idUser1);
            dal.AddVote(idSondage, 4, idUser1);
            dal.AddVote(idSondage, 1, idUser2);
            dal.AddVote(idSondage, 1, idUser3);
            dal.AddVote(idSondage, 3, idUser3);

            int idSondage2 = dal.CreateSondage();

            dal.AddVote(idSondage2, 2, idUser1);
            dal.AddVote(idSondage2, 3, idUser1);
            dal.AddVote(idSondage2, 1, idUser2);
            dal.AddVote(idSondage2, 4, idUser3);
            dal.AddVote(idSondage2, 3, idUser3);

            List<Results> resultats1 = dal.GetResults(idSondage);
            List<Results> resultats2 = dal.GetResults(idSondage2);

            Assert.AreEqual(3, resultats1[0].NbVotes);
            Assert.AreEqual("Resto pinière", resultats1[0].Name);
            Assert.AreEqual("0102030405", resultats1[0].Phone);
            Assert.AreEqual(2, resultats1[1].NbVotes);
            Assert.AreEqual("Resto mate", resultats1[1].Name);
            Assert.AreEqual("0102030405", resultats1[1].Phone);
            Assert.AreEqual(1, resultats1[2].NbVotes);
            Assert.AreEqual("Resto ride", resultats1[2].Name);
            Assert.AreEqual("0102030405", resultats1[2].Phone);

            Assert.AreEqual(1, resultats2[0].NbVotes);
            Assert.AreEqual("Resto pinambour", resultats2[0].Name);
            Assert.AreEqual("0102030405", resultats2[0].Phone);
            Assert.AreEqual(2, resultats2[1].NbVotes);
            Assert.AreEqual("Resto mate", resultats2[1].Name);
            Assert.AreEqual("0102030405", resultats2[1].Phone);
            Assert.AreEqual(1, resultats2[2].NbVotes);
            Assert.AreEqual("Resto pinière", resultats2[2].Name);
            Assert.AreEqual("0102030405", resultats2[2].Phone);
        }
    }
}
