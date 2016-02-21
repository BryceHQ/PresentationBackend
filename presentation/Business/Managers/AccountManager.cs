using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using presentation.Business.Models;

namespace presentation.Business.Managers
{
    public class AccountManager
    {
        private UsersContext context = new UsersContext();

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

        public UserProfile Add(UserProfile user)
        {
            return context.UserProfiles.Add(user);
        }

        public UserProfile Remove(int id)
        {
            var user = context.UserProfiles.FirstOrDefault(a => a.UserId == id);
            return context.UserProfiles.Remove(user);
        }

        public UserProfile Get(int id)
        {
            return context.UserProfiles.FirstOrDefault(a => a.UserId == id);
        }

        public UserProfile[] Where(Func<UserProfile, bool> predicate)
        {
            return context.UserProfiles.Where(predicate).ToArray();
        }

        public UserProfile FirstOrDefault(Func<UserProfile, bool> predicate)
        {
            return context.UserProfiles.FirstOrDefault(predicate);
        }

        public UserProfile[] All()
        {
            return context.UserProfiles.ToArray();
        }
    }
}