﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Business.Models;

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
        public History Add(History model)
        {
            return this.Add(model, true);
        }

        public History Add(History model, bool saveChanges)
        {
            return this.InternalAdd(this.context, model, saveChanges);
        }

        internal History InternalAdd(PresentationContext context, History model, bool saveChanges)
        {
            model.Guid = Guid.NewGuid().ToString();
            model.CreateTime = DateTime.Now;
            model.LastUpdateTime = DateTime.Now;

            var result = context.History.Add(model);
            if (saveChanges)
            {
                context.SaveChanges();
            }

            return result;
        } 
        #endregion

        
        #region Update
        public History Update(History model)
        {
            return this.Update(model, true);
        }

        public History Update(History model, bool saveChanges)
        {
            return this.InternalUpdate(this.context, model, saveChanges);
        }

        internal History InternalUpdate(PresentationContext context, History model, bool saveChanges)
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
        public History Remove(int id)
        {
            return this.Remove(id, true);
        }

        public History Remove(int id, bool saveChanges)
        {
            return this.InternalRemove(this.context, id, saveChanges);
        }
        
        public History InternalRemove(PresentationContext context, int id, bool saveChanges)
        {
            var model = context.History.FirstOrDefault(a => a.Id == id);
            var result = context.History.Remove(model);
            if (saveChanges)
            {
                context.SaveChanges();
            }
            return result;
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