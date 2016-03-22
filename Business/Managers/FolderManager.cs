using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Business.Models;
using Core;

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
        public ReturnValue<Folder> Add(Folder model)
        {
            return this.Add(model, true);
        }

        public ReturnValue<Folder> Add(Folder model, bool saveChanges)
        {
            return this.InternalAdd(this.context, model, saveChanges);
        }

        internal ReturnValue<Folder> InternalAdd(PresentationContext context, Folder model, bool saveChanges)
        {
            model.Guid = Guid.NewGuid().ToString();
            model.CreateTime = DateTime.Now;
            model.LastUpdateTime = DateTime.Now;

            var result = context.Folder.Add(model);
            ErrorCode error = null;
            if (saveChanges)
            {
                error = context.SafeSaveChanges();
            }
            return new ReturnValue<Folder>(result, error);
        }
        #endregion


        #region Update
        public ReturnValue<Folder> Update(Folder model)
        {
            return this.Update(model, true);
        }

        public ReturnValue<Folder> Update(Folder model, bool saveChanges)
        {
            return this.InternalUpdate(this.context, model, saveChanges);
        }

        internal ReturnValue<Folder> InternalUpdate(PresentationContext context, Folder model, bool saveChanges)
        {
            model.LastUpdateTime = DateTime.Now;
            ErrorCode error = null;
            if (saveChanges)
            {
                error = context.SafeSaveChanges();
            }
            return new ReturnValue<Folder>(model, error);
        }
        #endregion


        #region Remove
        public ReturnValue<Folder> Remove(int id)
        {
            return this.Remove(id, true);
        }

        public ReturnValue<Folder> Remove(int id, bool saveChanges)
        {
            var model = context.Folder.FirstOrDefault(a => a.Id == id);
            var result = context.Folder.Remove(model);
            ErrorCode error = null;
            if (saveChanges)
            {
                error = context.SafeSaveChanges();
            }
            return new ReturnValue<Folder>(result, error);
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