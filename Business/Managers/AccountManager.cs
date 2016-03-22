using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Business.Models;
using Core;

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


        #region Add
        public ReturnValue<User> Add(User model)
        {
            return this.Add(model, true);
        }

        public ReturnValue<User> Add(User model, bool saveChanges)
        {
            return this.InternalAdd(this.context, model, saveChanges);
        }

        internal ReturnValue<User> InternalAdd(PresentationContext context, User model, bool saveChanges)
        {
            model.NickName = model.Name;
            model.LastUpdateTime = DateTime.Now;
            model.CreateTime = DateTime.Now;
            var result = context.User.Add(model);
            ErrorCode error = null;
            if (saveChanges)
            {
                error = context.SafeSaveChanges();
            }
            return new ReturnValue<User>(result, error);
        } 
        #endregion


        #region Update
        public ReturnValue<User> Update(User model)
        {
            return this.Update(model, true);
        }

        public ReturnValue<User> Update(User model, bool saveChanges)
        {
            return this.InternalUpdate(this.context, model, saveChanges);
        }

        internal ReturnValue<User> InternalUpdate(PresentationContext context, User model, bool saveChanges)
        {
            model.LastUpdateTime = DateTime.Now;
            ErrorCode error = null;
            if (saveChanges)
            {
                error = context.SafeSaveChanges();
            }
            return new ReturnValue<User>(model, error);
        }
        #endregion


        #region Remove
        public ReturnValue<User> Remove(int id)
        {
            return this.Remove(id, true);
        }

        public ReturnValue<User> Remove(int id, bool saveChanges)
        {
            var presentation = context.User.FirstOrDefault(a => a.Id == id);
            var result = context.User.Remove(presentation);
            ErrorCode error = null;
            if (saveChanges)
            {
                error = context.SafeSaveChanges();
            }
            return new ReturnValue<User>(result, error);
        }
        #endregion


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