using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace ChoixResto.Models
{
    public class Dal : IDal
    {
        private BDDContext BDD;

        public Dal()
        {
            BDD = new BDDContext();
        }

        public void Dispose()
        {
            BDD.Dispose();
        }


        public void CreateRestaurant(string name, string phone)
        {
            BDD.Restos.Add(new Resto { Name = name, Phone = phone });
            BDD.SaveChanges();
        }

        public void UpdateRestaurant(int id, string name, string phone)
        {
            Resto restoTrouve = BDD.Restos.FirstOrDefault(resto => resto.Id == id);
            if (restoTrouve != null)
            {
                restoTrouve.Name = name;
                restoTrouve.Phone = phone;
                BDD.SaveChanges();
            }
        }

        public List<Resto> GetRestaurants()
        {
            return BDD.Restos.ToList();
        }

        public bool IsRestaurantExisting(string name)
        {
            return BDD.Restos.Any(resto => string.Compare(resto.Name, name, StringComparison.CurrentCultureIgnoreCase) == 0);
        }

        public int AddUser(string name, string password)
        {
            string encodedPassword = EncodeMD5(password);
            User u = new User { Name = name, Password = encodedPassword };
            BDD.Users.Add(u);
            BDD.SaveChanges();

            return u.Id;
        }

        public User Authentificate(string name, string password)
        {
            string encodedPassword = EncodeMD5(password);
            return BDD.Users.FirstOrDefault(u => u.Name == name && u.Password == encodedPassword);
        }

        public User GetUser(int id)
        {
            return BDD.Users.FirstOrDefault(u => u.Id == id);
        }

        public User GetUser(string id)
        {
            int iid;
            if (int.TryParse(id, out iid))
                return GetUser(iid);

            return null;
        }

        public int CreateSondage()
        {
            Sondage s = new Sondage { Date = DateTime.Now };
            BDD.Sondages.Add(s);
            BDD.SaveChanges();

            return s.Id;
        }

        public void AddVote(int idSondage, int idResto, int idUser)
        {
            Vote v = new Vote { Resto = BDD.Restos.First(r => r.Id == idResto), User = BDD.Users.First(u => u.Id == idSondage) };
            Sondage s = BDD.Sondages.First(so => so.Id == idSondage);
            if (s.Votes == null)
                s.Votes = new List<Vote>();

            s.Votes.Add(v);
            BDD.SaveChanges();
        }

        public List<Results> GetResults(int idSondage)
        {
            List<Resto> lr = GetRestaurants();
            List<Results> lResults = new List<Results>();
            Sondage s = BDD.Sondages.First(so => so.Id == idSondage);
            foreach (IGrouping<int, Vote> grouping in s.Votes.GroupBy(v => v.Resto.Id))
            {
                int idRest = grouping.Key;
                Resto r = lr.First(re => re.Id == idRest);
                int nbVotes = grouping.Count();
                lResults.Add(new Results { Name = r.Name, Phone = r.Phone, NbVotes = nbVotes });
            }

            return lResults;
        }

        public bool HasAlreadyVoted(int idSondage, string idStr)
        {
            int id;
            if(int.TryParse(idStr, out id))
            {
                Sondage s = BDD.Sondages.First(so => so.Id == idSondage);
                if (s.Votes == null)
                    return false;

                return s.Votes.Any(v => v.User != null && v.User.Id == id);
            }

            return false;
        }

        private string EncodeMD5(string password)
        {
            string saltedPassword = "ChoixResto" + password + "ASP.NET MVC";
            return BitConverter.ToString(new MD5CryptoServiceProvider().ComputeHash(ASCIIEncoding.Default.GetBytes(saltedPassword)));
        }
    }
}