using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using presentation.Business.Models;

namespace presentation.Business.Managers
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

        public History Add(History model)
        {
            model.CreateTime = DateTime.Now;
            model.LastUpdateTime = DateTime.Now;
            var result = context.History.Add(model);
            context.SaveChanges();
            return result;
        }

        public History Remove(int id)
        {
            var model = context.History.FirstOrDefault(a => a.Id == id);
            var result = context.History.Remove(model);
            context.SaveChanges();
            return result;
        }

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