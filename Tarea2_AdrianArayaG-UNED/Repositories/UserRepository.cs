using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tarea2_AdrianArayaG_UNED.Domain;

namespace Tarea2_AdrianArayaG_UNED.Repositories
{
    public class UserRepository
    {
        private readonly JsonFileStore<User> _store = new JsonFileStore<User>("~/App_Data/users.json");
        public User Find(string userName) => _store.Load().FirstOrDefault(u => u.UserName.ToLower() == userName.ToLower());
        public bool Exists(string userName) => Find(userName) != null;
        public void Add(User u)
        {
            var all = _store.Load();
            all.Add(u);
            _store.Save(all);
        }
    }
}