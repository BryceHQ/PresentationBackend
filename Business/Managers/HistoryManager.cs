using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Business.Models;
using Core;

namespace Business.Managers
{
    public class HistoryManager
    {
        private PresentationContext context = new PresentationContext();

        private static HistoryManager instance = new HistoryManager();

        public static HistoryManager Instance
        {
            get
            {
                return instance;
            }
        }

        private HistoryManager()
        {

        }


        #region Add
        public ReturnValue<History> Add(History model)
        {
            return this.Add(model, true);
        }

        public ReturnValue<History> Add(History model, bool saveChanges)
        {
            return this.InternalAdd(this.context, model, saveChanges);
        }

        internal ReturnValue<History> InternalAdd(PresentationContext context, History model, bool saveChanges)
        {
            model.Guid = Guid.NewGuid().ToString();
            model.CreateTime = DateTime.Now;
            model.LastUpdateTime = DateTime.Now;

            var result = context.History.Add(model);
            ErrorCode error = null;
            if (saveChanges)
            {
                error = context.SafeSaveChanges();
            }
            return new ReturnValue<History>(result, error);
        } 
        #endregion

        
        #region Update
        public ReturnValue<History> Update(History model)
        {
            return this.Update(model, true);
        }

        public ReturnValue<History> Update(History model, bool saveChanges)
        {
            return this.InternalUpdate(this.context, model, saveChanges);
        }

        internal ReturnValue<History> InternalUpdate(PresentationContext context, History model, bool saveChanges)
        {
            model.LastUpdateTime = DateTime.Now;
            ErrorCode error = null;
            if (saveChanges)
            {
                error = context.SafeSaveChanges();
            }
            return new ReturnValue<History>(model, error);
        }
        #endregion


        #region Remove
        public ReturnValue<History> Remove(int id)
        {
            return this.Remove(id, true);
        }

        public ReturnValue<History> Remove(int id, bool saveChanges)
        {
            return this.InternalRemove(this.context, id, saveChanges);
        }

        public ReturnValue<History> InternalRemove(PresentationContext context, int id, bool saveChanges)
        {
            var model = context.History.FirstOrDefault(a => a.Id == id);
            var result = context.History.Remove(model);
            ErrorCode error = null;
            if (saveChanges)
            {
                error = context.SafeSaveChanges();
            }
            return new ReturnValue<History>(result, error);
        } 
        #endregion


        public History Get(int id)
        {
            return context.History.FirstOrDefault(a => a.Id == id);
        }

        public History[] Where(Func<History, bool> predicate)
        {
            return context.History.Where(predicate).ToArray();
        }

        public History FirstOrDefault(Func<History, bool> predicate)
        {
            return context.History.FirstOrDefault(predicate);
        }

        public History[] All()
        {
            return context.History.ToArray();
        }
    }
}