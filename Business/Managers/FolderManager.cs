﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Business.Models;

namespace Business.Managers
{
    public class FolderManager
    {
        private PresentationContext context = new PresentationContext();

        private static FolderManager instance = new FolderManager();

        public static FolderManager Instance
        {
            get
            {
                return instance;
            }
        }

        private FolderManager()
        {
        }


        #region Add
        public Folder Add(Folder model)
        {
            return this.Add(model, true);
        }

        public Folder Add(Folder model, bool saveChanges)
        {
            return this.InternalAdd(this.context, model, saveChanges);
        }

        internal Folder InternalAdd(PresentationContext context, Folder model, bool saveChanges)
        {
            model.Guid = Guid.NewGuid().ToString();
            model.CreateTime = DateTime.Now;
            model.LastUpdateTime = DateTime.Now;

            var result = context.Folder.Add(model);
            if (saveChanges)
            {
                context.SaveChanges();
            }
            return result;
        }
        #endregion


        #region Update
        public Folder Update(Folder model)
        {
            return this.Update(model, true);
        }

        public Folder Update(Folder model, bool saveChanges)
        {
            return this.InternalUpdate(this.context, model, saveChanges);
        }

        internal Folder InternalUpdate(PresentationContext context, Folder model, bool saveChanges)
        {
            model.LastUpdateTime = DateTime.Now;
            if (saveChanges)
            {
                context.SaveChanges();
            }
            return model;
        }
        #endregion


        #region Remove
        public Folder Remove(int id)
        {
            return this.Remove(id, true);
        }

        public Folder Remove(int id, bool saveChanges)
        {
            var model = context.Folder.FirstOrDefault(a => a.Id == id);
            var result = context.Folder.Remove(model);
            if (saveChanges)
            {
                context.SaveChanges();
            }
            return result;
        }
        #endregion


        #region Get
        public Folder Get(int id)
        {
            return context.Folder.FirstOrDefault(a => a.Id == id);
        }

        public Folder[] Where(Func<Folder, bool> predicate)
        {
            return context.Folder.Where(predicate).ToArray();
        }

        public Folder FirstOrDefault(Func<Folder, bool> predicate)
        {
            return context.Folder.FirstOrDefault(predicate);
        }

        public Folder[] All()
        {
            return context.Folder.ToArray();
        }
        #endregion

    }
}