using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ChoixResto.Models;

namespace ChoixResto.Models
{
    public class DalEnDur : IDal
    {
        private List<Resto> listeDesRestaurants;
        private List<User> listeDesUtilisateurs;
        private List<Sondage> listeDessondages;

        public DalEnDur()
        {
            listeDesRestaurants = new List<Resto>
            {
            new Resto { Id = 1, Name = "Resto pinambour", Phone = "0102030405"},
            new Resto { Id = 2, Name = "Resto pinière", Phone = "0102030405"},
            new Resto { Id = 3, Name = "Resto toro", Phone = "0102030405"},
            };
            listeDesUtilisateurs = new List<User>();
            listeDessondages = new List<Sondage>();
        }

        public int AddUser(string name, string password)
        {
            int id = listeDesUtilisateurs.Count == 0 ? 1 : listeDesUtilisateurs.Max(u => u.Id) + 1;
            listeDesUtilisateurs.Add(new User { Id = id, Name = name, Password = password });

            return id;
        }

        public void AddVote(int idSondage, int idResto, int idUser)
        {
            Vote vote = new Vote
            {
                Resto = listeDesRestaurants.First(r => r.Id == idResto),
                User = listeDesUtilisateurs.First(u => u.Id == idUser)
            };
            Sondage sondage = listeDessondages.First(s => s.Id == idSondage);
            sondage.Votes.Add(vote);

        }

        public User Authentificate(string name, string password)
        {
            return listeDesUtilisateurs.FirstOrDefault(u => u.Name == name && u.Password == password);
        }

        public void CreateRestaurant(string name, string phone)
        {
            int id = listeDesRestaurants.Count == 0 ? 1 : listeDesRestaurants.Max(r => r.Id) + 1;
            listeDesRestaurants.Add(new Resto { Id = id, Name = name, Phone = phone });
        }

        public int CreateSondage()
        {
            int id = listeDessondages.Count == 0 ? 1 : listeDessondages.Max(s => s.Id) + 1;
            listeDessondages.Add(new Sondage { Id = id, Date = DateTime.Now, Votes = new List<Vote>() });

            return id;
        }

        public void Dispose()
        {
            listeDesRestaurants = new List<Resto>();
            listeDesUtilisateurs = new List<User>();
            listeDessondages = new List<Sondage>();
        }

        public List<Resto> GetRestaurants()
        {
            return listeDesRestaurants;
        }

        public List<Results> GetResults(int idSondage)
        {
            List<Resto> restaurants = GetRestaurants();
            List<Results> resultats = new List<Results>();
            Sondage sondage = listeDessondages.First(s => s.Id == idSondage);
            foreach (IGrouping<int, Vote> grouping in sondage.Votes.GroupBy(v => v.Resto.Id))
            {
                int idRestaurant = grouping.Key;
                Resto resto = restaurants.First(r => r.Id == idRestaurant);
                int nombreDeVotes = grouping.Count();
                resultats.Add(new Results { Name = resto.Name, Phone = resto.Phone, NbVotes = nombreDeVotes });
            }
            return resultats;
        }

        public User GetUser(string id)
        {
            int i;
            if (int.TryParse(id, out i))
                return GetUser(i);

            return null;
        }

        public User GetUser(int id)
        {
            return listeDesUtilisateurs.FirstOrDefault(u => u.Id == id);
        }

        public bool HasAlreadyVoted(int idSondage, string idStr)
        {
            User u = GetUser(idStr);
            if (u == null)
                return false;
            Sondage sondage = listeDessondages.First(s => s.Id == idSondage);
            return sondage.Votes.Any(v => v.User.Id == u.Id);
        }

        public bool IsRestaurantExisting(string name)
        {
            return listeDesRestaurants.Any(resto => string.Compare(resto.Name, name, StringComparison.CurrentCultureIgnoreCase) == 0);
        }

        public void UpdateRestaurant(int id, string name, string phone)
        {
            Resto resto = listeDesRestaurants.FirstOrDefault(r => r.Id == id);
            if (resto != null)
            {
                resto.Name = name;
                resto.Phone = phone;
            }
        }
    }
}