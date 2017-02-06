using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChoixResto.Models
{
    public interface IDal : IDisposable
    {
        void CreateRestaurant(string name, string phone);
        void UpdateRestaurant(int id, string name, string phone);
        List<Resto> GetRestaurants();
        bool IsRestaurantExisting(string name);

        int AddUser(string name, string password);
        User Authentificate(string name, string password);
        User GetUser(int id);
        User GetUser(string id);

        int CreateSondage();
        void AddVote(int idSondage, int idResto, int idUser);
        List<Results> GetResults(int idSondage);
        bool HasAlreadyVoted(int idSondage, string idStr);
    }
}