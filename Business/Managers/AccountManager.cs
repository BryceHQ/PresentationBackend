using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Business.Models;

namespace Business.Managers
{
    public class AccountManager
    {
        private PresentationContext context = new PresentationContext();

        private static AccountManager instance = new AccountManager();

        public static AccountManager Instance
        {
            get
            {
                return instance;
            }
        }

        private AccountManager()
        {

        }

        public User Add(User user)
        {
            user.NickName = user.Name;
            //user.Icon = user.
            return context.User.Add(user);
        }

        public User Remove(int id)
        {
            var user = context.User.FirstOrDefault(a => a.Id == id);
            return context.User.Remove(user);
        }

        public User Get(int id)
        {
            return context.User.FirstOrDefault(a => a.Id == id);
        }

        public User[] Where(Func<User, bool> predicate)
        {
            return context.User.Where(predicate).ToArray();
        }

        public User FirstOrDefault(Func<User, bool> predicate)
        {
            return context.User.FirstOrDefault(predicate);
        }

        public User[] All()
        {
            return context.User.ToArray();
        }
    }
}