using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Business.Models;

namespace Business.Managers
{
    public class PresentationManager
    {
        private PresentationContext context = new PresentationContext();

        private static PresentationManager instance = new PresentationManager();

        public static PresentationManager Instance
        {
            get
            {
                return instance;
            }
        }

        private PresentationManager()
        {

        }

        public Presentation Add(Presentation presentation)
        {
            presentation.CreateTime = DateTime.Now;
            presentation.LastUpdateTime = DateTime.Now;
            var result = context.Presentation.Add(presentation);
            context.SaveChanges();
            return result;
        }

        public Presentation Update(Presentation presentation)
        {
            presentation.LastUpdateTime = DateTime.Now;
            context.SaveChanges();
            return presentation;
        }

        public Presentation Remove(int id)
        {
            var presentation = context.Presentation.FirstOrDefault(a => a.UserId == id);
            var result = context.Presentation.Remove(presentation);
            context.SaveChanges();
            return result;
        }

        public Presentation Get(int id)
        {
            return context.Presentation.FirstOrDefault(a => a.Id == id);
        }

        public Presentation[] Where(Func<Presentation, bool> predicate)
        {
            return context.Presentation.Where(predicate).ToArray();
        }

        public Presentation FirstOrDefault(Func<Presentation, bool> predicate)
        {
            return context.Presentation.FirstOrDefault(predicate);
        }

        public Presentation[] All()
        {
            return context.Presentation.ToArray();
        }
    }
}